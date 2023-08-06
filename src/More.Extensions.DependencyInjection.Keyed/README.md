Provides extensions for creating keyed services using a `Type` for a key with **Microsoft.Extensions.DependencyInjection**.

The following containers are supported with this package alone:

- Default
- Autofac
- LightInject
- Unity

The following containers are supported via an additional package:

- [DryIoc](https://www.nuget.org/packages/More.DryIoc.Extensions.DependencyInjection.Keyed)
- [Grace](https://www.nuget.org/packages/More.Grace.Extensions.DependencyInjection.Keyed)
- [Lamar](https://www.nuget.org/packages/More.Lamar.Extensions.DependencyInjection.Keyed)
- [Stashbox](https://www.nuget.org/packages/More.Stashbox.Extensions.DependencyInjection.Keyed)
- [StructureMap](https://www.nuget.org/packages/More.StructureMap.Extensions.DependencyInjection.Keyed)

## Commonly Used Types

- Extensions.DependencyInjection.IKeyed<TKey,TService>
- More.Extensions.DependencyInjection.ServiceProviderExtensions
- More.Extensions.DependencyInjection.KeyedServiceCollectionExtensions

## Example

```c#
public interface IThing
{
    string ToString();
}

public abstract class ThingBase : IThing
{
    protected ThingBase() { }
    public override string ToString() => GetType().Name;
}

public sealed class Thing : ThingBase { }

public sealed class KeyedThing : ThingBase { }

public sealed class Thing1 : ThingBase { }

public sealed class Thing2 : ThingBase { }

public sealed class Thing3 : ThingBase { }

public static class Key
{
    public sealed class Thingies { }
    public sealed class Thing1 { }
    public sealed class Thing2 { }
}

public class CatInTheHat
{
    public CatInTheHat(
        IKeyed<Key.Thing1, IThing> thing1,
        IKeyed<Key.Thing2, IThing> thing2)
    {
        Thing1 = thing1.Value;
        Thing2 = thing2.Value;
    }

    public IThing Thing1 { get; }
    public IThing Thing2 { get; }
}
```

```c#
var services = new ServiceCollection();

// keyed types
services.AddSingleton<Key.Thing1, IThing, Thing1>();
services.AddTransient<Key.Thing2, IThing, Thing2>();

// non-keyed type with keyed type dependencies
services.AddSingleton<CatInTheHat>();

// keyed open generics
services.AddTransient(typeof(IGeneric<>), typeof(Generic<>));
services.AddSingleton(typeof(IKeyed<,>), typeof(KeyedOpenGeneric<,>));

// keyed IEnumerable<T>
services.TryAddEnumerable<Key.Thingies, IThing, Thing1>(ServiceLifetime.Transient);
services.TryAddEnumerable<Key.Thingies, IThing, Thing2>(ServiceLifetime.Transient);
services.TryAddEnumerable<Key.Thingies, IThing, Thing3>(ServiceLifetime.Transient);

var provider = services.BuildServiceProvider();

// resolve non-keyed type with keyed type dependencies
var catInTheHat = provider.GetRequiredService<CatInTheHat>();

// resolve keyed, open generic
var openGeneric = provider.GetRequiredService<Key.Thingy, IGeneric<object>>();

// resolve keyed IEnumerable<T>
var thingies = provider.GetServices<Key.Thingies, IThing>();

// related services such as IServiceProviderIsService
var query = provider.GetRequiredService<IServiceProviderIsService>();
var thing1Registered = query.IsService<Key.Thing1, IThing>();
var thing2Registered = query.IsService(typeof(Key.Thing2), typeof(IThing));
```

## Release Notes

