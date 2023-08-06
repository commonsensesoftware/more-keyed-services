// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Grace.DependencyInjection.Extensions;

using Microsoft.Extensions.DependencyInjection;

// HACK: forked because these are internal to Grace
internal static class GraceExtensions
{
    internal static IFluentExportStrategyConfiguration WithLifetime( this IFluentExportStrategyConfiguration configuration, ServiceLifetime lifetime ) =>
        lifetime switch
        {
            ServiceLifetime.Scoped => configuration.Lifestyle.SingletonPerScope(),
            ServiceLifetime.Singleton => configuration.Lifestyle.Singleton(),
            _ => configuration,
        };

    internal static IFluentExportInstanceConfiguration<T> WithLifetime<T>( this IFluentExportInstanceConfiguration<T> configuration, ServiceLifetime lifetime ) =>
        lifetime switch
        {
            ServiceLifetime.Scoped => configuration.Lifestyle.SingletonPerScope(),
            ServiceLifetime.Singleton => configuration.Lifestyle.Singleton(),
            _ => configuration,
        };
}