// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public class Keyed<TKey, TService> : IKeyed<TKey, TService>
    where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Keyed{TKey, TService}"/> class.
    /// </summary>
    /// <param name="serviceProvider">The associated <see cref="IServiceProvider">service provider</see>.</param>
    public Keyed( IServiceProvider serviceProvider )
    {
        ArgumentNullException.ThrowIfNull( serviceProvider, nameof( serviceProvider ) );
        Value = (TService) serviceProvider.GetRequiredService( Key );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Keyed{TKey, TService}"/> class.
    /// </summary>
    /// <param name="resolve">The function used to resolve the service.</param>
    protected internal Keyed( Func<Type, TService> resolve ) =>
        Value = ( resolve ?? throw new ArgumentNullException( nameof( resolve ) ) )( Key );

    /// <inheritdoc />
    public TService Value { get; }

    object IKeyed.Value => Value;

    private static Type Key => KeyedType.Create<TKey, TService>();
}