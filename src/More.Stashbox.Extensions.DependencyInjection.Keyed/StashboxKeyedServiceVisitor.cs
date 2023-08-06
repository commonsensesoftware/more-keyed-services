// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Stashbox.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection;
using Stashbox.Lifetime;

/// <summary>
/// Represents the Stashbox visitor for keyed services.
/// </summary>
public sealed class StashboxKeyedServiceVisitor : KeyedServiceDescriptorVisitor
{
    private readonly IStashboxContainer container;

    /// <summary>
    /// Initializes a new instance of the <see cref="StashboxKeyedServiceVisitor"/> class.
    /// </summary>
    /// <param name="container">The underlying container.</param>
    [CLSCompliant( false )]
    public StashboxKeyedServiceVisitor( IStashboxContainer container )
        : base( typeof( StashboxKey<,> ), typeof( StashboxKey<,,> ) ) => this.container = container;

    /// <inheritdoc />
    protected override void VisitInterface( ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var lifetime = ToLifetime( serviceDescriptor.Lifetime );

        container.Register(
            serviceDescriptor.ServiceType,
            serviceDescriptor.ImplementationType!,
            configure => configure.WithLifetime( lifetime ) );
    }

    /// <inheritdoc />
    protected override void VisitService( Type key, ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var lifetime = ToLifetime( serviceDescriptor.Lifetime );

        if ( serviceDescriptor.ImplementationType != null )
        {
            container.Register(
                serviceDescriptor.ServiceType,
                serviceDescriptor.ImplementationType,
                configure => configure.WithLifetime( lifetime )
                                      .WithName( key ) );
        }
        else if ( serviceDescriptor.ImplementationFactory is Func<IServiceProvider, object> factory )
        {
            container.Register(
                serviceDescriptor.ServiceType,
                configure => configure.WithLifetime( lifetime )
                                      .WithFactory( factory )
                                      .WithName( key ) );
        }
        else
        {
            var instance = serviceDescriptor.ImplementationInstance;

            container.Register(
                serviceDescriptor.ServiceType,
                configure => configure.WithLifetime( lifetime )
                                      .WithInstance( instance! )
                                      .WithName( key ) );
        }
    }

    private static LifetimeDescriptor ToLifetime( ServiceLifetime lifetime ) =>
        lifetime switch
        {
            ServiceLifetime.Scoped => Lifetimes.Scoped,
            ServiceLifetime.Singleton => Lifetimes.Singleton,
            _ => Lifetimes.Transient,
        };
}