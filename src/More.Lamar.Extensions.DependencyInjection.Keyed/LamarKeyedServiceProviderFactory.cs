// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Lamar.Microsoft.DependencyInjection;

using global::Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed <see cref="IServiceProviderFactory{TContainerBuilder}"/> for Lamar.
/// </summary>
[CLSCompliant( false )]
public sealed class LamarKeyedServiceProviderFactory :
    IServiceProviderFactory<ServiceRegistry>,
    IServiceProviderFactory<IServiceCollection>
{
    private readonly LamarServiceProviderFactory inner = new();

    /// <inheritdoc />
    public IServiceProvider CreateServiceProvider( ServiceRegistry containerBuilder ) =>
        inner.CreateServiceProvider( containerBuilder );

    /// <inheritdoc />
    public ServiceRegistry CreateBuilder( IServiceCollection services )
    {
        var keyedServices = services.RemoveKeyedServices();
        var registry = new ServiceRegistry();

        if ( keyedServices.Count > 0 )
        {
            var visitor = new LamarKeyedServiceVisitor( registry );
            visitor.Visit( keyedServices );
        }

        registry.AddRange( inner.CreateBuilder( services ) );

        return registry;
    }

    IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder( IServiceCollection services ) =>
        CreateBuilder( services );

    /// <inheritdoc />
    public IServiceProvider CreateServiceProvider( IServiceCollection containerBuilder ) =>
        inner.CreateServiceProvider( containerBuilder );
}