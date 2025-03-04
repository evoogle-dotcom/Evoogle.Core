// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class IntTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<int, bool>{ Name = "Int To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<int, bool>{ Name = "Int To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<int, byte>{ Name = "Int To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, byte[]>{ Name = "Int To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<int, char>{ Name = "Int To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<int, DateTime>{ Name = "Int To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<int, DateTimeOffset>{ Name = "Int To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<int, decimal>{ Name = "Int To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, double>{ Name = "Int To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, float>{ Name = "Int To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, Guid>{ Name = "Int To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, int>{ Name = "Int To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, long>{ Name = "Int To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, PrimaryColor>{ Name = "Int To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<int, PrimaryColor>{ Name = "Int To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, sbyte>{ Name = "Int To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, short>{ Name = "Int To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, string>{ Name = "Int To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<int, TimeSpan>{ Name = "Int To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, Type>{ Name = "Int To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, uint>{ Name = "Int To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, Ulid>{ Name = "Int To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, ulong>{ Name = "Int To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int, Uri>{ Name = "Int To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, ushort>{ Name = "Int To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<int, int?>{ Name = "Int To Nullable Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new int?(42) },
        new GenericCoerceTest<int?, int>{ Name = "Nullable Int To Int (null)", Input = new int?(), ExpectedResult = false },
        new GenericCoerceTest<int?, int>{ Name = "Nullable Int To Int (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<int?, int?>{ Name = "Nullable Int To Nullable Int (null)", Input = new int?(), ExpectedResult = true, ExpectedOutput = new int?() },
        new GenericCoerceTest<int?, int?>{ Name = "Nullable Int To Nullable Int (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = new int?(42) },

        new GenericCoerceTest<int, bool?>{ Name = "Int To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<int?, bool>{ Name = "Nullable Int To Bool (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<int?, bool?>{ Name = "Nullable Int To Nullable Bool (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<int, IInterface>{ Name = "Int To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, BaseClass>{ Name = "Int To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<int, DerivedClass>{ Name = "Int To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<int, bool>{ Name = "Int To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<int, bool>{ Name = "Int To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<int, byte>{ Name = "Int To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, byte[]>{ Name = "Int To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<int, char>{ Name = "Int To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<int, DateTime>{ Name = "Int To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<int, DateTimeOffset>{ Name = "Int To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<int, decimal>{ Name = "Int To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, double>{ Name = "Int To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, float>{ Name = "Int To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, Guid>{ Name = "Int To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, int>{ Name = "Int To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, long>{ Name = "Int To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, PrimaryColor>{ Name = "Int To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<int, PrimaryColor>{ Name = "Int To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, sbyte>{ Name = "Int To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, short>{ Name = "Int To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, string>{ Name = "Int To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<int, TimeSpan>{ Name = "Int To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, Type>{ Name = "Int To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, uint>{ Name = "Int To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, Ulid>{ Name = "Int To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, ulong>{ Name = "Int To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int, Uri>{ Name = "Int To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, ushort>{ Name = "Int To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<int, int?>{ Name = "Int To Nullable Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new int?(42) },
        new NonGenericCoerceTest<int?, int>{ Name = "Nullable Int To Int (null)", Input = new int?(), ExpectedResult = false },
        new NonGenericCoerceTest<int?, int>{ Name = "Nullable Int To Int (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<int?, int?>{ Name = "Nullable Int To Nullable Int (null)", Input = new int?(), ExpectedResult = true, ExpectedOutput = new int?() },
        new NonGenericCoerceTest<int?, int?>{ Name = "Nullable Int To Nullable Int (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = new int?(42) },

        new NonGenericCoerceTest<int, bool?>{ Name = "Int To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<int?, bool>{ Name = "Nullable Int To Bool (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<int?, bool?>{ Name = "Nullable Int To Nullable Bool (42)", Input = new int?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<int, IInterface>{ Name = "Int To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, BaseClass>{ Name = "Int To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<int, DerivedClass>{ Name = "Int To DerivedClass (42)", Input = 42, ExpectedResult = false },
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