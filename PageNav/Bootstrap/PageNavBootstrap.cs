using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Infrastructure;
using PageNav.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Bootstrap
{
   

    /// <summary>
    /// Fluent bootstrap to initialize PageNav in 1–3 lines,
    /// while still allowing hybrid registration (attributes + manual tweaks).
    /// </summary>
    public sealed class PageNavBootstrap
    {
        private readonly object _nativeHost;

        private Assembly _pagesAssembly;
        private Action<PageRegistryConfigurator> _pageConfig;
        private Action<ServiceLocator> _serviceConfig;

        private int _timeoutSeconds = 10;

        private PageNavBootstrap(object nativeHost)
        {
            if(nativeHost == null) throw new ArgumentNullException(nameof(nativeHost));
            _nativeHost = nativeHost;
        }

        // --------------------------------------------------------------------
        // Entry points
        // --------------------------------------------------------------------

        /// <summary>
        /// Use a specific platform adapter (WinForms/WPF/etc). You can call this multiple
        /// times across app startup, but resolution locks at first context creation.
        /// </summary>
        public static PageNavBootstrap Use(object nativeHost, IPlatformAdapter adapter)
        {
            if(adapter == null) throw new ArgumentNullException(nameof(adapter));
            PlataformAdapter.Register(adapter);
            return new PageNavBootstrap(nativeHost);
        }

        /// <summary>
        /// If you already registered adapters manually, use this.
        /// </summary>
        public static PageNavBootstrap UseRegistered(object nativeHost)
            => new PageNavBootstrap(nativeHost);

        // --------------------------------------------------------------------
        // Configuration
        // --------------------------------------------------------------------

        public PageNavBootstrap Timeout(int seconds)
        {
            if(seconds < 0) throw new ArgumentOutOfRangeException(nameof(seconds));
            _timeoutSeconds = seconds;
            return this;
        }

        /// <summary>
        /// Auto-register pages by scanning an assembly for IPageView + [PageBehavior] metadata.
        /// </summary>
        public PageNavBootstrap RegisterPagesFromAssembly(Assembly asm)
        {
            _pagesAssembly = asm ?? throw new ArgumentNullException(nameof(asm));
            return this;
        }

        /// <summary>
        /// Manual tweaks after attribute defaults. (Hybrid mode)
        /// </summary>
        public PageNavBootstrap ConfigurePages(Action<PageRegistryConfigurator> configure)
        {
            _pageConfig = configure;
            return this;
        }

        /// <summary>
        /// Optional: add/override services before ServiceLocator gets locked.
        /// </summary>
        public PageNavBootstrap ConfigureServices(Action<ServiceLocator> configure)
        {
            _serviceConfig = configure;
            return this;
        }

        // --------------------------------------------------------------------
        // Build
        // --------------------------------------------------------------------

        public NavigationContext Start()
        {
            // 1) Register pages by attribute scan (optional but typical)
            if(_pagesAssembly != null)
                PageRegistry.RegisterFromAssembly(_pagesAssembly);

            // 2) Apply manual tweaks (hybrid)
            if(_pageConfig != null)
                _pageConfig(new PageRegistryConfigurator());

            // 3) Build navigation context via builder (composition root)
            var ctx = new NavigationContextBuilder()
                .UseHost((IPageHost)_nativeHost)
                .UseTimeout(_timeoutSeconds)
                .Build();


            // 4) Allow app to add extra services before lock
            if(_serviceConfig != null)
                _serviceConfig(ctx.Services);

            // 5) Register context itself (handy for overlays / dialogs / DM extensions)
            ctx.Services.Register(ctx);

            // 6) Lock locator
            ctx.Services.Lock();

            return ctx;
        }

        // Convenience to keep legacy static facade alive if you still want it:
        public NavigationContext StartAsDefault()
        {
            var ctx = Start();
            // If you want to keep a global pointer for legacy, do it explicitly:
            // PageNav.Core.Services.Svc.Current = ctx;
            return ctx;
        }
    }
}
