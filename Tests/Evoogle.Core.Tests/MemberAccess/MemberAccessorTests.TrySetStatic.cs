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
    public static TheoryDataRow<IXUnitTest>[] TrySetStaticGenericTheoryData =>
    [
        new TrySetStaticGenericTest<string>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required string property for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            InitialClrValue = "Alice",
            ClrValue = "Bob",
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required string property for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            InitialClrValue = "Alice",
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<long>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required long property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            InitialClrValue = 123L,
            ClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },

        new TrySetStaticGenericTest<bool>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required bool property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            InitialClrValue = true,
            ClrValue = false,
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional string field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = "Bob",
            ClrValue = "Charlie",
            ExpectedSuccess = true,
            ExpectedClrValue = "Charlie"
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional string field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = "Bob",
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional long field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = 42L,
            ClrValue = 100L,
            ExpectedSuccess = true,
            ExpectedClrValue = 100L
        },

        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional long field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = 42L,
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<bool?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional bool field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = false,
            ClrValue = true,
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        new TrySetStaticGenericTest<bool?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional bool field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = false,
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<long>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required string property for non-null value with coercion from long",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            ShouldCoerce = true,
            InitialClrValue = "Alice",
            ClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = "42"
        },

        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required string property for null value with coercion from nullable long",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            ShouldCoerce = true,
            InitialClrValue = "Alice",
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<string>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required long property with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            InitialClrValue = 123L,
            ClrValue = "42",
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },

        new TrySetStaticGenericTest<string>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for required bool property with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            InitialClrValue = true,
            ClrValue = "false",
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TrySetStaticGenericTest<Ulid?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional string field for non-null value with coercion from nullable Ulid",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            ShouldCoerce = true,
            InitialClrValue = "Bob",
            ClrValue = TestUlid,
            ExpectedSuccess = true,
            ExpectedClrValue = TestUlidString
        },

        new TrySetStaticGenericTest<Ulid?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional string field for null value with coercion from nullable Ulid",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            ShouldCoerce = true,
            InitialClrValue = "Bob",
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional long field for non-null value with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            InitialClrValue = 42L,
            ClrValue = "100",
            ExpectedSuccess = true,
            ExpectedClrValue = 100L
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional long field for null value with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            InitialClrValue = 42L,
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional bool field for non-null value with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            InitialClrValue = false,
            ClrValue = "true",
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        new TrySetStaticGenericTest<string?>
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns success for optional bool field for null value with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            InitialClrValue = false,
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticGenericTest<string>
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)}<TValue> returns failure for instance member",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            IsStaticMember = false,
            InitialClrValue = "Alice",
            ClrValue = "Bob",
            ExpectedSuccess = false,
            ExpectedClrValue = "Alice"
        },
        new TrySetStaticGenericTest<long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static generic set long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static generic set long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredProperty)} static generic set non-null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredProperty),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredProperty)} static generic set non-null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredProperty),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredProperty)} static generic set null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredProperty)} static generic set null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static generic set non-null nullable long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static generic set null nullable long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static generic set non-null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static generic set null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TrySetStaticGenericTest<long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalField)} static generic set long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalField),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalField)} static generic set long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalField),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredField)} static generic set non-null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredField),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredField)} static generic set non-null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredField),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredField)} static generic set null nullable long to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredField),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredField)} static generic set null nullable long to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredField),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalField)} static generic set non-null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalField),
            InitialClrValue = 10L,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticGenericTest<long?>
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalField)} static generic set null nullable long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalField),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TrySetStaticNonGenericTheoryData =>
    [
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for required string property for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            InitialClrValue = "Alice",
            ClrValue = "Bob",
            ExpectedSuccess = true,
            ExpectedClrValue = "Bob"
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for required string property for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredName),
            InitialClrValue = "Alice",
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for required long property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            InitialClrValue = 123L,
            ClrValue = 42L,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for required bool property",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            InitialClrValue = true,
            ClrValue = false,
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional string field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = "Bob",
            ClrValue = "Charlie",
            ExpectedSuccess = true,
            ExpectedClrValue = "Charlie"
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalName)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional string field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalName),
            InitialClrValue = "Bob",
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional long field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = 42L,
            ClrValue = 100L,
            ExpectedSuccess = true,
            ExpectedClrValue = 100L
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional long field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            InitialClrValue = 42L,
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional bool field for non-null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = false,
            ClrValue = true,
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional bool field for null value",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            InitialClrValue = false,
            ClrValue = null,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredNumber)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for required long property with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredNumber),
            ShouldCoerce = true,
            InitialClrValue = 123L,
            ClrValue = "42",
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.RequiredPredicate)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for required bool property with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.RequiredPredicate),
            ShouldCoerce = true,
            InitialClrValue = true,
            ClrValue = "false",
            ExpectedSuccess = true,
            ExpectedClrValue = false
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalNumber)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional long field for non-null value with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalNumber),
            ShouldCoerce = true,
            InitialClrValue = 42L,
            ClrValue = "100",
            ExpectedSuccess = true,
            ExpectedClrValue = 100L
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(StaticScalarsOnly)}:{nameof(StaticScalarsOnly.OptionalPredicate)} {nameof(MemberAccessor.TrySetStaticValue)} returns success for optional bool field for non-null value with coercion from string",
            DeclaringType = typeof(StaticScalarsOnly),
            MemberName = nameof(StaticScalarsOnly.OptionalPredicate),
            ShouldCoerce = true,
            InitialClrValue = false,
            ClrValue = "true",
            ExpectedSuccess = true,
            ExpectedClrValue = true
        },

        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(ScalarsOnly)}:{nameof(ScalarsOnly.RequiredName)} {nameof(MemberAccessor.TrySetStaticValue)} returns failure for instance member",
            DeclaringType = typeof(ScalarsOnly),
            MemberName = nameof(ScalarsOnly.RequiredName),
            IsStaticMember = false,
            InitialClrValue = "Alice",
            ClrValue = "Bob",
            ExpectedSuccess = false,
            ExpectedClrValue = "Alice"
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static object set boxed long to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static object set null to nullable long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = false,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static object set boxed long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredProperty)} static object set null to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredProperty)} static object set null to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalProperty)} static object set null to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalProperty),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalField)} static object set boxed long to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalField),
            InitialClrValue = null,
            ClrValue = 42L,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = 42L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredField)} static object set null to long direct",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredField),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = false,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticRequiredField)} static object set null to long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticRequiredField),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = false,
            ExpectedClrValue = 10L
        },
        new TrySetStaticNonGenericTest
        {
            Name = $"{nameof(NullableValueMembers)}:{nameof(NullableValueMembers.StaticOptionalField)} static object set null to nullable long coercing",
            DeclaringType = typeof(NullableValueMembers),
            MemberName = nameof(NullableValueMembers.StaticOptionalField),
            InitialClrValue = 10L,
            ClrValue = null,
            ShouldCoerce = true,
            ExpectedSuccess = true,
            ExpectedClrValue = null
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TrySetStaticGenericTheoryData))]
    public void TrySetStaticGeneric(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TrySetStaticNonGenericTheoryData))]
    public void TrySetStaticNonGeneric(IXUnitTest test) => test.Execute(this);
    #endregion
}
