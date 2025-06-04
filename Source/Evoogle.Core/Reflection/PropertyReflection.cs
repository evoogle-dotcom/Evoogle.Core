// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

namespace Evoogle.Reflection;

/// <summary>
///     Reflection methods for the .NET <see cref="PropertyInfo"> class.
/// </summary>
public static class PropertyReflection
{
    #region Methods
    /// <summary>
    ///     Predicate if property is static or an instance property.
    /// </summary>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> metadata from property centric reflection method calls on the <see cref="Type"/> class.</param>
    /// <returns>True if the represented property metadata is a static property, false otherwise.</returns>
    public static bool IsStatic(PropertyInfo propertyInfo)
    {
        var isStatic = (propertyInfo.CanRead && propertyInfo.GetMethod!.IsStatic == true) || (propertyInfo.CanWrite && propertyInfo.SetMethod!.IsStatic == true);
        return isStatic;
    }
    #endregion
}
