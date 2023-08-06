// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

/// <summary>
/// Represents a service keyed by a string.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
/// <typeparam name="TImplementation">The type of service implementation.</typeparam>
public abstract class StringKey<TKey, TService, TImplementation> :
    IKeyed<TKey, TService>
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringKey{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="generateKey">The function used to generate the service key.</param>
    /// <param name="resolve">The function used to resolve the service.</param>
    [CLSCompliant( false )]
    protected unsafe StringKey(
        delegate*< Type, string > generateKey,
        Func<string, TImplementation> resolve )
    {
        ArgumentNullException.ThrowIfNull( resolve, nameof( resolve ) );
        Value = resolve( generateKey( KeyedType.Create<TKey, TImplementation>() ) );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringKey{TKey, TService, TImplementation}"/> class.
    /// </summary>
    /// <param name="resolve">The function used to resolve the service.</param>
    protected unsafe StringKey( Func<string, TImplementation> resolve )
        : this( &GenerateKey, resolve ) { }

    /// <inheritdoc />
    public TService Value { get; }

    object IKeyed.Value => Value;

    private static string GenerateKey( Type type ) => type.ToString();
}