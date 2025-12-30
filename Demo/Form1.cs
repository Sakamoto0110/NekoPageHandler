using PageNav.WinForms;
using PageNav.Bootstrap;
using PageNav.Core.Services;
using PageNav.Contracts.Plataform;
using PageNav.WinForms.Adapters;
using PageNav.Runtime;
using PageNav.Infrastructure;
using PageNav.WinForms.UIElements;
using PageNav.Contracts.Runtime;
namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();

            PanelPageHost pphHost = new PanelPageHost();

    
        
            var timeoutAdapter = new WinFormsPageTimeoutAdapter();

            var timeoutService = new PageTimeoutController(timeoutAdapter, 15);
             
            PageNavBootstrap.Use(pphHost, new WinFormsPlatformAdapter())
                .RegisterPagesFromAssembly(typeof(Form1).Assembly)
                .Timeout(30)
                .ConfigureServices(svc =>
                {
                    svc.Register<IPlatformAdapter>(new WinFormsPlatformAdapter());
                    var adapter = svc.Get<IPlatformAdapter>();
                    svc.Register(adapter.CreateEventDispatcher(pnHost));
                    svc.Register(adapter.CreateTimerAdapter());
                    svc.Register(adapter.CreateInteractionBlocker(pnHost));
                    svc.Register(adapter.CreateEventSubscriber(pnHost));
                    svc.Register(adapter.CreateInteractionObserverAdapter(pphHost));
                    svc.Register<IPageTimeoutService>(timeoutService);

                }).Start();


            PageRegistry.Register<PageA>();
            PageRegistry.Register<PageB>();
             
            var ctx= WinFormsNavigationBootstrap.Initialize(pnHost, 30);
            NavigationService.Initialize(ctx);
            NavigationService.SwitchPage<PageA>();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage<PageB>();

        }
    }
}
