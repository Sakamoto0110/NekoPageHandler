using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Runtime
{
    public interface IInteractionObserverService
    {
        /// <summary>
        /// Called when any user interaction is detected.
        /// </summary>
        event Action InteractionDetected;
    }

}
