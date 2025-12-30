using PageNav.Metadata;
using PageNav.WinForms;
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
    kind: PageKind.Home,
    cachePolicy: PageCachePolicy.StrongSingleton)]
    public partial class PageB : PageView
    {
        public PageB()
        {
            InitializeComponent();
        }
    }
}
