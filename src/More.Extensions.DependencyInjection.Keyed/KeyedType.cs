// Copyright (c) Commonsense Software and contributors. All rights reserved.

namespace More.Extensions.DependencyInjection;

using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

/// <summary>
/// Provides the utility functions for creating a keyed type.
/// </summary>
public static class KeyedType
{
    /// <summary>
    /// Creates and returns a new keyed type.
    /// </summary>
    /// <param name="key">The type representing the key.</param>
    /// <param name="type">The type associated with the key.</param>
    /// <returns>A new, keyed type.</returns>
    public static Type Create( Type key, Type type ) =>
        new KeyedTypeInfo(
            key ?? throw new ArgumentNullException( nameof( key ) ),
            type ?? throw new ArgumentNullException( nameof( type ) ) );

    /// <summary>
    /// Creates and returns a new keyed type.
    /// </summary>
    /// <typeparam name="TKey">The type representing the key.</typeparam>
    /// <typeparam name="TType">The type associated with the key.</typeparam>
    /// <returns>A new, keyed type.</returns>
    public static Type Create<TKey, TType>() where TType : notnull =>
        new KeyedTypeInfo( typeof( TKey ), typeof( TType ) );

    /// <summary>
    /// Returns a value indicating whether the specified type is a key.
    /// </summary>
    /// <param name="type">The type to evaluate.</param>
    /// <returns>True if the type represents a key; otherwise, false.</returns>
    public static bool IsKey( Type type ) => type is KeyedTypeInfo;

    /// <summary>
    /// Attempts to deconstruct the specifed type into its key and type components.
    /// </summary>
    /// <param name="type">The type to deconstruct.</param>
    /// <param name="keyType">The key type, if <paramref name="type"/> represents a key.</param>
    /// <param name="originalType">The original type constructed with the key or <paramref name="type"/>.</param>
    /// <returns>True if deconstruction succeeds (e.g. <paramref name="type"/> as a key); otherwise, false.</returns>
    public static bool TryDeconstruct( Type type, [MaybeNullWhen( false )] out Type? keyType, out Type originalType )
    {
        if ( type is KeyedTypeInfo key )
        {
            (keyType, originalType) = key;
            return true;
        }

        keyType = default;
        originalType = type;
        return false;
    }

