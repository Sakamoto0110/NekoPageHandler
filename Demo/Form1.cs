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

            PanelPageHost pphHost = new PanelPageHost(pnHost);




            var ctx = PageNavBootstrap
       .Use<WinFormsPlatformAdapter>(pphHost)
       .RegisterPagesFromAssembly(typeof(Form1).Assembly)
       .Timeout(40)
       .ConfigureServices((services, plt) =>
       {
           // Register platform adapter itself
       

           // Register platform primitives
           RegisterPrimitives(plt, services, pnHost, pphHost);

           // Register factories / services (NOT instances)


           services.Register<PageFactory>(
               new PageFactory( ));
       })
       .Start();




            NavigationService.Initialize(ctx);
            NavigationService.SwitchPage<PageA>();
        }
        public void RegisterPrimitives<TPlataform>(
TPlataform platform,
    ServiceLocator svc,
    Control pnHost,
    Control pphHost) where TPlataform : class, IPlatformAdapter
        {
            svc.Register(platform);
            svc.Register(platform.CreateEventDispatcher(pnHost));
            svc.Register(platform.CreateTimerAdapter());
            svc.Register(platform.CreateInteractionBlocker(pnHost ));
            svc.Register(platform.CreateEventSubscriber(pnHost ));
            svc.Register(platform.CreateInteractionObserverAdapter(pnHost ));
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage<PageB>();

        }
    }
}
