using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Plataform
{
    public interface IPageTimeoutAdapter : IDisposable
    {
        /// <summary>
        /// Starts the timeout countdown.
        /// </summary>
        void Start(int timeoutMilliseconds);

        /// <summary>
        /// Stops the timeout countdown.
        /// </summary>
        void Stop();

        /// <summary>
        /// Fired when the timeout elapses.
        /// </summary>
        event Action TimeoutElapsed;
    }

}
