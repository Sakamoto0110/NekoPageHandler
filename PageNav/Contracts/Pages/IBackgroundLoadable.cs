using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Pages
{
    /// <summary>
    /// Indicates that a page supports deferred/background loading.
    /// </summary>
    public interface IBackgroundLoadable
    {
        /// <summary>
        /// May be executed off the UI thread.
        /// Must NOT touch UI elements directly.
        /// </summary>
        Task LoadInBackgroundAsync(object args);

        /// <summary>
        /// Executed on the UI thread after background load completes.
        /// Safe to update UI.
        /// </summary>
        Task ApplyBackgroundResultAsync();
    }
}
