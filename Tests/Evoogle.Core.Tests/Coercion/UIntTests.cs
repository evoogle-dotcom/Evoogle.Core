// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class UIntTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<uint, bool>{ Name = "UInt To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<uint, bool>{ Name = "UInt To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<uint, byte>{ Name = "UInt To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, byte[]>{ Name = "UInt To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<uint, char>{ Name = "UInt To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<uint, DateTime>{ Name = "UInt To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<uint, DateTimeOffset>{ Name = "UInt To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<uint, decimal>{ Name = "UInt To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, double>{ Name = "UInt To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, float>{ Name = "UInt To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, Guid>{ Name = "UInt To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, int>{ Name = "UInt To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, long>{ Name = "UInt To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, ColorSet1>{ Name = "UInt To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new GenericCoerceTest<uint, ColorSet1>{ Name = "UInt To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, sbyte>{ Name = "UInt To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, short>{ Name = "UInt To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, string>{ Name = "UInt To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<uint, TimeSpan>{ Name = "UInt To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, Type>{ Name = "UInt To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, uint>{ Name = "UInt To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, Ulid>{ Name = "UInt To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, ulong>{ Name = "UInt To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint, Uri>{ Name = "UInt To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, ushort>{ Name = "UInt To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<uint, uint?>{ Name = "UInt To Nullable UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new uint?(42) },
        new GenericCoerceTest<uint?, uint>{ Name = "Nullable UInt To UInt (null)", Input = new uint?(), ExpectedResult = false },
        new GenericCoerceTest<uint?, uint>{ Name = "Nullable UInt To UInt (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<uint?, uint?>{ Name = "Nullable UInt To Nullable UInt (null)", Input = new uint?(), ExpectedResult = true, ExpectedOutput = new uint?() },
        new GenericCoerceTest<uint?, uint?>{ Name = "Nullable UInt To Nullable UInt (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = new uint?(42) },

        new GenericCoerceTest<uint, bool?>{ Name = "UInt To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<uint?, bool>{ Name = "Nullable UInt To Bool (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<uint?, bool?>{ Name = "Nullable UInt To Nullable Bool (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<uint, IInterface>{ Name = "UInt To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, BaseClass>{ Name = "UInt To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<uint, DerivedClass>{ Name = "UInt To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<uint, bool>{ Name = "UInt To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<uint, bool>{ Name = "UInt To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<uint, byte>{ Name = "UInt To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, byte[]>{ Name = "UInt To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<uint, char>{ Name = "UInt To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<uint, DateTime>{ Name = "UInt To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<uint, DateTimeOffset>{ Name = "UInt To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<uint, decimal>{ Name = "UInt To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, double>{ Name = "UInt To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, float>{ Name = "UInt To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, Guid>{ Name = "UInt To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, int>{ Name = "UInt To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, long>{ Name = "UInt To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, ColorSet1>{ Name = "UInt To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new NonGenericCoerceTest<uint, ColorSet1>{ Name = "UInt To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, sbyte>{ Name = "UInt To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, short>{ Name = "UInt To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, string>{ Name = "UInt To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<uint, TimeSpan>{ Name = "UInt To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, Type>{ Name = "UInt To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, uint>{ Name = "UInt To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, Ulid>{ Name = "UInt To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, ulong>{ Name = "UInt To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint, Uri>{ Name = "UInt To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, ushort>{ Name = "UInt To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<uint, uint?>{ Name = "UInt To Nullable UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new uint?(42) },
        new NonGenericCoerceTest<uint?, uint>{ Name = "Nullable UInt To UInt (null)", Input = new uint?(), ExpectedResult = false },
        new NonGenericCoerceTest<uint?, uint>{ Name = "Nullable UInt To UInt (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<uint?, uint?>{ Name = "Nullable UInt To Nullable UInt (null)", Input = new uint?(), ExpectedResult = true, ExpectedOutput = new uint?() },
        new NonGenericCoerceTest<uint?, uint?>{ Name = "Nullable UInt To Nullable UInt (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = new uint?(42) },

        new NonGenericCoerceTest<uint, bool?>{ Name = "UInt To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<uint?, bool>{ Name = "Nullable UInt To Bool (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<uint?, bool?>{ Name = "Nullable UInt To Nullable Bool (42)", Input = new uint?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<uint, IInterface>{ Name = "UInt To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, BaseClass>{ Name = "UInt To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<uint, DerivedClass>{ Name = "UInt To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GenericCoerceTheoryData))]
    public void GenericCoerce(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(NonGenericCoerceTheoryData))]
    public void NonGenericCoerce(IXUnitTest test) => test.Execute(this);
    #endregion
}
