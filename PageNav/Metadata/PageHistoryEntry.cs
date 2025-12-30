/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Metadata
{

    public sealed class PageHistoryEntry
    {
        public Type PageType { get; }
        public string PageName { get; }

        /// <summary>
        /// Page-owned history state (NOT NavigationArgs payload).
        /// </summary>
        public object State { get; }

        public DateTime Timestamp { get; }

        public PageHistoryEntry(Type pageType, string pageName, object state)
        {
            PageType = pageType;
            PageName = pageName;
            State = state;
            Timestamp = DateTime.Now;
        }
    }

}
