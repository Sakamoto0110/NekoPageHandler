using PageNav.Contracts.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PageNav.WinForms.Hosting
{
    public sealed class WinFormsPageHost : IPageHost
    {
        private readonly Control _container;

        public WinFormsPageHost(Control container)
        {
            _container = container
                ?? throw new ArgumentNullException(nameof(container));
        }

        public void Attach(IPageView page)
        {
            if(page.NativeView is not Control control)
                throw new InvalidOperationException("Page NativeView is not a Control");

            if(!_container.Controls.Contains(control))
            {
                control.Dock = DockStyle.Fill;
                _container.Controls.Add(control);
            }

            control.Visible = true;
            control.BringToFront();
        }

        public void Detach(IPageView page)
        {
            if(page.NativeView is Control control)
            {
                _container.Controls.Remove(control);
            }
        }

        public void BringToFront(IPageView page)
        {
            if(page.NativeView is Control control)
                control.BringToFront();
        }
    }

}
