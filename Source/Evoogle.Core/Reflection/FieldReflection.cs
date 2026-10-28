// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Reflection.Internal;

namespace Evoogle.Reflection;

/// <summary>
///     Reflection methods for the .NET <see cref="FieldInfo"/> class.
/// </summary>
public static class FieldReflection
{
    #region Methods
    /// <summary>
    ///     Gets a detailed breakdown of a field's nullability including:
    ///     whether the field itself is nullable, whether it is a collection (excluding <see cref="string"/>),
    ///     and the nullability of nested collection types and their elements (if any).
    /// </summary>
    /// <param name="fieldInfo">The <see cref="FieldInfo"/> to analyze.</param>
    /// <returns>
    ///     A <see cref="MemberNullableInfo"/> object containing nullability metadata of the field,
    ///     including recursive collection nullability if applicable.
    /// </returns>
    public static MemberNullableInfo GetNullabilityInfo(FieldInfo fieldInfo)
    {
        ArgumentNullException.ThrowIfNull(fieldInfo, nameof(fieldInfo));

        var context = new NullabilityInfoContext();
        var nullabilityInfo = context.Create(fieldInfo);

        return NullableInfoReflection.GetNullabilityInfo(fieldInfo.FieldType, nullabilityInfo);
    }

    /// <summary>
    ///     Predicate if field is static or an instance field.
    /// </summary>
    /// <param name="fieldInfo"><see cref="FieldInfo"/> metadata from field centric reflection method calls on the <see cref="Type"/> class.</param>
    /// <returns>True if the represented field metadata is a static field, false otherwise.</returns>
    public static bool IsStatic(FieldInfo fieldInfo)
    {
        ArgumentNullException.ThrowIfNull(fieldInfo, nameof(fieldInfo));

        var isStatic = fieldInfo.IsStatic;
        return isStatic;
    }
    #endregion
}
