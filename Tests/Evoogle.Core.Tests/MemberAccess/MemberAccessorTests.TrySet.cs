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
    public static TheoryDataRow<IXUnitTest>[] TrySetGenericTheoryData =>
    [
        // Required Properties
        new TrySetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required string property for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = "Bob",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Bob", 123, true),
        },

        new TrySetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required string property for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly(null!, 123, true),
        },

        new TrySetGenericTest<ScalarsOnly, long>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required long property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = 42L,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 42, true),
        },

        new TrySetGenericTest<ScalarsOnly, bool>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required bool property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = false,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, false),
        },

        // Optional Fields
        new TrySetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional string field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ClrValue = "Charlie",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Charlie" },
        },

        new TrySetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional string field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
        },

        new TrySetGenericTest<ScalarsOnly, long?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional long field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = 100L,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 100 },
        },

        new TrySetGenericTest<ScalarsOnly, long?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional long field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
        },

        new TrySetGenericTest<ScalarsOnly, bool?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional bool field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = true,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = true },
        },

        new TrySetGenericTest<ScalarsOnly, bool?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional bool field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
        },

        // Required Properties With Coercion
        new TrySetGenericTest<ScalarsOnly, long>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required string property for non-null value with coercion from long",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = 42,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("42", 123, true),
        },

        new TrySetGenericTest<ScalarsOnly, long?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required string property for null value with coercion from nullable long",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly(null!, 123, true),
        },

        new TrySetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required long property with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = "42",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 42, true),
        },

        new TrySetGenericTest<ScalarsOnly, string>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for required bool property with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = "false",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, false),
        },

        // Optional Fields With Coercion
        new TrySetGenericTest<ScalarsOnly, Ulid?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional string field for non-null value with coercion from nullable Ulid",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ClrValue = TestUlid,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = TestUlidString },
        },

        new TrySetGenericTest<ScalarsOnly, Ulid?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional string field for null value with coercion from nullable Ulid",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
        },

        new TrySetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional long field for non-null value with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = "100",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 100 },
        },

        new TrySetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional long field for null value with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
        },

        new TrySetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional bool field for non-null value with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = "true",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = true },
        },

        new TrySetGenericTest<ScalarsOnly, string?>
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)}<TObject,TValue> " +
                "returns success for optional bool field for null value with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TrySetByRefGenericTheoryData =>
    [
        // Required Fields
        new TrySetByRefGenericTest<Point, long>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.X)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for required long field",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.X),
            ClrObject = new Point { X = 1, Y = 2 },
            ClrValue = 10,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 10, Y = 2 },
        },

        // Required Fields With Coercion
        new TrySetByRefGenericTest<Point, string>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.X)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for required long field with coercion from string",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.X),
            ShouldCoerce = true,
            ClrObject = new Point { X = 1, Y = 2 },
            ClrValue = "10",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 10, Y = 2 },
        },

        // Required Properties
        new TrySetByRefGenericTest<Point, long>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.Y)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for required long property",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.Y),
            ClrObject = new Point { X = 1, Y = 2 },
            ClrValue = 20,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 1, Y = 20 },
        },

        // Required Properties With Coercion
        new TrySetByRefGenericTest<Point, string>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.Y)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for required long property with coercion from string",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.Y),
            ShouldCoerce = true,
            ClrObject = new Point { X = 1, Y = 2 },
            ClrValue = "20",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 1, Y = 20 },
        },

        // Optional Properties
        new TrySetByRefGenericTest<Point, string?>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.Note)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for optional string property for non-null value",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.Note),
            ClrObject = new Point { X = 1, Y = 2, Note = "Alice" },
            ClrValue = "Bob",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 1, Y = 2, Note = "Bob" },
        },

        new TrySetByRefGenericTest<Point, string?>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.Note)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for optional string property for null value",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.Note),
            ClrObject = new Point { X = 1, Y = 2, Note = "Alice" },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 1, Y = 2, Note = null },
        },

        // Optional Properties With Coercion
        new TrySetByRefGenericTest<Point, Guid?>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.Note)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for optional string property for non-null value with coercion from nullable Guid",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.Note),
            ShouldCoerce = true,
            ClrObject = new Point { X = 1, Y = 2, Note = "Alice" },
            ClrValue = TestGuid,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 1, Y = 2, Note = TestGuidString },
        },

        new TrySetByRefGenericTest<Point, Guid?>
        {
            Name =
                $"{nameof(Point)}:{nameof(Point.Note)} " +
                $"{nameof(MemberAccessor.TrySetValueByRef)}<TObject,TValue> " +
                "returns success for optional string property for null value with coercion from nullable Guid",
            DeclaringType = typeof(Point),
            MemberName = nameof(Point.Note),
            ShouldCoerce = true,
            ClrObject = new Point { X = 1, Y = 2, Note = "Alice" },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new Point { X = 1, Y = 2, Note = null },
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TrySetNonGenericTheoryData =>
    [
        // Required Properties
        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for required string property for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = "Bob",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Bob", 123, true),
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for required string property for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly(null!, 123, true),
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for required long property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = 42L,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 42, true),
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for required bool property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = false,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, false),
        },

        // Optional Fields
        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional string field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ClrValue = "Charlie",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Charlie" },
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional string field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional long field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = 100L,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 100 },
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional long field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional bool field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = true,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = true }
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional bool field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = null,
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null }
        },

        // Required Properties With Coercion
        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for required long property with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = "42",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 42, true),
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for required bool property with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValue = "false",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, false),
        },

        // Optional Fields With Coercion
        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional long field for non-null value with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValue = "100",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 100 },
        },

        new TrySetNonGenericTest
        {
            Name =
                $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} " +
                $"{nameof(MemberAccessor.TrySetValue)} " +
                "returns success for optional bool field for non-null value with coercion from string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValue = "true",
            ExpectedTrySetSuccess = true,
            ExpectedTrySetClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = true }
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TrySetGenericTheoryData))]
    public void TrySetGeneric(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TrySetByRefGenericTheoryData))]
    public void TrySetByRefGeneric(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TrySetNonGenericTheoryData))]
    public void TrySetNonGeneric(IXUnitTest test) => test.Execute(this);
    #endregion
}
