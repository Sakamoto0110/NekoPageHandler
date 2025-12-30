using PageNav.Contracts.Plataform;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PlataformAdapter
{
    private static readonly List<IPlatformAdapter> _adapters = new List<IPlatformAdapter>();
    private static bool _lockRegistration;
    private static IPlatformAdapter _resolvedAdapter;

    /// <summary>
    /// Registers a platform adapter. Must be called before NavigationService.Initialize().
    /// </summary>
    public static void Register(IPlatformAdapter adapter)
    {
        if(adapter == null)
            throw new ArgumentNullException(nameof(adapter));

        if(_lockRegistration)
            throw new InvalidOperationException("Platform adapters must be registered before initialization.");

        _adapters.Add(adapter);
    }

    /// <summary>
    /// Resolves the platform adapter based on the native host during initialization.
    /// This method will cache the resolved adapter and lock further registrations.
    /// </summary>
    public static IPlatformAdapter ResolveHost(object host)
    {
        if(host == null)
            throw new ArgumentNullException(nameof(host));

        // If already resolved, avoid re-evaluation
        if(_resolvedAdapter != null)
            return _resolvedAdapter;

        foreach(var adapter in _adapters)
        {
            if(adapter.CanHandle(host))
            {
                _resolvedAdapter = adapter;
                _lockRegistration = true;
                return adapter;
            }
        }

        throw new NotSupportedException(
            $"No registered platform adapter can handle host type: {host.GetType().FullName}.\n" +
            $"Registered adapters: {string.Join(", ", _adapters.Select(a => a.GetType().Name))}");
    }
}