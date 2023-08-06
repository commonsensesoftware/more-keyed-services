// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Stashbox.Extensions.DependencyInjection;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Stashbox keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public sealed class StashboxKey<TKey, TService> : Keyed<TKey, TService> where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StashboxKey{TKey, TService}"/> class.
    /// </summary>
    /// <param name="resolver">The resolver used to resolve the service.</param>
    [CLSCompliant( false )]
    public StashboxKey( IDependencyResolver resolver )
        : base( name => resolver.Resolve<TService>( name ) ) { }
}