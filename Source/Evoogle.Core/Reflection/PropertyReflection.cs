// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
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