// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Microsoft.Extensions.DependencyInjection;
using StructureMap;

[Trait("Container", nameof(StructureMap))]
public class StructureMapTests : ScenarioTests<Registry>
{
    protected override IServiceProviderFactory<Registry> Factory { get; } = new StructureMapKeyedServiceProviderFactory();
}