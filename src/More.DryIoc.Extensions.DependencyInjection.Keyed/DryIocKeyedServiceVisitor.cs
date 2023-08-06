// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace DryIoc.Microsoft.DependencyInjection;

using global::Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection;

/// <summary>
/// Represents the Dry IoC visitor for keyed services.
/// </summary>
public sealed class DryIocKeyedServiceVisitor : KeyedServiceDescriptorVisitor
{
    private readonly IContainer container;

    /// <summary>
    /// Initializes a new instance of the <see cref="DryIocKeyedServiceVisitor"/> class.
    /// </summary>
    /// <param name="container">The underlying container.</param>
    [CLSCompliant( false )]
    public DryIocKeyedServiceVisitor( IContainer container )
        : base( typeof( DryIocKey<,> ), typeof( DryIocKey<,,> ) ) => this.container = container;

    /// <inheritdoc />
    protected override void VisitInterface( ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var reuse = ToReuse( serviceDescriptor.Lifetime );

        container.Register(
            serviceDescriptor.ServiceType,
            serviceDescriptor.ImplementationType,
            reuse );
    }

    /// <inheritdoc />
    protected override void VisitService( Type key, ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var reuse = ToReuse( serviceDescriptor.Lifetime );

        // equivalent to RegisterDescriptor, but with a key
        if ( serviceDescriptor.ImplementationType != null )
        {
            container.Register(
                serviceDescriptor.ServiceType,
                serviceDescriptor.ImplementationType,
                reuse,
                serviceKey: key );
        }
        else if ( serviceDescriptor.ImplementationFactory != null )
        {
            container.RegisterDelegate(
                serviceDescriptor.ServiceType,
                serviceDescriptor.ImplementationFactory,
                reuse,
                serviceKey: key );
        }
        else
        {
            container.RegisterInstance(
                true,
                serviceDescriptor.ServiceType,
                serviceDescriptor.ImplementationInstance,
                serviceKey: key );
        }
    }

    private static IReuse ToReuse( ServiceLifetime lifetime ) =>
        lifetime switch
        {
            ServiceLifetime.Scoped => Reuse.ScopedOrSingleton,
            ServiceLifetime.Singleton => Reuse.Singleton,
            _ => Reuse.Transient,
        };
}