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
        PanelPageHost pphHost;
        NavigationContext ctx;
        public Form1()
        {

            InitializeComponent();
            pnMenu.Width = 0;
            InitPageHandler();


        }

        private void InitPageHandler()
        {
            pphHost = new PanelPageHost();
            var timeoutAdapter = new WinFormsPageTimeoutAdapter();
            var timeoutService = new PageTimeoutController(timeoutAdapter, 15);
            PageNavBootstrap.Use(pphHost, new WinFormsPlatformAdapter())
                .RegisterPagesFromAssembly(typeof(Form1).Assembly)
                .Timeout(30)
                .ConfigureServices(svc => {
                    svc.Register<IPlatformAdapter>(new WinFormsPlatformAdapter());
                    var adapter = svc.Get<IPlatformAdapter>();
                    svc.Register(adapter.CreateEventDispatcher(pnHost));
                    svc.Register(adapter.CreateTimerAdapter());
                    svc.Register(adapter.CreateInteractionBlocker(pnHost));
                    svc.Register(adapter.CreateEventSubscriber(pnHost));
                    svc.Register(adapter.CreateInteractionObserverAdapter(pphHost));
                    svc.Register<IPageTimeoutService>(timeoutService);

                }).Start();
            PageRegistry.Register<MarketplacePage>();
            PageRegistry.Register<PageB>();
            ctx = WinFormsNavigationBootstrap.Initialize(pnHost, 30);
            NavigationService.Initialize(ctx);
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            if(pnMenu.Width == 0)
            {
                ControlAnimator.Resize(pnMenu, pnMenu.Size, new Size(89, pnMenu.Height), 100);

            }
            else
            {
                ControlAnimator.Resize(pnMenu, pnMenu.Size, new Size(0, pnMenu.Height), 100);

            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage<MarketplacePage>();
        }
    }
}
