using System;
using System.Collections.Generic;
using System.Reflection;

public class DIContainer
{
    private readonly Dictionary<(Type, string), Type> _dependencyMap = new();
    private readonly Dictionary<(Type, string), object> _singletonInstances = new();
    private readonly Dictionary<(Type, string), object> _scopedInstances = new();
    private readonly Dictionary<(Type, string), object> _transientInstances = new();
    public enum Lifetime
    {
        Transient,
        Scoped,
        Singleton
    }
    public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient, string name = null)
    {
        var key = (typeof(TInterface), name);
        _dependencyMap[key] = typeof(TImplementation);

        if (lifetime == Lifetime.Singleton)
        {
            var instance = CreateInstance(typeof(TImplementation));
            _singletonInstances[key] = instance;
        }
    }

    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Resolve(typeof(TInterface), null);
    }

    public TInterface Resolve<TInterface>(string name)
    {
        return (TInterface)Resolve(typeof(TInterface), name);
    }

    public static void BeginScope()
    {
        // Optionally implement scope management, e.g., for web requests
    }

    public static void EndScope()
    {
        // Optionally implement scope management, e.g., for web requests
    }

    private object Resolve(Type type, string name)
    {
        var key = (type, name);

        if (_dependencyMap.TryGetValue(key, out var implementationType))
        {
            if (implementationType is null)
            {
                throw new InvalidOperationException($"Type {type} with name '{name}' is not registered.");
            }

            if (IsSingleton(key))
            {
                return GetSingletonInstance(key, implementationType);
            }

            if (IsScoped(key))
            {
                return GetScopedInstance(key, implementationType);
            }

            if (IsTransient(key))
            {
                return GetTransientInstance(key, implementationType);
            }

            return CreateInstance(implementationType);
        }
        else
        {
            throw new InvalidOperationException($"Type {type} with name '{name}' is not registered.");
        }
    }

    private object GetTransientInstance((Type type, string name) key, Type implementationType)
    {
        if (!_transientInstances.TryGetValue(key, out var instance))
        {
            instance = CreateInstance(implementationType);
            _transientInstances[key] = instance;
        }
        return instance;
    }

    private bool IsSingleton((Type, string) key)
    {
        return _singletonInstances.ContainsKey(key);
    }

    private object GetSingletonInstance((Type, string) key, Type implementationType)
    {
        if (!_singletonInstances.TryGetValue(key, out var instance))
        {
            instance = CreateInstance(implementationType);
            _singletonInstances[key] = instance;
        }
        return instance;
    }

    private bool IsScoped((Type, string) key)
    {
        return _scopedInstances.ContainsKey(key);
    }

    private object GetScopedInstance((Type, string) key, Type implementationType)
    {
        if (!_scopedInstances.TryGetValue(key, out var instance))
        {
            instance = CreateInstance(implementationType);
            _scopedInstances[key] = instance;
        }
        return instance;
    }

    private bool IsTransient((Type, string) key)
    {
        return _transientInstances.ContainsKey(key);
    }

    private object CreateInstance(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();
        if (constructors.Length == 0)
        {
            return Activator.CreateInstance(type);
        }

        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            args[i] = Resolve(parameters[i].ParameterType, null);
        }

        return Activator.CreateInstance(type, args);
    }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class InjectAttribute : Attribute
{
}
