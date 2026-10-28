// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

namespace Evoogle.Reflection.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal static class NullableInfoReflection
{
    #region Methods
    internal static MemberNullableInfo GetNullabilityInfo(Type memberType, NullabilityInfo nullabilityInfo)
    {
        var collectionChain = new List<MemberNullableInfo.CollectionInfo>();
        var currentType = memberType;
        var currentNullability = nullabilityInfo;
        var memberNullability = ToMemberNullability(currentNullability.ReadState);

        while (TypeReflection.IsEnumerableOfT(currentType, out var elementType))
        {
            var collectionNullability = ToMemberNullability(currentNullability.ReadState);
            var elementNullability = currentType.IsArray
                ? currentNullability.ElementType
                : currentNullability.GenericTypeArguments.FirstOrDefault();
            var elementMemberNullability = elementType.IsValueType
                ? Nullable.GetUnderlyingType(elementType) is not null
                    ? MemberNullability.Nullable
                    : MemberNullability.NonNullable
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
            MemberType = memberType,
            Nullability = memberNullability,
            CollectionChain = collectionChain
        };
    }

    private static MemberNullability ToMemberNullability(NullabilityState state) => state switch
    {
        NullabilityState.Nullable => MemberNullability.Nullable,
        NullabilityState.NotNull => MemberNullability.NonNullable,
        _ => MemberNullability.Unknown,
    };
    #endregion
}
