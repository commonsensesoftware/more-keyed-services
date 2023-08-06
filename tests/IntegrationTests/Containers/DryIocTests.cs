// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[Trait( "Container", nameof( DryIoc ) )]
public class DryIocTests : ScenarioTests<IContainer>
{
    protected override IServiceProviderFactory<IContainer> Factory { get; } = new DryIocKeyedServiceProviderFactory();
}