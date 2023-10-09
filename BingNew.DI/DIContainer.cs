using System.Reflection;

namespace BingNew.DI;
public class DIContainer
{
    private readonly Dictionary<(Type, string?), Type> _dependencyMap = new();
    private readonly Dictionary<(Type, string?), object> _singletonInstances = new();
    private readonly Dictionary<(Type, string?), object> _scopedInstances = new();
    private readonly Dictionary<(Type, string?), object> _transientInstances = new();


    public enum Lifetime
    {
        Transient,
        Scoped,
        Singleton
    }
    /// <summary>
    /// Register Dependency Injection with lifetime : Transient (default), Scope, Singleton
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="lifetime"> Lifetimes defined</param>
    /// <param name="nameObject"> When you want to register one interface with many typeNameObject, you need to pass second parameters</param>
    public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
    {
        string nameObject = typeof(TImplementation).Name;
        var key = (typeof(TInterface), nameObject) ;
        _dependencyMap[key] = typeof(TImplementation);

        switch (lifetime)
        {
            case Lifetime.Scoped:
                var instanceScoped = CreateInstance(typeof(TImplementation));
                _scopedInstances[key] = instanceScoped;
                break;
            case Lifetime.Singleton:
                var instanceSingleton = CreateInstance(typeof(TImplementation));
                _singletonInstances[key] = instanceSingleton;
                break;
            default:
                var instanceTransient = CreateInstance(typeof(TImplementation));
                _transientInstances[key] = instanceTransient;
                break;
        }
    }

    public TInterface Resolve<TInterface>()
    {
        string? instanceName = null;
        foreach (var depen in _dependencyMap.Keys)
        {
            instanceName ??= (depen.Item1 == typeof(TInterface)) ? depen.Item2 : null ;
        }

        return instanceName is not null ?  (TInterface)Resolve(typeof(TInterface), instanceName)
            : throw new InvalidOperationException($"Type {typeof(TInterface).Name} with not any Object is registered.");
    }

    /// <summary>
    /// Param : typeNameObject 
    /// <para>Example | Object People, has typeNameObject is <b>typeof(People).Name</b></para>
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="typeNameObject"></param>
    /// <returns> Returns an instance of a registered DI object </returns>
    public TInterface Resolve<TInterface>(string typeNameObject) 
    {
        return (TInterface)Resolve(typeof(TInterface), typeNameObject);
    }

    public static void BeginScope()
    {
        // Optionally typeNameObject scope management, e.g., for web requests
    }

    public static void EndScope()
    {
        // Optionally typeNameObject scope management, e.g., for web requests
    }

    private object Resolve(Type type, string name)
    {

        var key = (type, name);

        var errorFunc = new Func<(Type, string), object>(key => throw new InvalidOperationException($"Type {key.Item1} with nameObject '{key.Item2}' is not registered."));

        var resolutionStrategies = new List<(Func<(Type, string), bool> Condition, Func<(Type, string), object> Strategy)>
        {
            (key => !_dependencyMap.TryGetValue(key, out var implementationType) || implementationType is null, errorFunc),
            (IsSingleton, key => GetSingletonInstance(key, _dependencyMap[key])),
            (IsScoped, key => GetScopedInstance(key, _dependencyMap[key])),
            (IsTransient, key => GetTransientInstance(key, _dependencyMap[key]))
        };

        var selectedStrategy = resolutionStrategies.Find(strategy => strategy.Condition(key));

        return selectedStrategy != default
            ? selectedStrategy.Strategy(key)
            : CreateInstance(_dependencyMap[key]);
    }

    private object GetTransientInstance((Type type, string? name) key, Type implementationType)
    {
        if (!_transientInstances.TryGetValue(key, out var instance))
        {
            instance = CreateInstance(implementationType);
            _transientInstances[key] = instance;
        }
        return instance;
    }

    private bool IsSingleton((Type, string?) key)
    {
        return _singletonInstances.ContainsKey(key);
    }

    private object GetSingletonInstance((Type, string?) key, Type implementationType)
    {
        if (!_singletonInstances.TryGetValue(key, out var instance))
        {
            instance = CreateInstance(implementationType);
            _singletonInstances[key] = instance;
        }
        return instance;
    }

    private bool IsScoped((Type, string?) key)
    {
        return _scopedInstances.ContainsKey(key);
    }

    private object GetScopedInstance((Type, string?) key, Type implementationType)
    {
        if (!_scopedInstances.TryGetValue(key, out var instance))
        {
            instance = CreateInstance(implementationType);
            _scopedInstances[key] = instance;
        }
        return instance;
    }

    private bool IsTransient((Type, string?) key)
    {
        return _transientInstances.ContainsKey(key);
    }

    private object CreateInstance(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();
        if (constructors.Length == 0)
        {
            return Activator.CreateInstance(type)
                ?? throw new InvalidOperationException($"Failed to create an instance of type {type}.");
        }

        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];

        //for (int i = 0; i < parameters.Length; i++)
        //{
        //    args[i] = Resolve(parameters[i].ParameterType, parameters[i].Name);
        //}

        return Activator.CreateInstance(type, args)
            ?? throw new InvalidOperationException($"Failed to create an instance of type {type}.");
    }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class InjectAttribute : Attribute
{
}
