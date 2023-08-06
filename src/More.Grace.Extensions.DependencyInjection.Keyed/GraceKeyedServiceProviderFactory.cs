// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Grace.DependencyInjection.Extensions;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents a keyed <see cref="IServiceProviderFactory{TContainerBuilder}"/> for Grace.
/// </summary>
[CLSCompliant( false )]
public sealed class GraceKeyedServiceProviderFactory : IServiceProviderFactory<IServiceProvider>, IDisposable
{
    private readonly DependencyInjectionContainer container;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraceKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="configuration">The function used to configure the underlying container.</param>
    public GraceKeyedServiceProviderFactory( Action<InjectionScopeConfiguration>? configuration = null ) =>
        container = new( configuration );

    /// <summary>
    /// Initializes a new instance of the <see cref="GraceKeyedServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="configuration">The function used to configure the underlying container.</param>
    public GraceKeyedServiceProviderFactory( IInjectionScopeConfiguration configuration ) => container = new( configuration );

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        container.Dispose();
    }

    /// <inheritdoc />
    public IServiceProvider CreateBuilder( IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services, nameof( services ) );

        // note: we have to detect and remap ServiceDescriptor.ImplementationFactory;
        // otherwise we could have just used:
        //
        // return new DependencyInjectionContainer().Populate(services);
        var original = new ServiceCollection();

        for ( var i = 0; i < services.Count; i++ )
        {
            original.Insert( i, services[i] );
        }

        var keyedServices = services.RemoveKeyedServices();

        if ( keyedServices.Count == 0 || RemapNotRequired( keyedServices ) )
        {
            return container.Populate( original );
        }

        var visitor = new GraceKeyedServiceVisitor( container );

        visitor.Visit( keyedServices );

        return container.Populate( services );
    }

    /// <inheritdoc />
    public IServiceProvider CreateServiceProvider( IServiceProvider containerBuilder ) => containerBuilder;

    private static bool RemapNotRequired( IReadOnlyList<KeyValuePair<Type, IServiceCollection>> keyedServices )
    {
        for ( var i = 0; i < keyedServices.Count; i++ )
        {
            var services = keyedServices[i].Value;

            for ( var j = 0; j < services.Count; j++ )
            {
                if ( services[j].ImplementationFactory is not null )
                {
                    return false;
                }
            }
        }

        return true;
    }
}