// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Grace.DependencyInjection.Extensions;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Grace keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
/// <typeparam name="TImplementation">The type of service implementation.</typeparam>
public sealed class GraceKey<TKey, TService, TImplementation> :
    Keyed<TKey, TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraceKey{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="locator">The locator used to resolve the service.</param>
    [CLSCompliant( false )]
    public GraceKey( ILocatorService locator )
        : base( key => locator.Locate<TImplementation>( withKey: key ) ) { }
}