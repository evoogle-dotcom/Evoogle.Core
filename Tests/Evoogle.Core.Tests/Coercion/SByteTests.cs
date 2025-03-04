// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class SByteTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<sbyte, bool>{ Name = "SByte To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<sbyte, bool>{ Name = "SByte To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<sbyte, byte>{ Name = "SByte To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, byte[]>{ Name = "SByte To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<sbyte, char>{ Name = "SByte To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<sbyte, DateTime>{ Name = "SByte To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<sbyte, DateTimeOffset>{ Name = "SByte To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<sbyte, decimal>{ Name = "SByte To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, double>{ Name = "SByte To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, float>{ Name = "SByte To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, Guid>{ Name = "SByte To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, int>{ Name = "SByte To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, long>{ Name = "SByte To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, PrimaryColor>{ Name = "SByte To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<sbyte, PrimaryColor>{ Name = "SByte To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, sbyte>{ Name = "SByte To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, short>{ Name = "SByte To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, string>{ Name = "SByte To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<sbyte, TimeSpan>{ Name = "SByte To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, Type>{ Name = "SByte To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, uint>{ Name = "SByte To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, Ulid>{ Name = "SByte To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, ulong>{ Name = "SByte To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte, Uri>{ Name = "SByte To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, ushort>{ Name = "SByte To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<sbyte, sbyte?>{ Name = "SByte To Nullable SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new sbyte?(42) },
        new GenericCoerceTest<sbyte?, sbyte>{ Name = "Nullable SByte To SByte (null)", Input = new sbyte?(), ExpectedResult = false },
        new GenericCoerceTest<sbyte?, sbyte>{ Name = "Nullable SByte To SByte (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<sbyte?, sbyte?>{ Name = "Nullable SByte To Nullable SByte (null)", Input = new sbyte?(), ExpectedResult = true, ExpectedOutput = new sbyte?() },
        new GenericCoerceTest<sbyte?, sbyte?>{ Name = "Nullable SByte To Nullable SByte (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = new sbyte?(42) },

        new GenericCoerceTest<sbyte, bool?>{ Name = "SByte To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<sbyte?, bool>{ Name = "Nullable SByte To Bool (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<sbyte?, bool?>{ Name = "Nullable SByte To Nullable Bool (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<sbyte, IInterface>{ Name = "SByte To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, BaseClass>{ Name = "SByte To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<sbyte, DerivedClass>{ Name = "SByte To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<sbyte, bool>{ Name = "SByte To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<sbyte, bool>{ Name = "SByte To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<sbyte, byte>{ Name = "SByte To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, byte[]>{ Name = "SByte To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<sbyte, char>{ Name = "SByte To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<sbyte, DateTime>{ Name = "SByte To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<sbyte, DateTimeOffset>{ Name = "SByte To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<sbyte, decimal>{ Name = "SByte To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, double>{ Name = "SByte To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, float>{ Name = "SByte To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, Guid>{ Name = "SByte To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, int>{ Name = "SByte To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, long>{ Name = "SByte To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, PrimaryColor>{ Name = "SByte To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<sbyte, PrimaryColor>{ Name = "SByte To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, sbyte>{ Name = "SByte To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, short>{ Name = "SByte To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, string>{ Name = "SByte To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<sbyte, TimeSpan>{ Name = "SByte To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, Type>{ Name = "SByte To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, uint>{ Name = "SByte To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, Ulid>{ Name = "SByte To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, ulong>{ Name = "SByte To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte, Uri>{ Name = "SByte To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, ushort>{ Name = "SByte To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<sbyte, sbyte?>{ Name = "SByte To Nullable SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new sbyte?(42) },
        new NonGenericCoerceTest<sbyte?, sbyte>{ Name = "Nullable SByte To SByte (null)", Input = new sbyte?(), ExpectedResult = false },
        new NonGenericCoerceTest<sbyte?, sbyte>{ Name = "Nullable SByte To SByte (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<sbyte?, sbyte?>{ Name = "Nullable SByte To Nullable SByte (null)", Input = new sbyte?(), ExpectedResult = true, ExpectedOutput = new sbyte?() },
        new NonGenericCoerceTest<sbyte?, sbyte?>{ Name = "Nullable SByte To Nullable SByte (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = new sbyte?(42) },

        new NonGenericCoerceTest<sbyte, bool?>{ Name = "SByte To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<sbyte?, bool>{ Name = "Nullable SByte To Bool (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<sbyte?, bool?>{ Name = "Nullable SByte To Nullable Bool (42)", Input = new sbyte?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<sbyte, IInterface>{ Name = "SByte To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, BaseClass>{ Name = "SByte To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<sbyte, DerivedClass>{ Name = "SByte To DerivedClass (42)", Input = 42, ExpectedResult = false },
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