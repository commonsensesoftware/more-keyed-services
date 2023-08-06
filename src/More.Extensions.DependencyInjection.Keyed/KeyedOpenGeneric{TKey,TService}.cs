// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

/// <summary>
/// Represents a open generic, keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of service.</typeparam>
public sealed class KeyedOpenGeneric<TKey, TService> : IKeyed<TKey, TService>
    where TService : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyedOpenGeneric{TKey, TService}"/> class.
    /// </summary>
    /// <param name="value">A open generic, keyed service.</param>
    public KeyedOpenGeneric( TService value ) =>
        Value = value ?? throw new ArgumentNullException( nameof( value ) );

    /// <inheritdoc />
    public TService Value { get; }

    object IKeyed.Value => Value;
}