// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Microsoft.Extensions.DependencyInjection;

using More.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides extension methods for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets a service of the specific type and key.
    /// </summary>
    /// <param name="serviceProvider">The extended <see cref="IServiceProvider">service provider</see>.</param>
    /// <param name="serviceType">The type of service to retrieve.</param>
    /// <param name="key">The service key.</param>
    /// <returns>The matching service instance or <c>null</c>.</returns>
    public static object? GetService( this IServiceProvider serviceProvider, Type serviceType, Type key )
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );
        ArgumentNullException.ThrowIfNull( serviceType, nameof( serviceType ) );
        ArgumentNullException.ThrowIfNull( key, nameof( key ) );

        return ( (IKeyed?) serviceProvider.GetService( IKeyedType.Make( key, serviceType ) ) )?.Value;
    }

    /// <summary>
    /// Gets a required service of the specific type and key.
    /// </summary>
    /// <param name="serviceProvider">The extended <see cref="IServiceProvider">service provider</see>.</param>
    /// <param name="serviceType">The type of service to retrieve.</param>
    /// <param name="key">The service key.</param>
    /// <returns>The matching service instance.</returns>
    public static object GetRequiredService( this IServiceProvider serviceProvider, Type serviceType, Type key )
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );
        ArgumentNullException.ThrowIfNull( serviceType, nameof( serviceType ) );
        ArgumentNullException.ThrowIfNull( key, nameof( key ) );

        return serviceProvider.GetService( serviceType, key ) ?? throw NoSuchService( key, serviceType );
    }

    /// <summary>
    /// Gets a sequence of services of the specific type and key.
    /// </summary>
    /// <param name="serviceProvider">The extended <see cref="IServiceProvider">service provider</see>.</param>
    /// <param name="serviceType">The type of service to retrieve.</param>
    /// <param name="key">The service key.</param>
    /// <returns>A <see cref="IEnumerable{T}">sequence</see> of matching services.</returns>
    public static IEnumerable<object?> GetServices( this IServiceProvider serviceProvider, Type serviceType, Type key )
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );
        ArgumentNullException.ThrowIfNull( serviceType, nameof( serviceType ) );
        ArgumentNullException.ThrowIfNull( key, nameof( key ) );

        foreach ( var dependency in serviceProvider.GetServices( IKeyedType.Make( key, serviceType ) ).Cast<IKeyed?>() )
        {
            yield return dependency?.Value;
        }
    }

    /// <summary>
    /// Gets a service of the specific type and key.
    /// </summary>
    /// <typeparam name="TKey">The service key.</typeparam>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <param name="serviceProvider">The extended <see cref="IServiceProvider">service provider</see>.</param>
    /// <returns>The matching service instance of type <typeparamref name="TService"/> or <c>null</c>.</returns>
    public static TService? GetService<TKey, TService>( this IServiceProvider serviceProvider ) where TService : notnull
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );

        var dependency = serviceProvider.GetService<IKeyed<TKey, TService>>();
        return dependency is null ? default : dependency.Value;
    }

    /// <summary>
    /// Gets a required service of the specific type and key.
    /// </summary>
    /// <typeparam name="TKey">The service key.</typeparam>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <param name="serviceProvider">The extended <see cref="IServiceProvider">service provider</see>.</param>
    /// <returns>The matching service instance of type <typeparamref name="TService"/>.</returns>
    public static TService GetRequiredService<TKey, TService>( this IServiceProvider serviceProvider ) where TService : notnull
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );

        return ( serviceProvider.GetService<IKeyed<TKey, TService>>() ?? throw NoSuchService( typeof( TKey ), typeof( TService ) ) ).Value;
    }

    /// <summary>
    /// Gets a sequence of services of the specific type and key.
    /// </summary>
    /// <typeparam name="TKey">The service key.</typeparam>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <param name="serviceProvider">The extended <see cref="IServiceProvider">service provider</see>.</param>
    /// <returns>A <see cref="IEnumerable{T}">sequence</see> of matching services of type <typeparamref name="TService"/>.</returns>
    public static IEnumerable<TService> GetServices<TKey, TService>( this IServiceProvider serviceProvider ) where TService : notnull
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );

        foreach ( var dependency in serviceProvider.GetServices<IKeyed<TKey, TService>>() )
        {
            yield return dependency.Value;
        }
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static NotSupportedException NoSuchService( Type key, Type serviceType ) =>
        new( $"No service of type {serviceType.Name} with key {key.Name} could be found." );
}