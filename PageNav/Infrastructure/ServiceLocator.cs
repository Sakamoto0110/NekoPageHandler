/// <summary>
/// TODO: Document this type.
/// Describe responsibility, lifecycle expectations,
/// threading guarantees, and ownership rules.
/// </summary>
using PageNav.Metadata;
using PageNav.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNav.Infrastructure;

public sealed class ServiceLocator
{
    private readonly Dictionary<Type, object> _services = new();
    private bool _locked;

    // ------------------------------------------------------------
    // Registration
    // ------------------------------------------------------------

    public void Register<T>(T instance)
        where T : class
    {
        if(_locked)
            throw new InvalidOperationException("Service registration is locked.");

        _services[typeof(T)] = instance
            ?? throw new ArgumentNullException(nameof(instance));
    }

    // ------------------------------------------------------------
    // Resolution
    // ------------------------------------------------------------

    /// <summary>
    /// Gets a required service or throws a clear error.
    /// </summary>
    public T GetRequired<T>() where T : class
    {
        if(_services.TryGetValue(typeof(T), out var obj))
            return (T)obj;

        throw new InvalidOperationException(
            $"Required service '{typeof(T).Name}' is not registered.");
    }

    /// <summary>
    /// Gets a service or throws KeyNotFoundException (legacy behavior).
    /// </summary>
    public T Get<T>() where T : class
        => (T)_services[typeof(T)];

    public object Get(Type t)
        => _services[t];

    public bool TryGet<T>(out T service) where T : class
    {
        if(_services.TryGetValue(typeof(T), out var obj))
        {
            service = (T)obj;
            return true;
        }
        service = null;
        return false;
    }

    public bool TryGet<T>(Type t, out T service) where T : class
    {
        if(_services.TryGetValue(t, out var obj))
        {
            service = (T)obj;
            return true;
        }
        service = null;
        return false;
    }

    // ------------------------------------------------------------
    // Lifecycle
    // ------------------------------------------------------------

    public void Lock() => _locked = true;

    public void Clear()
    {
        _services.Clear();
        _locked = false;
    }
}



