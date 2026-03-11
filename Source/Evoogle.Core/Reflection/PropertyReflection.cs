// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

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

        var propertyType = propertyInfo.PropertyType;
        var collectionChain = new List<MemberNullableInfo.CollectionInfo>();

        var currentType = propertyType;
        var currentNullability = nullabilityInfo;

        var isPropertyNullable = currentNullability.ReadState == NullabilityState.Nullable;

        // Handle collections and possible collections within collections (IEnumerable<T>, arrays, etc.)
        while (TypeReflection.IsEnumerableOfT(currentType, out var elementType))
        {
            var collectionNullable = currentNullability.ReadState == NullabilityState.Nullable;

            var elementNullability = currentType.IsArray
                ? currentNullability.ElementType
                : currentNullability.GenericTypeArguments.FirstOrDefault();

            var elementNullable = elementType.IsValueType
                ? Nullable.GetUnderlyingType(elementType) != null
                : elementNullability?.ReadState == NullabilityState.Nullable;

            collectionChain.Add(new MemberNullableInfo.CollectionInfo
            {
                CollectionType = currentType,
                IsCollectionNullable = collectionNullable,
                ElementType = elementType,
                IsElementNullable = elementNullable
            });

            if (elementNullability is null)
            {
                break;
            }

            currentType = elementType;
            currentNullability = elementNullability;
        }

        return new MemberNullableInfo
        {
            MemberType = propertyType,
            IsNullable = isPropertyNullable,
            CollectionChain = collectionChain
        };
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
