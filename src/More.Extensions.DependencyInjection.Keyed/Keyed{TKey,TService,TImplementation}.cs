// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
/// <typeparam name="TImplementation">The type of service implementation.</typeparam>
public class Keyed<TKey, TService, TImplementation> :
    IKeyed<TKey, TService>
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Keyed{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="serviceProvider">The associated <see cref="IServiceProvider">service provider</see>.</param>
    public Keyed( IServiceProvider serviceProvider )
        : this( key => (TImplementation) serviceProvider.GetRequiredService( key ) )
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );
        Value = (TImplementation) serviceProvider.GetRequiredService( Key );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Keyed{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="resolve">The function used to resolve the service.</param>
    protected internal Keyed( Func<Type, TImplementation> resolve ) =>
        Value = ( resolve ?? throw new ArgumentNullException( nameof( resolve ) ))( Key );

    /// <inheritdoc />
    public TService Value { get; }

    object IKeyed.Value => Value;

    private static Type Key => KeyedType.Create<TKey, TImplementation>();
}