// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.Reflection.Internal;

namespace Evoogle.Reflection;

/// <summary>
///     Reflection methods for the .NET <see cref="PropertyInfo"/> class.
/// </summary>
public static class PropertyReflection
{
    #region Methods
    /// <summary>
    ///     Gets a detailed breakdown of a property’s nullability including:
    ///     whether the property itself is nullable, whether it is a collection (excluding <see cref="string"/>),
    ///     and the nullability of nested collection types and their elements (if any).
    /// </summary>
    /// <param name="propertyInfo">The <see cref="PropertyInfo"/> to analyze.</param>
    /// <returns>
    ///     A <see cref="MemberNullableInfo"/> object containing nullability metadata of the property,
    ///     including recursive collection nullability if applicable.
    /// </returns>
    public static MemberNullableInfo GetNullabilityInfo(PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        var context = new NullabilityInfoContext();
        var nullabilityInfo = context.Create(propertyInfo);

        return NullableInfoReflection.GetNullabilityInfo(propertyInfo.PropertyType, nullabilityInfo);
    }

    /// <summary>
    ///     Predicate if property is static or an instance property.
    /// </summary>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> metadata from property centric reflection method calls on the <see cref="Type"/> class.</param>
    /// <returns>True if the represented property metadata is a static property, false otherwise.</returns>
    public static bool IsStatic(PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        var isStatic = (propertyInfo.CanRead && propertyInfo.GetMethod?.IsStatic == true) || (propertyInfo.CanWrite && propertyInfo.SetMethod?.IsStatic == true);
        return isStatic;
    }
    #endregion
}
