// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Evoogle.Reflection;

/// <summary>
///     Reflection methods for the .NET <see cref="Type"> class.
/// </summary>
public static class TypeReflection
{
    #region Fields
    private const BindingFlags _defaultConstructorReflectionFlags = BindingFlags.DeclaredOnly | BindingFlags.Public;

    private const BindingFlags _defaultFieldReflectionFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    private const BindingFlags _defaultMethodReflectionFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    private const BindingFlags _defaultPropertyReflectionFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    private static readonly Type[] _emptyTypes = Type.EmptyTypes;

    private static readonly HashSet<Type> _floatingPointTypes =
        [
            typeof(decimal),
            typeof(double),
            typeof(float)
        ];

    private static readonly HashSet<Type> _integerTypes =
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

    private static readonly HashSet<Type> _primitiveTypes =
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

    private static readonly ConcurrentDictionary<Type, bool> _isComplexCache = new();

    private static readonly ConcurrentDictionary<Type, (bool IsEnumerable, Type? ElementType)> _isEnumerableOfTCache = new();

    private static readonly ConcurrentDictionary<Type, bool> _isSimpleCache = new();

    private static readonly ConcurrentDictionary<Type, bool> _isNullableEnumCache = new();

    private static readonly ConcurrentDictionary<Type, bool> _isNullableTypeCache = new();
    #endregion

