// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Lamar.Microsoft.DependencyInjection;

using global::Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection;
using System.Reflection;

/// <summary>
/// Represents the Lamar visitor for keyed services.
/// </summary>
public sealed class LamarKeyedServiceVisitor : KeyedServiceDescriptorVisitor
{
    private readonly ServiceRegistry registry;

    /// <summary>
    /// Initializes a new instance of the <see cref="LamarKeyedServiceVisitor"/> class.
    /// </summary>
    /// <param name="registry">The underlying service registry.</param>
    [CLSCompliant( false )]
    public LamarKeyedServiceVisitor( ServiceRegistry registry )
        : base( typeof( LamarKey<,> ), typeof( LamarKey<,,> ) ) => this.registry = registry;

    /// <inheritdoc />
    protected override void VisitInterface( ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var instance = registry.For( serviceDescriptor.ServiceType )
                               .Use( serviceDescriptor.ImplementationType );

        instance.Lifetime = serviceDescriptor.Lifetime;
    }

    /// <inheritdoc />
    protected override void VisitService( Type key, ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( key, nameof( key ) );
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        // TODO: yuck! how do we use ServiceDescriptor.ImplementationFactory or
        // ServiceDescriptor.ImplementationInstance with the non-generic API?
        // there has to be a better way
        var name = key.ToString();
        var serviceType = serviceDescriptor.ServiceType;
        var lifetime = serviceDescriptor.Lifetime;

        if ( serviceDescriptor.ImplementationType is Type implementationType )
        {
            registry.For( serviceType ).Use( implementationType ).Named( name ).Lifetime = lifetime;
        }
        else if ( serviceDescriptor.ImplementationFactory is Func<IServiceProvider, object> factory )
        {
            var methodOfT = GetType().GetRuntimeMethods().First( m => m.Name == nameof( RegisterFactory ) )!;
            var method = methodOfT.MakeGenericMethod( serviceType );
            method.Invoke( this, new object[] { name, factory, lifetime } );
        }
        else
        {
            var instance = serviceDescriptor.ImplementationInstance!;
            var methodOfT = GetType().GetRuntimeMethods().First( m => m.Name == nameof( RegisterInstance ) )!;
            var method = methodOfT.MakeGenericMethod( serviceType, instance.GetType() );
            method.Invoke( this, new object[] { name, instance, lifetime } );
        }
    }

    private void RegisterFactory<TSvc>( string name, Func<IServiceProvider, object> factory, ServiceLifetime lifetime )
        where TSvc : class
    {
        var instance = registry.For<TSvc>().Add( c => (TSvc) factory( c ) ).Named( name );
        instance.Lifetime = lifetime;
    }

    private void RegisterInstance<TSvc, TImpl>( string name, TImpl existing, ServiceLifetime lifetime )
        where TSvc : class
        where TImpl : class, TSvc
    {
        var instance = registry.For<TSvc>().Add( existing ).Named( name );
        instance.Lifetime = lifetime;
    }
}