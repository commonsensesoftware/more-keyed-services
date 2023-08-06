// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection.Services;

[Trait( "Container", "Default" )]
public class DefaultTests : ScenarioTests<IServiceCollection>
{
    [Theory]
    [InlineData( typeof( IKeyed<Key.Thing1, IThing> ), true )]
    [InlineData( typeof( IKeyed<Key.Thing2, IThing> ), true )]
    [InlineData( typeof( IKeyed<Key.Thingy, IThing> ), false )]
    public void is_service_should_return_expected_result( Type serviceType, bool expected )
    {
        // arrange
        var services = new ServiceCollection();

        services.AddSingleton<Key.Thing1, IThing, Thing1>();
        services.AddTransient<Key.Thing2, IThing, Thing2>();

        var provider = NewServiceProvider( services );

        // act
        var query = provider.GetRequiredService<IServiceProviderIsService>();

        // assert
        query.IsService( serviceType ).Should().Be( expected );
    }

    protected override IServiceProviderFactory<IServiceCollection> Factory { get; } = new DefaultServiceProviderFactory();
}