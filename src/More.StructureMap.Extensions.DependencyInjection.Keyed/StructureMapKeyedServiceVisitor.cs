// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace StructureMap;

using Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection;
using StructureMap.Pipeline;

/// <summary>
/// Represents the Structure Map visitor for keyed services.
/// </summary>
public sealed class StructureMapKeyedServiceVisitor : KeyedServiceDescriptorVisitor
{
    private readonly Registry registry;

    /// <summary>
    /// Initializes a new instance of the <see cref="StructureMapKeyedServiceVisitor"/> class.
    /// </summary>
    /// <param name="registry">The underlying registry.</param>
    public StructureMapKeyedServiceVisitor( Registry registry )
        : base( typeof( StructureMapKey<,> ), typeof( StructureMapKey<,,> ) ) => this.registry = registry;

    /// <inheritdoc />
    protected override void VisitInterface( ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        registry.For( serviceDescriptor.ServiceType )
                .LifecycleIs( ToLifecycle( serviceDescriptor.Lifetime ) )
                .Use( serviceDescriptor.ImplementationType );
    }

    /// <inheritdoc />
    protected override void VisitService( Type key, ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( key, nameof( key ) );
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var name = key.ToString();
        var lifecycle = ToLifecycle( serviceDescriptor.Lifetime );

        if ( serviceDescriptor.ImplementationType != null )
        {
            registry.For( serviceDescriptor.ServiceType )
                    .LifecycleIs( lifecycle )
                    .Use( serviceDescriptor.ImplementationType )
                    .Named( name );
        }
        else if ( serviceDescriptor.ImplementationFactory is Func<IServiceProvider, object> factory )
        {
            registry.For( serviceDescriptor.ServiceType )
                    .LifecycleIs( lifecycle )
                    .Use( context => factory( context.GetInstance<IServiceProvider>() ) )
                    .Named( name );
        }
        else
        {
            registry.For( serviceDescriptor.ServiceType )
                    .LifecycleIs( lifecycle )
                    .Use( serviceDescriptor.ImplementationInstance )
                    .Named( name );
        }
    }

    private static ILifecycle ToLifecycle( ServiceLifetime lifetime ) =>
        lifetime switch
        {
            ServiceLifetime.Scoped => Lifecycles.Container,
            ServiceLifetime.Singleton => Lifecycles.Singleton,
            _ => Lifecycles.Unique,
        };
}