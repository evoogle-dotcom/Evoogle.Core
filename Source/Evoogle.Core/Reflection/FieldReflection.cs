// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

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

        var fieldType = fieldInfo.FieldType;
        var collectionChain = new List<MemberNullableInfo.CollectionInfo>();

        var currentType = fieldType;
        var currentNullability = nullabilityInfo;

        var fieldNullability = ToMemberNullability(currentNullability.ReadState);

        // Handle collections and possible collections within collections (IEnumerable<T>, arrays, etc.)
        while (TypeReflection.IsEnumerableOfT(currentType, out var elementType))
        {
            var collectionNullability = ToMemberNullability(currentNullability.ReadState);

            var elementNullability = currentType.IsArray
                ? currentNullability.ElementType
                : currentNullability.GenericTypeArguments.FirstOrDefault();

            var elementMemberNullability = elementType.IsValueType
                ? (Nullable.GetUnderlyingType(elementType) != null ? MemberNullability.Nullable : MemberNullability.NonNullable)
                : ToMemberNullability(elementNullability?.ReadState ?? NullabilityState.Unknown);

            collectionChain.Add(new MemberNullableInfo.CollectionInfo
            {
                CollectionType = currentType,
                CollectionNullability = collectionNullability,
                ElementType = elementType,
                ElementNullability = elementMemberNullability
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
            MemberType = fieldType,
            Nullability = fieldNullability,
            CollectionChain = collectionChain
        };
    }

    private static MemberNullability ToMemberNullability(NullabilityState state) => state switch
    {
        NullabilityState.Nullable => MemberNullability.Nullable,
        NullabilityState.NotNull => MemberNullability.NonNullable,
        _ => MemberNullability.Unknown,
    };

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
