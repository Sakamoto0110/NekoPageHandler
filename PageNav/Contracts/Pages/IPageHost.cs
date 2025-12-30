using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Pages
{

    /// <summary>
    /// Container that knows how to attach/detach pages (page-level operations).
    /// </summary>
    public interface IPageHost
    {
        void Attach(IPageView page);
        void Detach(IPageView page);
        void BringToFront(IPageView page);
    }
    /// <summary>
    /// Low-level view operations for platform-specific services (overlays, focus, z-order).
    /// </summary>
    public interface IViewHost
    {
        void AddView(object view);
        void RemoveView(object view);
        void BringToFront(object view);
        void Focus(object view);
    }

     
}
