// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Stashbox.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed <see cref="IServiceProviderFactory{TContainerBuilder}"/> for Stashbox.
/// </summary>
[CLSCompliant( false )]
public sealed class StashboxKeyedServiceProviderFactory : IServiceProviderFactory<IStashboxContainer>
{
    private readonly StashboxServiceProviderFactory inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="StashboxKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="configure">The function used to configure the container.</param>
    public StashboxKeyedServiceProviderFactory( Action<IStashboxContainer>? configure = null ) => inner = new( configure );

    /// <summary>
    /// Initializes a new instance of the <see cref="StashboxKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="container">The underlying container.</param>
    public StashboxKeyedServiceProviderFactory( IStashboxContainer container ) => inner = new( container );

    /// <inheritdoc />
    public IStashboxContainer CreateBuilder( IServiceCollection services )
    {
        var keyedServices = services.RemoveKeyedServices();

        if ( keyedServices.Count == 0 )
        {
            return inner.CreateBuilder( services );
        }

        return services.CreateBuilder(
            container =>
            {
                var visitor = new StashboxKeyedServiceVisitor( container );
                visitor.Visit( keyedServices );
            } );
    }

    /// <inheritdoc />
    public IServiceProvider CreateServiceProvider( IStashboxContainer containerBuilder ) =>
        inner.CreateServiceProvider( containerBuilder );
}