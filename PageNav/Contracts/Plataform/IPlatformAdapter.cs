using PageNav.Contracts.Pages;
using PageNav.Contracts.Runtime;

/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
namespace PageNav.Contracts.Plataform
{
    public interface IPlatformAdapter
    {
        
        bool CanHandle(object host);
        IPageHost CreateHost(object host);
        IEventDispatcherAdapter CreateEventDispatcher(object host);
        IEventSubscriptionAdapter CreateEventSubscriber(object host);

        IInteractionBlocker CreateInteractionBlocker(object host);
        ITimerAdapter CreateTimerAdapter();

        IPageOverlay CreateOverlayService(object host);
        IPageTimeoutAdapter CreateTimeoutAdapter();

          IInteractionObserverService CreateInteractionObserverAdapter(object host);


    }
}
