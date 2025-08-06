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
    public static PropertyNullableInfo GetNullabilityInfo(PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);

        var context = new NullabilityInfoContext();
        var nullabilityInfo = context.Create(property);

        var collectionChain = new List<PropertyNullableInfo.CollectionLayerInfo>();

        var currentType = property.PropertyType;
        var currentNullability = nullabilityInfo;

        bool isPropertyNullable = currentNullability.ReadState == NullabilityState.Nullable;

        while (IsCollectionType(currentType, out var collectionType, out var elementType))
        {
            bool collectionNullable = currentNullability.ReadState == NullabilityState.Nullable;

            NullabilityInfo? elementNullability = currentType.IsArray
                ? currentNullability.ElementType
                : currentNullability.GenericTypeArguments.FirstOrDefault();

            bool elementNullable = elementType.IsValueType
                ? Nullable.GetUnderlyingType(elementType) != null
                : elementNullability?.ReadState == NullabilityState.Nullable;

            collectionChain.Add(new PropertyNullableInfo.CollectionLayerInfo
            {
                CollectionType = collectionType,
                IsCollection = true,
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

        return new PropertyNullableInfo
        {
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

    #region Implementation Methods
    private static bool IsCollectionType(Type type, out Type collectionType, out Type elementType)
    {
        if (type.IsArray)
        {
            collectionType = type;
            elementType = type.GetElementType()!;
            return true;
        }

        if (type.IsGenericType)
        {
            var def = type.GetGenericTypeDefinition();
            if (def == typeof(IEnumerable<>) || def == typeof(ICollection<>) || def == typeof(IList<>) ||
                def == typeof(List<>) || def == typeof(IReadOnlyCollection<>) || def == typeof(IReadOnlyList<>))
            {
                collectionType = type;
                elementType = type.GetGenericArguments()[0];
                return true;
            }
        }

        collectionType = null!;
        elementType = null!;
        return false;
    }
    #endregion
}
