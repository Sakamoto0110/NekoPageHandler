// FILE: PageNav.Core/Services/PageLifecycleCleanupService.cs
using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Diagnostics;
using PageNav.Metadata;
using System;
using System.Threading.Tasks;

namespace PageNav.Runtime
{
    /// <summary>
    /// Instance-scoped cleanup service.
    /// Responsible for releasing and disposing pages according to cache policy.
    /// </summary>
    public sealed class PageLifecycleCleanupService
    {
        private readonly IEventDispatcherAdapter _dispatcher;

        public PageLifecycleCleanupService(IEventDispatcherAdapter dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task CleanupAsync(
            IPageView page,
            PageDescriptor descriptor,
            bool forceDispose = false)
        {
            if (page == null)
                return;

            // ✅ descriptor can be null ONLY when forcing disposal (shutdown/reset).
            if (descriptor == null && !forceDispose)
                throw new ArgumentNullException(nameof(descriptor));

            var effectivePolicy =
                forceDispose ? PageCachePolicy.Disabled : descriptor.CachePolicy;

            PageLifecycleTracker.Update(page, PageLifecycleState.Leaving);

            // Best-effort detach of framework-owned event wiring (if any).
            // NOTE: Do NOT hardcode Timeout reset here anymore.
            // Context handles timeout reset wiring.
            // If you still detach events here, make sure you detach only what you attached.

            // Cache policy behavior
            switch (effectivePolicy)
            {
                case PageCachePolicy.Disabled:
                case PageCachePolicy.Stackable:
                    await SafeReleaseAsync(page);
                    DisposePage(page);
                    PageLifecycleTracker.Update(page, PageLifecycleState.Disposed);
                    PageLifecycleTracker.Unregister(page);
                    break;

                case PageCachePolicy.WeakSingleton:
                    await SafeReleaseAsync(page);
                    PageLifecycleTracker.Update(page, PageLifecycleState.Released);
                    break;

                case PageCachePolicy.StrongSingleton:
                    PageLifecycleTracker.Update(page, PageLifecycleState.Detached);
                    break;
            }
        }

        private static async Task SafeReleaseAsync(IPageView page)
        {
            try
            {
                if (page is IPageResources resources)
                    await resources.ReleaseResourcesAsync();
            }
            catch (Exception ex)
            {
                PageLogger.LogError($"ReleaseResources failed on {page.Name}: {ex}");
            }
        }

        private static void DisposePage(IPageView page)
        {
            try { page.Dispose(); }
            catch (Exception ex)
            {
                PageLogger.LogError($"Dispose failed on {page.Name}: {ex}");
            }
        }
    }
}
