// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Lamar.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[Trait( "Container", nameof( Lamar ) )]
public class LamarTests : ScenarioTests<IServiceCollection>
{
    protected override IServiceProviderFactory<IServiceCollection> Factory { get; } = new LamarKeyedServiceProviderFactory();
}