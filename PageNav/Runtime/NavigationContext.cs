/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
﻿// FILE: PageNav.Core/Services/NavigationContext.cs
using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Contracts.Runtime;

using PageNav.Core.Services;
using PageNav.Diagnostics;
using PageNav.Infrastructure;
using PageNav.Metadata;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PageNav.Runtime
{
    /// <summary>
    /// Passive container for navigation-scoped state and services.
    ///
    /// Responsibilities:
    /// - Hold references shared by the navigation runtime
    /// - Expose immutable configuration data
    ///
    /// Non-responsibilities:
    /// - No navigation logic
    /// - No service resolution
    /// - No lifecycle management
    /// - No event wiring
    /// - No platform interaction
    /// </summary>
    public sealed class NavigationContext
    {
        public IPageHost Host { get; }
        public ServiceLocator Services { get; }
        public NavigationHistory History { get; }
        public TimeSpan Timeout { get; }

        public NavigationContext(
            IPageHost host,
            ServiceLocator services,
            TimeSpan timeout)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Timeout = timeout;

            History = new NavigationHistory();
        }
    }
}
