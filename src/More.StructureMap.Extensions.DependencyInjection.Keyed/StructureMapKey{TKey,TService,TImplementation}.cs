// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace StructureMap;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Structure Map keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
/// <typeparam name="TImplementation">The type of service implementation.</typeparam>
public sealed class StructureMapKey<TKey, TService, TImplementation> :
    StringKey<TKey, TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StructureMapKey{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="container">The container used to resolve the service.</param>
    public StructureMapKey( IContainer container )
        : base( ( container ?? throw new ArgumentNullException( nameof( container ) ) ).GetInstance<TImplementation> ) { }
}