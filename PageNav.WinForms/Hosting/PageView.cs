using PageNav.Contracts.Pages;
using PageNav.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PageNav.WinForms
{

    public class PageView : UserControl, IPageView
    {
         
        public object NativeView => this;
        public new bool IsDisposed { get; private set; }
        public new  bool DesignMode =>
            base.DesignMode ||
            LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        protected PageView()
        {
            Name = GetType().FullName!;
            // absolutely nothing non-designer-safe here
        }

        public virtual Task OnNavigatedToAsync(NavigationArgs args)
            => Task.CompletedTask;

        public virtual Task OnNavigatedFromAsync()
            => Task.CompletedTask;

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }


    //public class PageView :
    //  UserControl,
    //  IPageView,
    //  IPageLifecycle,
    //  IPageInteraction,
    //  IPageResources
    //{
    //    // ------------------------------------------------------------
    //    // Design-time detection (single source of truth)
    //    // ------------------------------------------------------------

    //    protected PageView()
    //    {
    //        IsDesignMode =
    //        LicenseManager.UsageMode == LicenseUsageMode.Designtime;

    //        if(!IsDesignMode)
    //        {
    //            base.Visible = false;
    //        }
    //    }

    //    protected bool IsDesignMode { get; }




    //    // ------------------------------------------------------------
    //    // IPageView
    //    // ------------------------------------------------------------

    //    [Browsable(false)]
    //    public object NativeView => this;



    //    bool IPageView.DesignMode => IsDesignMode;

    //    // ------------------------------------------------------------
    //    // Visibility (framework-controlled)
    //    // ------------------------------------------------------------

    //    [Browsable(false)]
    //    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    //    internal void ShowPage() => base.Visible = true;

    //    [Browsable(false)]
    //    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    //    internal void HidePage() => base.Visible = false;

    //    // ------------------------------------------------------------
    //    // Child view hooks (framework-level, not public API)
    //    // ------------------------------------------------------------

    //    protected override void OnControlAdded(ControlEventArgs e)
    //    {
    //        base.OnControlAdded(e);
    //        if(!IsDesignMode)
    //            OnChildViewAdded(e.Control);
    //    }

    //    protected override void OnControlRemoved(ControlEventArgs e)
    //    {
    //        base.OnControlRemoved(e);
    //        if(!IsDesignMode)
    //            OnChildViewRemoved(e.Control);
    //    }

    //    protected virtual void OnChildViewAdded(Control child) { }
    //    protected virtual void OnChildViewRemoved(Control child) { }

    //    // ------------------------------------------------------------
    //    // Lifecycle
    //    // ------------------------------------------------------------

    //    public virtual Task OnNavigatedToAsync(NavigationArgs args)
    //        => Task.CompletedTask;

    //    public virtual Task OnNavigatedFromAsync()
    //        => Task.CompletedTask;

    //    // ------------------------------------------------------------
    //    // Interaction
    //    // ------------------------------------------------------------

    //    public virtual void EnableInteraction()
    //    { if(!IsDesignMode) 
    //        Enabled = true;
    //    }

    //    public virtual void DisableInteraction()
    //    {
    //        if(!IsDesignMode)  
    //        Enabled = false;
    //    }

    //    // ------------------------------------------------------------
    //    // Resources
    //    // ------------------------------------------------------------

    //    public virtual Task LoadResourcesAsync()
    //        => Task.CompletedTask;

    //    public virtual Task ReleaseResourcesAsync()
    //        => Task.CompletedTask;

    //    // ------------------------------------------------------------
    //    // Disposal
    //    // ------------------------------------------------------------

    //    protected override void Dispose(bool disposing)
    //    {
    //        base.Dispose(disposing);
    //    }


    //}


}