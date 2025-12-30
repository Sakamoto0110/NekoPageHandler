using PageNav.Contracts.Plataform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PageNav.WinForms.Adapters
{
    public sealed class WinFormsEventDispatcherAdapter
     : IEventDispatcherAdapter
    {
        private readonly Control _root;

        public WinFormsEventDispatcherAdapter(Control root)
        {
            _root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public void Invoke(Action action)
        {
            if(_root.InvokeRequired)
                _root.Invoke(action);
            else
                action();
        }

        public void BeginInvoke(Action action)
        {
            _root.BeginInvoke(action);
        }
    }

}
