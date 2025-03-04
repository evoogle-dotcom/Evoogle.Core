// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class DateTimeOffsetTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<DateTimeOffset, bool>{ Name = $"DateTimeOffset To Bool ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, byte>{ Name = $"DateTimeOffset To Byte ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, byte[]>{ Name = $"DateTimeOffset To ByteArray ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, char>{ Name = $"DateTimeOffset To Char ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, DateTime>{ Name = $"DateTimeOffset To DateTime ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<DateTimeOffset, DateTimeOffset>{ Name = $"DateTimeOffset To DateTimeOffset ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<DateTimeOffset, decimal>{ Name = $"DateTimeOffset To Decimal ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, double>{ Name = $"DateTimeOffset To Double ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, float>{ Name = $"DateTimeOffset To Float ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, Guid>{ Name = $"DateTimeOffset To Guid ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, int>{ Name = $"DateTimeOffset To Int ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, long>{ Name = $"DateTimeOffset To Long ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, PrimaryColor>{ Name = $"DateTimeOffset To Enum ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, sbyte>{ Name = $"DateTimeOffset To SByte ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, short>{ Name = $"DateTimeOffset To Short ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, string>{ Name = $"DateTimeOffset To String ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTimeOffsetString },
        new GenericCoerceTest<DateTimeOffset, string>{ Name = $"DateTimeOffset To String With Format ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffsetStringWithFormat },
        new GenericCoerceTest<DateTimeOffset, string>{ Name = $"DateTimeOffset To String With Format And FormatProvider ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffsetStringWithFormatAndFormatProvider },
        new GenericCoerceTest<DateTimeOffset, TimeSpan>{ Name = $"DateTimeOffset To TimeSpan ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, Type>{ Name = $"DateTimeOffset To Type ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, uint>{ Name = $"DateTimeOffset To UInt ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, Ulid>{ Name = $"DateTimeOffset To Ulid ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, ulong>{ Name = $"DateTimeOffset To ULong ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, Uri>{ Name = $"DateTimeOffset To Uri ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, ushort>{ Name = $"DateTimeOffset To UShort ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<DateTimeOffset, DateTime?>{ Name = $"DateTimeOffset To Nullable DateTime ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },
        new GenericCoerceTest<DateTimeOffset?, DateTime>{ Name = "Nullable DateTime To DateTime (null)", Input = new DateTime?(), ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset?, DateTime>{ Name = $"Nullable DateTime To DateTime ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new GenericCoerceTest<DateTimeOffset?, DateTime?>{ Name = "Nullable DateTime To Nullable DateTime (null)", Input = new DateTime?(), ExpectedResult = true, ExpectedOutput = new DateTime?() },
        new GenericCoerceTest<DateTimeOffset?, DateTime?>{ Name = $"Nullable DateTime To Nullable DateTime ({TestDateTimeOffsetString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },

        new GenericCoerceTest<DateTimeOffset, DateTimeOffset?>{ Name = $"DateTimeOffset To Nullable DateTimeOffset ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },
        new GenericCoerceTest<DateTimeOffset?, DateTimeOffset>{ Name = $"Nullable DateTime To DateTimeOffset ({TestDateTimeOffsetString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new GenericCoerceTest<DateTimeOffset?, DateTimeOffset?>{ Name = $"Nullable DateTime To Nullable DateTimeOffset ({TestDateTimeOffsetString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },

        // Interface/Class Types
        new GenericCoerceTest<DateTimeOffset, IInterface>{ Name = $"DateTimeOffset To Interface ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, BaseClass>{ Name = $"DateTimeOffset To BaseClass ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new GenericCoerceTest<DateTimeOffset, DerivedClass>{ Name = $"DateTimeOffset To DerivedClass ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<DateTimeOffset, bool>{ Name = $"DateTimeOffset To Bool ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, byte>{ Name = $"DateTimeOffset To Byte ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, byte[]>{ Name = $"DateTimeOffset To ByteArray ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, char>{ Name = $"DateTimeOffset To Char ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, DateTime>{ Name = $"DateTimeOffset To DateTime ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<DateTimeOffset, DateTimeOffset>{ Name = $"DateTimeOffset To DateTimeOffset ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<DateTimeOffset, decimal>{ Name = $"DateTimeOffset To Decimal ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, double>{ Name = $"DateTimeOffset To Double ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, float>{ Name = $"DateTimeOffset To Float ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, Guid>{ Name = $"DateTimeOffset To Guid ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, int>{ Name = $"DateTimeOffset To Int ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, long>{ Name = $"DateTimeOffset To Long ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, PrimaryColor>{ Name = $"DateTimeOffset To Enum ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, sbyte>{ Name = $"DateTimeOffset To SByte ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, short>{ Name = $"DateTimeOffset To Short ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, string>{ Name = $"DateTimeOffset To String ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTimeOffsetString },
        new NonGenericCoerceTest<DateTimeOffset, string>{ Name = $"DateTimeOffset To String With Format ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffsetStringWithFormat },
        new NonGenericCoerceTest<DateTimeOffset, string>{ Name = $"DateTimeOffset To String With Format And FormatProvider ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ContextFactoryExpression = () => CreateTestDateTimeOffsetContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestDateTimeOffsetStringWithFormatAndFormatProvider },
        new NonGenericCoerceTest<DateTimeOffset, TimeSpan>{ Name = $"DateTimeOffset To TimeSpan ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, Type>{ Name = $"DateTimeOffset To Type ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, uint>{ Name = $"DateTimeOffset To UInt ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, Ulid>{ Name = $"DateTimeOffset To Ulid ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, ulong>{ Name = $"DateTimeOffset To ULong ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, Uri>{ Name = $"DateTimeOffset To Uri ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, ushort>{ Name = $"DateTimeOffset To UShort ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<DateTimeOffset, DateTime?>{ Name = $"DateTimeOffset To Nullable DateTime ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },
        new NonGenericCoerceTest<DateTimeOffset?, DateTime>{ Name = "Nullable DateTime To DateTime (null)", Input = new DateTime?(), ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset?, DateTime>{ Name = $"Nullable DateTime To DateTime ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = TestDateTime },
        new NonGenericCoerceTest<DateTimeOffset?, DateTime?>{ Name = "Nullable DateTime To Nullable DateTime (null)", Input = new DateTime?(), ExpectedResult = true, ExpectedOutput = new DateTime?() },
        new NonGenericCoerceTest<DateTimeOffset?, DateTime?>{ Name = $"Nullable DateTime To Nullable DateTime ({TestDateTimeOffsetString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTime?(TestDateTime) },

        new NonGenericCoerceTest<DateTimeOffset, DateTimeOffset?>{ Name = $"DateTimeOffset To Nullable DateTimeOffset ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },
        new NonGenericCoerceTest<DateTimeOffset?, DateTimeOffset>{ Name = $"Nullable DateTime To DateTimeOffset ({TestDateTimeOffsetString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = TestDateTimeOffset },
        new NonGenericCoerceTest<DateTimeOffset?, DateTimeOffset?>{ Name = $"Nullable DateTime To Nullable DateTimeOffset ({TestDateTimeOffsetString})", Input = new DateTime?(TestDateTime), ExpectedResult = true, ExpectedOutput = new DateTimeOffset?(TestDateTimeOffset) },

        // Interface/Class Types
        new NonGenericCoerceTest<DateTimeOffset, IInterface>{ Name = $"DateTimeOffset To Interface ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, BaseClass>{ Name = $"DateTimeOffset To BaseClass ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
        new NonGenericCoerceTest<DateTimeOffset, DerivedClass>{ Name = $"DateTimeOffset To DerivedClass ({TestDateTimeOffsetString})", Input = TestDateTimeOffset, ExpectedResult = false },
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