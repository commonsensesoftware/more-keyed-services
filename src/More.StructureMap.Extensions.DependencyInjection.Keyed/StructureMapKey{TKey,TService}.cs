// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace StructureMap;

using More.Extensions.DependencyInjection;

/// <summary>
/// Represents a Structure Map keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public sealed class StructureMapKey<TKey, TService> : StringKey<TKey, TService> where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StructureMapKey{TKey, TService}"/> class.
    /// </summary>
    /// <param name="container">The container used to resolve the service.</param>
    public StructureMapKey( IContainer container )
        : base( ( container ?? throw new ArgumentNullException( nameof( container ) ) ).GetInstance<TService> ) { }
}