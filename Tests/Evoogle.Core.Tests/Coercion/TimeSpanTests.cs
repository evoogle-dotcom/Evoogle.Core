// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class TimeSpanTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<TimeSpan, bool>{ Name = $"TimeSpan To Bool ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, byte>{ Name = $"TimeSpan To Byte ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, byte[]>{ Name = $"TimeSpan To ByteArray ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, char>{ Name = $"TimeSpan To Char ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, DateTime>{ Name = $"TimeSpan To DateTime ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, DateTimeOffset>{ Name = $"TimeSpan To DateTimeOffset ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, decimal>{ Name = $"TimeSpan To Decimal ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, double>{ Name = $"TimeSpan To Double ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, float>{ Name = $"TimeSpan To Float ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, Guid>{ Name = $"TimeSpan To Guid ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, int>{ Name = $"TimeSpan To Int ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, long>{ Name = $"TimeSpan To Long ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, PrimaryColor>{ Name = $"TimeSpan To Enum ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, sbyte>{ Name = $"TimeSpan To SByte ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, short>{ Name = $"TimeSpan To Short ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, string>{ Name = $"TimeSpan To String ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpanString },
        new GenericCoerceTest<TimeSpan, string>{ Name = $"TimeSpan To String With Format ({TestTimeSpanString})", Input = TestTimeSpan, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestTimeSpanStringWithFormat },
        new GenericCoerceTest<TimeSpan, string>{ Name = $"TimeSpan To String With Format And FormatProvider ({TestTimeSpanString})", Input = TestTimeSpan, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestTimeSpanStringWithFormatAndFormatProvider },
        new GenericCoerceTest<TimeSpan, TimeSpan>{ Name = $"TimeSpan To TimeSpan ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new GenericCoerceTest<TimeSpan, Type>{ Name = $"TimeSpan To Type ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, uint>{ Name = $"TimeSpan To UInt ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, Ulid>{ Name = $"TimeSpan To Ulid ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, ulong>{ Name = $"TimeSpan To ULong ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, Uri>{ Name = $"TimeSpan To Uri ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, ushort>{ Name = $"TimeSpan To UShort ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<TimeSpan, TimeSpan?>{ Name = $"TimeSpan To Nullable TimeSpan ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = new TimeSpan?(TestTimeSpan) },
        new GenericCoerceTest<TimeSpan?, TimeSpan>{ Name = "Nullable TimeSpan To TimeSpan (null)", Input = new TimeSpan?(), ExpectedResult = false },
        new GenericCoerceTest<TimeSpan?, TimeSpan>{ Name = $"Nullable TimeSpan To TimeSpan ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new GenericCoerceTest<TimeSpan?, TimeSpan?>{ Name = "Nullable TimeSpan To Nullable TimeSpan (null)", Input = new TimeSpan?(), ExpectedResult = true, ExpectedOutput = new TimeSpan?() },
        new GenericCoerceTest<TimeSpan?, TimeSpan?>{ Name = $"Nullable TimeSpan To Nullable TimeSpan ({TestTimeSpanString})", Input = new TimeSpan?(TestTimeSpan), ExpectedResult = true, ExpectedOutput = new TimeSpan?(TestTimeSpan) },

        new GenericCoerceTest<TimeSpan, string?>{ Name = $"TimeSpan To Nullable String ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpanString },
        new GenericCoerceTest<TimeSpan?, string>{ Name = $"Nullable TimeSpan To String ({TestTimeSpanString})", Input = new TimeSpan?(TestTimeSpan), ExpectedResult = true, ExpectedOutput = TestTimeSpanString },
        new GenericCoerceTest<TimeSpan?, string?>{ Name = $"Nullable TimeSpan To Nullable String ({TestTimeSpanString})", Input = new TimeSpan?(TestTimeSpan), ExpectedResult = true, ExpectedOutput = TestTimeSpanString },

        // Interface/Class Types
        new GenericCoerceTest<TimeSpan, IInterface>{ Name = $"TimeSpan To Interface ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, BaseClass>{ Name = $"TimeSpan To BaseClass ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new GenericCoerceTest<TimeSpan, DerivedClass>{ Name = $"TimeSpan To DerivedClass ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<TimeSpan, bool>{ Name = $"TimeSpan To Bool ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, byte>{ Name = $"TimeSpan To Byte ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, byte[]>{ Name = $"TimeSpan To ByteArray ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, char>{ Name = $"TimeSpan To Char ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, DateTime>{ Name = $"TimeSpan To DateTime ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, DateTimeOffset>{ Name = $"TimeSpan To DateTimeOffset ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, decimal>{ Name = $"TimeSpan To Decimal ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, double>{ Name = $"TimeSpan To Double ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, float>{ Name = $"TimeSpan To Float ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, Guid>{ Name = $"TimeSpan To Guid ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, int>{ Name = $"TimeSpan To Int ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, long>{ Name = $"TimeSpan To Long ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, PrimaryColor>{ Name = $"TimeSpan To Enum ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, sbyte>{ Name = $"TimeSpan To SByte ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, short>{ Name = $"TimeSpan To Short ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, string>{ Name = $"TimeSpan To String ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpanString },
        new NonGenericCoerceTest<TimeSpan, string>{ Name = $"TimeSpan To String With Format ({TestTimeSpanString})", Input = TestTimeSpan, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormat(), ExpectedResult = true, ExpectedOutput = TestTimeSpanStringWithFormat },
        new NonGenericCoerceTest<TimeSpan, string>{ Name = $"TimeSpan To String With Format And FormatProvider ({TestTimeSpanString})", Input = TestTimeSpan, ContextFactoryExpression = () => CreateTestTimeSpanContextWithFormatAndFormatProvider(), ExpectedResult = true, ExpectedOutput = TestTimeSpanStringWithFormatAndFormatProvider },
        new NonGenericCoerceTest<TimeSpan, TimeSpan>{ Name = $"TimeSpan To TimeSpan ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new NonGenericCoerceTest<TimeSpan, Type>{ Name = $"TimeSpan To Type ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, uint>{ Name = $"TimeSpan To UInt ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, Ulid>{ Name = $"TimeSpan To Ulid ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, ulong>{ Name = $"TimeSpan To ULong ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, Uri>{ Name = $"TimeSpan To Uri ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, ushort>{ Name = $"TimeSpan To UShort ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<TimeSpan, TimeSpan?>{ Name = $"TimeSpan To Nullable TimeSpan ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = new TimeSpan?(TestTimeSpan) },
        new NonGenericCoerceTest<TimeSpan?, TimeSpan>{ Name = "Nullable TimeSpan To TimeSpan (null)", Input = new TimeSpan?(), ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan?, TimeSpan>{ Name = $"Nullable TimeSpan To TimeSpan ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpan },
        new NonGenericCoerceTest<TimeSpan?, TimeSpan?>{ Name = "Nullable TimeSpan To Nullable TimeSpan (null)", Input = new TimeSpan?(), ExpectedResult = true, ExpectedOutput = new TimeSpan?() },
        new NonGenericCoerceTest<TimeSpan?, TimeSpan?>{ Name = $"Nullable TimeSpan To Nullable TimeSpan ({TestTimeSpanString})", Input = new TimeSpan?(TestTimeSpan), ExpectedResult = true, ExpectedOutput = new TimeSpan?(TestTimeSpan) },

        new NonGenericCoerceTest<TimeSpan, string?>{ Name = $"TimeSpan To Nullable String ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = true, ExpectedOutput = TestTimeSpanString },
        new NonGenericCoerceTest<TimeSpan?, string>{ Name = $"Nullable TimeSpan To String ({TestTimeSpanString})", Input = new TimeSpan?(TestTimeSpan), ExpectedResult = true, ExpectedOutput = TestTimeSpanString },
        new NonGenericCoerceTest<TimeSpan?, string?>{ Name = $"Nullable TimeSpan To Nullable String ({TestTimeSpanString})", Input = new TimeSpan?(TestTimeSpan), ExpectedResult = true, ExpectedOutput = TestTimeSpanString },

        // Interface/Class Types
        new NonGenericCoerceTest<TimeSpan, IInterface>{ Name = $"TimeSpan To Interface ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, BaseClass>{ Name = $"TimeSpan To BaseClass ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
        new NonGenericCoerceTest<TimeSpan, DerivedClass>{ Name = $"TimeSpan To DerivedClass ({TestTimeSpanString})", Input = TestTimeSpan, ExpectedResult = false },
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