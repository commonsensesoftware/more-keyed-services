// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Lamar.Microsoft.DependencyInjection;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Lamar keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public sealed class LamarKey<TKey, TService> : StringKey<TKey, TService> where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LamarKey{TKey, TService}"/> class.
    /// </summary>
    /// <param name="context">The container used to resolve the service.</param>
    [CLSCompliant( false )]
    public LamarKey( IServiceContext context )
        : base( ( context ?? throw new ArgumentNullException( nameof( context ) ) ).GetInstance<TService> ) { }
}