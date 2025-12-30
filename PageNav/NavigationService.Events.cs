// FILE: PageNav.Core/Services/NavigationService.Events.cs
 using PageNav.Diagnostics;
using PageNav.Metadata;
using PageNav.Runtime;
using System;
using System.Threading.Tasks;

namespace PageNav.Core.Services
{
    public static partial class NavigationService
    {
        /// <summary>
        /// Handles timeout events forwarded from NavigationContext.
        /// This method keeps legacy timeout behavior but delegates navigation
        /// to the instance-based NavigationContext.
        /// </summary>
        private static async void OnTimeout()
        {
            if (_context == null)
                return;

            var current = _runtime.Current;
            PageDescriptor desc = null;

            try
            {
                if (current != null)
                {
                    if (!PageRegistry.TryGetDescriptor(current.GetType(), out desc))
                        throw new InvalidOperationException(
                            $"PageDescriptor of {current.GetType()} not found.");

                    switch (desc.Timeout)
                    {
                        case PageTimeoutBehavior.IgnoreTimeout:
                            return;

                        case PageTimeoutBehavior.OverrideHome:
                            break; // fallthrough to home resolution
                    }
                }

                // Default behavior: resolve home page
                var home = PageRegistry.ResolveTimeoutTarget();
                if (home == null)
                    return;
                await _runtime.NavigateAsync(home.PageType, NavigationArgs.Default());
                 
            }
            catch (Exception ex)
            {
                PageLogger.LogError(
                    $"Timeout navigation failed: {ex}");
            }
        }
    }
}
