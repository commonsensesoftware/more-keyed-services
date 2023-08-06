// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[Trait( "Container", nameof( LightInject ) )]
public class LightInjectTests : ScenarioTests<IServiceContainer>
{
    protected override IServiceProviderFactory<IServiceContainer> Factory { get; } = new LightInjectServiceProviderFactory();
}