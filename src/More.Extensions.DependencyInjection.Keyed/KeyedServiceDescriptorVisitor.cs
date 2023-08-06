// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

// NOTE: this type is not strictly necessary, but it removes a lot of ceremony for what other containers
// might need to do in order to remap service registrations to their specific implementation

/// <summary>
/// Represents the base implementation for a keyed <see cref="ServiceDescriptor"/> visitor.
/// </summary>
public abstract class KeyedServiceDescriptorVisitor
{
    private readonly Type keyedOf2;
    private readonly Type keyedOf3;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyedServiceDescriptorVisitor"/> class.
    /// </summary>
    /// <param name="keyedOf2">The replacement type for <see cref="Keyed{TKey, TService}"/>.</param>
    /// <param name="keyedOf3">The replacement type for <see cref="Keyed{TKey, TService, TImplementation}"/>.</param>
    protected KeyedServiceDescriptorVisitor( Type keyedOf2, Type keyedOf3 )
    {
        this.keyedOf2 = ValidateType( keyedOf2, 2 );
        this.keyedOf3 = ValidateType( keyedOf3, 3 );
    }

    /// <summary>
    /// Visits the specified collection of keyed services.
    /// </summary>
    /// <param name="keyedServices">A read-only list of keyed services.</param>
    public void Visit( IReadOnlyList<KeyValuePair<Type, IServiceCollection>> keyedServices )
    {
        ArgumentNullException.ThrowIfNull( keyedServices, nameof( keyedServices ) );

        for ( var i = 0; i < keyedServices.Count; i++ )
        {
            var (key, services) = keyedServices[i];

            for ( var j = 0; j < services.Count; j++ )
            {
                var service = services[j];

                if ( IKeyedType.IsImplementedBy( service.ServiceType ) )
                {
                    VisitInterface( Remap( service ) );
                }
                else
                {
                    VisitService( key, service );
                }
            }
        }
    }

    /// <summary>
    /// Remaps the specified <see cref="ServiceDescriptor"/> to a container-specific
    /// <see cref="ServiceDescriptor"/> for <see cref="IKeyed{TKey, TService}"/>.
    /// </summary>
    /// <param name="serviceDescriptor">The service descriptor to remap.</param>
    /// <returns>A new, remapped <see cref="ServiceDescriptor"/>.</returns>
    /// <exception cref="ArgumentException"><see cref="ServiceDescriptor.ImplementationType"/>
    /// in <paramref name="serviceDescriptor"/> is <c>null</c>.</exception>
    protected virtual ServiceDescriptor Remap( ServiceDescriptor serviceDescriptor )
    {
        ArgumentNullException.ThrowIfNull( serviceDescriptor, nameof( serviceDescriptor ) );

        if ( serviceDescriptor.ImplementationType is not Type implementationType )
        {
            var message = $"{nameof( ServiceDescriptor )}.{nameof( ServiceDescriptor.ImplementationType )} cannot be null.";
            throw new ArgumentException( message );
        }

        var serviceType = serviceDescriptor.ServiceType;
        var args = serviceType.GenericTypeArguments;
        var keyType = args[0];
        serviceType = args[1];

        // remap: Keyed<,> or Keyed<,,>
        // to:    [Container]Key<,> or [Container]Key<,,>
        //
        // note: additional validation recommended
        args = implementationType.GenericTypeArguments;
        var keyedType = args.Length switch
        {
            2 => NewKeyedType( keyType, serviceType ),
            3 => NewKeyedType( keyType, serviceType, args[^1] ),
            _ => throw Unexpected( implementationType ),
        };

        return new( NewIKeyedTyped( keyType, serviceType ), keyedType, serviceDescriptor.Lifetime );

        static NotSupportedException Unexpected( Type type ) =>
            new( $"Type {type} was expected to be a generic type with 2 or 3 type arguments." );
    }

    /// <summary>
    /// Visits the <see cref="ServiceDescriptor"/> for <see cref="IKeyed{TKey, TService}"/>.
    /// </summary>
    /// <param name="serviceDescriptor">The service descriptor to visit.</param>
    protected abstract void VisitInterface( ServiceDescriptor serviceDescriptor );

    /// <summary>
    /// Visits the <see cref="ServiceDescriptor"/> for a service.
    /// </summary>
    /// <param name="key">The type representing the key associated with the service.</param>
    /// <param name="serviceDescriptor">The service descriptor to visit.</param>
    protected abstract void VisitService( Type key, ServiceDescriptor serviceDescriptor );

    /// <summary>
    /// Creates and returns a new, closed type for <see cref="IKeyed{TKey, TService}"/>.
    /// </summary>
    /// <param name="keyType">The key type.</param>
    /// <param name="serviceType">The service type.</param>
    /// <returns>A new closed type for <see cref="IKeyed{TKey, TService}"/>.</returns>
    protected static Type NewIKeyedTyped( Type keyType, Type serviceType ) => IKeyedType.Make( keyType, serviceType );

    /// <summary>
    /// Creates and returns a new, closed type for a container-specific implementation of
    /// <see cref="Keyed{TKey, TService}"/>.
    /// </summary>
    /// <param name="keyType">The key type.</param>
    /// <param name="serviceType">The service type.</param>
    /// <returns>A new closed type.</returns>
    protected Type NewKeyedType( Type keyType, Type serviceType ) => keyedOf2.MakeGenericType( keyType, serviceType );

    /// <summary>
    /// Creates and returns a new, closed type for a container-specific implementation of
    /// <see cref="Keyed{TKey, TService, TImplementation}"/>.
    /// </summary>
    /// <param name="keyType">The key type.</param>
    /// <param name="serviceType">The service type.</param>
    /// <param name="implementationType">The service implementation type.</param>
    /// <returns>A new closed type.</returns>
    protected Type NewKeyedType( Type keyType, Type serviceType, Type implementationType ) =>
        keyedOf3.MakeGenericType( keyType, serviceType, implementationType );

    private static Type ValidateType( Type keyedType, int expectedTypeArgCount )
    {
        if ( keyedType == null )
        {
            throw new ArgumentNullException( "keyedOf" + expectedTypeArgCount );
        }

        if ( !keyedType.IsGenericTypeDefinition )
        {
            throw new ArgumentException( $"{keyedType} is not a generic type definition." );
        }

        var interfaces = keyedType.GetInterfaces();
        var found = false;

        for ( var i = 0; i < interfaces.Length; i++ )
        {
            var iface = interfaces[i];

            if ( !iface.IsGenericType )
            {
                continue;
            }

            var typeDef = iface.IsGenericTypeDefinition ? iface : iface.GetGenericTypeDefinition();

            if ( found = typeDef.Equals( IKeyedType.TypeDefinition ) )
            {
                break;
            }
        }

        if ( !found )
        {
            throw new ArgumentException( $"{keyedType} does not implement {IKeyedType.TypeDefinition}." );
        }

        if ( keyedType.GetGenericArguments().Length != expectedTypeArgCount )
        {
            throw new ArgumentException( $"{keyedType} is expected to have {expectedTypeArgCount} type arguments." );
        }

        return keyedType;
    }
}