    // much, but not all, of this can be done with TypeDelegator
    // REF: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Reflection/TypeDelegator.cs
    // REF: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Type.cs
    private sealed class KeyedTypeInfo :
        TypeInfo,
        IReflectableType,
        ICloneable,
        IEquatable<Type>,
        IEquatable<TypeInfo>,
        IEquatable<KeyedTypeInfo>
    {
        private readonly int hashCode;
        private readonly Type key;
        private readonly Type type;
        private Protected? @protected;

        public KeyedTypeInfo( Type key, Type type )
        {
            this.key = key;
            this.type = type;
            hashCode = HashCode.Combine( key, type );
        }

        public void Deconstruct( out Type key, out Type type )
        {
            key = this.key;
            type = this.type;
        }

        public override int GetHashCode() => hashCode;

        public override bool Equals( object? obj ) => ( obj is KeyedTypeInfo o && Equals( o ) ) || type.Equals( obj );

        public override bool Equals( Type? o ) => ( o is KeyedTypeInfo other && Equals( other ) ) || type.Equals( o );

        public bool Equals( TypeInfo? o ) => ( o is KeyedTypeInfo other && Equals( other ) ) || type.GetTypeInfo().Equals( o );

        public bool Equals( KeyedTypeInfo? o ) => o != null && o.hashCode == hashCode;

        public static bool operator ==( KeyedTypeInfo? left, Type? right ) => left?.type == right;
        public static bool operator ==( Type? left, KeyedTypeInfo? right ) => left == right?.type;
        public static bool operator ==( KeyedTypeInfo? left, KeyedTypeInfo? right ) => left?.hashCode == right?.hashCode;

        public static bool operator !=( KeyedTypeInfo? left, Type? right ) => left?.type == right;
        public static bool operator !=( Type? left, KeyedTypeInfo? right ) => !( left == right?.type );
        public static bool operator !=( KeyedTypeInfo? left, KeyedTypeInfo? right ) => left?.hashCode != right?.hashCode;

        public override Assembly Assembly => type.Assembly;
        public override Type? BaseType => type.BaseType;
        public override bool IsByRefLike => type.IsByRefLike;
        public override bool IsConstructedGenericType => type.IsConstructedGenericType;
        public override bool IsGenericType => type.IsGenericType;
        public override bool IsGenericTypeDefinition => type.IsGenericType;
        public override bool IsGenericParameter => type.IsGenericParameter;
        public override bool IsTypeDefinition => type.IsTypeDefinition;
        public override bool IsSecurityCritical => type.IsSecurityCritical;
        public override bool IsSecuritySafeCritical => type.IsSecuritySafeCritical;
        public override bool IsSecurityTransparent => type.IsSecurityTransparent;
        public override MemberTypes MemberType => type.MemberType;
        public override int MetadataToken => type.MetadataToken;
        public override Module Module => type.Module;
        public override Type? ReflectedType => type.ReflectedType;
        public override RuntimeTypeHandle TypeHandle => type.TypeHandle;
        public override Type UnderlyingSystemType => type.UnderlyingSystemType;
        public override string? AssemblyQualifiedName => type.AssemblyQualifiedName;
        public override string? FullName => type.FullName;
        public override Guid GUID => type.GUID;
        public override string? Namespace => type.Namespace;
        public override string Name => type.Name;
        public object Clone() => this;
        public override int GetArrayRank() => type.GetArrayRank();
        protected override TypeAttributes GetAttributeFlagsImpl() => ( @protected ??= new( type ) ).GetAttributeFlagsImpl();
        public override object[] GetCustomAttributes( bool inherit ) => type.GetCustomAttributes( inherit );
        public override object[] GetCustomAttributes( Type attributeType, bool inherit ) => type.GetCustomAttributes( attributeType, inherit );
        public override IList<CustomAttributeData> GetCustomAttributesData() => type.GetCustomAttributesData();

        [DynamicallyAccessedMembers( PublicFields | PublicMethods | PublicEvents | PublicProperties | PublicConstructors | PublicNestedTypes )]
        public override MemberInfo[] GetDefaultMembers() => type.GetDefaultMembers();
        public override Type? GetElementType() => type.GetElementType();
        public override string? GetEnumName( object value ) => type.GetEnumName( value );
        public override string[] GetEnumNames() => type.GetEnumNames();
        public override Array GetEnumValues() => type.GetEnumValues();
        public override Type GetEnumUnderlyingType() => type.GetEnumUnderlyingType();
        public override Type GetGenericTypeDefinition() => type.GetGenericTypeDefinition();
        protected override TypeCode GetTypeCodeImpl() => ( @protected ??= new( type ) ).GetTypeCodeImpl();
        protected override bool HasElementTypeImpl() => ( @protected ??= new( type ) ).HasElementTypeImpl();
        protected override bool IsArrayImpl() => ( @protected ??= new( type ) ).IsArrayImpl();
        protected override bool IsContextfulImpl() => ( @protected ??= new( type ) ).IsContextfulImpl();
        public override bool IsDefined( Type attributeType, bool inherit ) => type.IsDefined( attributeType, inherit );
        public override bool IsEnumDefined( object value ) => type.IsEnumDefined( value );
        protected override bool IsValueTypeImpl() => ( @protected ??= new( type ) ).IsValueTypeImpl();
        protected override bool IsByRefImpl() => ( @protected ??= new( type ) ).IsByRefImpl();
        protected override bool IsPrimitiveImpl() => ( @protected ??= new( type ) ).IsPrimitiveImpl();
        protected override bool IsPointerImpl() => ( @protected ??= new( type ) ).IsPointerImpl();
        protected override bool IsCOMObjectImpl() => ( @protected ??= new( type ) ).IsCOMObjectImpl();
        public override bool IsInstanceOfType( [NotNullWhen( true )] object? o ) => type.IsInstanceOfType( o );
        public override bool IsAssignableFrom( [NotNullWhen( true )] TypeInfo? typeInfo ) => type.IsAssignableFrom( typeInfo );
        public override bool IsAssignableFrom( [NotNullWhen( true )] Type? c ) => type.IsAssignableFrom( c );
        public override bool IsEquivalentTo( [NotNullWhen( true )] Type? other ) => type == other;
        public override bool IsSubclassOf( Type c ) => type.IsSubclassOf( c );

        [DynamicallyAccessedMembers( All )]
        public override object? InvokeMember(
            string name,
            BindingFlags bindingFlags,
            Binder? binder,
            object? target,
            object?[]? providedArgs,
            ParameterModifier[]? modifiers,
            CultureInfo? culture,
            string[]? namedParams ) =>
            type.InvokeMember( name, bindingFlags, binder, target, providedArgs, modifiers, culture, namedParams );

        protected override ConstructorInfo? GetConstructorImpl( BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers ) =>
            ( @protected ??= new( type ) ).GetConstructorImpl( bindingAttr, binder, callConvention, types, modifiers );

        public override ConstructorInfo[] GetConstructors( BindingFlags bindingAttr ) => type.GetConstructors( bindingAttr );
        public override EventInfo? GetEvent( string name, BindingFlags bindingAttr ) => type.GetEvent( name, bindingAttr );
        public override EventInfo[] GetEvents( BindingFlags bindingAttr ) => type.GetEvents( bindingAttr );
        public override FieldInfo? GetField( string name, BindingFlags bindingAttr ) => type.GetField( name, bindingAttr );
        public override FieldInfo[] GetFields( BindingFlags bindingAttr ) => type.GetFields( bindingAttr );

        [return: DynamicallyAccessedMembers( Interfaces )]
        public override Type? GetInterface( string name, bool ignoreCase ) => type.GetInterface( name, ignoreCase );
        public override Type[] GetInterfaces() => type.GetInterfaces();

        public override InterfaceMapping GetInterfaceMap( [DynamicallyAccessedMembers( PublicMethods | NonPublicMethods )] Type interfaceType ) =>
            type.GetInterfaceMap( interfaceType );

        public override MemberInfo[] GetMembers( BindingFlags bindingAttr ) => type.GetMembers( bindingAttr );

        protected override MethodInfo? GetMethodImpl( string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers ) =>
            ( @protected ??= new( type ) ).GetMethodImpl( name, bindingAttr, binder, callConvention, types, modifiers );

        protected override MethodInfo? GetMethodImpl( string name, int genericParameterCount, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers ) =>
            ( @protected ??= new( type ) ).GetMethodImpl( name, genericParameterCount, bindingAttr, binder, callConvention, types, modifiers );

        public override MethodInfo[] GetMethods( BindingFlags bindingAttr ) => type.GetMethods( bindingAttr );
        public override Type? GetNestedType( string name, BindingFlags bindingAttr ) => type.GetNestedType( name, bindingAttr );
        public override Type[] GetNestedTypes( BindingFlags bindingAttr ) => type.GetNestedTypes( bindingAttr );
        public override PropertyInfo[] GetProperties( BindingFlags bindingAttr ) => type.GetProperties( bindingAttr );

        protected override PropertyInfo? GetPropertyImpl( string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers ) =>
            ( @protected ??= new( type ) ).GetPropertyImpl( name, bindingAttr, binder, returnType, types, modifiers );

        protected override bool IsMarshalByRefImpl() => ( @protected ??= new( type ) ).IsMarshalByRefImpl();
        public override bool IsSZArray => type.IsSZArray;
        public override bool IsSignatureType => type.IsSignatureType;
        public override StructLayoutAttribute? StructLayoutAttribute => type.StructLayoutAttribute;
        public override Type[] GenericTypeArguments => type.GenericTypeArguments;
        public override Type[] GetGenericArguments() => type.GetGenericArguments();
        public override int GenericParameterPosition => type.GenericParameterPosition;
        public override GenericParameterAttributes GenericParameterAttributes => type.GenericParameterAttributes;
        public override Type[] GetGenericParameterConstraints() => type.GetGenericParameterConstraints();
        public override Type MakeGenericType( params Type[] typeArguments ) => type.MakeGenericType( typeArguments );
        public override Type MakeArrayType() => type.MakeArrayType();
        public override Type MakeArrayType( int rank ) => type.MakeArrayType( rank );
        public override Type MakeByRefType() => type.MakeByRefType();
        public override Type MakePointerType() => type.MakePointerType();
        public override MemberInfo GetMemberWithSameMetadataDefinitionAs( MemberInfo member ) => type.GetMemberWithSameMetadataDefinitionAs( member );

        //// Override everything (necessary?)
        public override bool ContainsGenericParameters => type.ContainsGenericParameters;
        public override bool IsVariableBoundArray => type.IsVariableBoundArray;
        public override bool IsSerializable => type.IsSerializable;
        public override bool IsGenericTypeParameter => type.IsGenericTypeParameter;
        public override bool IsGenericMethodParameter => type.IsGenericMethodParameter;
        public override bool IsEnum => type.IsEnum;
        public override IEnumerable<CustomAttributeData> CustomAttributes => type.CustomAttributes;
        public override IEnumerable<ConstructorInfo> DeclaredConstructors => type.GetTypeInfo().DeclaredConstructors;
        public override IEnumerable<EventInfo> DeclaredEvents => type.GetTypeInfo().DeclaredEvents;
        public override IEnumerable<FieldInfo> DeclaredFields => type.GetTypeInfo().DeclaredFields;
        public override IEnumerable<MemberInfo> DeclaredMembers => type.GetTypeInfo().DeclaredMembers;
        public override IEnumerable<MethodInfo> DeclaredMethods => type.GetTypeInfo().DeclaredMethods;
        public override IEnumerable<PropertyInfo> DeclaredProperties => type.GetTypeInfo().DeclaredProperties;
        public override MethodBase? DeclaringMethod => type.DeclaringMethod;
        public override Type? DeclaringType => type.DeclaringType;
        public override Type[] FindInterfaces( TypeFilter filter, object? filterCriteria ) => type.FindInterfaces( filter, filterCriteria );
        public override MemberInfo[] FindMembers( MemberTypes memberType, BindingFlags bindingAttr, MemberFilter? filter, object? filterCriteria ) =>
            type.FindMembers( memberType, bindingAttr, filter, filterCriteria );

        public override Type[] GenericTypeParameters => type.GetTypeInfo().GenericTypeParameters;
        public override EventInfo? GetDeclaredEvent( string name ) => type.GetTypeInfo().GetDeclaredEvent( name );
        public override FieldInfo? GetDeclaredField( string name ) => type.GetTypeInfo().GetDeclaredField( name );
        public override MethodInfo? GetDeclaredMethod( string name ) => type.GetTypeInfo().GetDeclaredMethod( name );
        public override TypeInfo? GetDeclaredNestedType( string name ) => type.GetTypeInfo().GetDeclaredNestedType( name );
        public override PropertyInfo? GetDeclaredProperty( string name ) => type.GetTypeInfo().GetDeclaredProperty( name );
        public override EventInfo[] GetEvents() => type.GetEvents();
        public override MemberInfo[] GetMember( string name, BindingFlags bindingAttr ) => type.GetMember( name, bindingAttr );
        public override MemberInfo[] GetMember( string name, MemberTypes type, BindingFlags bindingAttr ) => this.type.GetMember( name, type, bindingAttr );
        public override bool HasSameMetadataDefinitionAs( MemberInfo other ) => type.HasSameMetadataDefinitionAs( other );
        public override IEnumerable<Type> ImplementedInterfaces => type.GetTypeInfo().ImplementedInterfaces;
        public override bool IsCollectible => type.IsCollectible;
        public override string ToString() => $"{key.FullName}+{type.FullName}";

        //// TypeInfo + IReflectableType

        TypeInfo IReflectableType.GetTypeInfo() => type.GetTypeInfo();
        public override Type AsType() => type.GetTypeInfo().AsType();

        [DynamicallyAccessedMembers( PublicMethods | NonPublicMethods )]
        public override IEnumerable<MethodInfo> GetDeclaredMethods( string name ) => type.GetTypeInfo().GetDeclaredMethods( name );

        public override IEnumerable<TypeInfo> DeclaredNestedTypes
        {
            [DynamicallyAccessedMembers( PublicNestedTypes | NonPublicNestedTypes )]
            get => type.GetTypeInfo().DeclaredNestedTypes;
        }
    }

