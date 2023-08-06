// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace DryIoc.Microsoft.DependencyInjection;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Dry IoC keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public sealed class DryIocKey<TKey, TService> : Keyed<TKey, TService> where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DryIocKey{TKey, TService}"/> class.
    /// </summary>
    /// <param name="container">The container used to resolve the service.</param>
    [CLSCompliant( false )]
    public DryIocKey( IContainer container )
        : base( key => container.Resolve<TService>( serviceKey: key ) ) { }
}