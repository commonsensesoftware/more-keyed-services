// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace StructureMap;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed <see cref="IServiceProviderFactory{TContainerBuilder}"/> for Structure Map.
/// </summary>
public sealed class StructureMapKeyedServiceProviderFactory : IServiceProviderFactory<Registry>
{
    private readonly StructureMapServiceProviderFactory inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="StructureMapKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="registry">The registry used by the factory.</param>
    public StructureMapKeyedServiceProviderFactory( Registry? registry = null ) => inner = new( registry );

    /// <inheritdoc />
    public Registry CreateBuilder( IServiceCollection services )
    {
        var keyedServices = services.RemoveKeyedServices();
        var registry = new Registry();

        if ( keyedServices.Count > 0 )
        {
            var visitor = new StructureMapKeyedServiceVisitor( registry );
            visitor.Visit( keyedServices );
        }

        registry.IncludeRegistry( inner.CreateBuilder( services ) );

        return registry;
    }

    /// <inheritdoc />
    public IServiceProvider CreateServiceProvider( Registry containerBuilder ) =>
        inner.CreateServiceProvider( containerBuilder );
}