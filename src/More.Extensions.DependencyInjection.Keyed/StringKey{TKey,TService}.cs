// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

/// <summary>
/// Represents a service keyed by a string.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public abstract class StringKey<TKey, TService> : IKeyed<TKey, TService>
    where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringKey{TKey, TService}"/> class.
    /// </summary>
    /// <param name="generateKey">The function used to generate the service key.</param>
    /// <param name="resolve">The function used to resolve the service.</param>
    [CLSCompliant( false )]
    protected unsafe StringKey(
            delegate*< Type, string > generateKey,
            Func<string, TService> resolve )
    {
        ArgumentNullException.ThrowIfNull( resolve, nameof( resolve ) );
        Value = resolve( generateKey( KeyedType.Create<TKey, TService>() ) );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringKey{TKey, TService}"/> class.
    /// </summary>
    /// <param name="resolve">The function used to resolve the service.</param>
    protected unsafe StringKey( Func<string, TService> resolve )
        : this( &GenerateKey, resolve ) { }

    /// <inheritdoc />
    public TService Value { get; }

    object IKeyed.Value => Value;

    private static string GenerateKey( Type type ) => type.ToString();
}