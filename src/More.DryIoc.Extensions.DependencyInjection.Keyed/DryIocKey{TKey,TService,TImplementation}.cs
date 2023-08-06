// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace DryIoc.Microsoft.DependencyInjection;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Dry IoC keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
/// <typeparam name="TImplementation">The type of service implementation.</typeparam>
public sealed class DryIocKey<TKey, TService, TImplementation> :
    Keyed<TKey, TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DryIocKey{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="container">The container used to resolve the service.</param>
    [CLSCompliant( false )]
    public DryIocKey( IContainer container )
        : base( key => container.Resolve<TImplementation>( serviceKey: key ) ) { }
}