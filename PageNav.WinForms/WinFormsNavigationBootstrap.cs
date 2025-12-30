using PageNav.Bootstrap;
using PageNav.Contracts.Plataform;
using PageNav.Contracts.Runtime;
using PageNav.Core.Services;
using PageNav.Runtime;
using PageNav.WinForms.Adapters;
using PageNav.WinForms.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PageNav.WinForms
{
    public static class WinFormsNavigationBootstrap
    {
        public static NavigationContext Initialize(
            Control root,
            int timeoutSeconds = 10)
        {
            if(root == null)
                throw new ArgumentNullException(nameof(root));

            // Platform services
            var interactionObserver = new WinFormsInteractionObserver(root);
            var dispatcher = new WinFormsEventDispatcherAdapter(root);
            var subscriptions = new WinFormsEventSubscriptionAdapter();
            var timer = new WinFormsTimerAdapter();
            var blocker = new WinFormsInteractionBlocker(root);
            var host = new PanelPageHost((Panel)root);

            var timeoutAdapter = new WinFormsPageTimeoutAdapter();

            var timeoutService = new PageTimeoutController(timeoutAdapter, timeoutSeconds);
             
       
            // Core builder
            var builder = new NavigationContextBuilder()
                .UseHost(host)
                .UseTimeout(timeoutSeconds)
                .UseService<IEventDispatcherAdapter>(dispatcher)
                .UseService<ITimerAdapter>(timer)
                .UseService<IInteractionBlocker>(blocker)
                .UseService<IEventDispatcherAdapter>(dispatcher)
                .UseService<IEventSubscriptionAdapter>(subscriptions)
                .UseService<IInteractionObserverService>(interactionObserver)
                .UseService< IPageTimeoutService>(timeoutService);

           

            var context = builder.Build();

            // Static bind
            return context;
        }
    }
}
