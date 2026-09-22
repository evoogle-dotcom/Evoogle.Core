// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Coercion;
using Evoogle.MemberAccess.Internal;

namespace Evoogle.MemberAccess;

/// <summary>Returns cached, compiled property and field access delegates.</summary>
public static partial class MemberAccessorFactory
{
    #region Creation Methods
    /// <summary>Creates a getter that returns the member value as an object.</summary>
    /// <param name="memberInfo">The member to read.</param>
    /// <returns>The cached getter.</returns>
    public static Func<object, object?> CreateGetter(MemberInfo memberInfo)
        => Get<Func<object, object?>>(memberInfo, AccessOperation.Get, isStatic: false);

    /// <summary>Creates a getter that can convert to a requested runtime type.</summary>
    /// <param name="memberInfo">The member to read.</param>
    /// <returns>The cached coercing getter.</returns>
    public static Func<object, Type, TypeCoercion, TypeCoercionContext?, object?> CreateCoercingGetter(MemberInfo memberInfo)
        => Get<Func<object, Type, TypeCoercion, TypeCoercionContext?, object?>>(memberInfo, AccessOperation.CoercingGet, isStatic: false);

    /// <summary>Creates an object-based setter.</summary>
    /// <param name="memberInfo">The member to write.</param>
    /// <returns>The cached setter.</returns>
    public static Action<object, object?> CreateSetter(MemberInfo memberInfo) =>
        Get<Action<object, object?>>(memberInfo, AccessOperation.Set, isStatic: false);

    /// <summary>Creates an object-based setter that converts its input.</summary>
    /// <param name="memberInfo">The member to write.</param>
    /// <returns>The cached coercing setter.</returns>
    public static Action<object, object?, TypeCoercion, TypeCoercionContext?> CreateCoercingSetter(MemberInfo memberInfo)
        => Get<Action<object, object?, TypeCoercion, TypeCoercionContext?>>(memberInfo, AccessOperation.CoercingSet, isStatic: false);

    /// <summary>Creates a typed instance getter.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The member to read.</param>
    /// <returns>The cached getter.</returns>
    public static Func<TObject, TValue?> CreateGetter<TObject, TValue>(MemberInfo memberInfo)
        => Get<Func<TObject, TValue?>>(memberInfo, AccessOperation.Get, isStatic: false);

    /// <summary>Creates a typed instance getter with per-call coercion.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The member to read.</param>
    /// <returns>The cached coercing getter.</returns>
    public static Func<TObject, TypeCoercion, TypeCoercionContext?, TValue?>CreateCoercingGetter<TObject, TValue>(MemberInfo memberInfo)
        => Get<Func<TObject, TypeCoercion, TypeCoercionContext?, TValue?>>(memberInfo, AccessOperation.CoercingGet, isStatic: false);

    /// <summary>Creates a typed instance setter.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <returns>The cached setter.</returns>
    public static Action<TObject, TValue?> CreateSetter<TObject, TValue>(MemberInfo memberInfo)
        => Get<Action<TObject, TValue?>>(memberInfo, AccessOperation.Set, isStatic: false);

    /// <summary>Creates a typed instance setter with per-call coercion.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <returns>The cached coercing setter.</returns>
    public static Action<TObject, TValue?, TypeCoercion, TypeCoercionContext?> CreateCoercingSetter<TObject, TValue>(MemberInfo memberInfo)
        => Get<Action<TObject, TValue?, TypeCoercion, TypeCoercionContext?>>(memberInfo, AccessOperation.CoercingSet, isStatic: false);
    #endregion

    #region TryCreate Methods
    /// <summary>Attempts to create an object-based getter.</summary>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateGetter(MemberInfo? memberInfo, out Func<object, object?>? getter)
        => TryCreate(() => CreateGetter(memberInfo!), out getter);

    /// <summary>Attempts to create an object-based coercing getter.</summary>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingGetter(MemberInfo? memberInfo, out Func<object, Type, TypeCoercion, TypeCoercionContext?, object?>? getter)
        => TryCreate(() => CreateCoercingGetter(memberInfo!), out getter);

    /// <summary>Attempts to create an object-based setter.</summary>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateSetter(MemberInfo? memberInfo, out Action<object, object?>? setter)
        => TryCreate(() => CreateSetter(memberInfo!), out setter);

    /// <summary>Attempts to create an object-based coercing setter.</summary>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingSetter(MemberInfo? memberInfo, out Action<object, object?, TypeCoercion, TypeCoercionContext?>? setter)
        => TryCreate(() => CreateCoercingSetter(memberInfo!), out setter);

    /// <summary>Attempts to create a typed instance getter.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateGetter<TObject, TValue>(MemberInfo? memberInfo, out Func<TObject, TValue?>? getter)
        => TryCreate(() => CreateGetter<TObject, TValue>(memberInfo!), out getter);

    /// <summary>Attempts to create a typed coercing getter.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The returned value type.</typeparam>
    /// <param name="memberInfo">The member to read.</param>
    /// <param name="getter">The getter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingGetter<TObject, TValue>(MemberInfo? memberInfo, out Func<TObject, TypeCoercion, TypeCoercionContext?, TValue?>? getter)
        => TryCreate(() => CreateCoercingGetter<TObject, TValue>(memberInfo!), out getter);

    /// <summary>Attempts to create a typed instance setter.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateSetter<TObject, TValue>(MemberInfo? memberInfo, out Action<TObject, TValue?>? setter)
        => TryCreate(() => CreateSetter<TObject, TValue>(memberInfo!), out setter);

    /// <summary>Attempts to create a typed coercing setter.</summary>
    /// <typeparam name="TObject">The target type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingSetter<TObject, TValue>(MemberInfo? memberInfo, out Action<TObject, TValue?, TypeCoercion, TypeCoercionContext?>? setter)
        => TryCreate(() => CreateCoercingSetter<TObject, TValue>(memberInfo!), out setter);
    #endregion

    #region Implementation Methods
    private static TDelegate Get<TDelegate>(MemberInfo memberInfo, AccessOperation operation, bool isStatic)
        where TDelegate : Delegate
    {
        var accessor = MemberAccessor.Create(memberInfo);
        if (accessor.IsStatic != isStatic)
        {
            throw new MemberAccessException(
                $"Member '{memberInfo.Name}' requires " +
                $"{(accessor.IsStatic ? "static" : "instance")} access.");
        }

        return MemberAccessCompiler.Get<TDelegate>(accessor, operation);
    }

    private static bool TryCreate<TDelegate>(Func<TDelegate> create, out TDelegate? accessor)
        where TDelegate : Delegate
    {
        try
        {
            accessor = create();
            return true;
        }
        catch (Exception)
        {
            accessor = null;
            return false;
        }
    }
    #endregion
}
