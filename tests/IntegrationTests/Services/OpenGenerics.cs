// Copyright (c) Commonsense Software and contributors. All rights reserved.

#pragma warning disable CA1040 // Avoid empty interfaces
#pragma warning disable CA1724 // Avoid type name conflicts
#pragma warning disable SA1649 // File name should match first type name

namespace More.Extensions.DependencyInjection.Services;

public interface IGeneric<T> { }

public class Generic<T> : IGeneric<T> { }