    private sealed class Protected
    {
        private const BindingFlags Visibility = BindingFlags.Instance | BindingFlags.NonPublic;
        private static Func<Type, string, BindingFlags, Binder?, Type?, Type[]?, ParameterModifier[]?, PropertyInfo?>? getPropertyImpl;
        private static Func<Type, string, BindingFlags, Binder?, CallingConventions, Type[]?, ParameterModifier[]?, MethodInfo?>? getMethodImpl;
        private static Func<Type, string, int, BindingFlags, Binder?, CallingConventions, Type[]?, ParameterModifier[]?, MethodInfo?>? getMethodImpl2;
        private static Func<Type, BindingFlags, Binder?, CallingConventions, Type[], ParameterModifier[]?, ConstructorInfo?>? getConstructorImpl;
        private static Func<Type, TypeAttributes>? getAttributeFlagsImpl;
        private static Func<Type, TypeCode>? getTypeCodeImpl;
        private static Func<Type, bool>? hasElementTypeImpl;
        private static Func<Type, bool>? isArrayImpl;
        private static Func<Type, bool>? isContextfulImpl;
        private static Func<Type, bool>? isValueTypeImpl;
        private static Func<Type, bool>? isByRefImpl;
        private static Func<Type, bool>? isPrimitiveImpl;
        private static Func<Type, bool>? isPointerImpl;
        private static Func<Type, bool>? isCOMObjectImpl;
        private static Func<Type, bool>? isMarshalByRefImpl;
        private readonly Type type;

