using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Runtime
{
    /// <summary>
    /// Provides page idle timeout control for the navigation runtime.
    /// </summary>
    public interface IPageTimeoutService : IDisposable
    {
        /// <summary>
        /// Starts or resumes the timeout countdown.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the timeout countdown.
        /// </summary>
        void Stop();

        /// <summary>
        /// Resets the timeout countdown.
        /// </summary>
        void Reset();

        /// <summary>
        /// Raised when the timeout is reached.
        /// </summary>
        event Action TimeoutReached;
    }
}
