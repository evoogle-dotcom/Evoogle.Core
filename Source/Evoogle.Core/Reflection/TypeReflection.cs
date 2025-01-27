// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Reflection;
using System.Text;

namespace Evoogle.Reflection;

/// <summary>
///     Reflection methods for the .NET <see cref="Type"> class.
/// </summary>
public static class TypeReflection
{
    #region Fields
    private const BindingFlags DefaultConstructorReflectionFlags =
        BindingFlags.Public;

    private const BindingFlags DefaultFieldReflectionFlags =
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    private const BindingFlags DefaultMethodReflectionFlags =
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    private const BindingFlags DefaultPropertyReflectionFlags =
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    private static readonly Type[] EmptyTypes = Type.EmptyTypes;

    private static readonly HashSet<Type> FloatingPointTypes =
        [
            typeof(decimal),
            typeof(double),
            typeof(float)
        ];

    private static readonly HashSet<Type> IntegerTypes =
        [
            typeof(sbyte),
            typeof(byte),
            typeof(char),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong)
        ];

    private static readonly HashSet<Type> PrimitiveTypes =
        [
            typeof(byte[]),
            typeof(decimal),
            typeof(string),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Guid),
            typeof(TimeSpan),
            typeof(Type),
            typeof(Uri)
        ];
    #endregion

    #region Constructor Methods
    public static ConstructorInfo? GetConstructor(Type type, params Type[] parameterTypes)
    {
        if (parameterTypes == null || parameterTypes.Length == 0)
            return GetDefaultConstructor(type, DefaultConstructorReflectionFlags);

        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
        if (parameterTypes == null || parameterTypes.Length == 0)
            return GetDefaultConstructor(type, bindingFlags);

        return GetConstructors(type.GetTypeInfo(), bindingFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetConstructor(Type type, IEnumerable<Type> parameterTypes)
    {
        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes)
    {
        return GetConstructors(type.GetTypeInfo(), bindingFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetDefaultConstructor(Type type)
    {
        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, EmptyTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetDefaultConstructor(Type type, BindingFlags bindingFlags)
    {
        return GetConstructors(type.GetTypeInfo(), bindingFlags, EmptyTypes).SingleOrDefault();
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
    {
        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, null);
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingFlags)
    {
        return GetConstructors(type.GetTypeInfo(), bindingFlags, null);
    }
    #endregion

    #region Field Methods
    public static FieldInfo? GetField(Type type, string fieldName)
    {
        return GetFields(type.GetTypeInfo(), fieldName, DefaultFieldReflectionFlags).SingleOrDefault();
    }

    public static FieldInfo? GetField(Type type, string fieldName, BindingFlags bindingFlags)
    {
        return GetFields(type.GetTypeInfo(), fieldName, bindingFlags).SingleOrDefault();
    }

    public static IEnumerable<FieldInfo> GetFields(Type type)
    {
        return GetFields(type.GetTypeInfo(), null, DefaultFieldReflectionFlags);
    }

    public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
    {
        return GetFields(type.GetTypeInfo(), null, bindingFlags);
    }
    #endregion

    #region Method Methods
    public static MethodInfo? GetMethod(Type type, string methodName)
    {
        return GetMethods(type.GetTypeInfo(), methodName, DefaultMethodReflectionFlags, EmptyTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(Type type, string methodName, params Type[] parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, DefaultMethodReflectionFlags, parameterTypes ?? EmptyTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, bindingFlags, parameterTypes ?? EmptyTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(Type type, string methodName, IEnumerable<Type> parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, DefaultMethodReflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, bindingFlags, parameterTypes).SingleOrDefault();
    }

    public static IEnumerable<MethodInfo> GetMethods(Type type)
    {
        return GetMethods(type.GetTypeInfo(), null, DefaultMethodReflectionFlags, null);
    }

    public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags)
    {
        return GetMethods(type.GetTypeInfo(), null, bindingFlags, null);
    }
    #endregion

    #region Miscellaneous Methods
    public static Type? GetBaseType(Type type)
    {
        return type.GetTypeInfo().BaseType;
    }

    public static IEnumerable<Type> GetBaseTypes(Type type)
    {
        var baseType = GetBaseType(type);
        if (baseType == null)
            yield break;

        while (baseType != null)
        {
            yield return baseType;
            baseType = GetBaseType(baseType);
        }
    }

    /// <summary>
    ///     Represents a compact(partial) form of the <c>AssemblyQualifiedName</c> string property.
    ///     The compact(partial) type name can be used by the static method Type.GetType(string) to create .NET <c>Type</c> object like a factory method.
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    public static string? GetCompactQualifiedName(Type type)
    {
        var assemblyQualifiedName = type.AssemblyQualifiedName;
        if (assemblyQualifiedName == null)
            return null;

        var compactQualifiedName = RemoveAssemblyDetails(assemblyQualifiedName);
        return compactQualifiedName;
    }
    #endregion

    #region Predicate Methods
    public static bool IsAbstract(Type type)
    {
        return type.GetTypeInfo().IsAbstract;
    }

    public static bool IsAssignableFrom(Type type, Type fromType)
    {
        return fromType != null && type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
    }

    public static bool IsBoolean(Type type)
    {
        return type == typeof(bool);
    }

    public static bool IsClass(Type type)
    {
        return type.GetTypeInfo().IsClass;
    }

    /// <summary>
    ///     A complex type is a type that cannot be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type cannot be converted with a type converter, false otherwise.</returns>
    public static bool IsComplex(Type type)
    {
        return !IsSimple(type);
    }

    public static bool IsEnum(Type type)
    {
        return type.GetTypeInfo().IsEnum;
    }

    public static bool IsEnumerableOfT(Type type)
    {
        return IsEnumerableOfT(type, out _);
    }

    public static bool IsEnumerableOfT(Type type, out Type? enumerableType)
    {
        enumerableType = null;

        var typeInfo = type.GetTypeInfo();

        if (type == typeof(IEnumerable<>))
        {
            enumerableType = typeInfo.GenericTypeParameters.FirstOrDefault();
            return true;
        }

        if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            enumerableType = typeInfo.GenericTypeArguments.FirstOrDefault();
            return true;
        }

        if (!typeInfo.IsGenericType && !typeInfo.IsArray)
            return false;

        var enumerableGenericTypeArguments = typeInfo
            .ImplementedInterfaces
            .Where(t => IsGenericType(t) && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            .Select(t => t.GenericTypeArguments.FirstOrDefault())
            .Where(t => t != null)
            .ToList();

        var enumerableGenericTypeArgumentsCount = enumerableGenericTypeArguments.Count;
        if (enumerableGenericTypeArgumentsCount == 0)
            return false;

        if (enumerableGenericTypeArgumentsCount > 1)
        {
            var message = $"CLR type {{Name={type.Name}}} implements multiple versions of IEnumerable<T>.";
            throw new InvalidOperationException(message);
        }

        enumerableType = enumerableGenericTypeArguments[0];
        return true;
    }

    public static bool IsFloatingPoint(Type type)
    {
        return FloatingPointTypes.Contains(type);
    }

    public static bool IsGenericTypeDefinition(Type type)
    {
        return type.GetTypeInfo().IsGenericTypeDefinition;
    }

    public static bool IsGenericType(Type type)
    {
        return type.GetTypeInfo().IsGenericType;
    }

    public static bool IsGuid(Type type)
    {
        return type == typeof(Guid);
    }

    public static bool IsImplementationOf(Type type, Type interfaceType)
    {
        return interfaceType != null && IsImplementationOf(type.GetTypeInfo(), interfaceType.GetTypeInfo());
    }

    public static bool IsInteger(Type type)
    {
        return IntegerTypes.Contains(type);
    }

    public static bool IsNullableType(Type type)
    {
        return IsGenericType(type) && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static bool IsNullableEnum(Type type)
    {
        var isNullableType = IsNullableType(type);
        if (!isNullableType)
            return false;

        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (nullableUnderlyingType == null)
            return false;

        return IsEnum(nullableUnderlyingType);
    }

    public static bool IsNumber(Type type)
    {
        return IsInteger(type) || IsFloatingPoint(type);
    }

    public static bool IsPrimitive(Type type)
    {
        return type.GetTypeInfo().IsPrimitive || PrimitiveTypes.Contains(type);
    }

    /// <summary>
    ///     A simple type is a type that can be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type can be converted with a type converter, false otherwise.</returns>
    public static bool IsSimple(Type type)
    {
        while (true)
        {
            if (IsPrimitive(type))
                return true;

            if (IsNullableType(type))
            {
                var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
                if (nullableUnderlyingType == null)
                    return false;

                type = nullableUnderlyingType;
                continue;
            }

            if (IsEnum(type))
            {
                var enumUnderlyingType = Enum.GetUnderlyingType(type);
                if (enumUnderlyingType == null)
                    return false;

                type = enumUnderlyingType;
                continue;
            }

            return false;
        }
    }

    public static bool IsString(Type type)
    {
        return type == typeof(string);
    }

    public static bool IsSubclassOf(Type type, Type baseClass)
    {
        return baseClass != null && type.GetTypeInfo().IsSubclassOf(baseClass);
    }

    public static bool IsSubclassOrImplementationOf(Type type, Type baseClassOrInterfaceType)
    {
        return baseClassOrInterfaceType != null && IsSubclassOrImplementationOf(type.GetTypeInfo(), baseClassOrInterfaceType.GetTypeInfo());
    }

    public static bool IsValueType(Type type)
    {
        return type.GetTypeInfo().IsValueType;
    }

    public static bool IsVoid(Type type)
    {
        return type == typeof(void);
    }
    #endregion

    #region Property Methods
    public static PropertyInfo? GetProperty(Type type, string propertyName)
    {
        return GetProperties(type.GetTypeInfo(), propertyName, DefaultPropertyReflectionFlags).SingleOrDefault();
    }

    public static PropertyInfo? GetProperty(Type type, string propertyName, BindingFlags bindingFlags)
    {
        return GetProperties(type.GetTypeInfo(), propertyName, bindingFlags).SingleOrDefault();
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        return GetProperties(type.GetTypeInfo(), null, DefaultPropertyReflectionFlags);
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags bindingFlags)
    {
        return GetProperties(type.GetTypeInfo(), null, bindingFlags);
    }
    #endregion

    #region Methods
    private static IEnumerable<T> FilterOnName<T>(IEnumerable<T> query, string? name, BindingFlags bindingFlags)
        where T : MemberInfo
    {
        if (string.IsNullOrWhiteSpace(name))
            return query;

        var comparisonType = bindingFlags.HasFlag(BindingFlags.IgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return query.Where(x => string.Equals(x.Name, name, comparisonType));
    }

    private static IEnumerable<T> FilterOnParameterTypes<T>(IEnumerable<T> query, IEnumerable<Type>? parameterTypes)
        where T : MethodBase
    {
        if (parameterTypes == null)
            return query;

        return query.Where(x => x.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
    }

    private static IEnumerable<T> FilterOnInstanceAndStatic<T>(IEnumerable<T> query, BindingFlags bindingFlags)
        where T : MethodBase
    {
        var isInstance = bindingFlags.HasFlag(BindingFlags.Instance);
        var isStatic = bindingFlags.HasFlag(BindingFlags.Static);

        ValidateInstanceAndStaticBindingFlags(isInstance, isStatic);

        if (isInstance && !isStatic)
        {
            return query.Where(x => !x.IsStatic);
        }
        else if (!isInstance && isStatic)
        {
            return query.Where(x => x.IsStatic);
        }

        return query;
    }

    private static IEnumerable<T> FilterOnPublicAndNonPublic<T>(IEnumerable<T> query, BindingFlags bindingFlags)
        where T : MethodBase
    {
        var isPublic = bindingFlags.HasFlag(BindingFlags.Public);
        var isNonPublic = bindingFlags.HasFlag(BindingFlags.NonPublic);

        ValidatePublicAndNonPublicBindingFlags(isPublic, isNonPublic);

        if (isPublic && !isNonPublic)
        {
            return query.Where(x => x.IsPublic);
        }
        else if (!isPublic && isNonPublic)
        {
            // IsFamilyOrAssembly == protected internal
            // IsFamily == protected
            // IsAssembly == internal
            // IsPrivate == private
            return query.Where(x => x.IsFamilyOrAssembly || x.IsFamily || x.IsAssembly || x.IsPrivate);
        }

        return query;
    }

    private static IEnumerable<FieldInfo> FilterOnInstanceAndStatic(IEnumerable<FieldInfo> query, BindingFlags bindingFlags)
    {
        var isInstance = bindingFlags.HasFlag(BindingFlags.Instance);
        var isStatic = bindingFlags.HasFlag(BindingFlags.Static);

        ValidateInstanceAndStaticBindingFlags(isInstance, isStatic);

        if (isInstance && !isStatic)
        {
            return query.Where(x => !x.IsStatic);
        }
        else if (!isInstance && isStatic)
        {
            return query.Where(x => x.IsStatic);
        }

        return query;
    }

    private static IEnumerable<FieldInfo> FilterOnPublicAndNonPublic(IEnumerable<FieldInfo> query, BindingFlags bindingFlags)
    {
        var isPublic = bindingFlags.HasFlag(BindingFlags.Public);
        var isNonPublic = bindingFlags.HasFlag(BindingFlags.NonPublic);

        ValidatePublicAndNonPublicBindingFlags(isPublic, isNonPublic);

        if (isPublic && !isNonPublic)
        {
            return query.Where(x => x.IsPublic);
        }
        else if (!isPublic && isNonPublic)
        {
            // IsFamilyOrAssembly == protected internal
            // IsFamily == protected
            // IsAssembly == internal
            // IsPrivate == private
            return query.Where(x => x.IsFamilyOrAssembly || x.IsFamily || x.IsAssembly || x.IsPrivate);
        }

        return query;
    }

    private static IEnumerable<PropertyInfo> FilterOnInstanceAndStatic(IEnumerable<PropertyInfo> query, BindingFlags bindingFlags)
    {
        var isInstance = bindingFlags.HasFlag(BindingFlags.Instance);
        var isStatic = bindingFlags.HasFlag(BindingFlags.Static);

        ValidateInstanceAndStaticBindingFlags(isInstance, isStatic);

        if (isInstance && !isStatic)
        {
            return query.Where(x => x.SetMethod is { } && x.GetMethod is { } && ((x.CanRead && !x.GetMethod.IsStatic) || (x.CanWrite && !x.SetMethod.IsStatic)));
        }
        else if (!isInstance && isStatic)
        {
            return query.Where(x => x.SetMethod is { } && x.GetMethod is { } && ((x.CanRead && x.GetMethod.IsStatic) || (x.CanWrite && x.SetMethod.IsStatic)));
        }

        return query;
    }

    private static IEnumerable<PropertyInfo> FilterOnPublicAndNonPublic(IEnumerable<PropertyInfo> query, BindingFlags bindingFlags)
    {
        var isPublic = bindingFlags.HasFlag(BindingFlags.Public);
        var isNonPublic = bindingFlags.HasFlag(BindingFlags.NonPublic);

        ValidatePublicAndNonPublicBindingFlags(isPublic, isNonPublic);

        if (isPublic && !isNonPublic)
        {
            return query.Where(x => x.SetMethod is { } && x.GetMethod is { } && ((x.CanRead && x.GetMethod.IsPublic) || (x.CanWrite && x.SetMethod.IsPublic)));
        }
        else if (!isPublic && isNonPublic)
        {
            // IsFamilyOrAssembly == protected internal
            // IsFamily == protected
            // IsAssembly == internal
            // IsPrivate == private
            return query.Where(x => x.SetMethod is { } && x.GetMethod is { } && ((x.CanRead && (x.GetMethod.IsFamilyOrAssembly || x.GetMethod.IsFamily || x.GetMethod.IsAssembly || x.GetMethod.IsPrivate)) ||
                                                                                 (x.CanWrite && (x.SetMethod.IsFamilyOrAssembly || x.SetMethod.IsFamily || x.SetMethod.IsAssembly || x.SetMethod.IsPrivate))));
        }

        return query;
    }

    private static IEnumerable<ConstructorInfo> GetConstructors(TypeInfo typeInfo, BindingFlags bindingFlags, IEnumerable<Type>? parameterTypes)
    {
        if (typeInfo == null)
            return Enumerable.Empty<ConstructorInfo>();

        var constructors = new List<ConstructorInfo>();

        var constructorsToAdd = typeInfo.DeclaredConstructors;

        constructorsToAdd = FilterOnParameterTypes(constructorsToAdd, parameterTypes);
        constructorsToAdd = FilterOnPublicAndNonPublic(constructorsToAdd, bindingFlags);

        constructors.AddRange(constructorsToAdd);

        return constructors;
    }

    private static IEnumerable<FieldInfo> GetFields(TypeInfo typeInfo, string? fieldName, BindingFlags bindingFlags)
    {
        if (typeInfo == null)
            return Enumerable.Empty<FieldInfo>();

        var fields = new List<FieldInfo>();
        while (true)
        {
            var fieldsToAdd = typeInfo.DeclaredFields;

            fieldsToAdd = FilterOnName(fieldsToAdd, fieldName, bindingFlags);
            fieldsToAdd = FilterOnInstanceAndStatic(fieldsToAdd, bindingFlags);
            fieldsToAdd = FilterOnPublicAndNonPublic(fieldsToAdd, bindingFlags);

            fields.AddRange(fieldsToAdd);

            if (bindingFlags.HasFlag(BindingFlags.DeclaredOnly))
            {
                break;
            }

            var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();
            if (baseTypeInfo == null)
            {
                break;
            }

            typeInfo = baseTypeInfo;
        }

        return fields;
    }

    private static IEnumerable<MethodInfo> GetMethods(TypeInfo typeInfo, string? methodName, BindingFlags bindingFlags, IEnumerable<Type>? parameterTypes)
    {
        if (typeInfo == null)
            return Enumerable.Empty<MethodInfo>();

        var methods = new List<MethodInfo>();
        while (true)
        {
            var methodsToAdd = typeInfo.DeclaredMethods;

            methodsToAdd = FilterOnName(methodsToAdd, methodName, bindingFlags);
            methodsToAdd = FilterOnParameterTypes(methodsToAdd, parameterTypes);
            methodsToAdd = FilterOnInstanceAndStatic(methodsToAdd, bindingFlags);
            methodsToAdd = FilterOnPublicAndNonPublic(methodsToAdd, bindingFlags);

            methods.AddRange(methodsToAdd);

            if (bindingFlags.HasFlag(BindingFlags.DeclaredOnly))
            {
                break;
            }

            var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();
            if (baseTypeInfo == null)
            {
                break;
            }

            typeInfo = baseTypeInfo;
        }

        return methods;
    }

    private static IEnumerable<PropertyInfo> GetProperties(TypeInfo typeInfo, string? propertyName, BindingFlags bindingFlags)
    {
        if (typeInfo == null)
            return Enumerable.Empty<PropertyInfo>();

        var properties = new List<PropertyInfo>();
        while (true)
        {
            var propertiesToAdd = typeInfo.DeclaredProperties;

            propertiesToAdd = FilterOnName(propertiesToAdd, propertyName, bindingFlags);
            propertiesToAdd = FilterOnInstanceAndStatic(propertiesToAdd, bindingFlags);
            propertiesToAdd = FilterOnPublicAndNonPublic(propertiesToAdd, bindingFlags);

            properties.AddRange(propertiesToAdd);

            if (bindingFlags.HasFlag(BindingFlags.DeclaredOnly))
            {
                break;
            }

            var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();
            if (baseTypeInfo == null)
            {
                break;
            }

            typeInfo = baseTypeInfo;
        }

        return properties;
    }

    private static bool IsImplementationOf(TypeInfo instanceTypeInfo, TypeInfo interfaceTypeInfo)
    {
        if (instanceTypeInfo == null || interfaceTypeInfo == null)
            return false;

        return interfaceTypeInfo.IsGenericType
            ? instanceTypeInfo.ImplementedInterfaces
                              .Select(x => x.GetTypeInfo())
                              .Any(x => x.IsGenericType && x.GetGenericTypeDefinition().GetTypeInfo().Equals(interfaceTypeInfo))
            : instanceTypeInfo.ImplementedInterfaces
                              .Select(x => x.GetTypeInfo())
                              .Any(x => !x.IsGenericType && x.Equals(interfaceTypeInfo));
    }

    private static bool IsSubclassOrImplementationOf(TypeInfo instanceTypeInfo, TypeInfo baseClassOrInterfaceTypeInfo)
    {
        if (instanceTypeInfo == null || baseClassOrInterfaceTypeInfo == null)
            return false;

        if (instanceTypeInfo.IsSubclassOf(baseClassOrInterfaceTypeInfo.AsType()))
            return true;

        if (IsImplementationOf(instanceTypeInfo, baseClassOrInterfaceTypeInfo))
            return true;

        if (!baseClassOrInterfaceTypeInfo.IsGenericType)
            return false;

        if (instanceTypeInfo.BaseType != null)
        {
            var baseTypeInfo = instanceTypeInfo.BaseType.GetTypeInfo();
            while (!baseTypeInfo.Equals(typeof(object).GetTypeInfo()))
            {
                if (baseClassOrInterfaceTypeInfo.Equals(baseTypeInfo))
                    return true;

                if (baseTypeInfo.IsGenericType)
                {
                    var baseGenericTypeDefinitionInfo = baseTypeInfo.GetGenericTypeDefinition().GetTypeInfo();
                    if (baseClassOrInterfaceTypeInfo.Equals(baseGenericTypeDefinitionInfo))
                        return true;
                }

                if (baseTypeInfo.BaseType != null) baseTypeInfo = baseTypeInfo.BaseType.GetTypeInfo();
            }
        }

        return false;
    }

    private static string RemoveAssemblyDetails(string assemblyQualifiedName)
    {
        // Loop through the type name and filter out qualified assembly
        // details from nested type names.
        var stringBuilder = new StringBuilder();
        var writingAssemblyName = false;
        var skippingAssemblyDetails = false;
        foreach (var current in assemblyQualifiedName)
        {
            switch (current)
            {
                case '[':
                    {
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        stringBuilder.Append(current);
                        break;
                    }

                case ']':
                    {
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        stringBuilder.Append(current);
                        break;
                    }

                case ',':
                    {
                        if (!writingAssemblyName)
                        {
                            writingAssemblyName = true;
                            stringBuilder.Append(current);
                        }
                        else
                        {
                            skippingAssemblyDetails = true;
                        }

                        break;
                    }

                default:
                    {
                        if (!skippingAssemblyDetails)
                        {
                            stringBuilder.Append(current);
                        }

                        break;
                    }
            }
        }

        return stringBuilder.ToString();
    }

    private static void ValidateInstanceAndStaticBindingFlags(bool isInstance, bool isStatic)
    {
        if (isInstance || isStatic)
            return;

        var message = $"{nameof(BindingFlags)} must at least specify either {BindingFlags.Instance} or {BindingFlags.Static}";
        throw new ArgumentException(message);
    }

    private static void ValidatePublicAndNonPublicBindingFlags(bool isPublic, bool isNonPublic)
    {
        if (isPublic || isNonPublic)
            return;

        var message = $"{nameof(BindingFlags)} must at least specify either {BindingFlags.Public} or {BindingFlags.NonPublic}";
        throw new ArgumentException(message);
    }
    #endregion
}