        public Protected( Type type ) => this.type = type;

        public PropertyInfo? GetPropertyImpl( string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers )
        {
            getPropertyImpl ??=
                (Func<Type, string, BindingFlags, Binder?, Type?, Type[]?, ParameterModifier[]?, PropertyInfo?>)
                Delegate.CreateDelegate(
                    typeof( Func<Type, string, BindingFlags, Binder?, Type?, Type[]?, ParameterModifier[]?, PropertyInfo?> ),
                    typeof( Type ).GetMethod( nameof( GetPropertyImpl ), Protected.Visibility )! );

            return getPropertyImpl( type, name, bindingAttr, binder, returnType, types, modifiers );
        }

        public MethodInfo? GetMethodImpl( string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers )
        {
            getMethodImpl ??=
                (Func<Type, string, BindingFlags, Binder?, CallingConventions, Type[]?, ParameterModifier[]?, MethodInfo?>)
                Delegate.CreateDelegate(
                    typeof( Func<Type, string, BindingFlags, Binder?, CallingConventions, Type[]?, ParameterModifier[]?, MethodInfo?> ),
                    typeof( Type ).GetMethod( nameof( GetMethodImpl ), Protected.Visibility )! );

            return getMethodImpl( type, name, bindingAttr, binder, callConvention, types, modifiers );
        }

