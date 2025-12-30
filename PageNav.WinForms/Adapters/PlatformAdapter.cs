using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Contracts.Runtime;
using PageNav.WinForms.Adapters;
using PageNav.WinForms.Hosting;
using PageNav.WinForms.UIElements;
using System;
using System.Windows.Forms;
namespace PageNav.WinForms
{

    public sealed class WinFormsPlatformAdapter : IPlatformAdapter
    {
        public bool CanHandle(object host)
          => host is Control;

        public IPageHost CreateHost(object host)
        {
            // host is ALWAYS the native Control passed to Initialize

            if(host.GetType() == typeof(Panel)) return new PanelPageHost((Panel)host);
            throw new ArgumentException();
        }

        public IPageOverlay CreateOverlayService(object host)
        {
            // host is STILL the native Control
            return null;
        }

        public IInteractionBlocker CreateInteractionBlocker(object host) => new WinFormsInteractionBlocker((Control)host);
        public IEventSubscriptionAdapter CreateEventSubscriber(object host) => new WinFormsEventSubscriptionAdapter();
        public IEventDispatcherAdapter CreateEventDispatcher(object host) => new WinFormsEventDispatcherAdapter((Control)host);
        public ITimerAdapter CreateTimerAdapter() => new WinFormsTimerAdapter();
        public IPageTimeoutAdapter CreateTimeoutAdapter() => new WinFormsPageTimeoutAdapter();
        public IInteractionObserverService CreateInteractionObserverAdapter(object host) => new WinFormsInteractionObserver((Control)host);
        public void InvokeOnUI(Action action)
        {
            if(Application.OpenForms.Count == 0)
                return;

            var form = Application.OpenForms[0];

            if(form.InvokeRequired)
                form.Invoke(action);
            else
                action();
        }
    }

    


    
}
