// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Microsoft.Extensions.DependencyInjection;

using More.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions methods for <see cref="IServiceCollection"/>.
/// </summary>
public static partial class KeyedServiceCollectionExtensions
{
    /// <summary>
    /// Removes keyed services from the specified service collection into a dictionary of keyed services, if any.
    /// </summary>
    /// <param name="services">The service collection to remove keyed services from.</param>
    /// <returns>A read-only list containing of services mapped by key.</returns>
    public static IReadOnlyList<KeyValuePair<Type, IServiceCollection>> RemoveKeyedServices( this IServiceCollection services ) =>
        services.RemoveKeyedServices( static () => new ServiceCollection() );

    /// <summary>
    /// Removes keyed services from the specified service collection into a dictionary of keyed services, if any.
    /// </summary>
    /// <param name="services">The service collection to remove keyed services from.</param>
    /// <param name="newServiceCollection">The factory function used to create new service collections.</param>
    /// <returns>A read-only list containing of services mapped by key.</returns>
    public static IReadOnlyList<KeyValuePair<Type, IServiceCollection>> RemoveKeyedServices(
        this IServiceCollection services,
        Func<IServiceCollection> newServiceCollection )
    {
        ArgumentNullException.ThrowIfNull( services, nameof( services ) );
        ArgumentNullException.ThrowIfNull( newServiceCollection, nameof( newServiceCollection ) );

        var removed = default( Dictionary<Type, IServiceCollection> );

        // remove and bucketize all services where ServiceDescriptor.ServicType is from:
        // 1. KeyedType.Create
        // 2. IKeyed<TKey,TService>
        for ( var i = services.Count - 1; i >= 0; i-- )
        {
            var service = services[i];
            var key = service.ServiceType;

            if ( KeyedType.TryDeconstruct( key, out _, out var serviceType ) )
            {
                service = CloneWithoutKey( service, serviceType );
            }
            else if ( IKeyedType.IsImplementedBy( key ) )
            {
                var args = key.GenericTypeArguments;
                key = KeyedType.Create( args[0], args[0] );
            }
            else
            {
                continue;
            }

            removed ??= new();

            if ( !removed.TryGetValue( key, out var keyedServices ) )
            {
                removed.Add( key, keyedServices = newServiceCollection() );
            }

            keyedServices.Add( service );
            services.RemoveAt( i );
        }

        if ( removed is null )
        {
            return Array.Empty<KeyValuePair<Type, IServiceCollection>>();
        }

        return removed.ToArray();
    }

    private static ServiceDescriptor CloneWithoutKey( ServiceDescriptor service, Type serviceType ) =>
        service switch
        {
            { ImplementationFactory: var f, Lifetime: var l } when f != null => new( serviceType, f, l ),
            { ImplementationInstance: var i } when i != null => new( serviceType, i ),
            { ImplementationType: var t, Lifetime: var l } when t != null => new( serviceType, t, l ),
            _ => throw new NotSupportedException( "ServiceDescriptor could not be cloned." ),
        };
}