using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Plataform
{
    /// <summary>
    /// Attaches and detaches event handlers dynamically.
    /// </summary>
    public interface IEventSubscriptionAdapter
    {
        void Attach<THandler>(
            object receiver,
            string eventName,
            THandler handler)
            where THandler : Delegate;

        void Detach<THandler>(
            object receiver,
            string eventName,
            THandler handler)
            where THandler : Delegate;
    }
}
