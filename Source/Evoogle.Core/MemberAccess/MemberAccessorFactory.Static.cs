// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Coercion;
using Evoogle.MemberAccess.Internal;

namespace Evoogle.MemberAccess;

/// <inheritdoc cref="MemberAccessorFactory"/>
public static partial class MemberAccessorFactory
{
    #region Creation Methods
    /// <summary>Creates an object-based static getter.</summary>
    /// <param name="memberInfo">The static member to read.</param>
    /// <returns>The cached getter.</returns>
    public static Func<object?> CreateStaticGetter(MemberInfo memberInfo)
        => Get<Func<object?>>(memberInfo, AccessOperation.Get, isStatic: true);

    /// <summary>Creates a static getter with runtime result coercion.</summary>
    /// <param name="memberInfo">The static member to read.</param>
    /// <returns>The cached coercing getter.</returns>
    public static Func<Type, TypeCoercion, TypeCoercionContext?, object?> CreateCoercingStaticGetter(MemberInfo memberInfo)
        => Get<Func<Type, TypeCoercion, TypeCoercionContext?, object?>>(memberInfo, AccessOperation.CoercingGet, isStatic: true);

    /// <summary>Creates an object-based static setter.</summary>
    /// <param name="memberInfo">The static member to write.</param>
    /// <returns>The cached setter.</returns>
    public static Action<object?> CreateStaticSetter(MemberInfo memberInfo)
        => Get<Action<object?>>(memberInfo, AccessOperation.Set, isStatic: true);

    /// <summary>Creates a static setter with per-call coercion.</summary>
    /// <param name="memberInfo">The static member to write.</param>
    /// <returns>The cached coercing setter.</returns>
    public static Action<object?, TypeCoercion, TypeCoercionContext?>CreateCoercingStaticSetter(MemberInfo memberInfo)
        => Get<Action<object?, TypeCoercion, TypeCoercionContext?>>(memberInfo, AccessOperation.CoercingSet, isStatic: true);

    /// <summary>Creates a typed static getter.</summary>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The static member to read.</param>
    /// <returns>The cached getter.</returns>
    public static Func<TValue?> CreateStaticGetter<TValue>(MemberInfo memberInfo)
        => Get<Func<TValue?>>(memberInfo, AccessOperation.Get, isStatic: true);

    /// <summary>Creates a typed static getter with per-call coercion.</summary>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The static member to read.</param>
    /// <returns>The cached coercing getter.</returns>
    public static Func<TypeCoercion, TypeCoercionContext?, TValue?> CreateCoercingStaticGetter<TValue>(MemberInfo memberInfo)
        => Get<Func<TypeCoercion, TypeCoercionContext?, TValue?>>(memberInfo, AccessOperation.CoercingGet, isStatic: true);

    /// <summary>Creates a typed static setter.</summary>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The static member to write.</param>
    /// <returns>The cached setter.</returns>
    public static Action<TValue?> CreateStaticSetter<TValue>(MemberInfo memberInfo)
        => Get<Action<TValue?>>(memberInfo, AccessOperation.Set, isStatic: true);

    /// <summary>Creates a typed static setter with per-call coercion.</summary>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The static member to write.</param>
    /// <returns>The cached coercing setter.</returns>
    public static Action<TValue?, TypeCoercion, TypeCoercionContext?> CreateCoercingStaticSetter<TValue>(MemberInfo memberInfo)
        => Get<Action<TValue?, TypeCoercion, TypeCoercionContext?>>(memberInfo, AccessOperation.CoercingSet, isStatic: true);
    #endregion

    #region TryCreate Methods
    /// <summary>Attempts to create an object-based static getter.</summary>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateStaticGetter(MemberInfo? memberInfo, out Func<object?>? getter)
        => TryCreate(memberInfo, static member => CreateStaticGetter(member!), out getter);

    /// <summary>Attempts to create an object-based coercing static getter.</summary>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingStaticGetter(MemberInfo? memberInfo, out Func<Type, TypeCoercion, TypeCoercionContext?, object?>? getter)
        => TryCreate(memberInfo, static member => CreateCoercingStaticGetter(member!), out getter);

    /// <summary>Attempts to create an object-based static setter.</summary>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateStaticSetter(MemberInfo? memberInfo, out Action<object?>? setter)
        => TryCreate(memberInfo, static member => CreateStaticSetter(member!), out setter);

    /// <summary>Attempts to create an object-based coercing static setter.</summary>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingStaticSetter(MemberInfo? memberInfo, out Action<object?, TypeCoercion, TypeCoercionContext?>? setter)
        => TryCreate(memberInfo, static member => CreateCoercingStaticSetter(member!), out setter);

    /// <summary>Attempts to create a typed static getter.</summary>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateStaticGetter<TValue>(MemberInfo? memberInfo, out Func<TValue?>? getter)
        => TryCreate(memberInfo, static member => CreateStaticGetter<TValue>(member!),out getter);

    /// <summary>Attempts to create a typed coercing static getter.</summary>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingStaticGetter<TValue>(MemberInfo? memberInfo, out Func<TypeCoercion, TypeCoercionContext?, TValue?>? getter)
        => TryCreate(memberInfo, static member => CreateCoercingStaticGetter<TValue>(member!), out getter);

    /// <summary>Attempts to create a typed static setter.</summary>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateStaticSetter<TValue>(MemberInfo? memberInfo, out Action<TValue?>? setter)
        => TryCreate(memberInfo, static member => CreateStaticSetter<TValue>(member!), out setter);

    /// <summary>Attempts to create a typed coercing static setter.</summary>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingStaticSetter<TValue>(MemberInfo? memberInfo, out Action<TValue?, TypeCoercion, TypeCoercionContext?>? setter)
        => TryCreate(memberInfo, static member => CreateCoercingStaticSetter<TValue>(member!), out setter);
    #endregion
}
