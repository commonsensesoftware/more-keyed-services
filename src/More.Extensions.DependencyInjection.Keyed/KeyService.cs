// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

/// <summary>
/// Provides utility functions for a keyed service.
/// </summary>
public static class KeyService
{
    /// <summary>
    /// Returns a service with the specified key.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TService">The type of service.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="service">The service instance.</param>
    /// <returns>A new <see cref="IKeyed{TKey, TService}">keyed service</see>.</returns>
    public static IKeyed<TKey, TService> Of<TKey, TService>( TKey key, TService service )
        where TService : notnull => new KeyedOpenGeneric<TKey, TService>( service );
}