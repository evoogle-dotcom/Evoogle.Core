// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class StringTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<string, bool>{ Name = "String To Bool (false)", Input = "false", ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<string, bool>{ Name = "String To Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<string, byte>{ Name = "String To Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, byte[]>{ Name = $"String To ByteArray ({TestByteArrayString})", Input = TestByteArrayString, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new GenericCoerceTest<string, char>{ Name = "String To Char (*)", Input = "*", ExpectedResult = true, ExpectedOutput = '*' },
        new GenericCoerceTest<string, DateTime>{ Name = $"String To DateTime ({TestDateTimeString})", Input = TestDateTimeString, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<string, DateTime>{ Name = $"String To DateTime With Format ({TestDateTimeStringWithFormat})", Input = TestDateTimeStringWithFormat, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<string, DateTime>{ Name = $"String To DateTime With Format And FormatProvider ({TestDateTimeStringWithFormatAndFormatProvider})", Input = TestDateTimeStringWithFormatAndFormatProvider, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<string, DateTimeOffset>{ Name = $"String To DateTimeOffset ({TestDateTimeOffsetString})", Input = TestDateTimeOffsetString, ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<string, DateTimeOffset>{ Name = $"String To DateTimeOffset With Format ({TestDateTimeOffsetStringWithFormat})", Input = TestDateTimeOffsetStringWithFormat, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<string, DateTimeOffset>{ Name = $"String To DateTimeOffset With Format And FormatProvider ({TestDateTimeOffsetStringWithFormatAndFormatProvider})", Input = TestDateTimeOffsetStringWithFormatAndFormatProvider, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<string, decimal>{ Name = "String To Decimal (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, double>{ Name = "String To Double (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, float>{ Name = "String To Float (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, Guid>{ Name = $"String To Guid ({TestGuidString})", Input = TestGuidString, ExpectedResult = true, ExpectedOutput = TestGuid },
        new GenericCoerceTest<string, int>{ Name = "String To Int (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, long>{ Name = "String To Long (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, PrimaryColor>{ Name = "String To Enum (1)", Input = "1", ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<string, PrimaryColor>{ Name = "String To Enum (42)", Input = "42", ExpectedResult = false },
        new GenericCoerceTest<string, PrimaryColor>{ Name = "String To Enum (Green)", Input = "Green", ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<string, sbyte>{ Name = "String To SByte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, short>{ Name = "String To Short (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, string>{ Name = "String To String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<string, TimeSpan>{ Name = $"String To TimeSpan ({TestTimeSpanString})", Input = TestTimeSpanString, ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new GenericCoerceTest<string, TimeSpan>{ Name = $"String To TimeSpan With Format ({TestTimeSpanStringWithFormat})", Input = TestTimeSpanStringWithFormat, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new GenericCoerceTest<string, TimeSpan>{ Name = $"String To TimeSpan With Format And FormatProvider ({TestTimeSpanStringWithFormatAndFormatProvider})", Input = TestTimeSpanStringWithFormatAndFormatProvider, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new GenericCoerceTest<string, Type>{ Name = $"String To Type ({TestTypeString})", Input = TestTypeString, ExpectedResult = true, ExpectedOutput = TestType },
        new GenericCoerceTest<string, uint>{ Name = "String To UInt (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, Ulid>{ Name = $"String To Ulid ({TestUlidString})", Input = TestUlidString, ExpectedResult = true, ExpectedOutput = TestUlid },
        new GenericCoerceTest<string, ulong>{ Name = "String To ULong (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string, Uri>{ Name = $"String To Uri ({TestUriString})", Input = TestUriString, ExpectedResult = true, ExpectedOutput = TestUri },
        new GenericCoerceTest<string, ushort>{ Name = "String To UShort (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<string, string?>{ Name = "String To Nullable String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<string?, string>{ Name = "Nullable String To String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<string?, string>{ Name = "Nullable String To String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<string?, string?>{ Name = "Nullable String To Nullable String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<string?, string?>{ Name = "Nullable String To Nullable String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },

        new GenericCoerceTest<string, bool?>{ Name = "String To Nullable Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<string?, bool>{ Name = "Nullable String To Bool (null)", Input = null, ExpectedResult = false },
        new GenericCoerceTest<string?, bool>{ Name = "Nullable String To Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<string?, bool?>{ Name = "Nullable String To Nullable Bool (null)", Input = null, ExpectedResult = true, ExpectedOutput = new bool?() },
        new GenericCoerceTest<string?, bool?>{ Name = "Nullable String To Nullable Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = new bool?(true) },

        new GenericCoerceTest<string, byte?>{ Name = "String To Nullable Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = new byte?(42) },
        new GenericCoerceTest<string?, byte>{ Name = "Nullable String To Byte (null)", Input = null, ExpectedResult = false },
        new GenericCoerceTest<string?, byte>{ Name = "Nullable String To Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<string?, byte?>{ Name = "Nullable String To Nullable Byte (null)", Input = null, ExpectedResult = true, ExpectedOutput = new byte?() },
        new GenericCoerceTest<string?, byte?>{ Name = "Nullable String To Nullable Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = new byte?(42) },

        // Interface/Class Types
        new GenericCoerceTest<string, IInterface>{ Name = "String To Interface (42)", Input = "42", ExpectedResult = false },
        new GenericCoerceTest<string, BaseClass>{ Name = "String To BaseClass (42)", Input = "42", ExpectedResult = false },
        new GenericCoerceTest<string, DerivedClass>{ Name = "String To DerivedClass (42)", Input = "42", ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<string, bool>{ Name = "String To Bool (false)", Input = "false", ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<string, bool>{ Name = "String To Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<string, byte>{ Name = "String To Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, byte[]>{ Name = $"String To ByteArray ({TestByteArrayString})", Input = TestByteArrayString, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new NonGenericCoerceTest<string, char>{ Name = "String To Char (*)", Input = "*", ExpectedResult = true, ExpectedOutput = '*' },
        new NonGenericCoerceTest<string, DateTime>{ Name = $"String To DateTime ({TestDateTimeString})", Input = TestDateTimeString, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<string, DateTime>{ Name = $"String To DateTime With Format ({TestDateTimeStringWithFormat})", Input = TestDateTimeStringWithFormat, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<string, DateTime>{ Name = $"String To DateTime With Format And FormatProvider ({TestDateTimeStringWithFormatAndFormatProvider})", Input = TestDateTimeStringWithFormatAndFormatProvider, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<string, DateTimeOffset>{ Name = $"String To DateTimeOffset ({TestDateTimeOffsetString})", Input = TestDateTimeOffsetString, ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<string, DateTimeOffset>{ Name = $"String To DateTimeOffset With Format ({TestDateTimeOffsetStringWithFormat})", Input = TestDateTimeOffsetStringWithFormat, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<string, DateTimeOffset>{ Name = $"String To DateTimeOffset With Format And FormatProvider ({TestDateTimeOffsetStringWithFormatAndFormatProvider})", Input = TestDateTimeOffsetStringWithFormatAndFormatProvider, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<string, decimal>{ Name = "String To Decimal (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, double>{ Name = "String To Double (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, float>{ Name = "String To Float (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, Guid>{ Name = $"String To Guid ({TestGuidString})", Input = TestGuidString, ExpectedResult = true, ExpectedOutput = TestGuid },
        new NonGenericCoerceTest<string, int>{ Name = "String To Int (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, long>{ Name = "String To Long (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, PrimaryColor>{ Name = "String To Enum (1)", Input = "1", ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<string, PrimaryColor>{ Name = "String To Enum (42)", Input = "42", ExpectedResult = false },
        new NonGenericCoerceTest<string, PrimaryColor>{ Name = "String To Enum (Green)", Input = "Green", ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<string, sbyte>{ Name = "String To SByte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, short>{ Name = "String To Short (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, string>{ Name = "String To String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<string, TimeSpan>{ Name = $"String To TimeSpan ({TestTimeSpanString})", Input = TestTimeSpanString, ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new NonGenericCoerceTest<string, TimeSpan>{ Name = $"String To TimeSpan With Format ({TestTimeSpanStringWithFormat})", Input = TestTimeSpanStringWithFormat, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new NonGenericCoerceTest<string, TimeSpan>{ Name = $"String To TimeSpan With Format And FormatProvider ({TestTimeSpanStringWithFormatAndFormatProvider})", Input = TestTimeSpanStringWithFormatAndFormatProvider, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new NonGenericCoerceTest<string, Type>{ Name = $"String To Type ({TestTypeString})", Input = TestTypeString, ExpectedResult = true, ExpectedOutput = TestType },
        new NonGenericCoerceTest<string, uint>{ Name = "String To UInt (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, Ulid>{ Name = $"String To Ulid ({TestUlidString})", Input = TestUlidString, ExpectedResult = true, ExpectedOutput = TestUlid },
        new NonGenericCoerceTest<string, ulong>{ Name = "String To ULong (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string, Uri>{ Name = $"String To Uri ({TestUriString})", Input = TestUriString, ExpectedResult = true, ExpectedOutput = TestUri },
        new NonGenericCoerceTest<string, ushort>{ Name = "String To UShort (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<string, string?>{ Name = "String To Nullable String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<string?, string>{ Name = "Nullable String To String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<string?, string>{ Name = "Nullable String To String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<string?, string?>{ Name = "Nullable String To Nullable String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<string?, string?>{ Name = "Nullable String To Nullable String (42)", Input = "42", ExpectedResult = true, ExpectedOutput = "42" },

        new NonGenericCoerceTest<string, bool?>{ Name = "String To Nullable Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<string?, bool>{ Name = "Nullable String To Bool (null)", Input = null, ExpectedResult = false },
        new NonGenericCoerceTest<string?, bool>{ Name = "Nullable String To Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<string?, bool?>{ Name = "Nullable String To Nullable Bool (null)", Input = null, ExpectedResult = true, ExpectedOutput = new bool?() },
        new NonGenericCoerceTest<string?, bool?>{ Name = "Nullable String To Nullable Bool (true)", Input = "true", ExpectedResult = true, ExpectedOutput = new bool?(true) },

        new NonGenericCoerceTest<string, byte?>{ Name = "String To Nullable Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = new byte?(42) },
        new NonGenericCoerceTest<string?, byte>{ Name = "Nullable String To Byte (null)", Input = null, ExpectedResult = false },
        new NonGenericCoerceTest<string?, byte>{ Name = "Nullable String To Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<string?, byte?>{ Name = "Nullable String To Nullable Byte (null)", Input = null, ExpectedResult = true, ExpectedOutput = new byte?() },
        new NonGenericCoerceTest<string?, byte?>{ Name = "Nullable String To Nullable Byte (42)", Input = "42", ExpectedResult = true, ExpectedOutput = new byte?(42) },

        // Interface/Class Types
        new NonGenericCoerceTest<string, IInterface>{ Name = "String To Interface (42)", Input = "42", ExpectedResult = false },
        new NonGenericCoerceTest<string, BaseClass>{ Name = "String To BaseClass (42)", Input = "42", ExpectedResult = false },
        new NonGenericCoerceTest<string, DerivedClass>{ Name = "String To DerivedClass (42)", Input = "42", ExpectedResult = false },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GenericCoerceTheoryData))]
    public void GenericCoerce(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(NonGenericCoerceTheoryData))]
    public void NonGenericCoerce(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}