        public MethodInfo? GetMethodImpl( string name, int genericParameterCount, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers )
        {
            getMethodImpl2 ??=
                (Func<Type, string, int, BindingFlags, Binder?, CallingConventions, Type[]?, ParameterModifier[]?, MethodInfo?>)
                Delegate.CreateDelegate(
                    typeof( Func<Type, string, int, BindingFlags, Binder?, CallingConventions, Type[]?, ParameterModifier[]?, MethodInfo?> ),
                    typeof( Type ).GetMethod( nameof( GetMethodImpl ), Protected.Visibility )! );

            return getMethodImpl2( type, name, genericParameterCount, bindingAttr, binder, callConvention, types, modifiers );
        }

        public ConstructorInfo? GetConstructorImpl( BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers )
        {
            getConstructorImpl ??=
                (Func<Type, BindingFlags, Binder?, CallingConventions, Type[], ParameterModifier[]?, ConstructorInfo?>)
                Delegate.CreateDelegate(
                    typeof( Func<Type, BindingFlags, Binder?, CallingConventions, Type[], ParameterModifier[]?, ConstructorInfo?> ),
                    typeof( Type ).GetMethod( nameof( GetConstructorImpl ), Protected.Visibility )! );

            return getConstructorImpl( type, bindingAttr, binder, callConvention, types, modifiers );
        }

        public TypeAttributes GetAttributeFlagsImpl() => ( getAttributeFlagsImpl ??= NewFunc<TypeAttributes>() )( type );

        public TypeCode GetTypeCodeImpl() => ( getTypeCodeImpl ??= NewFunc<TypeCode>() )( type );

        public bool HasElementTypeImpl() => ( hasElementTypeImpl ??= NewFunc<bool>() )( type );

        public bool IsArrayImpl() => ( isArrayImpl ??= NewFunc<bool>() )( type );

        public bool IsContextfulImpl() => ( isContextfulImpl ??= NewFunc<bool>() )( type );

        public bool IsValueTypeImpl() => ( isValueTypeImpl ??= NewFunc<bool>() )( type );

        public bool IsByRefImpl() => ( isByRefImpl ??= NewFunc<bool>() )( type );

        public bool IsPrimitiveImpl() => ( isPrimitiveImpl ??= NewFunc<bool>() )( type );

        public bool IsPointerImpl() => ( isPointerImpl ??= NewFunc<bool>() )( type );

        public bool IsCOMObjectImpl() => ( isCOMObjectImpl ??= NewFunc<bool>() )( type );

        public bool IsMarshalByRefImpl() => ( isMarshalByRefImpl ??= NewFunc<bool>() )( type );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private static Func<Type, T> NewFunc<T>( [CallerMemberName] string name = "" ) =>
            (Func<Type, T>) Delegate.CreateDelegate( typeof( Func<Type, T> ), typeof( Type ).GetMethod( name, Visibility )! );
    }
}