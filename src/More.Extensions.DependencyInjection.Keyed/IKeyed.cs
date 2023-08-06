// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

/// <summary>
/// Defines the behavior of a keyed service.
/// </summary>
public interface IKeyed
{
    /// <summary>
    /// Gets the value of the resolved, keyed service.
    /// </summary>
    /// <value>The resolved, keyed service value.</value>
    object Value { get; }
}