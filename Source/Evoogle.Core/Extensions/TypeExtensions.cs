// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Reflection;
using System.Text;

namespace Evoogle;

/// <summary>
///     Extension methods for .NET <see cref="Type"> class.
/// </summary>
public static class TypeExtensions
{
    #region Fields
    private const TypeReflectionFlags DefaultConstructorReflectionFlags =
        TypeReflectionFlags.Public;

    private const TypeReflectionFlags DefaultFieldReflectionFlags =
        TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static;

    private const TypeReflectionFlags DefaultMethodReflectionFlags =
        TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static;

    private const TypeReflectionFlags DefaultPropertyReflectionFlags =
        TypeReflectionFlags.DeclaredOnly | TypeReflectionFlags.Public | TypeReflectionFlags.Instance | TypeReflectionFlags.Static;

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
    public static ConstructorInfo? GetConstructor(this Type type, params Type[] parameterTypes)
    {
        if (parameterTypes == null || parameterTypes.Length == 0)
            return GetDefaultConstructor(type, DefaultConstructorReflectionFlags);

        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetConstructor(this Type type, TypeReflectionFlags reflectionFlags, params Type[] parameterTypes)
    {
        if (parameterTypes == null || parameterTypes.Length == 0)
            return GetDefaultConstructor(type, reflectionFlags);

        return GetConstructors(type.GetTypeInfo(), reflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetConstructor(this Type type, IEnumerable<Type> parameterTypes)
    {
        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetConstructor(this Type type, TypeReflectionFlags reflectionFlags, IEnumerable<Type> parameterTypes)
    {
        return GetConstructors(type.GetTypeInfo(), reflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetDefaultConstructor(this Type type)
    {
        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, EmptyTypes).SingleOrDefault();
    }

    public static ConstructorInfo? GetDefaultConstructor(this Type type, TypeReflectionFlags reflectionFlags)
    {
        return GetConstructors(type.GetTypeInfo(), reflectionFlags, EmptyTypes).SingleOrDefault();
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
    {
        return GetConstructors(type.GetTypeInfo(), DefaultConstructorReflectionFlags, null);
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(this Type type, TypeReflectionFlags reflectionFlags)
    {
        return GetConstructors(type.GetTypeInfo(), reflectionFlags, null);
    }
    #endregion

    #region Field Methods
    public static FieldInfo? GetField(this Type type, string fieldName)
    {
        return GetFields(type.GetTypeInfo(), fieldName, DefaultFieldReflectionFlags).SingleOrDefault();
    }

    public static FieldInfo? GetField(this Type type, string fieldName, TypeReflectionFlags reflectionFlags)
    {
        return GetFields(type.GetTypeInfo(), fieldName, reflectionFlags).SingleOrDefault();
    }

    public static IEnumerable<FieldInfo> GetFields(this Type type)
    {
        return GetFields(type.GetTypeInfo(), null, DefaultFieldReflectionFlags);
    }

    public static IEnumerable<FieldInfo> GetFields(this Type type, TypeReflectionFlags reflectionFlags)
    {
        return GetFields(type.GetTypeInfo(), null, reflectionFlags);
    }
    #endregion

    #region Method Methods
    public static MethodInfo? GetMethod(this Type type, string methodName)
    {
        return GetMethods(type.GetTypeInfo(), methodName, DefaultMethodReflectionFlags, EmptyTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, params Type[] parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, DefaultMethodReflectionFlags, parameterTypes ?? EmptyTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, TypeReflectionFlags reflectionFlags, params Type[] parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, reflectionFlags, parameterTypes ?? EmptyTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, IEnumerable<Type> parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, DefaultMethodReflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, TypeReflectionFlags reflectionFlags, IEnumerable<Type> parameterTypes)
    {
        return GetMethods(type.GetTypeInfo(), methodName, reflectionFlags, parameterTypes).SingleOrDefault();
    }

    public static IEnumerable<MethodInfo> GetMethods(this Type type)
    {
        return GetMethods(type.GetTypeInfo(), null, DefaultMethodReflectionFlags, null);
    }

    public static IEnumerable<MethodInfo> GetMethods(this Type type, TypeReflectionFlags reflectionFlags)
    {
        return GetMethods(type.GetTypeInfo(), null, reflectionFlags, null);
    }
    #endregion

    #region Miscellaneous Methods
    public static Type? GetBaseType(this Type type)
    {
        return type.GetTypeInfo().BaseType;
    }

    public static IEnumerable<Type> GetBaseTypes(this Type type)
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
    public static string? GetCompactQualifiedName(this Type type)
    {
        var assemblyQualifiedName = type.AssemblyQualifiedName;
        if (assemblyQualifiedName == null)
            return null;

        var compactQualifiedName = RemoveAssemblyDetails(assemblyQualifiedName);
        return compactQualifiedName;
    }
    #endregion

    #region Predicate Methods
    public static bool IsAbstract(this Type type)
    {
        return type.GetTypeInfo().IsAbstract;
    }

    public static bool IsAssignableFrom(this Type type, Type fromType)
    {
        return fromType != null && type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
    }

    public static bool IsBoolean(this Type type)
    {
        return type == typeof(bool);
    }

    public static bool IsClass(this Type type)
    {
        return type.GetTypeInfo().IsClass;
    }

    /// <summary>
    ///     A complex type is a type that cannot be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type cannot be converted with a type converter, false otherwise.</returns>
    public static bool IsComplex(this Type type)
    {
        return !IsSimple(type);
    }

    public static bool IsEnum(this Type type)
    {
        return type.GetTypeInfo().IsEnum;
    }

    public static bool IsEnumerableOfT(this Type type)
    {
        return IsEnumerableOfT(type, out _);
    }

    public static bool IsEnumerableOfT(this Type type, out Type? enumerableType)
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
            throw new InvalidOperationException($"CLR type {{Name={type.Name}}} implements multiple versions of IEnumerable<T>.");

        enumerableType = enumerableGenericTypeArguments[0];
        return true;
    }

    public static bool IsFloatingPoint(this Type type)
    {
        return FloatingPointTypes.Contains(type);
    }

    public static bool IsGenericTypeDefinition(this Type type)
    {
        return type.GetTypeInfo().IsGenericTypeDefinition;
    }

    public static bool IsGenericType(this Type type)
    {
        return type.GetTypeInfo().IsGenericType;
    }

    public static bool IsGuid(this Type type)
    {
        return type == typeof(Guid);
    }

    public static bool IsImplementationOf(this Type type, Type interfaceType)
    {
        return interfaceType != null && IsImplementationOf(type.GetTypeInfo(), interfaceType.GetTypeInfo());
    }

    public static bool IsInteger(this Type type)
    {
        return IntegerTypes.Contains(type);
    }

    public static bool IsNullableType(this Type type)
    {
        return IsGenericType(type) && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static bool IsNullableEnum(this Type type)
    {
        var isNullableType = IsNullableType(type);
        if (!isNullableType)
            return false;

        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (nullableUnderlyingType == null)
            return false;

        return IsEnum(nullableUnderlyingType);
    }

    public static bool IsNumber(this Type type)
    {
        return IsInteger(type) || IsFloatingPoint(type);
    }

    public static bool IsPrimitive(this Type type)
    {
        return type.GetTypeInfo().IsPrimitive || PrimitiveTypes.Contains(type);
    }

    /// <summary>
    ///     A simple type is a type that can be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type can be converted with a type converter, false otherwise.</returns>
    public static bool IsSimple(this Type type)
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

    public static bool IsString(this Type type)
    {
        return type == typeof(string);
    }

    public static bool IsSubclassOf(this Type type, Type baseClass)
    {
        return baseClass != null && type.GetTypeInfo().IsSubclassOf(baseClass);
    }

    public static bool IsSubclassOrImplementationOf(this Type type, Type baseClassOrInterfaceType)
    {
        return baseClassOrInterfaceType != null && IsSubclassOrImplementationOf(type.GetTypeInfo(), baseClassOrInterfaceType.GetTypeInfo());
    }

    public static bool IsValueType(this Type type)
    {
        return type.GetTypeInfo().IsValueType;
    }

    public static bool IsVoid(this Type type)
    {
        return type == typeof(void);
    }
    #endregion

    #region Property Methods
    public static PropertyInfo? GetProperty(this Type type, string propertyName)
    {
        return GetProperties(type.GetTypeInfo(), propertyName, DefaultPropertyReflectionFlags).SingleOrDefault();
    }

    public static PropertyInfo? GetProperty(this Type type, string propertyName, TypeReflectionFlags reflectionFlags)
    {
        return GetProperties(type.GetTypeInfo(), propertyName, reflectionFlags).SingleOrDefault();
    }

    public static IEnumerable<PropertyInfo> GetProperties(this Type type)
    {
        return GetProperties(type.GetTypeInfo(), null, DefaultPropertyReflectionFlags);
    }

    public static IEnumerable<PropertyInfo> GetProperties(this Type type, TypeReflectionFlags reflectionFlags)
    {
        return GetProperties(type.GetTypeInfo(), null, reflectionFlags);
    }
    #endregion

    #region Methods
    private static IEnumerable<T> FilterOnName<T>(IEnumerable<T> query, string? name, TypeReflectionFlags reflectionFlags)
        where T : MemberInfo
    {
        if (string.IsNullOrWhiteSpace(name))
            return query;

        var comparisonType = reflectionFlags.HasFlag(TypeReflectionFlags.IgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return query.Where(x => string.Equals(x.Name, name, comparisonType));
    }

    private static IEnumerable<T> FilterOnParameterTypes<T>(IEnumerable<T> query, IEnumerable<Type>? parameterTypes)
        where T : MethodBase
    {
        if (parameterTypes == null)
            return query;

        return query.Where(x => x.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
    }

    private static IEnumerable<T> FilterOnInstanceAndStatic<T>(IEnumerable<T> query, TypeReflectionFlags reflectionFlags)
        where T : MethodBase
    {
        var isInstance = reflectionFlags.HasFlag(TypeReflectionFlags.Instance);
        var isStatic = reflectionFlags.HasFlag(TypeReflectionFlags.Static);

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

    private static IEnumerable<T> FilterOnPublicAndNonPublic<T>(IEnumerable<T> query, TypeReflectionFlags reflectionFlags)
        where T : MethodBase
    {
        var isPublic = reflectionFlags.HasFlag(TypeReflectionFlags.Public);
        var isNonPublic = reflectionFlags.HasFlag(TypeReflectionFlags.NonPublic);

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

    private static IEnumerable<FieldInfo> FilterOnInstanceAndStatic(IEnumerable<FieldInfo> query, TypeReflectionFlags reflectionFlags)
    {
        var isInstance = reflectionFlags.HasFlag(TypeReflectionFlags.Instance);
        var isStatic = reflectionFlags.HasFlag(TypeReflectionFlags.Static);

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

    private static IEnumerable<FieldInfo> FilterOnPublicAndNonPublic(IEnumerable<FieldInfo> query, TypeReflectionFlags reflectionFlags)
    {
        var isPublic = reflectionFlags.HasFlag(TypeReflectionFlags.Public);
        var isNonPublic = reflectionFlags.HasFlag(TypeReflectionFlags.NonPublic);

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

    private static IEnumerable<PropertyInfo> FilterOnInstanceAndStatic(IEnumerable<PropertyInfo> query, TypeReflectionFlags reflectionFlags)
    {
        var isInstance = reflectionFlags.HasFlag(TypeReflectionFlags.Instance);
        var isStatic = reflectionFlags.HasFlag(TypeReflectionFlags.Static);

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

    private static IEnumerable<PropertyInfo> FilterOnPublicAndNonPublic(IEnumerable<PropertyInfo> query, TypeReflectionFlags reflectionFlags)
    {
        var isPublic = reflectionFlags.HasFlag(TypeReflectionFlags.Public);
        var isNonPublic = reflectionFlags.HasFlag(TypeReflectionFlags.NonPublic);

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

    private static IEnumerable<ConstructorInfo> GetConstructors(TypeInfo typeInfo, TypeReflectionFlags reflectionFlags, IEnumerable<Type>? parameterTypes)
    {
        if (typeInfo == null)
            return Enumerable.Empty<ConstructorInfo>();

        var constructors = new List<ConstructorInfo>();

        var constructorsToAdd = typeInfo.DeclaredConstructors;

        constructorsToAdd = FilterOnParameterTypes(constructorsToAdd, parameterTypes);
        constructorsToAdd = FilterOnPublicAndNonPublic(constructorsToAdd, reflectionFlags);

        constructors.AddRange(constructorsToAdd);

        return constructors;
    }

    private static IEnumerable<FieldInfo> GetFields(TypeInfo typeInfo, string? fieldName, TypeReflectionFlags reflectionFlags)
    {
        if (typeInfo == null)
            return Enumerable.Empty<FieldInfo>();

        var fields = new List<FieldInfo>();
        while (true)
        {
            var fieldsToAdd = typeInfo.DeclaredFields;

            fieldsToAdd = FilterOnName(fieldsToAdd, fieldName, reflectionFlags);
            fieldsToAdd = FilterOnInstanceAndStatic(fieldsToAdd, reflectionFlags);
            fieldsToAdd = FilterOnPublicAndNonPublic(fieldsToAdd, reflectionFlags);

            fields.AddRange(fieldsToAdd);

            if (reflectionFlags.HasFlag(TypeReflectionFlags.DeclaredOnly))
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

    private static IEnumerable<MethodInfo> GetMethods(TypeInfo typeInfo, string? methodName, TypeReflectionFlags reflectionFlags, IEnumerable<Type>? parameterTypes)
    {
        if (typeInfo == null)
            return Enumerable.Empty<MethodInfo>();

        var methods = new List<MethodInfo>();
        while (true)
        {
            var methodsToAdd = typeInfo.DeclaredMethods;

            methodsToAdd = FilterOnName(methodsToAdd, methodName, reflectionFlags);
            methodsToAdd = FilterOnParameterTypes(methodsToAdd, parameterTypes);
            methodsToAdd = FilterOnInstanceAndStatic(methodsToAdd, reflectionFlags);
            methodsToAdd = FilterOnPublicAndNonPublic(methodsToAdd, reflectionFlags);

            methods.AddRange(methodsToAdd);

            if (reflectionFlags.HasFlag(TypeReflectionFlags.DeclaredOnly))
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

    private static IEnumerable<PropertyInfo> GetProperties(TypeInfo typeInfo, string? propertyName, TypeReflectionFlags reflectionFlags)
    {
        if (typeInfo == null)
            return Enumerable.Empty<PropertyInfo>();

        var properties = new List<PropertyInfo>();
        while (true)
        {
            var propertiesToAdd = typeInfo.DeclaredProperties;

            propertiesToAdd = FilterOnName(propertiesToAdd, propertyName, reflectionFlags);
            propertiesToAdd = FilterOnInstanceAndStatic(propertiesToAdd, reflectionFlags);
            propertiesToAdd = FilterOnPublicAndNonPublic(propertiesToAdd, reflectionFlags);

            properties.AddRange(propertiesToAdd);

            if (reflectionFlags.HasFlag(TypeReflectionFlags.DeclaredOnly))
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

        const string bindingFlagsName = nameof(TypeReflectionFlags);
        var message = $"{bindingFlagsName} must at least specify either {TypeReflectionFlags.Instance} or {TypeReflectionFlags.Static}";

        throw new ArgumentException(message);
    }

    private static void ValidatePublicAndNonPublicBindingFlags(bool isPublic, bool isNonPublic)
    {
        if (isPublic || isNonPublic)
            return;

        const string bindingFlagsName = nameof(TypeReflectionFlags);
        var message = $"{bindingFlagsName} must at least specify either {TypeReflectionFlags.Public} or {TypeReflectionFlags.NonPublic}";

        throw new ArgumentException(message);
    }
    #endregion
}
