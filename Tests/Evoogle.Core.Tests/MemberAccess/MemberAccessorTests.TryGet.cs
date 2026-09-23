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
    public static TheoryDataRow<IXUnitTest>[] TryGetGenericTheoryData =>
    [
        // Required Properties
        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns failure for null object",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = null,
            ExpectedSuccess = false
        },

        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for required string property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "Alice"
        },

        new TryGetGenericTest<ScalarsOnly, long>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for required long property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = 123
        },

        new TryGetGenericTest<ScalarsOnly, bool>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for required bool property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        // Optional Fields
        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional string field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional string field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetGenericTest<ScalarsOnly, long?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional long field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ExpectedSuccess = true,
            ExpectedClrValue = 42
        },

        new TryGetGenericTest<ScalarsOnly, long?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional long field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetGenericTest<ScalarsOnly, bool?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional bool field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TryGetGenericTest<ScalarsOnly, bool?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional bool field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        // Required Properties With Coercion
        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for required long property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "123"
        },

        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for required bool property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "true"
        },

        // Optional Fields With Coercion
        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional long field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional long field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional bool field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ExpectedSuccess = true,
            ExpectedClrValue = "false"
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)}<TObject,TValue> " +
                "returns success for optional bool field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TryGetNonGenericTheoryData =>
    [
        // Required Properties
        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns failure for null object",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = null,
            ExpectedSuccess = false
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for required string property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "Alice"
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for required long property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = 123
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for required bool property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        // Optional Fields
        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional string field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional string field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional long field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ExpectedSuccess = true,
            ExpectedClrValue = 42
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional long field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional bool field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional bool field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        // Required Properties With Coercion
        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for required long property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "123"
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for required bool property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "true"
        },

        // Optional Fields With Coercion
        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional long field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional long field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional bool field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "false"
        },

        new TryGetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TryGetValue)} " +
                "returns success for optional bool field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TryGetGenericTheoryData))]
    public void TryGetGeneric(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryGetNonGenericTheoryData))]
    public void TryGetNonGeneric(IXUnitTest test) => test.Execute(this);
    #endregion
}
