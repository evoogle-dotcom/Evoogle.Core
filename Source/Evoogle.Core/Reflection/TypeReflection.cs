// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
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
        BindingFlags.DeclaredOnly | BindingFlags.Public;

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
            typeof(Ulid),
            typeof(Uri)
        ];
    #endregion

    #region Constructor Methods
    public static ConstructorInfo? GetConstructor(Type type, params Type[] parameterTypes)
    {
        return GetConstructor(type, DefaultConstructorReflectionFlags, parameterTypes);
    }

    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
        return type.GetConstructor(bindingFlags, parameterTypes ?? EmptyTypes);
    }

    public static ConstructorInfo? GetConstructor(Type type, IEnumerable<Type> parameterTypes)
    {
        return GetConstructor(type, DefaultConstructorReflectionFlags, parameterTypes);
    }

    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes)
    {
        return type.GetConstructor(bindingFlags, [.. parameterTypes ?? EmptyTypes]);
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
    {
        return GetConstructors(type, DefaultConstructorReflectionFlags);
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingFlags)
    {
        return type.GetConstructors(bindingFlags);
    }

    public static ConstructorInfo? GetDefaultConstructor(Type type)
    {
        return GetConstructor(type, DefaultConstructorReflectionFlags, EmptyTypes);
    }

    public static ConstructorInfo? GetDefaultConstructor(Type type, BindingFlags bindingFlags)
    {
        return GetConstructor(type, bindingFlags, EmptyTypes);
    }
    #endregion

    #region Field Methods
    public static FieldInfo? GetField(Type type, string fieldName)
    {
        return GetField(type, fieldName, DefaultFieldReflectionFlags);
    }

    public static FieldInfo? GetField(Type type, string fieldName, BindingFlags bindingFlags)
    {
        var field = type.GetField(fieldName, bindingFlags);
        return field;
    }

    public static IEnumerable<FieldInfo> GetFields(Type type)
    {
        return GetFields(type, DefaultFieldReflectionFlags);
    }

    public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
    {
        var fields = type.GetFields(bindingFlags);
        return fields;
    }
    #endregion

    #region Method Methods
    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName)
    {
        return GetGenericMethodDefinition(type, methodName, DefaultMethodReflectionFlags);
    }

    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, BindingFlags bindingFlags)
    {
        return type
            .GetMethods(bindingFlags)
            .SingleOrDefault(method => method.Name == methodName && method.IsGenericMethodDefinition);
    }

    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, int parameterCount)
    {
        return GetGenericMethodDefinition(type, methodName, DefaultMethodReflectionFlags, parameterCount);
    }

    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, BindingFlags bindingFlags, int parameterCount)
    {
        return type
            .GetMethods(bindingFlags)
            .SingleOrDefault(method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetParameters().Length == parameterCount);
    }

    public static MethodInfo? GetMethod(Type type, string methodName)
    {
        return GetMethod(type, methodName, DefaultMethodReflectionFlags);
    }

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags)
    {
        return type.GetMethod(methodName, bindingFlags);
    }

    public static MethodInfo? GetMethod(Type type, string methodName, params Type[] parameterTypes)
    {
        return GetMethod(type, methodName, DefaultMethodReflectionFlags, parameterTypes);
    }

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
        return type.GetMethod(methodName, bindingFlags, parameterTypes ?? EmptyTypes);
    }

    public static MethodInfo? GetMethod(Type type, string methodName, IEnumerable<Type> parameterTypes)
    {
        return GetMethod(type, methodName, DefaultMethodReflectionFlags, parameterTypes);
    }

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes)
    {
        return type.GetMethod(methodName, bindingFlags, [.. parameterTypes ?? EmptyTypes]);
    }

    public static IEnumerable<MethodInfo> GetMethods(Type type)
    {
        return GetMethods(type, DefaultMethodReflectionFlags);
    }

    public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags)
    {
        var methods = type.GetMethods(bindingFlags);
        return methods;
    }
    #endregion

    #region Miscellaneous Methods
    public static Type? GetBaseType(Type type)
    {
        return type.BaseType;
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
    public static string GetCompactQualifiedName(Type type)
    {
        var assemblyQualifiedName = type.AssemblyQualifiedName ?? throw new NullReferenceException($"{nameof(Type)} property {{Name={nameof(Type.AssemblyQualifiedName)}}} is null.");
        var compactQualifiedName = RemoveAssemblyDetails(assemblyQualifiedName);
        return compactQualifiedName;
    }
    #endregion

    #region Predicate Methods
    /// <summary>
    ///     Predicate if objects of this type can be null or not.
    ///     Works with nullable type definitions.
    ///     Performant with big O notation of O(1) because it uses two checks, both optimized in the .NET runtime.
    /// </summary>
    /// <param name="type">Type object to check if objects of this type can be null or not.</param>
    /// <returns>True if objects of this type can be null, false otherwise.</returns>
    public static bool CanBeNull(Type type)
    {
        if (!type.IsValueType)
            return true; // Reference types can be null

        if (Nullable.GetUnderlyingType(type) != null)
            return true; // Nullable<T> can be null

        return false; // Non-nullable value types (e.g., int, bool) cannot be null
    }

    /// <summary>
    ///     Predicate if objects of this type can be null or not.
    ///     Works with nullable type definitions.
    ///     Performant with big O notation of O(1) because no reflection of typeof(T) needed and inline at compile time for known types.
    /// </summary>
    /// <typeparam name="T">Type object to check if objects of this type can be null or not.</typeparam>
    /// <returns>True if objects of this type can be null, false otherwise.</returns>
    public static bool CanBeNull<T>()
    {
        return default(T) == null;
    }

    public static bool IsAbstract(Type type)
    {
        return type.IsAbstract;
    }

    public static bool IsAssignableFrom(Type type, Type fromType)
    {
        return fromType != null && type.IsAssignableFrom(fromType);
    }

    public static bool IsBoolean(Type type)
    {
        return type == typeof(bool);
    }

    public static bool IsClass(Type type)
    {
        return type.IsClass;
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
        return type.IsEnum;
    }

    public static bool IsEnumerableOfT(Type type)
    {
        return IsEnumerableOfT(type, out _);
    }

    public static bool IsEnumerableOfT(Type type, out Type? enumerableType)
    {
        enumerableType = null;

        if (type == typeof(IEnumerable<>))
        {
            enumerableType = type.GetGenericArguments().FirstOrDefault();
            return true;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            enumerableType = type.GenericTypeArguments.FirstOrDefault();
            return true;
        }

        if (!type.IsGenericType && !type.IsArray)
            return false;

        var enumerableGenericTypeArguments = type
            .GetInterfaces()
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
        return type.IsGenericTypeDefinition;
    }

    public static bool IsGenericType(Type type)
    {
        return type.IsGenericType;
    }

    public static bool IsGuid(Type type)
    {
        return type == typeof(Guid);
    }

    public static bool IsImplementationOf(Type type, Type interfaceType)
    {
        if (type == null || interfaceType == null)
            return false;

        return interfaceType.IsGenericType
            ? type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(interfaceType))
            : type.GetInterfaces().Any(x => !x.IsGenericType && x.Equals(interfaceType));
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
        return type.IsPrimitive || PrimitiveTypes.Contains(type);
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
        return baseClass != null && type.IsSubclassOf(baseClass);
    }

    public static bool IsSubclassOrImplementationOf(Type type, Type baseClassOrInterfaceType)
    {
        if (type.IsSubclassOf(baseClassOrInterfaceType))
            return true;

        if (IsImplementationOf(type, baseClassOrInterfaceType))
            return true;

        if (!baseClassOrInterfaceType.IsGenericType)
            return false;

        if (type.BaseType != null)
        {
            var baseType = type.BaseType;
            while (!baseType.Equals(typeof(object)))
            {
                if (baseClassOrInterfaceType.Equals(baseType))
                    return true;

                if (baseType.IsGenericType)
                {
                    var baseGenericTypeDefinition = baseType.GetGenericTypeDefinition();
                    if (baseClassOrInterfaceType.Equals(baseGenericTypeDefinition))
                        return true;
                }

                if (baseType.BaseType != null)
                    baseType = baseType.BaseType;
            }
        }

        return false;
    }

    public static bool IsValueType(Type type)
    {
        return type.IsValueType;
    }

    public static bool IsVoid(Type type)
    {
        return type == typeof(void);
    }
    #endregion

    #region Property Methods
    public static PropertyInfo? GetProperty(Type type, string propertyName)
    {
        return GetProperty(type, propertyName, DefaultPropertyReflectionFlags);
    }

    public static PropertyInfo? GetProperty(Type type, string propertyName, BindingFlags bindingFlags)
    {
        var property = type.GetProperty(propertyName, bindingFlags);
        return property;
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        return GetProperties(type, DefaultPropertyReflectionFlags);
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags bindingFlags)
    {
        var properties = type.GetProperties(bindingFlags);
        return properties;
    }
    #endregion

    #region Methods
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
    #endregion
}
