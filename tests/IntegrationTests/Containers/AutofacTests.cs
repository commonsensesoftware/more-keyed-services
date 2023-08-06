// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[Trait( "Container", nameof( Autofac ) )]
public class AutofacTests : ScenarioTests<ContainerBuilder>
{
    protected override IServiceProviderFactory<ContainerBuilder> Factory { get; } = new AutofacServiceProviderFactory();
}