// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
namespace Evoogle.Reflection;

internal static class MemberNullabilityTestData
{
    #region Test Data
    public static readonly IReadOnlyDictionary<string, MemberNullableInfo> ExpectedByMemberName =
        new Dictionary<string, MemberNullableInfo>
        {
            [nameof(PropertyReflectionTests.Sample.NullableString)] =
                CreateMemberNullableInfo
                (
                    typeof(string),
                    MemberNullability.Nullable
                ),
            [nameof(PropertyReflectionTests.Sample.String)] =
                CreateMemberNullableInfo
                (
                    typeof(string),
                    MemberNullability.NonNullable
                ),
            [nameof(PropertyReflectionTests.Sample.NullableInt)] =
                CreateMemberNullableInfo
                (
                    typeof(int?),
                    MemberNullability.Nullable
                ),
            [nameof(PropertyReflectionTests.Sample.Int)] =
                CreateMemberNullableInfo
                (
                    typeof(int),
                    MemberNullability.NonNullable
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfNullableStrings)] =
                CreateMemberNullableInfo
                (
                    typeof(List<string?>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<string?>),
                        typeof(string),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfStrings)] =
                CreateMemberNullableInfo
                (
                    typeof(List<string>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<string>),
                        typeof(string),
                        MemberNullability.Nullable,
                        MemberNullability.NonNullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.ListOfNullableStrings)] =
                CreateMemberNullableInfo
                (
                    typeof(List<string?>),
                    MemberNullability.NonNullable,
                    CreateCollectionInfo
                    (
                        typeof(List<string?>),
                        typeof(string),
                        MemberNullability.NonNullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.ListOfStrings)] =
                CreateMemberNullableInfo
                (
                    typeof(List<string>),
                    MemberNullability.NonNullable,
                    CreateCollectionInfo
                    (
                        typeof(List<string>),
                        typeof(string),
                        MemberNullability.NonNullable,
                        MemberNullability.NonNullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfNullableInts)] =
                CreateMemberNullableInfo
                (
                    typeof(List<int?>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<int?>),
                        typeof(int?),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfInts)] =
                CreateMemberNullableInfo
                (
                    typeof(List<int>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<int>),
                        typeof(int),
                        MemberNullability.Nullable,
                        MemberNullability.NonNullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.ListOfNullableInts)] =
                CreateMemberNullableInfo
                (
                    typeof(List<int?>),
                    MemberNullability.NonNullable,
                    CreateCollectionInfo
                    (
                        typeof(List<int?>),
                        typeof(int?),
                        MemberNullability.NonNullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.ListOfInts)] =
                CreateMemberNullableInfo
                (
                    typeof(List<int>),
                    MemberNullability.NonNullable,
                    CreateCollectionInfo
                    (
                        typeof(List<int>),
                        typeof(int),
                        MemberNullability.NonNullable,
                        MemberNullability.NonNullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfNullableListsOfNullableStrings)] =
                CreateMemberNullableInfo
                (
                    typeof(List<List<string?>?>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<List<string?>?>),
                        typeof(List<string?>),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    ),
                    CreateCollectionInfo
                    (
                        typeof(List<string?>),
                        typeof(string),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfNullableListsOfNullableListsOfNullableStrings)] =
                CreateMemberNullableInfo
                (
                    typeof(List<List<List<string?>?>?>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<List<List<string?>?>?>),
                        typeof(List<List<string?>?>),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    ),
                    CreateCollectionInfo
                    (
                        typeof(List<List<string?>?>),
                        typeof(List<string?>),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    ),
                    CreateCollectionInfo
                    (
                        typeof(List<string?>),
                        typeof(string),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfNullableListsOfNullableInts)] =
                CreateMemberNullableInfo
                (
                    typeof(List<List<int?>?>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<List<int?>?>),
                        typeof(List<int?>),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    ),
                    CreateCollectionInfo
                    (
                        typeof(List<int?>),
                        typeof(int?),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.Sample.NullableListOfNullableListsOfNullableListsOfNullableInts)] =
                CreateMemberNullableInfo
                (
                    typeof(List<List<List<int?>?>?>),
                    MemberNullability.Nullable,
                    CreateCollectionInfo
                    (
                        typeof(List<List<List<int?>?>?>),
                        typeof(List<List<int?>?>),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    ),
                    CreateCollectionInfo
                    (
                        typeof(List<List<int?>?>),
                        typeof(List<int?>),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    ),
                    CreateCollectionInfo
                    (
                        typeof(List<int?>),
                        typeof(int?),
                        MemberNullability.Nullable,
                        MemberNullability.Nullable
                    )
                ),
            [nameof(PropertyReflectionTests.UnknownSample.UnknownReference)] =
                CreateMemberNullableInfo
                (
                    typeof(string),
                    MemberNullability.Unknown
                ),
            [nameof(PropertyReflectionTests.UnknownSample.UnknownCollectionWithUnknownReferenceElements)] =
                CreateMemberNullableInfo
                (
                    typeof(List<string>),
                    MemberNullability.Unknown,
                    CreateCollectionInfo
                    (
                        typeof(List<string>),
                        typeof(string),
                        MemberNullability.Unknown,
                        MemberNullability.Unknown
                    )
                )
        };
    #endregion

    #region Helper Methods
    private static MemberNullableInfo CreateMemberNullableInfo
    (
        Type memberType,
        MemberNullability nullability,
        params MemberNullableInfo.CollectionInfo[] collectionChain
    ) =>
        new()
        {
            MemberType = memberType,
            Nullability = nullability,
            CollectionChain = collectionChain
        };

    private static MemberNullableInfo.CollectionInfo CreateCollectionInfo
    (
        Type collectionType,
        Type elementType,
        MemberNullability collectionNullability,
        MemberNullability elementNullability
    ) =>
        new()
        {
            CollectionType = collectionType,
            ElementType = elementType,
            CollectionNullability = collectionNullability,
            ElementNullability = elementNullability
        };
    #endregion
}
