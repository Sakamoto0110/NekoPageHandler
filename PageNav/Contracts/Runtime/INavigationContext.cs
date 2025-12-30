/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Contracts.Runtime
{


    public interface INavigationContext  
    {
        IPageHost Host { get; }
        

        IPageOverlay Overlay { get; }
        IInteractionBlocker Blocker { get; }
 
         
    }

}
