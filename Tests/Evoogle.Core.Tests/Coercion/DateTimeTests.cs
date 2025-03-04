// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class DateTimeTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<DateTime, bool>{ Name = $"DateTime To Bool ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, byte>{ Name = $"DateTime To Byte ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, byte[]>{ Name = $"DateTime To ByteArray ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, char>{ Name = $"DateTime To Char ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, DateTime>{ Name = $"DateTime To DateTime ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<DateTime, DateTimeOffset>{ Name = $"DateTime To DateTimeOffset ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<DateTime, decimal>{ Name = $"DateTime To Decimal ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, double>{ Name = $"DateTime To Double ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, float>{ Name = $"DateTime To Float ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, Guid>{ Name = $"DateTime To Guid ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, int>{ Name = $"DateTime To Int ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, long>{ Name = $"DateTime To Long ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, PrimaryColor>{ Name = $"DateTime To Enum ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, sbyte>{ Name = $"DateTime To SByte ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, short>{ Name = $"DateTime To Short ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, string>{ Name = $"DateTime To String ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTimeString },
        new GenericCoerceTest<DateTime, string>{ Name = $"DateTime To String With Format ({TestDateTimeString})", Input = TestDateTime, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTimeStringWithFormat },
        new GenericCoerceTest<DateTime, string>{ Name = $"DateTime To String With Format And FormatProvider ({TestDateTimeString})", Input = TestDateTime, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTimeStringWithFormatAndFormatProvider },
        new GenericCoerceTest<DateTime, TimeSpan>{ Name = $"DateTime To TimeSpan ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, Type>{ Name = $"DateTime To Type ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, uint>{ Name = $"DateTime To UInt ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, Ulid>{ Name = $"DateTime To Ulid ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, ulong>{ Name = $"DateTime To ULong ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, Uri>{ Name = $"DateTime To Uri ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, ushort>{ Name = $"DateTime To UShort ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<DateTime, DateTime?>{ Name = $"DateTime To Nullable DateTime ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },
        new GenericCoerceTest<DateTime?, DateTime>{ Name = "Nullable DateTime To DateTime (null)", Input = new DateTime?(), ExpectedResult = false },
        new GenericCoerceTest<DateTime?, DateTime>{ Name = $"Nullable DateTime To DateTime ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<DateTime?, DateTime?>{ Name = "Nullable DateTime To Nullable DateTime (null)", Input = new DateTime?(), ExpectedResult = true, ExpectedOutput = new DateTime?() },
        new GenericCoerceTest<DateTime?, DateTime?>{ Name = $"Nullable DateTime To Nullable DateTime ({TestDateTimeString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },

        new GenericCoerceTest<DateTime, DateTimeOffset?>{ Name = $"DateTime To Nullable DateTimeOffset ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },
        new GenericCoerceTest<DateTime?, DateTimeOffset>{ Name = $"Nullable DateTime To DateTimeOffset ({TestDateTimeString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<DateTime?, DateTimeOffset?>{ Name = $"Nullable DateTime To Nullable DateTimeOffset ({TestDateTimeString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },

        // Interface/Class Types
        new GenericCoerceTest<DateTime, IInterface>{ Name = $"DateTime To Interface ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, BaseClass>{ Name = $"DateTime To BaseClass ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new GenericCoerceTest<DateTime, DerivedClass>{ Name = $"DateTime To DerivedClass ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<DateTime, bool>{ Name = $"DateTime To Bool ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, byte>{ Name = $"DateTime To Byte ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, byte[]>{ Name = $"DateTime To ByteArray ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, char>{ Name = $"DateTime To Char ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, DateTime>{ Name = $"DateTime To DateTime ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<DateTime, DateTimeOffset>{ Name = $"DateTime To DateTimeOffset ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<DateTime, decimal>{ Name = $"DateTime To Decimal ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, double>{ Name = $"DateTime To Double ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, float>{ Name = $"DateTime To Float ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, Guid>{ Name = $"DateTime To Guid ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, int>{ Name = $"DateTime To Int ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, long>{ Name = $"DateTime To Long ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, PrimaryColor>{ Name = $"DateTime To Enum ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, sbyte>{ Name = $"DateTime To SByte ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, short>{ Name = $"DateTime To Short ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, string>{ Name = $"DateTime To String ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTimeString },
        new NonGenericCoerceTest<DateTime, string>{ Name = $"DateTime To String With Format ({TestDateTimeString})", Input = TestDateTime, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTimeStringWithFormat },
        new NonGenericCoerceTest<DateTime, string>{ Name = $"DateTime To String With Format And FormatProvider ({TestDateTimeString})", Input = TestDateTime, ContextFactoryExpression = () => CreateTestDateTimeContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTimeStringWithFormatAndFormatProvider },
        new NonGenericCoerceTest<DateTime, TimeSpan>{ Name = $"DateTime To TimeSpan ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, Type>{ Name = $"DateTime To Type ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, uint>{ Name = $"DateTime To UInt ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, Ulid>{ Name = $"DateTime To Ulid ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, ulong>{ Name = $"DateTime To ULong ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, Uri>{ Name = $"DateTime To Uri ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, ushort>{ Name = $"DateTime To UShort ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<DateTime, DateTime?>{ Name = $"DateTime To Nullable DateTime ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },
        new NonGenericCoerceTest<DateTime?, DateTime>{ Name = "Nullable DateTime To DateTime (null)", Input = new DateTime?(), ExpectedResult = false },
        new NonGenericCoerceTest<DateTime?, DateTime>{ Name = $"Nullable DateTime To DateTime ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<DateTime?, DateTime?>{ Name = "Nullable DateTime To Nullable DateTime (null)", Input = new DateTime?(), ExpectedResult = true, ExpectedOutput = new DateTime?() },
        new NonGenericCoerceTest<DateTime?, DateTime?>{ Name = $"Nullable DateTime To Nullable DateTime ({TestDateTimeString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },

        new NonGenericCoerceTest<DateTime, DateTimeOffset?>{ Name = $"DateTime To Nullable DateTimeOffset ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },
        new NonGenericCoerceTest<DateTime?, DateTimeOffset>{ Name = $"Nullable DateTime To DateTimeOffset ({TestDateTimeString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<DateTime?, DateTimeOffset?>{ Name = $"Nullable DateTime To Nullable DateTimeOffset ({TestDateTimeString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },

        // Interface/Class Types
        new NonGenericCoerceTest<DateTime, IInterface>{ Name = $"DateTime To Interface ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, BaseClass>{ Name = $"DateTime To BaseClass ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
        new NonGenericCoerceTest<DateTime, DerivedClass>{ Name = $"DateTime To DerivedClass ({TestDateTimeString})", Input = TestDateTime, ExpectedResult = false },
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