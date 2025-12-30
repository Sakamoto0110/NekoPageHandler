/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Plataform
{
    /// <summary>
    /// Dispatches execution to the platform's UI thread.
    /// </summary>
    public interface IEventDispatcherAdapter
    {
        /// <summary>
        /// Executes the action synchronously on the UI thread.
        /// </summary>
        void Invoke(Action action);

        /// <summary>
        /// Executes the action asynchronously on the UI thread.
        /// </summary>
        void BeginInvoke(Action action);
    }
}
