// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

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
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns failure for null object",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = null,
            ExpectedSuccess = false
        },

        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for required string property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "Alice"
        },

        new TryGetGenericTest<ScalarsOnly, long>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for required long property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = 123
        },

        new TryGetGenericTest<ScalarsOnly, bool>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for required bool property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        // Optional Fields
        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional string field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional string field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetGenericTest<ScalarsOnly, long?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional long field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ExpectedSuccess = true,
            ExpectedClrValue = 42
        },

        new TryGetGenericTest<ScalarsOnly, long?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional long field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetGenericTest<ScalarsOnly, bool?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional bool field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TryGetGenericTest<ScalarsOnly, bool?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional bool field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        // Required Properties With Coercion
        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for required long property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "123"
        },

        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for required bool property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "true"
        },

        // Optional Fields With Coercion
        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional long field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional long field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional bool field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ExpectedSuccess = true,
            ExpectedClrValue = "false"
        },

        new TryGetGenericTest<ScalarsOnly, string?>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)}<TObject,TValue> returns success for optional bool field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredProperty)} generic get long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredProperty = 42L },
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredProperty)} generic get long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredProperty = 42L },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get non-null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 0L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get non-null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 0L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = 0L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get non-null nullable long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get null nullable long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get non-null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} generic get null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredField)} generic get long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredField = 42L },
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredField)} generic get long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredField = 42L },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} generic get non-null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = 42L },
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 0L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} generic get non-null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = 42L },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} generic get null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = null },
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 0L
        },
        new TryGetGenericTest<NullableValueMembers, long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} generic get null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = null },
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = 0L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} generic get non-null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = 42L },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<NullableValueMembers, long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} generic get null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = null },
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetGenericTest<ScalarsOnly, long>
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicRequiredNumberPropertyName} generic get private property direct",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicRequiredNumberPropertyName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 10L
        },
        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicRequiredNumberPropertyName} generic get private property coercing to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicRequiredNumberPropertyName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = "10"
        },
        new TryGetGenericTest<ScalarsOnly, long?>
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} generic get private nullable field direct for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, 42L),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetGenericTest<ScalarsOnly, long?>
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} generic get private nullable field direct for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} generic get private nullable field coercing to string for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, 42L),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },
        new TryGetGenericTest<ScalarsOnly, string>
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} generic get private nullable field coercing to string for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TryGetNonGenericTheoryData =>
    [
        // Required Properties
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} {nameof(MemberAccessor.TryGetValue)} returns failure for null object",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = null,
            ExpectedSuccess = false
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} {nameof(MemberAccessor.TryGetValue)} returns success for required string property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = "Alice"
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TryGetValue)} returns success for required long property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = 123
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TryGetValue)} returns success for required bool property",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        // Optional Fields
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} {nameof(MemberAccessor.TryGetValue)} returns success for optional string field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = "Bob" },
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalName)} {nameof(MemberAccessor.TryGetValue)} returns success for optional string field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalName),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalName = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)} returns success for optional long field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ExpectedSuccess = true,
            ExpectedClrValue = 42
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)} returns success for optional long field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)} returns success for optional bool field for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)} returns success for optional bool field for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        // Required Properties With Coercion
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TryGetValue)} returns success for required long property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "123"
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TryGetValue)} returns success for required bool property with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true),
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "true"
        },

        // Optional Fields With Coercion
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)} returns success for optional long field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = 42 },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TryGetValue)} returns success for optional long field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalNumber),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalNumber = null },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)} returns success for optional bool field for non-null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = false },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = "false"
        },

        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TryGetValue)} returns success for optional bool field for null value with coercion to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.OptionalPredicate),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ShouldCoerce = true,
            ClrObject = new ScalarsOnly("Alice", 123, true) { OptionalPredicate = null },
            ClrValueType = typeof(string),
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredProperty)} object get long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredProperty = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredProperty)} object get long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredProperty = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get non-null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ClrValueType = typeof(long),
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get non-null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ClrValueType = typeof(long),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ClrValueType = typeof(long),
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ClrValueType = typeof(long),
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get non-null nullable long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get null nullable long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ClrValueType = typeof(long?),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get non-null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalProperty)} object get null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalProperty = null },
            ClrValueType = typeof(long?),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredField)} object get long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredField = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.RequiredField)} object get long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.RequiredField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { RequiredField = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} object get non-null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = 42L },
            ClrValueType = typeof(long),
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} object get non-null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = 42L },
            ClrValueType = typeof(long),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} object get null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = null },
            ClrValueType = typeof(long),
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} object get null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = null },
            ClrValueType = typeof(long),
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} object get non-null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = 42L },
            ClrValueType = typeof(long?),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.OptionalField)} object get null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.OptionalField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            ClrObject = new NullableValueMembers { OptionalField = null },
            ClrValueType = typeof(long?),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicRequiredNumberPropertyName} object get private property direct",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicRequiredNumberPropertyName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ClrValueType = typeof(long),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 10L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicRequiredNumberPropertyName} object get private property coercing to string",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicRequiredNumberPropertyName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ClrValueType = typeof(string),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = "10"
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} object get private nullable field direct for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, 42L),
            ClrValueType = typeof(long?),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} object get private nullable field direct for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ClrValueType = typeof(long?),
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} object get private nullable field coercing to string for non-null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, 42L),
            ClrValueType = typeof(string),
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },
        new TryGetNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{ScalarsOnly.NonPublicOptionalNumberFieldName} object get private nullable field coercing to string for null value",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = ScalarsOnly.NonPublicOptionalNumberFieldName,
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            ClrObject = new ScalarsOnly("Alice", 123L, true, 10L, null),
            ClrValueType = typeof(string),
            ShouldCoerce = true,
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
