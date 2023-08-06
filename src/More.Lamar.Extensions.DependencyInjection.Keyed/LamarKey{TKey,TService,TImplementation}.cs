// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Lamar.Microsoft.DependencyInjection;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Lamar keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
/// <typeparam name="TImplementation">The type of service implementation.</typeparam>
public sealed class LamarKey<TKey, TService, TImplementation> :
    StringKey<TKey, TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LamarKey{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="context">The context used to resolve the service.</param>
    [CLSCompliant( false )]
    public LamarKey( IServiceContext context )
        : base( ( context ?? throw new ArgumentNullException( nameof( context ) ) ).GetInstance<TImplementation> ) { }
}