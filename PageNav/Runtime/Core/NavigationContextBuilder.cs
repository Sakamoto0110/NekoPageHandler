using PageNav.Contracts.Pages;
using PageNav.Contracts.Plataform;
using PageNav.Contracts.Runtime;
using PageNav.Core.Services;
using PageNav.Infrastructure;
using PageNav.Runtime;
using PageNav.Runtime.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Runtime.Core
{
    public sealed class NavigationContextBuilder
    {
        private IPageHost _host;
        private int _timeoutSeconds = 10;

        private readonly ServiceLocator _services = new ServiceLocator();

        public NavigationContextBuilder UseHost(IPageHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            return this;
        }

        public NavigationContextBuilder UseService<T>(T instance)
            where T : class
        {
            _services.Register(instance);
            return this;
        }

        public NavigationContextBuilder UseTimeout(int seconds)
        {
            _timeoutSeconds = seconds;
            return this;
        }

        public NavigationContext Build()
        {
            if(_host == null)
                throw new InvalidOperationException("IPageHost is required.");

            // Core-owned services
            _services.Register(_host);
            var pageFactory = new PageFactory();
            _services.Register(pageFactory);
            var context = new NavigationContext(
                _host,
                _services,
               TimeSpan.FromSeconds(10)
            );

            _services.Register(context);
             

            return context;
        }
    }


 }
