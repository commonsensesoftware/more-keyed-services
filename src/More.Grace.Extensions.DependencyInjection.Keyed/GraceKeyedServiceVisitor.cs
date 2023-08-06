// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace Grace.DependencyInjection.Extensions;

using Microsoft.Extensions.DependencyInjection;
using More.Extensions.DependencyInjection;

/// <summary>
/// Represents the Grace visitor for keyed services.
/// </summary>
public sealed class GraceKeyedServiceVisitor : KeyedServiceDescriptorVisitor
{
    private readonly IInjectionScope scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraceKeyedServiceVisitor"/> class.
    /// </summary>
    /// <param name="scope">The underlying scope.</param>
    [CLSCompliant( false )]
    public GraceKeyedServiceVisitor( IInjectionScope scope )
        : base( typeof( GraceKey<,> ), typeof( GraceKey<,,> ) ) => this.scope = scope;

    /// <inheritdoc />
    protected override void VisitInterface( ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var serviceType = serviceDescriptor.ServiceType;
        var implementationType = serviceDescriptor.ImplementationType!;
        var lifetime = serviceDescriptor.Lifetime;

        scope.Configure( c => c.Export( implementationType ).As( serviceType ).WithLifetime( lifetime ) );
    }

    /// <inheritdoc />
    protected override void VisitService( Type key, ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( key, nameof( key ) );
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        var type = serviceDescriptor.ServiceType;
        var lifetime = serviceDescriptor.Lifetime;

        if ( serviceDescriptor.ImplementationType is Type implementationType )
        {
            scope.Configure( c => c.Export( implementationType ).AsKeyed( type, key ).WithLifetime( lifetime ) );
        }
        else if ( serviceDescriptor.ImplementationFactory is Func<IServiceProvider, object> factory )
        {
            scope.Configure( c => c.ExportFactory( factory ).AsKeyed( type, key ).WithLifetime( lifetime ) );
        }
        else
        {
            var instance = serviceDescriptor.ImplementationInstance;
            scope.Configure( c => c.ExportInstance( instance ).AsKeyed( type, key ).WithLifetime( lifetime ) );
        }
    }
}