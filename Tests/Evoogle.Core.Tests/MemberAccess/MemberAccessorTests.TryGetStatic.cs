// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.MemberAccess;

public partial class MemberAccessorTests
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] TryGetStaticGenericTheoryData =>
    [
        new TryGetStaticGenericTest<string>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for required string property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            InitialClrValue = "Alice",
            ExpectedSuccess = true,
            ExpectedClrValue = "Alice"
        },

        new TryGetStaticGenericTest<long>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for required long property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            InitialClrValue = 123L,
            ExpectedSuccess = true,
            ExpectedClrValue = 123L
        },

        new TryGetStaticGenericTest<bool>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for required bool property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            InitialClrValue = true,
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        new TryGetStaticGenericTest<string?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional string field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = "Bob",
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TryGetStaticGenericTest<string?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional string field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticGenericTest<long?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional long field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },

        new TryGetStaticGenericTest<long?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional long field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticGenericTest<bool?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional bool field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = false,
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TryGetStaticGenericTest<bool?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional bool field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticGenericTest<string>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for required long property with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            InitialClrValue = 123L,
            ExpectedSuccess = true,
            ExpectedClrValue = "123"
        },

        new TryGetStaticGenericTest<string>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for required bool property with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            InitialClrValue = true,
            ExpectedSuccess = true,
            ExpectedClrValue = "true"
        },

        new TryGetStaticGenericTest<string?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional long field for non-null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            InitialClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TryGetStaticGenericTest<string?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional long field for null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticGenericTest<string?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional bool field for non-null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            InitialClrValue = false,
            ExpectedSuccess = true,
            ExpectedClrValue = "false"
        },

        new TryGetStaticGenericTest<string?>
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns success for optional bool field for null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticGenericTest<string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)}<TValue> " +
                "returns failure for instance member",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            IsStaticMember = false,
            InitialClrValue = "Alice",
            ExpectedSuccess = false
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TryGetStaticNonGenericTheoryData =>
    [
        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for required string property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            InitialClrValue = "Alice",
            ExpectedSuccess = true,
            ExpectedClrValue = "Alice"
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for required long property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            InitialClrValue = 123L,
            ExpectedSuccess = true,
            ExpectedClrValue = 123L
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for required bool property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            InitialClrValue = true,
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional string field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = "Bob",
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional string field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional long field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional long field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional bool field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = false,
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional bool field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for required long property with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            ClrValueType = typeof(string),
            InitialClrValue = 123L,
            ExpectedSuccess = true,
            ExpectedClrValue = "123"
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for required bool property with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            ClrValueType = typeof(string),
            InitialClrValue = true,
            ExpectedSuccess = true,
            ExpectedClrValue = "true"
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional long field for non-null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrValueType = typeof(string),
            InitialClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional long field for null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrValueType = typeof(string),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional bool field for non-null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrValueType = typeof(string),
            InitialClrValue = false,
            ExpectedSuccess = true,
            ExpectedClrValue = "false"
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns success for optional bool field for null value with coercion to string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrValueType = typeof(string),
            InitialClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetStaticNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetStaticValue)} " +
                "returns failure for instance member",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            IsStaticMember = false,
            InitialClrValue = "Alice",
            ExpectedSuccess = false
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TryGetStaticGenericTheoryData))]
    public void TryGetStaticGeneric(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryGetStaticNonGenericTheoryData))]
    public void TryGetStaticNonGeneric(IXUnitTest test) => test.Execute(this);
    #endregion
}
