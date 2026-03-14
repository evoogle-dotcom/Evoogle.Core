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
///     Reflection methods for the .NET <see cref="Type"/> class.
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
    /// <summary>Gets the public declared constructor of <paramref name="type"/> matching the given parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="parameterTypes">The parameter types of the desired constructor.</param>
    /// <returns>The matching <see cref="ConstructorInfo"/>, or <see langword="null"/> if not found.</returns>
    public static ConstructorInfo? GetConstructor(Type type, params Type[] parameterTypes) => GetConstructor(type, _defaultConstructorReflectionFlags, parameterTypes);

    /// <summary>Gets the constructor of <paramref name="type"/> matching the given binding flags and parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter constructors.</param>
    /// <param name="parameterTypes">The parameter types of the desired constructor.</param>
    /// <returns>The matching <see cref="ConstructorInfo"/>, or <see langword="null"/> if not found.</returns>
    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, params Type[] parameterTypes) => type.GetConstructor(bindingFlags, parameterTypes ?? _emptyTypes);

    /// <summary>Gets the public declared constructor of <paramref name="type"/> matching the given parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="parameterTypes">The parameter types of the desired constructor.</param>
    /// <returns>The matching <see cref="ConstructorInfo"/>, or <see langword="null"/> if not found.</returns>
    public static ConstructorInfo? GetConstructor(Type type, IEnumerable<Type> parameterTypes) => GetConstructor(type, _defaultConstructorReflectionFlags, parameterTypes);

    /// <summary>Gets the constructor of <paramref name="type"/> matching the given binding flags and parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter constructors.</param>
    /// <param name="parameterTypes">The parameter types of the desired constructor.</param>
    /// <returns>The matching <see cref="ConstructorInfo"/>, or <see langword="null"/> if not found.</returns>
    public static ConstructorInfo? GetConstructor(Type type, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes) => type.GetConstructor(bindingFlags, [.. parameterTypes ?? _emptyTypes]);

    /// <summary>Gets all public declared constructors of <paramref name="type"/>.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <returns>A sequence of <see cref="ConstructorInfo"/> objects.</returns>
    public static IEnumerable<ConstructorInfo> GetConstructors(Type type) => GetConstructors(type, _defaultConstructorReflectionFlags);

    /// <summary>Gets all constructors of <paramref name="type"/> matching the given binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter constructors.</param>
    /// <returns>A sequence of <see cref="ConstructorInfo"/> objects.</returns>
    public static IEnumerable<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingFlags) => type.GetConstructors(bindingFlags);

    /// <summary>Gets the public declared default (parameterless) constructor of <paramref name="type"/>.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <returns>The default <see cref="ConstructorInfo"/>, or <see langword="null"/> if not found.</returns>
    public static ConstructorInfo? GetDefaultConstructor(Type type) => GetConstructor(type, _defaultConstructorReflectionFlags, _emptyTypes);

    /// <summary>Gets the default (parameterless) constructor of <paramref name="type"/> matching the given binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter the constructor.</param>
    /// <returns>The default <see cref="ConstructorInfo"/>, or <see langword="null"/> if not found.</returns>
    public static ConstructorInfo? GetDefaultConstructor(Type type, BindingFlags bindingFlags) => GetConstructor(type, bindingFlags, _emptyTypes);
    #endregion

    #region Field Methods
    /// <summary>Gets the public declared field of <paramref name="type"/> with the given name.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The matching <see cref="FieldInfo"/>, or <see langword="null"/> if not found.</returns>
    public static FieldInfo? GetField(Type type, string fieldName) => GetField(type, fieldName, _defaultFieldReflectionFlags);

    /// <summary>Gets the field of <paramref name="type"/> with the given name and binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="fieldName">The name of the field.</param>
    /// <param name="bindingFlags">The binding flags to filter fields.</param>
    /// <returns>The matching <see cref="FieldInfo"/>, or <see langword="null"/> if not found.</returns>
    public static FieldInfo? GetField(Type type, string fieldName, BindingFlags bindingFlags) => type.GetField(fieldName, bindingFlags);

    /// <summary>Gets all public declared fields of <paramref name="type"/>.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <returns>A sequence of <see cref="FieldInfo"/> objects.</returns>
    public static IEnumerable<FieldInfo> GetFields(Type type) => GetFields(type, _defaultFieldReflectionFlags);

    /// <summary>Gets all fields of <paramref name="type"/> matching the given binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter fields.</param>
    /// <returns>A sequence of <see cref="FieldInfo"/> objects.</returns>
    public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags) => type.GetFields(bindingFlags);
    #endregion

    #region Method Methods
    /// <summary>Gets the generic method definition on <paramref name="type"/> with the given name.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName) => GetGenericMethodDefinition(type, methodName, _defaultMethodReflectionFlags);

    /// <summary>Gets the generic method definition on <paramref name="type"/> with the given name and binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags to filter methods.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, BindingFlags bindingFlags) => type.GetMethods(bindingFlags).SingleOrDefault(method => method.Name == methodName && method.IsGenericMethodDefinition);

    /// <summary>Gets the generic method definition on <paramref name="type"/> with the given name and parameter count.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="parameterCount">The required number of parameters.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, int parameterCount) => GetGenericMethodDefinition(type, methodName, _defaultMethodReflectionFlags, parameterCount);

    /// <summary>Gets the generic method definition on <paramref name="type"/> with the given name, binding flags, and parameter count.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags to filter methods.</param>
    /// <param name="parameterCount">The required number of parameters.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetGenericMethodDefinition(Type type, string methodName, BindingFlags bindingFlags, int parameterCount) => type.GetMethods(bindingFlags).SingleOrDefault(method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetParameters().Length == parameterCount);

    /// <summary>Gets the public declared method on <paramref name="type"/> with the given name.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetMethod(Type type, string methodName) => GetMethod(type, methodName, _defaultMethodReflectionFlags);

    /// <summary>Gets the method on <paramref name="type"/> with the given name and binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags to filter methods.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags) => type.GetMethod(methodName, bindingFlags);

    /// <summary>Gets the public declared method on <paramref name="type"/> with the given name and parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="parameterTypes">The parameter types of the desired method.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetMethod(Type type, string methodName, params Type[] parameterTypes) => GetMethod(type, methodName, _defaultMethodReflectionFlags, parameterTypes);

    /// <summary>Gets the method on <paramref name="type"/> with the given name, binding flags, and parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags to filter methods.</param>
    /// <param name="parameterTypes">The parameter types of the desired method.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, params Type[] parameterTypes) => type.GetMethod(methodName, bindingFlags, parameterTypes ?? _emptyTypes);

    /// <summary>Gets the public declared method on <paramref name="type"/> with the given name and parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="parameterTypes">The parameter types of the desired method.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetMethod(Type type, string methodName, IEnumerable<Type> parameterTypes) => GetMethod(type, methodName, _defaultMethodReflectionFlags, parameterTypes);

    /// <summary>Gets the method on <paramref name="type"/> with the given name, binding flags, and parameter types.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <param name="bindingFlags">The binding flags to filter methods.</param>
    /// <param name="parameterTypes">The parameter types of the desired method.</param>
    /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> if not found.</returns>
    public static MethodInfo? GetMethod(Type type, string methodName, BindingFlags bindingFlags, IEnumerable<Type> parameterTypes) => type.GetMethod(methodName, bindingFlags, [.. parameterTypes ?? _emptyTypes]);

    /// <summary>Gets all public declared methods of <paramref name="type"/>.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <returns>A sequence of <see cref="MethodInfo"/> objects.</returns>
    public static IEnumerable<MethodInfo> GetMethods(Type type) => GetMethods(type, _defaultMethodReflectionFlags);

    /// <summary>Gets all methods of <paramref name="type"/> matching the given binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter methods.</param>
    /// <returns>A sequence of <see cref="MethodInfo"/> objects.</returns>
    public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags) => type.GetMethods(bindingFlags);
    #endregion

    #region Miscellaneous Methods
    /// <summary>Gets the direct base type of <paramref name="type"/>, or <see langword="null"/> if none.</summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns>The base <see cref="Type"/>, or <see langword="null"/>.</returns>
    public static Type? GetBaseType(Type type) => type.BaseType;

    /// <summary>Enumerates all base types of <paramref name="type"/> up the inheritance chain, excluding <see cref="object"/>.</summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns>A sequence of base <see cref="Type"/> objects from immediate parent up to (but not including) <see cref="object"/>.</returns>
    public static IEnumerable<Type> GetBaseTypes(Type type)
    {
        var baseType = GetBaseType(type);
        if (baseType == null)
        {
            yield break;
        }

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
    /// <returns>A compact assembly-qualified name string suitable for use with <see cref="Type.GetType(string)"/>.</returns>
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
        {
            return true; // Reference types can be null
        }

        if (Nullable.GetUnderlyingType(type) != null)
        {
            return true; // Nullable<T> can be null
        }

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

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is abstract.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsAbstract(Type type) => type.IsAbstract;

    /// <summary>Returns <see langword="true"/> if <paramref name="fromType"/> is assignable to <paramref name="type"/>.</summary>
    /// <param name="type">The target type.</param>
    /// <param name="fromType">The source type to check.</param>
    public static bool IsAssignableFrom(Type type, Type fromType) => fromType != null && type.IsAssignableFrom(fromType);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is <see cref="bool"/>.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsBoolean(Type type) => type == typeof(bool);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a class.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsClass(Type type) => type.IsClass;

    /// <summary>
    ///     A complex type is a type that cannot be converted with default "type converters".
    /// </summary>
    /// <param name="type">.NET type to call extension method on.</param>
    /// <returns>True is this type cannot be converted with a type converter, false otherwise.</returns>
    public static bool IsComplex(Type type) => _isComplexCache.GetOrAdd(type, t => !IsSimple(t));

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is an enum.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsEnum(Type type) => type.IsEnum;

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> implements <c>IEnumerable&lt;T&gt;</c> for some element type.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsEnumerableOfT(Type type) => IsEnumerableOfT(type, out _);

    /// <summary>
    ///     Returns <see langword="true"/> if <paramref name="type"/> implements <c>IEnumerable&lt;T&gt;</c>,
    ///     and outputs the element type <paramref name="elementType"/>.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="elementType">When <see langword="true"/>, the element type; otherwise <see langword="null"/>.</param>
    public static bool IsEnumerableOfT(Type type, [NotNullWhen(true)] out Type? elementType)
    {
        var (isEnumerable, foundElementType) = _isEnumerableOfTCache.GetOrAdd(type, static t =>
        {
            if (t == typeof(IEnumerable<>))
            {
                return (true, t.GetGenericArguments().FirstOrDefault());
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return (true, t.GenericTypeArguments.FirstOrDefault());
            }

            if (!t.IsGenericType && !t.IsArray)
            {
                return (false, null);
            }

            var candidates = t.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(x => x.GenericTypeArguments.FirstOrDefault())
                .Where(x => x != null)
                .ToList();

            if (candidates.Count == 1)
            {
                return (true, candidates[0]);
            }

            if (candidates.Count > 1)
            {
                throw new InvalidOperationException($"Type {t.Name} implements multiple IEnumerable<T> interfaces.");
            }

            return (false, null);
        });

        elementType = foundElementType;
        return isEnumerable;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a floating-point numeric type.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsFloatingPoint(Type type) => _floatingPointTypes.Contains(type);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is an open generic type definition.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsGenericTypeDefinition(Type type) => type.IsGenericTypeDefinition;

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a closed or open generic type.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsGenericType(Type type) => type.IsGenericType;

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is <see cref="Guid"/>.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsGuid(Type type) => type == typeof(Guid);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> directly or indirectly implements <paramref name="interfaceType"/>.</summary>
    /// <param name="type">The type to check.</param>
    /// <param name="interfaceType">The interface type to look for. May be an open generic interface.</param>
    public static bool IsImplementationOf(Type type, Type interfaceType)
    {
        if (type == null || interfaceType == null)
        {
            return false;
        }

        return interfaceType.IsGenericType
            ? type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(interfaceType))
            : type.GetInterfaces().Any(x => !x.IsGenericType && x.Equals(interfaceType));
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is an integer numeric type.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsInteger(Type type) => _integerTypes.Contains(type);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is <c>Nullable&lt;T&gt;</c>.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsNullableType(Type type) => _isNullableTypeCache.GetOrAdd(type, t => IsGenericType(t) && t.GetGenericTypeDefinition() == typeof(Nullable<>));

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a nullable enum (i.e., <c>Nullable&lt;TEnum&gt;</c>).</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsNullableEnum(Type type)
    {
        return _isNullableEnumCache.GetOrAdd(type, static t =>
        {
            if (!IsNullableType(t))
            {
                return false;
            }

            var nullableUnderlyingType = Nullable.GetUnderlyingType(t);
            if (nullableUnderlyingType == null)
            {
                return false;
            }

            return IsEnum(nullableUnderlyingType);
        });
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is any numeric type (integer or floating-point).</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsNumber(Type type) => IsInteger(type) || IsFloatingPoint(type);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a primitive or well-known simple type.</summary>
    /// <param name="type">The type to check.</param>
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
                {
                    return true;
                }

                if (IsNullableType(t))
                {
                    var nullableUnderlyingType = Nullable.GetUnderlyingType(t);
                    if (nullableUnderlyingType == null)
                    {
                        return false;
                    }

                    t = nullableUnderlyingType;
                    continue;
                }

                if (IsEnum(t))
                {
                    var enumUnderlyingType = Enum.GetUnderlyingType(t);
                    if (enumUnderlyingType == null)
                    {
                        return false;
                    }

                    t = enumUnderlyingType;
                    continue;
                }

                return false;
            }
        });
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is <see cref="string"/>.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsString(Type type) => type == typeof(string);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a subclass of <paramref name="baseClass"/>.</summary>
    /// <param name="type">The type to check.</param>
    /// <param name="baseClass">The base class to check against.</param>
    public static bool IsSubclassOf(Type type, Type baseClass) => baseClass != null && type.IsSubclassOf(baseClass);

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a subclass of, or implements, <paramref name="baseClassOrInterfaceType"/>.</summary>
    /// <param name="type">The type to check.</param>
    /// <param name="baseClassOrInterfaceType">The base class or interface type to check against.</param>
    public static bool IsSubclassOrImplementationOf(Type type, Type baseClassOrInterfaceType)
    {
        if (type.IsSubclassOf(baseClassOrInterfaceType))
        {
            return true;
        }

        if (IsImplementationOf(type, baseClassOrInterfaceType))
        {
            return true;
        }

        if (!baseClassOrInterfaceType.IsGenericType)
        {
            return false;
        }

        if (type.BaseType != null)
        {
            var baseType = type.BaseType;
            while (!baseType.Equals(typeof(object)))
            {
                if (baseClassOrInterfaceType.Equals(baseType))
                {
                    return true;
                }

                if (baseType.IsGenericType)
                {
                    var baseGenericTypeDefinition = baseType.GetGenericTypeDefinition();
                    if (baseClassOrInterfaceType.Equals(baseGenericTypeDefinition))
                    {
                        return true;
                    }
                }

                if (baseType.BaseType != null)
                {
                    baseType = baseType.BaseType;
                }
            }
        }

        return false;
    }

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is a value type.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsValueType(Type type) => type.IsValueType;

    /// <summary>Returns <see langword="true"/> if <paramref name="type"/> is <see cref="void"/>.</summary>
    /// <param name="type">The type to check.</param>
    public static bool IsVoid(Type type) => type == typeof(void);
    #endregion

    #region Property Methods
    /// <summary>Gets the public declared property on <paramref name="type"/> with the given name.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The matching <see cref="PropertyInfo"/>, or <see langword="null"/> if not found.</returns>
    public static PropertyInfo? GetProperty(Type type, string propertyName) => GetProperty(type, propertyName, _defaultPropertyReflectionFlags);

    /// <summary>Gets the property on <paramref name="type"/> with the given name and binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="bindingFlags">The binding flags to filter properties.</param>
    /// <returns>The matching <see cref="PropertyInfo"/>, or <see langword="null"/> if not found.</returns>
    public static PropertyInfo? GetProperty(Type type, string propertyName, BindingFlags bindingFlags) => type.GetProperty(propertyName, bindingFlags);

    /// <summary>Gets all public declared properties of <paramref name="type"/>.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <returns>A sequence of <see cref="PropertyInfo"/> objects.</returns>
    public static IEnumerable<PropertyInfo> GetProperties(Type type) => GetProperties(type, _defaultPropertyReflectionFlags);

    /// <summary>Gets all properties of <paramref name="type"/> matching the given binding flags.</summary>
    /// <param name="type">The type to reflect.</param>
    /// <param name="bindingFlags">The binding flags to filter properties.</param>
    /// <returns>A sequence of <see cref="PropertyInfo"/> objects.</returns>
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
