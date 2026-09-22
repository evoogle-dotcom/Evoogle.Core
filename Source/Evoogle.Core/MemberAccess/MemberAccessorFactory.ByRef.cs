// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.MemberAccess.Internal;

namespace Evoogle.MemberAccess;

/// <inheritdoc cref="MemberAccessorFactory"/>
public static partial class MemberAccessorFactory
{
    #region Setter Methods
    /// <summary>Creates a setter for a struct passed by reference.</summary>
    /// <typeparam name="TObject">The struct type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <returns>The cached setter.</returns>
    public static MemberSetterByRef<TObject, TValue> CreateSetterByRef<TObject, TValue>(MemberInfo memberInfo) where TObject : struct
        => Get<MemberSetterByRef<TObject, TValue>>(memberInfo, AccessOperation.Set, isStatic: false);

    /// <summary>Creates a coercing setter for a struct passed by reference.</summary>
    /// <typeparam name="TObject">The struct type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <returns>The cached coercing setter.</returns>
    public static CoercingMemberSetterByRef<TObject, TValue> CreateCoercingSetterByRef<TObject, TValue>(MemberInfo memberInfo) where TObject : struct
        => Get<CoercingMemberSetterByRef<TObject, TValue>>(memberInfo, AccessOperation.CoercingSet, isStatic: false);
    #endregion

    #region TryCreate Methods
    /// <summary>Attempts to create a struct setter.</summary>
    /// <typeparam name="TObject">The struct type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateSetterByRef<TObject, TValue>(MemberInfo? memberInfo, out MemberSetterByRef<TObject, TValue>? setter) where TObject : struct
        => TryCreate(() => CreateSetterByRef<TObject, TValue>(memberInfo!), out setter);

    /// <summary>Attempts to create a coercing struct setter.</summary>
    /// <typeparam name="TObject">The struct type.</typeparam>
    /// <typeparam name="TValue">The supplied value type.</typeparam>
    /// <param name="memberInfo">The member to write.</param>
    /// <param name="setter">The setter when successful.</param>
    /// <returns>Whether creation succeeded.</returns>
    public static bool TryCreateCoercingSetterByRef<TObject, TValue>(MemberInfo? memberInfo, out CoercingMemberSetterByRef<TObject, TValue>? setter) where TObject : struct
        => TryCreate(() => CreateCoercingSetterByRef<TObject, TValue>(memberInfo!), out setter);
    #endregion
}
