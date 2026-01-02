 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Plataform
{
     
    /// <summary>
    /// Dispatches actions onto the UI thread.
    /// If already on UI thread, Invoke MUST execute inline.
    /// Must be safe to call multiple times.
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
