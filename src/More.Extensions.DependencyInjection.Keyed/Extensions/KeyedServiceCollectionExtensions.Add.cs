// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection.Extensions;
using More.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides extensions methods for <see cref="IServiceCollection"/>.
/// </summary>
public static partial class KeyedServiceCollectionExtensions
{
    private static Type? keyed2;
    private static Type? keyed3;

    /// <summary>
    /// Adds a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddSingleton<TKey, TService, TImplementation>( this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddSingleton( KeyedType.Create<TKey, TService>(), typeof( TImplementation ) );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="instance">The existing service instance.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddSingleton<TKey, TService, TImplementation>(
        this IServiceCollection services,
        TImplementation instance )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddSingleton( KeyedType.Create<TKey, TService>(), instance );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="factory">The factory function use to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddSingleton<TKey, TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> factory )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddSingleton( KeyedType.Create<TKey, TService>(), factory );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddSingleton(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType )
    {
        services.AddSingleton( KeyedType.Create( keyType, serviceType ), implementationType );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Adds a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementation">The existing service instance.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddSingleton(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        object implementation )
    {
        services.AddSingleton( KeyedType.Create( keyType, serviceType ), implementation );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Adds a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddSingleton(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Func<IServiceProvider, object> factory )
    {
        services.AddSingleton( KeyedType.Create( keyType, serviceType ), factory );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddSingleton<TKey, TService, TImplementation>( this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAddSingleton( KeyedType.Create<TKey, TService>(), typeof( TImplementation ) );
        services.TryAddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Attempts to add a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="factory">The factory function use to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddSingleton<TKey, TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> factory )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAddSingleton( KeyedType.Create<TKey, TService>(), factory );
        services.TryAddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Attempts to add a singleton service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddSingleton(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType )
    {
        services.TryAddSingleton( KeyedType.Create( keyType, serviceType ), implementationType );
        services.TryAddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Adds a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddTransient<TKey, TService, TImplementation>( this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddTransient( KeyedType.Create<TKey, TService>(), typeof( TImplementation ) );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddTransient<TKey, TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> factory )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddTransient( KeyedType.Create<TKey, TService>(), factory );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddTransient(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType )
    {
        services.AddTransient( KeyedType.Create( keyType, serviceType ), implementationType );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Adds a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddTransient(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Func<IServiceProvider, object> factory )
    {
        services.AddTransient( KeyedType.Create( keyType, serviceType ), factory );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddTransient<TKey, TService, TImplementation>( this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAddTransient( KeyedType.Create<TKey, TService>(), typeof( TImplementation ) );
        services.TryAddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Attempts to add a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddTransient<TKey, TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> factory )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAddTransient( KeyedType.Create<TKey, TService>(), factory );
        services.TryAddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Attempts to add a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddTransient(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType )
    {
        services.TryAddTransient( KeyedType.Create( keyType, serviceType ), implementationType );
        services.TryAddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a transient service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddTransient(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Func<IServiceProvider, object> factory )
    {
        services.TryAddTransient( KeyedType.Create( keyType, serviceType ), factory );
        services.TryAddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Adds a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddScoped<TKey, TService, TImplementation>( this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddScoped( KeyedType.Create<TKey, TService>(), typeof( TImplementation ) );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddScoped<TKey, TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> factory )
        where TService : class
        where TImplementation : class, TService
    {
        services.AddScoped( KeyedType.Create<TKey, TService>(), factory );
        services.AddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Adds a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddScoped(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType )
    {
        services.AddScoped( KeyedType.Create( keyType, serviceType ), implementationType );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Adds a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddScoped(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Func<IServiceProvider, object> factory )
    {
        services.AddScoped( KeyedType.Create( keyType, serviceType ), factory );
        services.AddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddScoped<TKey, TService, TImplementation>( this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAddScoped( KeyedType.Create<TKey, TService>(), typeof( TImplementation ) );
        services.TryAddScoped<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Attempts to add a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddScoped<TKey, TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> factory )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAddScoped( KeyedType.Create<TKey, TService>(), factory );
        services.TryAddTransient<IKeyed<TKey, TService>, Keyed<TKey, TService>>();
        return services;
    }

    /// <summary>
    /// Attempts to add a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddScoped(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType )
    {
        services.TryAddScoped( KeyedType.Create( keyType, serviceType ), implementationType );
        services.TryAddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="factory">The factory function used to create the service.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddScoped(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Func<IServiceProvider, object> factory )
    {
        services.TryAddScoped( KeyedType.Create( keyType, serviceType ), factory );
        services.TryAddTransient( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a service with the specified key, implementation, and lifetime to the service collection.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <typeparam name="TImplementation">The service implementation type.</typeparam>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime">lifetime</see> of the service to add.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddEnumerable<TKey, TService, TImplementation>( this IServiceCollection services, ServiceLifetime lifetime )
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAdd( new ServiceDescriptor( KeyedType.Create<TKey, TImplementation>(), typeof( TImplementation ), lifetime ) );
        services.TryAddEnumerable( new ServiceDescriptor( typeof( IKeyed<TKey, TService> ), typeof( Keyed<TKey, TService, TImplementation> ), ServiceLifetime.Transient ) );
        return services;
    }

    /// <summary>
    /// Attempts to add a scoped service with the specified key and implementation to the service collection.
    /// </summary>
    /// <param name="services">The extended <see cref="IServiceCollection">service collection</see>.</param>
    /// <param name="keyType">The type of key.</param>
    /// <param name="serviceType">The type of service.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime">lifetime</see> of the service to add.</param>
    /// <returns>The original <paramref name="services"/> instance.</returns>
    public static IServiceCollection TryAddEnumerable(
        this IServiceCollection services,
        Type keyType,
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime )
    {
        services.TryAdd( new ServiceDescriptor( KeyedType.Create( keyType, implementationType ), implementationType, lifetime ) );
        services.TryAddEnumerable( new ServiceDescriptor( IKeyed( keyType, serviceType ), Keyed( keyType, serviceType, implementationType ), ServiceLifetime.Transient ) );
        return services;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static Type IKeyed( Type keyType, Type serviceType ) => IKeyedType.Make( keyType, serviceType );

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static Type Keyed( Type keyType, Type serviceType ) =>
        ( keyed2 ??= typeof( Keyed<,> ) ).MakeGenericType( keyType, serviceType );

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static Type Keyed( Type keyType, Type serviceType, Type implementationType ) =>
        ( keyed3 ??= typeof( Keyed<,,> ) ).MakeGenericType( keyType, serviceType, implementationType );
}