    #region Constructor Methods
    public static ConstructorInfo? GetConstructor(Type type, params Type[] parameterTypes) => GetConstructor(type, _defaultConstructorReflectionFlags, parameterTypes);

    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, params Type[] parameterTypes) => type.GetConstructor(bindingFlags, parameterTypes ?? _emptyTypes);

    public static ConstructorInfo? GetConstructor(Type type, IEnumerable<Type> parameterTypes) => GetConstructor(type, _defaultConstructorReflectionFlags, parameterTypes);

    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes) => type.GetConstructor(bindingFlags, [.. parameterTypes ?? _emptyTypes]);

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type) => GetConstructors(type, _defaultConstructorReflectionFlags);

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingFlags) => type.GetConstructors(bindingFlags);

    public static ConstructorInfo? GetDefaultConstructor(Type type) => GetConstructor(type, _defaultConstructorReflectionFlags, _emptyTypes);

    public static ConstructorInfo? GetDefaultConstructor(Type type, BindingFlags bindingFlags) => GetConstructor(type, bindingFlags, _emptyTypes);
    #endregion

    #region Field Methods
    public static FieldInfo? GetField(Type type, string fieldName) => GetField(type, fieldName, _defaultFieldReflectionFlags);

    public static FieldInfo? GetField(Type type, string fieldName, BindingFlags bindingFlags) => type.GetField(fieldName, bindingFlags);

    public static IEnumerable<FieldInfo> GetFields(Type type) => GetFields(type, _defaultFieldReflectionFlags);

    public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags) => type.GetFields(bindingFlags);
    #endregion

    #region Method Methods
    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName) => GetGenericMethodDefinition(type, methodName, _defaultMethodReflectionFlags);

    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, BindingFlags bindingFlags) => type.GetMethods(bindingFlags).SingleOrDefault(method => method.Name == methodName && method.IsGenericMethodDefinition);

    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, int parameterCount) => GetGenericMethodDefinition(type, methodName, _defaultMethodReflectionFlags, parameterCount);

    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, BindingFlags bindingFlags, int parameterCount) => type.GetMethods(bindingFlags).SingleOrDefault(method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetParameters().Length == parameterCount);

    public static MethodInfo? GetMethod(Type type, string methodName) => GetMethod(type, methodName, _defaultMethodReflectionFlags);

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags) => type.GetMethod(methodName, bindingFlags);

    public static MethodInfo? GetMethod(Type type, string methodName, params Type[] parameterTypes) => GetMethod(type, methodName, _defaultMethodReflectionFlags, parameterTypes);

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, params Type[] parameterTypes) => type.GetMethod(methodName, bindingFlags, parameterTypes ?? _emptyTypes);

    public static MethodInfo? GetMethod(Type type, string methodName, IEnumerable<Type> parameterTypes) => GetMethod(type, methodName, _defaultMethodReflectionFlags, parameterTypes);

    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes) => type.GetMethod(methodName, bindingFlags, [.. parameterTypes ?? _emptyTypes]);

    public static IEnumerable<MethodInfo> GetMethods(Type type) => GetMethods(type, _defaultMethodReflectionFlags);

    public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags) => type.GetMethods(bindingFlags);
    #endregion

    #region Miscellaneous Methods
    public static Type? GetBaseType(Type type) => type.BaseType;

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
    public static bool CanBeNull<T>() => default(T) == null;

    public static bool IsAbstract(Type type) => type.IsAbstract;

    public static bool IsAssignableFrom(Type type, Type fromType) => fromType != null && type.IsAssignableFrom(fromType);

    public static bool IsBoolean(Type type) => type == typeof(bool);

    public static bool IsClass(Type type) => type.IsClass;

    /// <summary>
    ///     A complex type is a type that cannot be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type cannot be converted with a type converter, false otherwise.</returns>
    public static bool IsComplex(Type type) => _isComplexCache.GetOrAdd(type, t => !IsSimple(t));

    public static bool IsEnum(Type type) => type.IsEnum;

    public static bool IsEnumerableOfT(Type type) => IsEnumerableOfT(type, out _);

    public static bool IsEnumerableOfT(Type type, [NotNullWhen(true)] out Type? elementType)
    {
        var (IsEnumerable, ElementType) = _isEnumerableOfTCache.GetOrAdd(type, static t =>
        {
            if (t == typeof(IEnumerable<>))
                return (true, t.GetGenericArguments().FirstOrDefault());

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return (true, t.GenericTypeArguments.FirstOrDefault());

            if (!t.IsGenericType && !t.IsArray)
                return (false, null);

            var candidates = t.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(x => x.GenericTypeArguments.FirstOrDefault())
                .Where(x => x != null)
                .ToList();

            if (candidates.Count == 1)
                return (true, candidates[0]);

            if (candidates.Count > 1)
                throw new InvalidOperationException($"Type {t.Name} implements multiple IEnumerable<T> interfaces.");

            return (false, null);
        });

        elementType = ElementType;
        return IsEnumerable;
    }

    public static bool IsFloatingPoint(Type type) => _floatingPointTypes.Contains(type);

    public static bool IsGenericTypeDefinition(Type type) => type.IsGenericTypeDefinition;

    public static bool IsGenericType(Type type) => type.IsGenericType;

    public static bool IsGuid(Type type) => type == typeof(Guid);

    public static bool IsImplementationOf(Type type, Type interfaceType)
    {
        if (type == null || interfaceType == null)
            return false;

        return interfaceType.IsGenericType
            ? type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(interfaceType))
            : type.GetInterfaces().Any(x => !x.IsGenericType && x.Equals(interfaceType));
    }

    public static bool IsInteger(Type type) => _integerTypes.Contains(type);

    public static bool IsNullableType(Type type) => _isNullableTypeCache.GetOrAdd(type, t => IsGenericType(t) && t.GetGenericTypeDefinition() == typeof(Nullable<>));

    public static bool IsNullableEnum(Type type)
    {
        return _isNullableEnumCache.GetOrAdd(type, static t =>
        {
            if (!IsNullableType(t))
                return false;

            var nullableUnderlyingType = Nullable.GetUnderlyingType(t);
            if (nullableUnderlyingType == null)
                return false;

            return IsEnum(nullableUnderlyingType);
        });
    }

    public static bool IsNumber(Type type) => IsInteger(type) || IsFloatingPoint(type);

    public static bool IsPrimitive(Type type) => type.IsPrimitive || _primitiveTypes.Contains(type);

    /// <summary>
    ///     A simple type is a type that can be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type can be converted with a type converter, false otherwise.</returns>
    public static bool IsSimple(Type type)
    {
        return _isSimpleCache.GetOrAdd(type, static t =>
        {
            while (true)
            {
                if (IsPrimitive(t))
                    return true;

                if (IsNullableType(t))
                {
                    var nullableUnderlyingType = Nullable.GetUnderlyingType(t);
                    if (nullableUnderlyingType == null)
                        return false;

                    t = nullableUnderlyingType;
                    continue;
                }

                if (IsEnum(t))
                {
                    var enumUnderlyingType = Enum.GetUnderlyingType(t);
                    if (enumUnderlyingType == null)
                        return false;

                    t = enumUnderlyingType;
                    continue;
                }

                return false;
            }
        });
    }

    public static bool IsString(Type type) => type == typeof(string);

    public static bool IsSubclassOf(Type type, Type baseClass) => baseClass != null && type.IsSubclassOf(baseClass);

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

    public static bool IsValueType(Type type) => type.IsValueType;

    public static bool IsVoid(Type type) => type == typeof(void);
    #endregion

    #region Property Methods
    public static PropertyInfo? GetProperty(Type type, string propertyName) => GetProperty(type, propertyName, _defaultPropertyReflectionFlags);

    public static PropertyInfo? GetProperty(Type type, string propertyName, BindingFlags bindingFlags) => type.GetProperty(propertyName, bindingFlags);

    public static IEnumerable<PropertyInfo> GetProperties(Type type) => GetProperties(type, _defaultPropertyReflectionFlags);

    public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags bindingFlags) => type.GetProperties(bindingFlags);
    #endregion

    #region Implementation Methods
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
