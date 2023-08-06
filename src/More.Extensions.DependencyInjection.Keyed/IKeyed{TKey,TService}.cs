// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

/// <summary>
/// Defines the behavior of a keyed service.
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TService">The type of dependency.</typeparam>
public interface IKeyed<in TKey, out TService> :
    IKeyed
    where TService : notnull
{
    /// <summary>
    /// Gets the value of the resolved, keyed service.
    /// </summary>
    /// <value>The resolved, keyed service value.</value>
    new TService Value { get; }
}