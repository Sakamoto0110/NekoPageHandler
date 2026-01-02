using PageNav.WinForms;
using PageNav.Bootstrap;
using PageNav.Core.Services;
using PageNav.Contracts.Plataform;
using PageNav.WinForms.Adapters;
using PageNav.Runtime;
using PageNav.WinForms.UIElements;
using PageNav.Contracts.Runtime;
using PageNav.Runtime.Factories;
using PageNav.Runtime.Registry;
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


           services.Register<PageFactory>(new PageFactory());
       })
       .Start();




            NavigationService.Initialize(ctx);

            NavigationService.CurrentChanged += (pg) =>
            {
                var page = NavigationService.Current;
                Text = $"Demo - Current Page: {page?.Name ?? "null"}";
            };
            NavigationService.OnFirstPageAttached += (pg) =>
            {
                    Console.WriteLine("First page attached.");
                };

            NavigationService.Navigated += (from, to, args) =>
            {
                Console.WriteLine($"Navigated from {from?.GetType().Name ?? "null"} to {to?.Name ?? "null"}");
            };
            NavigationService.Navigating += (from, to, args) =>
            {
                Console.WriteLine($"Navigating from {from?.GetType().Name ?? "null"} to {to?.Name ?? "null"}");
            };



            
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
            svc.Register(platform.CreateInteractionBlocker(pnHost));
            svc.Register(platform.CreateEventSubscriber(pnHost));
            svc.Register(platform.CreateInteractionObserverAdapter(pnHost));
        }
        private async void button1_Click(object sender, EventArgs e)
        {
           

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage(typeof(PageA));
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage(typeof(PageB));

        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage(typeof(PageC));

        }

        private async void button6_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage(typeof(PageD));

        }

        private async void button7_Click(object sender, EventArgs e)
        {
            await NavigationService.SwitchPage(typeof(PageE));

        }
    }
}
