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

        _ = lifetime switch
        {
            Lifetime.Scoped => GetScopedInstance(key, typeof(TImplementation)),
            Lifetime.Singleton => GetSingletonInstance(key, typeof(TImplementation)),
            _ => GetTransientInstance(key, typeof(TImplementation)),
        };
    }


    public TInterface Resolve<TInterface>()
    {
        string? instanceName = null;
        foreach (var depen in _dependencyMap.Keys)
        {
            instanceName ??= (depen.Item1 == typeof(TInterface)) ? depen.Item2 : null ;
        }
        var instance = (instanceName is not null) ? (TInterface)Resolve(typeof(TInterface), instanceName) 
            : throw new InvalidOperationException($"Type {typeof(TInterface).Name} with not any Object is registered.");

        var instanceType = instance.GetType();
        var constructors = instanceType.GetConstructors().FirstOrDefault();

        return constructors != null ? 
            (TInterface)InjectProperties(constructors, instanceType, instance) : instance;
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
        _transientInstances.TryGetValue(key, out var instance);
        var result = instance ?? CreateInstance(implementationType);
        _transientInstances[key] = result;
        return result;
    }

    private object InjectProperties(ConstructorInfo constructors, Type instanceType, object instance)
    {
        var parameters = constructors.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var instanceName = GetInstanceName(parameters[i].ParameterType);
            args[i] ??= instanceName is not null ? Resolve(parameters[i].ParameterType, instanceName) : args[i];
        }
        return Activator.CreateInstance(instanceType, args) ?? instance;
    }

    private bool IsSingleton((Type, string?) key)
    {
        return _singletonInstances.ContainsKey(key);
    }

    private object GetSingletonInstance((Type, string?) key, Type implementationType)
    {
        _singletonInstances.TryGetValue(key, out var instance);
        var result = instance ?? CreateInstance(implementationType);
        _singletonInstances[key] = result;
        return result;
    }

    private bool IsScoped((Type, string?) key)
    {
        return _scopedInstances.ContainsKey(key);
    }

    private object GetScopedInstance((Type, string?) key, Type implementationType)
    {
        _scopedInstances.TryGetValue(key, out var instance);
        var result = instance ?? CreateInstance(implementationType);
        _scopedInstances[key] = result;
        return result;
    }

    private bool IsTransient((Type, string?) key)
    {
        return _transientInstances.ContainsKey(key);
    }

    private object CreateInstance(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();

        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var instanceName = GetInstanceName(parameters[i].ParameterType);
            args[i] ??= instanceName is not null ? Resolve(parameters[i].ParameterType, instanceName) : args[i];
        }

        return Activator.CreateInstance(type, args) ?? throw new InvalidOperationException($"Failed to create an instance of type {type}.");
    }


    private string? GetInstanceName(Type type)
    {
        string? instanceName = null;
        foreach (var depen in _dependencyMap.Keys)
        {
            instanceName ??= (depen.Item1 == type) ? depen.Item2 : null;
        }
        return instanceName;
    }
}
