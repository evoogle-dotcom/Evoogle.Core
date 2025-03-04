// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class ULongTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<ulong, bool>{ Name = "ULong To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<ulong, bool>{ Name = "ULong To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<ulong, byte>{ Name = "ULong To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, byte[]>{ Name = "ULong To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<ulong, char>{ Name = "ULong To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<ulong, DateTime>{ Name = "ULong To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<ulong, DateTimeOffset>{ Name = "ULong To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<ulong, decimal>{ Name = "ULong To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, double>{ Name = "ULong To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, float>{ Name = "ULong To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, Guid>{ Name = "ULong To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, int>{ Name = "ULong To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, long>{ Name = "ULong To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, PrimaryColor>{ Name = "ULong To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<ulong, PrimaryColor>{ Name = "ULong To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, sbyte>{ Name = "ULong To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, short>{ Name = "ULong To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, string>{ Name = "ULong To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<ulong, TimeSpan>{ Name = "ULong To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, Type>{ Name = "ULong To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, uint>{ Name = "ULong To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, Ulid>{ Name = "ULong To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, ulong>{ Name = "ULong To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong, Uri>{ Name = "ULong To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, ushort>{ Name = "ULong To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<ulong, ulong?>{ Name = "ULong To Nullable ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new ulong?(42) },
        new GenericCoerceTest<ulong?, ulong>{ Name = "Nullable ULong To ULong (null)", Input = new ulong?(), ExpectedResult = false },
        new GenericCoerceTest<ulong?, ulong>{ Name = "Nullable ULong To ULong (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ulong?, ulong?>{ Name = "Nullable ULong To Nullable ULong (null)", Input = new ulong?(), ExpectedResult = true, ExpectedOutput = new ulong?() },
        new GenericCoerceTest<ulong?, ulong?>{ Name = "Nullable ULong To Nullable ULong (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = new ulong?(42) },

        new GenericCoerceTest<ulong, bool?>{ Name = "ULong To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<ulong?, bool>{ Name = "Nullable ULong To Bool (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<ulong?, bool?>{ Name = "Nullable ULong To Nullable Bool (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<ulong, IInterface>{ Name = "ULong To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, BaseClass>{ Name = "ULong To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ulong, DerivedClass>{ Name = "ULong To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<ulong, bool>{ Name = "ULong To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<ulong, bool>{ Name = "ULong To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<ulong, byte>{ Name = "ULong To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, byte[]>{ Name = "ULong To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<ulong, char>{ Name = "ULong To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<ulong, DateTime>{ Name = "ULong To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<ulong, DateTimeOffset>{ Name = "ULong To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<ulong, decimal>{ Name = "ULong To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, double>{ Name = "ULong To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, float>{ Name = "ULong To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, Guid>{ Name = "ULong To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, int>{ Name = "ULong To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, long>{ Name = "ULong To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, PrimaryColor>{ Name = "ULong To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<ulong, PrimaryColor>{ Name = "ULong To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, sbyte>{ Name = "ULong To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, short>{ Name = "ULong To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, string>{ Name = "ULong To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<ulong, TimeSpan>{ Name = "ULong To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, Type>{ Name = "ULong To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, uint>{ Name = "ULong To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, Ulid>{ Name = "ULong To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, ulong>{ Name = "ULong To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong, Uri>{ Name = "ULong To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, ushort>{ Name = "ULong To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<ulong, ulong?>{ Name = "ULong To Nullable ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new ulong?(42) },
        new NonGenericCoerceTest<ulong?, ulong>{ Name = "Nullable ULong To ULong (null)", Input = new ulong?(), ExpectedResult = false },
        new NonGenericCoerceTest<ulong?, ulong>{ Name = "Nullable ULong To ULong (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ulong?, ulong?>{ Name = "Nullable ULong To Nullable ULong (null)", Input = new ulong?(), ExpectedResult = true, ExpectedOutput = new ulong?() },
        new NonGenericCoerceTest<ulong?, ulong?>{ Name = "Nullable ULong To Nullable ULong (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = new ulong?(42) },

        new NonGenericCoerceTest<ulong, bool?>{ Name = "ULong To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<ulong?, bool>{ Name = "Nullable ULong To Bool (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<ulong?, bool?>{ Name = "Nullable ULong To Nullable Bool (42)", Input = new ulong?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<ulong, IInterface>{ Name = "ULong To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, BaseClass>{ Name = "ULong To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ulong, DerivedClass>{ Name = "ULong To DerivedClass (42)", Input = 42, ExpectedResult = false },
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