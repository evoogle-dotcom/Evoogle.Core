// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class ShortTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<short, bool>{ Name = "Short To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<short, bool>{ Name = "Short To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<short, byte>{ Name = "Short To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, byte[]>{ Name = "Short To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<short, char>{ Name = "Short To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<short, DateTime>{ Name = "Short To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<short, DateTimeOffset>{ Name = "Short To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<short, decimal>{ Name = "Short To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, double>{ Name = "Short To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, float>{ Name = "Short To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, Guid>{ Name = "Short To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, int>{ Name = "Short To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, long>{ Name = "Short To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, PrimaryColor>{ Name = "Short To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<short, PrimaryColor>{ Name = "Short To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, sbyte>{ Name = "Short To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, short>{ Name = "Short To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, string>{ Name = "Short To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<short, TimeSpan>{ Name = "Short To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, Type>{ Name = "Short To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, uint>{ Name = "Short To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, Ulid>{ Name = "Short To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, ulong>{ Name = "Short To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short, Uri>{ Name = "Short To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, ushort>{ Name = "Short To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<short, short?>{ Name = "Short To Nullable Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new short?(42) },
        new GenericCoerceTest<short?, short>{ Name = "Nullable Short To Short (null)", Input = new short?(), ExpectedResult = false },
        new GenericCoerceTest<short?, short>{ Name = "Nullable Short To Short (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<short?, short?>{ Name = "Nullable Short To Nullable Short (null)", Input = new short?(), ExpectedResult = true, ExpectedOutput = new short?() },
        new GenericCoerceTest<short?, short?>{ Name = "Nullable Short To Nullable Short (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = new short?(42) },

        new GenericCoerceTest<short, bool?>{ Name = "Short To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<short?, bool>{ Name = "Nullable Short To Bool (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<short?, bool?>{ Name = "Nullable Short To Nullable Bool (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<short, IInterface>{ Name = "Short To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, BaseClass>{ Name = "Short To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<short, DerivedClass>{ Name = "Short To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<short, bool>{ Name = "Short To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<short, bool>{ Name = "Short To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<short, byte>{ Name = "Short To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, byte[]>{ Name = "Short To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<short, char>{ Name = "Short To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<short, DateTime>{ Name = "Short To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<short, DateTimeOffset>{ Name = "Short To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<short, decimal>{ Name = "Short To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, double>{ Name = "Short To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, float>{ Name = "Short To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, Guid>{ Name = "Short To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, int>{ Name = "Short To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, long>{ Name = "Short To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, PrimaryColor>{ Name = "Short To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<short, PrimaryColor>{ Name = "Short To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, sbyte>{ Name = "Short To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, short>{ Name = "Short To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, string>{ Name = "Short To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<short, TimeSpan>{ Name = "Short To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, Type>{ Name = "Short To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, uint>{ Name = "Short To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, Ulid>{ Name = "Short To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, ulong>{ Name = "Short To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short, Uri>{ Name = "Short To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, ushort>{ Name = "Short To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<short, short?>{ Name = "Short To Nullable Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new short?(42) },
        new NonGenericCoerceTest<short?, short>{ Name = "Nullable Short To Short (null)", Input = new short?(), ExpectedResult = false },
        new NonGenericCoerceTest<short?, short>{ Name = "Nullable Short To Short (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<short?, short?>{ Name = "Nullable Short To Nullable Short (null)", Input = new short?(), ExpectedResult = true, ExpectedOutput = new short?() },
        new NonGenericCoerceTest<short?, short?>{ Name = "Nullable Short To Nullable Short (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = new short?(42) },

        new NonGenericCoerceTest<short, bool?>{ Name = "Short To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<short?, bool>{ Name = "Nullable Short To Bool (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<short?, bool?>{ Name = "Nullable Short To Nullable Bool (42)", Input = new short?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<short, IInterface>{ Name = "Short To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, BaseClass>{ Name = "Short To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<short, DerivedClass>{ Name = "Short To DerivedClass (42)", Input = 42, ExpectedResult = false },
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