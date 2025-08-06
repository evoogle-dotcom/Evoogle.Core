// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.ObjectModel;
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
        var propertyType = property.PropertyType;
        var (nullableFlags, index) = ReadNullableFlags(property);

        bool isPropertyNullable = DetermineNullability(propertyType, nullableFlags, ref index);
        var collectionChain = new List<PropertyNullableInfo.CollectionLayerInfo>();

        Type currentType = propertyType;

        while (IsCollectionType(currentType, out var collectionType, out var elementType))
        {
            var collectionNullable = DetermineNullability(currentType, nullableFlags, ref index);
            bool elementNullable;

            if (elementType.IsValueType)
            {
                elementNullable = Nullable.GetUnderlyingType(elementType) != null;
            }
            else
            {
                elementNullable = DetermineNullability(elementType, nullableFlags, ref index);
            }

            collectionChain.Add(new PropertyNullableInfo.CollectionLayerInfo
            {
                CollectionType = collectionType,
                IsCollection = true,
                IsCollectionNullable = collectionNullable,
                ElementType = elementType,
                IsElementNullable = elementNullable
            });

            currentType = elementType;
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
    private static (byte[] flags, int index) ReadNullableFlags(PropertyInfo property)
    {
        var attr = property.CustomAttributes
            .FirstOrDefault(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

        if (attr != null && attr.ConstructorArguments.Count > 0)
        {
            var arg = attr.ConstructorArguments[0];
            if (arg.ArgumentType == typeof(byte[]))
            {
                var flags = ((ReadOnlyCollection<CustomAttributeTypedArgument>)arg.Value!)
                    .Select(x => (byte)x.Value!).ToArray();
                return (flags, 0);
            }

            if (arg.ArgumentType == typeof(byte))
            {
                return (new[] { (byte)arg.Value! }, 0);
            }
        }

        return (Array.Empty<byte>(), 0);
    }

    private static bool DetermineNullability(Type type, byte[] flags, ref int index)
    {
        if (type.IsValueType)
            return Nullable.GetUnderlyingType(type) != null;

        if (flags.Length == 0 || index >= flags.Length)
            return false;

        var flag = flags[index++];
        return flag == 2;
    }

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
