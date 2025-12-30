// FILE: PageNav.Core/Services/NavigationService.cs
using PageNav.Bootstrap;
using PageNav.Contracts.Pages;
using PageNav.Diagnostics;
using PageNav.Metadata;
using PageNav.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PageNav.Core.Services
{
    /// <summary>
    /// Thin static facade over a default NavigationContext.
    /// Keeps legacy call sites working while the framework becomes instance-based.
    /// </summary>
    public static partial class NavigationService
    {
        private static NavigationContext _context;

        // -------------------------------------------------------------------------
        // PUBLIC STATE
        // -------------------------------------------------------------------------

        public static IPageView Current => _context?.Current;

        // -------------------------------------------------------------------------
        // PUBLIC EVENTS (forwarded from context)
        // -------------------------------------------------------------------------

        public static event Action<IPageView, Type, NavigationArgs> Navigating;
        public static event Action<IPageView, IPageView, NavigationArgs> Navigated;
        public static event Action<IPageView, Type, Exception> NavigationFailed;
        public static event Action<IPageView> CurrentChanged;
        public static event Action HistoryChanged;

        public static event Action<IPageView> OnFirstPageAttached;
        public static event Action OnNoPageAttached;
        public static event Action OnNoPageVisible;

        private static int _attachedPages;
        private static int _visiblePages;


        // -------------------------------------------------------------------------
        // INIT / SHUTDOWN
        // -------------------------------------------------------------------------

        public static void Initialize(NavigationContext context)
        {
#if DEBUG
            if(context == null)
                throw new ArgumentNullException(nameof(context));
            if (_context != null)
                throw new InvalidOperationException(
                    "NavigationService.Initialize called twice without Shutdown().");
#endif
            _context = context;
            WireContextEvents();
            
        }

        public static async Task Shutdown()
        {
            if (_context == null)
                return;

            UnwireContextEvents();

            await _context.DisposeAsync();
            _context = null;
        }

        // -------------------------------------------------------------------------
        // PUBLIC API (forwarders)
        // -------------------------------------------------------------------------

        public static Task SwitchPage<T>(object args = null)
            where T : IPageView
            => Ensure().NavigateAsync(typeof(T), NavigationArgs.Default(args));

        public static Task SwitchPage(Type type, object args = null)
            => Ensure().NavigateAsync(type, NavigationArgs.Default(args));

        public static Task SwitchTransient<T>(object args = null)
            where T : IPageView
            => Ensure().NavigateAsync(typeof(T), NavigationArgs.Transient(args));

        public static Task SwitchTransient(Type type, object args = null)
            => Ensure().NavigateAsync(type, NavigationArgs.Transient(args));
  
        public static async Task GoHomeAsync(object args = null)
        {
            // This still relies on PageRegistry metadata (static is fine).
            var desc = PageRegistry.ResolveTimeoutTarget();
            if (desc == null)
                return;

            await Ensure().NavigateAsync(desc.PageType, NavigationArgs.Default(args));
        }

        public static Task<bool> GoBackAsync()
            => (Task<bool>)Ensure().GoBackAsync();
         

#if DEBUG
        public static void AssertFrameworkIsDown()
        {
            if (_context != null)
                throw new InvalidOperationException("NavigationContext is still alive.");

            var leaks = PageLifecycleTracker
                .SuspectedLeaks(TimeSpan.FromSeconds(1))
                .ToList();

            if (leaks.Count > 0)
            {
                throw new InvalidOperationException(
                    "Leaked pages detected:\n" +
                    string.Join("\n", leaks.Select(l =>
                        $"{l.Name} ({l.State}, policy={l.CachePolicy})")));
            }
        }
#endif

        // -------------------------------------------------------------------------
        // INTERNALS
        // -------------------------------------------------------------------------

        private static NavigationContext Ensure()
        {
            if (_context == null)
                throw new InvalidOperationException(
                    "NavigationService.Initialize must be called first.");

            return _context;
        }

        private static void WireContextEvents()
        {
            // Forward instance events to the static facade events.
            _context.Navigating += OnNavigating;
            _context.Navigated += OnNavigated;
            _context.NavigationFailed += OnNavigationFailed;
            _context.CurrentChanged += OnCurrentChanged;
            _context.HistoryChanged += OnHistoryChanged;

            // Timeout event forwarding is handled in NavigationService.Events.cs (OnTimeout)
            _context.TimeoutReached += OnTimeout;
        }

        private static void UnwireContextEvents()
        {
            _context.TimeoutReached -= OnTimeout;

            _context.Navigating -= OnNavigating;
            _context.Navigated -= OnNavigated;
            _context.NavigationFailed -= OnNavigationFailed;
            _context.CurrentChanged -= OnCurrentChanged;
            _context.HistoryChanged -= OnHistoryChanged;
        }

        private static void OnNavigating(IPageView from, Type to, NavigationArgs args)
            => Navigating?.Invoke(from, to, args);

        private static void OnNavigated(IPageView from, IPageView to, NavigationArgs args)
            => Navigated?.Invoke(from, to, args);

        private static void OnNavigationFailed(IPageView from, Type to, Exception ex)
            => NavigationFailed?.Invoke(from, to, ex);

        private static void OnCurrentChanged(IPageView current)
            => CurrentChanged?.Invoke(current);

        private static void OnHistoryChanged()
            => HistoryChanged?.Invoke();
    }
}
