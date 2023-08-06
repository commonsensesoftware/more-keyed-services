// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

using System.Runtime.CompilerServices;

internal struct IKeyedType
{
    internal static Type TypeDefinition = typeof( IKeyed<,> );

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal static Type Make( Type keyType, Type serviceType ) =>
        TypeDefinition.MakeGenericType( keyType, serviceType );

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    internal static bool IsImplementedBy( Type type ) =>
        type.IsGenericType
        && !type.IsGenericTypeDefinition
        && type.GetGenericTypeDefinition().IsAssignableFrom( TypeDefinition );
}