using PageNav.Core.Services;
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

namespace Demo;
public partial class PageBase : PageView
{
    public PageBase()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        NavigationService.SwitchPage<Demo.PageA>();
    }
}
