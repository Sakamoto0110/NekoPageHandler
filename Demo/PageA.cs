using PageNav.Contracts.Pages;
using PageNav.Metadata;
using PageNav.WinForms;
using PageNav.WinForms.UIElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    [PageBehavior(
    kind: PageKind.Default,
    cachePolicy: PageCachePolicy.StrongSingleton)]
    public partial class MarketplacePage : PageView, IPageResources
    {
        public MarketplacePage()
        {
            InitializeComponent();
        }

        public Task LoadResourcesAsync()
        {
            Console.WriteLine("Loading marketplace");
            

            return Task.CompletedTask;
        }

        public Task ReleaseResourcesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
