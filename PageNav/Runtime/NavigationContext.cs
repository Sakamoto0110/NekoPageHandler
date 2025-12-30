/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
﻿// FILE: PageNav.Core/Services/NavigationContext.cs
using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Contracts.Runtime;

using PageNav.Core.Services;
using PageNav.Diagnostics;
using PageNav.Infrastructure;
using PageNav.Metadata;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PageNav.Runtime
{
    public sealed class NavigationContext : INavigationContext
    {
        private bool _isNavigating;

        public IPageHost Host { get; }
        public ServiceLocator Services { get; }
        public NavigationHistory History { get; }

        private readonly IPageTimeoutService _timeout;
        private readonly PageLifecycleCleanupService _cleanup;

        public IPageView Current { get; private set; }

        public IPageOverlay Overlay => Services.Get<IPageOverlay>();
        public IInteractionBlocker Blocker => Services.Get<IInteractionBlocker>();

        public event Action<IPageView, Type, NavigationArgs> Navigating;
        public event Action<IPageView, IPageView, NavigationArgs> Navigated;
        public event Action<IPageView, Type, Exception> NavigationFailed;
        public event Action<IPageView> CurrentChanged;
        public event Action HistoryChanged;
        public event Action TimeoutReached;
        private readonly IInteractionObserverService _interactionObserver;
        private readonly HashSet<IPageView> _attachedPages = new();

        public NavigationContext(IPageHost host, ServiceLocator services, TimeSpan? timeSpan)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            timeSpan = timeSpan ?? throw new ArgumentNullException(nameof(timeSpan));

            History = new NavigationHistory();

            if (Services.TryGet(out IEventDispatcherAdapter dispatcher))
                _cleanup = new PageLifecycleCleanupService(dispatcher);

            // Optional dependency
            if (services.TryGet<IInteractionObserverService>(out _interactionObserver))
            {
                _interactionObserver.InteractionDetected += OnInteractionDetected;
            }
            if (services.TryGet<IPageTimeoutService>(out _timeout))
                _timeout.TimeoutReached += () => TimeoutReached?.Invoke();
        }
        private void OnInteractionDetected()
        {
            // If no pages are attached, ignore interaction
            if(_attachedPages.Count == 0)
                return;

            //Services.GetRequired<IPageTimeoutService>().Reset();
        }
        public Task NavigateAsync(Type pageType, NavigationArgs args = null)
            => SwitchInternalAsync(pageType, args ?? NavigationArgs.Empty);

        public async Task GoBackAsync()
        {
            var entry = History.PopBack();

            // push current into forward stack (optional)
            if(Current != null)
            {
                History.PushForward(new PageHistoryEntry(
                    Current.GetType(),
                    Current.Name,
                    (Current as IPageStateful)?.CaptureState()
                ));
            }

            await NavigateAsync(entry.PageType, NavigationArgs.Default(entry.State)); 
        }

        public async Task ResetAsync()
        {
            // force dispose current page
            if (Current != null)
            {
                Host.Detach(Current);
                await _cleanup.CleanupAsync(Current, descriptor: null, forceDispose: true);
                Current = null;
            }

            History.Clear();
            HistoryChanged?.Invoke();
        }
     
        private async Task SwitchInternalAsync(Type pageType, NavigationArgs navArgs)
        {
            if (pageType == null)
                throw new ArgumentNullException(nameof(pageType));

            if (_isNavigating)
                return;

            _isNavigating = true;

            IPageView from = Current;
            IPageView to = null;
            PageDescriptor toDesc = null;
            PageDescriptor fromDesc = null;

            try
            {
                if (!PageRegistry.TryGetDescriptor(pageType, out toDesc))
                    throw new InvalidOperationException($"PageDescriptor of {pageType} not found.");

                if (from != null)
                    PageRegistry.TryGetDescriptor(from.GetType(), out fromDesc);

                Navigating?.Invoke(from, pageType, navArgs);

                // optional: block interaction during switch
                if (!navArgs.Behavior.HasFlag(NavigationBehavior.NoMask))
                    Blocker?.Block();

                // ✅ reset timeout at start of navigation
                _timeout.Reset();

                // Create/resolve page instance using locator factory
 
                var pageFactory = Services.Get<PageFactory>();

                to = PageRegistry.ResolveInstance(
                    toDesc,
                    t => pageFactory.Create(t));


                PageLifecycleTracker.Register(to, toDesc);

                // Leave
                if (from is IPageLifecycle leave)
                    await leave.OnNavigatedToAsync(navArgs);

                // Detach + cleanup old page using its OWN descriptor
                if (from != null)
                {
                    Host.Detach(from);
                    await _cleanup.CleanupAsync(from, fromDesc ?? toDesc, forceDispose: false);
                }

                // Attach new page
                Host.Attach(to);
                Host.BringToFront(to);

                if(to is IPageVisibility vis)
                    vis.ShowPage();

                Current = to;
                CurrentChanged?.Invoke(Current);

                PageLifecycleTracker.Update(to, PageLifecycleState.Attached);

                // Load strategy (background if supported + requested)
                await LoadAsync(to, navArgs.Payload);

                // Enter
                if (to is IPageLifecycle enter)
                    await enter.OnNavigatedToAsync(navArgs);

                PageLifecycleTracker.Update(to, PageLifecycleState.Entered);

                // History
                if (!navArgs.Behavior.HasFlag(NavigationBehavior.NoHistory) && from != null)
                {
                    object historyState = null;
                    if(from is IPageStateful stateful)
                    {
                        try
                        {
                            historyState = stateful.CaptureState();
                        }
                        catch
                        {
                            // Never fail navigation because of history
                            historyState = null;
                        }
                    }

                    History.Record(new PageHistoryEntry(
                        from.GetType(),
                        from.Name,
                        historyState
                    ));
                    HistoryChanged?.Invoke();
                }

                Navigated?.Invoke(from, to, navArgs);
                PageLoggerService.OnNavigationSuccess(from, to, navArgs);
                
            }
            catch (Exception ex)
            {
                NavigationFailed?.Invoke(from, pageType, ex);
                PageLoggerService.OnNavigationFailure(from, to, navArgs);

                throw;
            }
            finally
            {
                try { Blocker?.Unblock(); } catch { }
                _isNavigating = false;
            }
        }

        private static async Task LoadAsync(IPageView page, object payload)
        {
            if (page is IBackgroundLoadable bg)
            {
                await Task.Run(() => bg.LoadInBackgroundAsync(payload)).ConfigureAwait(false);
                await bg.ApplyBackgroundResultAsync();
            }
        }

        public async Task DisposeAsync()
        {
            if (Current != null)
            {
                Host.Detach(Current);
                await _cleanup.CleanupAsync(Current, descriptor: null, forceDispose: true);
                Current = null;
            }
            if(_interactionObserver != null)
                _interactionObserver.InteractionDetected -= OnInteractionDetected;
            _timeout.Stop();
            _timeout.Dispose();

            Services.Clear();
        }

        public void Dispose()
            => DisposeAsync().GetAwaiter().GetResult();
    }
}
