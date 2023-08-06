// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection.Containers;

using Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection.Services;
using Stashbox;
using Stashbox.Extensions.DependencyInjection;

[Trait( "Container", nameof( Stashbox ) )]
public class StashboxTests : ScenarioTests<IStashboxContainer>
{
    [Fact]
    public void resolve_IEnumerableX3CTX3E_without_key_should_return_expected_services()
    {
        // arrange
        using var container = new StashboxContainer();
        var expected = new[] { typeof( Thing1 ), typeof( Thing2 ), typeof( Thing3 ) };

        container.Register<IThing, Thing1>();
        container.Register<IThing, Thing2>();
        container.Register<IThing, Thing3>();

        // act
        var thingies = container.Resolve<IEnumerable<IThing>>();

        // assert
        thingies.Select( t => t.GetType() ).Should().BeEquivalentTo( expected );
    }

    [Fact]
    public void resolve_IEnumerableX3CTX3E_with_key_should_return_expected_services()
    {
        // arrange
        using var container = new StashboxContainer();
        var name = KeyedType.Create<Key.Thingies, IThing>().ToString();
        var expected = new[] { typeof( Thing1 ), typeof( Thing2 ), typeof( Thing3 ) };

        container.Register<IThing, Thing1>( name );
        container.Register<IThing, Thing2>( name );
        container.Register<IThing, Thing3>( name );

        // act
        var thingies = container.Resolve<IEnumerable<IThing>>( name );

        // assert
        thingies.Select( t => t.GetType() ).Should().BeEquivalentTo( expected );
    }

    protected override IServiceProviderFactory<IStashboxContainer> Factory { get; } = new StashboxKeyedServiceProviderFactory();
}