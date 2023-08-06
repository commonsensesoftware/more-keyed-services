// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Microsoft.Extensions.DependencyInjection;
using Unity = Unity.Microsoft.DependencyInjection.ServiceProviderExtensions;

[Trait( "Container", nameof( Unity ) )]
public class UnityTests : ScenarioTests<IServiceProvider>
{
    protected override IServiceProviderFactory<IServiceProvider> Factory { get; } = new UnityServiceProviderFactory();

    private sealed class UnityServiceProviderFactory : IServiceProviderFactory<IServiceProvider>
    {
        public IServiceProvider CreateBuilder( IServiceCollection services ) => Unity.BuildServiceProvider( services );

        public IServiceProvider CreateServiceProvider( IServiceProvider containerBuilder ) => containerBuilder;
    }
}