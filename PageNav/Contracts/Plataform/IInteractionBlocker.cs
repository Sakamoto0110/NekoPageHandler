/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
using System;

namespace PageNav.Contracts.Plataform
{

 

    public interface IInteractionBlocker  
    {
        void Block();
        void Unblock();
    }
}
