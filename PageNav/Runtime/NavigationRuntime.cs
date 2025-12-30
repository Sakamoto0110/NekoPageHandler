using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Contracts.Runtime;
using PageNav.Core.Services;
using PageNav.Diagnostics;
using PageNav.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Runtime;
internal sealed class NavigationRuntime : IAsyncDisposable
{
    private bool _isNavigating;
    private readonly NavigationContext _ctx;

    private IPageTimeoutService _timeout;
    private PageLifecycleCleanupService _cleanup;
    private IInteractionObserverService _interactionObserver;

    private readonly HashSet<IPageView> _attachedPages = new();
    private readonly HashSet<IPageView> _visiblePages = new();

    public IPageView Current { get; private set; }

    // ---------------------------------------------------------------------
    // EVENTS
    // ---------------------------------------------------------------------

    public event Action<IPageView, Type, NavigationArgs> Navigating;
    public event Action<IPageView, IPageView, NavigationArgs> Navigated;
    public event Action<IPageView, Type, Exception> NavigationFailed;
    public event Action<IPageView> CurrentChanged;
    public event Action HistoryChanged;
    public event Action TimeoutReached;
    public event Action<IPageView> OnFirstPageAttached;
    public event Action OnNoPageAttached;
    public event Action OnNoPageVisible;

    // ---------------------------------------------------------------------
    // CTOR
    // ---------------------------------------------------------------------

    public NavigationRuntime(NavigationContext ctx)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
    }

    // ---------------------------------------------------------------------
    // PUBLIC API
    // ---------------------------------------------------------------------

    public Task NavigateAsync(Type pageType, NavigationArgs args)
    {
        EnsureRuntimeServices();
        return SwitchInternalAsync(pageType, args ?? NavigationArgs.Empty);
    }

    public async Task GoBackAsync()
    {
        EnsureRuntimeServices();

        var entry = _ctx.History.PopBack();
        if (entry == null)
            return;

        if (Current != null)
        {
            _ctx.History.PushForward(new PageHistoryEntry(
                Current.GetType(),
                Current.Name,
                (Current as IPageStateful)?.CaptureState()
            ));
        }

        await SwitchInternalAsync(
            entry.PageType,
            NavigationArgs.Default(entry.State));
    }

    public async Task ResetAsync()
    {
        if (Current != null)
        {
            _ctx.Host.Detach(Current);
            await _cleanup.CleanupAsync(Current, null, forceDispose: true);
            Current = null;
            CurrentChanged?.Invoke(null);
        }

        _ctx.History.Clear();
        HistoryChanged?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        if (Current != null)
        {
            _ctx.Host.Detach(Current);
            await _cleanup.CleanupAsync(Current, null, true);
            Current = null;
        }

        if (_interactionObserver != null)
            _interactionObserver.InteractionDetected -= OnInteractionDetected;

        _timeout?.Stop();
        _timeout?.Dispose();
    }

    // ---------------------------------------------------------------------
    // INTERNALS
    // ---------------------------------------------------------------------

    private async Task SwitchInternalAsync(Type pageType, NavigationArgs navArgs)
    {
        if (pageType == null)
            throw new ArgumentNullException(nameof(pageType));

        if (_isNavigating)
            return;

        _isNavigating = true;

        IPageView from = Current;
        IPageView to = null;

        try
        {
            if (!PageRegistry.TryGetDescriptor(pageType, out var toDesc))
                throw new InvalidOperationException(
                    $"PageDescriptor not found for {pageType.FullName}");

            PageRegistry.TryGetDescriptor(from?.GetType(), out var fromDesc);

            Navigating?.Invoke(from, pageType, navArgs);

            if (!navArgs.Behavior.HasFlag(NavigationBehavior.NoMask))
                _ctx.Services.Get<IInteractionBlocker>()?.Block();

            _timeout?.Reset();

            var factory = _ctx.Services.Get<PageFactory>();
            to = PageRegistry.ResolveInstance(toDesc, factory.Create);

            PageLifecycleTracker.Register(to, toDesc);

            if (from is IPageLifecycle leave)
                await leave.OnNavigatedFromAsync();

            if (from != null)
            {
                _ctx.Host.Detach(from);
                await _cleanup.CleanupAsync(from, fromDesc, false);
            }

            _ctx.Host.Attach(to);
            _ctx.Host.BringToFront(to);

            if (to is IPageVisibility vis)
                vis.ShowPage();

            TrackAttachment(to);

            Current = to;
            CurrentChanged?.Invoke(Current);

            await LoadAsync(to, navArgs.Payload);

            if (to is IPageLifecycle enter)
                await enter.OnNavigatedToAsync(navArgs);

            TrackVisibility(to);

            if (!navArgs.Behavior.HasFlag(NavigationBehavior.NoHistory) && from != null)
            {
                _ctx.History.Record(new PageHistoryEntry(
                    from.GetType(),
                    from.Name,
                    (from as IPageStateful)?.CaptureState()
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
            try { _ctx.Services.Get<IInteractionBlocker>()?.Unblock(); } catch { }
            _isNavigating = false;
        }
    }

    private static async Task LoadAsync(IPageView page, object payload)
    {
        if (page is IBackgroundLoadable bg)
        {
            await Task.Run(() => bg.LoadInBackgroundAsync(payload));
            await bg.ApplyBackgroundResultAsync();
        }
    }

    private void EnsureRuntimeServices()
    {
        if (_timeout == null &&
            _ctx.Services.TryGet(out _timeout))
        {
            _timeout.TimeoutReached += () => TimeoutReached?.Invoke();
        }

        if (_cleanup == null &&
            _ctx.Services.TryGet(out IEventDispatcherAdapter dispatcher))
        {
            _cleanup = new PageLifecycleCleanupService(dispatcher);
        }

        if (_interactionObserver == null &&
            _ctx.Services.TryGet(out _interactionObserver))
        {
            _interactionObserver.InteractionDetected += OnInteractionDetected;
        }
    }

    private void OnInteractionDetected()
    {
        _timeout?.Reset();
    }

    private void TrackAttachment(IPageView page)
    {
        if (_attachedPages.Add(page) && _attachedPages.Count == 1)
            OnFirstPageAttached?.Invoke(page);
    }

    private void TrackVisibility(IPageView page)
    {
        _visiblePages.Add(page);
    }

    
}
