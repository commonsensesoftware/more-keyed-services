// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace DryIoc.Microsoft.DependencyInjection;

using global::Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed <see cref="IServiceProviderFactory{TContainerBuilder}"/> for Dry IoC.
/// </summary>
[CLSCompliant( false )]
public sealed class DryIocKeyedServiceProviderFactory : IServiceProviderFactory<IContainer>
{
    private readonly DryIocServiceProviderFactory inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="DryIocKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="container">The underlying container, if any.</param>
    /// <param name="registerDescriptor">The function used to register descriptors.</param>
    public DryIocKeyedServiceProviderFactory(
           IContainer? container = null,
           Func<IRegistrator, ServiceDescriptor, bool>? registerDescriptor = null ) :
           this( container, RegistrySharing.CloneAndDropCache, registerDescriptor )
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DryIocKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="container">The underlying container, if any.</param>
    /// <param name="registrySharing">Indicates the type of registry sharing.</param>
    /// <param name="registerDescriptor">The function used to register descriptors.</param>
    public DryIocKeyedServiceProviderFactory(
        IContainer? container,
        RegistrySharing registrySharing,
        Func<IRegistrator, ServiceDescriptor, bool>? registerDescriptor = null ) =>
        inner = new( container, registrySharing, registerDescriptor );

    /// <inheritdoc />
    public IContainer CreateBuilder( IServiceCollection services )
    {
        var keyedServices = services.RemoveKeyedServices();
        var container = inner.CreateBuilder( services );

        if ( keyedServices.Count > 0 )
        {
            var visitor = new DryIocKeyedServiceVisitor( container );
            visitor.Visit( keyedServices );
        }

        return container;
    }

    /// <inheritdoc />
    public IServiceProvider CreateServiceProvider( IContainer containerBuilder ) => containerBuilder.BuildServiceProvider();
}