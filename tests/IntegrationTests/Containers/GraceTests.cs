// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Grace.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

[Trait( "Container", nameof( Grace ) )]
public class GraceTests : ScenarioTests<IServiceProvider>
{
    protected override IServiceProviderFactory<IServiceProvider> Factory { get; } = new GraceKeyedServiceProviderFactory();
}