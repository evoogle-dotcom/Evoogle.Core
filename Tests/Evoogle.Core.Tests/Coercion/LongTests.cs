// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class LongTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<long, bool>{ Name = "Long To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<long, bool>{ Name = "Long To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<long, byte>{ Name = "Long To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, byte[]>{ Name = "Long To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<long, char>{ Name = "Long To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<long, DateTime>{ Name = "Long To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<long, DateTimeOffset>{ Name = "Long To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<long, decimal>{ Name = "Long To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, double>{ Name = "Long To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, float>{ Name = "Long To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, Guid>{ Name = "Long To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, int>{ Name = "Long To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, long>{ Name = "Long To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, PrimaryColor>{ Name = "Long To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<long, PrimaryColor>{ Name = "Long To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, sbyte>{ Name = "Long To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, short>{ Name = "Long To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, string>{ Name = "Long To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<long, TimeSpan>{ Name = "Long To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, Type>{ Name = "Long To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, uint>{ Name = "Long To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, Ulid>{ Name = "Long To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, ulong>{ Name = "Long To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long, Uri>{ Name = "Long To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, ushort>{ Name = "Long To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<long, long?>{ Name = "Long To Nullable Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new long?(42) },
        new GenericCoerceTest<long?, long>{ Name = "Nullable Long To Long (null)", Input = new long?(), ExpectedResult = false },
        new GenericCoerceTest<long?, long>{ Name = "Nullable Long To Long (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<long?, long?>{ Name = "Nullable Long To Nullable Long (null)", Input = new long?(), ExpectedResult = true, ExpectedOutput = new long?() },
        new GenericCoerceTest<long?, long?>{ Name = "Nullable Long To Nullable Long (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = new long?(42) },

        new GenericCoerceTest<long, bool?>{ Name = "Long To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<long?, bool>{ Name = "Nullable Long To Bool (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<long?, bool?>{ Name = "Nullable Long To Nullable Bool (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<long, IInterface>{ Name = "Long To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, BaseClass>{ Name = "Long To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<long, DerivedClass>{ Name = "Long To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<long, bool>{ Name = "Long To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<long, bool>{ Name = "Long To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<long, byte>{ Name = "Long To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, byte[]>{ Name = "Long To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<long, char>{ Name = "Long To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<long, DateTime>{ Name = "Long To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<long, DateTimeOffset>{ Name = "Long To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<long, decimal>{ Name = "Long To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, double>{ Name = "Long To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, float>{ Name = "Long To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, Guid>{ Name = "Long To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, int>{ Name = "Long To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, long>{ Name = "Long To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, PrimaryColor>{ Name = "Long To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<long, PrimaryColor>{ Name = "Long To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, sbyte>{ Name = "Long To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, short>{ Name = "Long To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, string>{ Name = "Long To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<long, TimeSpan>{ Name = "Long To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, Type>{ Name = "Long To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, uint>{ Name = "Long To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, Ulid>{ Name = "Long To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, ulong>{ Name = "Long To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long, Uri>{ Name = "Long To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, ushort>{ Name = "Long To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<long, long?>{ Name = "Long To Nullable Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new long?(42) },
        new NonGenericCoerceTest<long?, long>{ Name = "Nullable Long To Long (null)", Input = new long?(), ExpectedResult = false },
        new NonGenericCoerceTest<long?, long>{ Name = "Nullable Long To Long (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<long?, long?>{ Name = "Nullable Long To Nullable Long (null)", Input = new long?(), ExpectedResult = true, ExpectedOutput = new long?() },
        new NonGenericCoerceTest<long?, long?>{ Name = "Nullable Long To Nullable Long (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = new long?(42) },

        new NonGenericCoerceTest<long, bool?>{ Name = "Long To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<long?, bool>{ Name = "Nullable Long To Bool (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<long?, bool?>{ Name = "Nullable Long To Nullable Bool (42)", Input = new long?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<long, IInterface>{ Name = "Long To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, BaseClass>{ Name = "Long To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<long, DerivedClass>{ Name = "Long To DerivedClass (42)", Input = 42, ExpectedResult = false },
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