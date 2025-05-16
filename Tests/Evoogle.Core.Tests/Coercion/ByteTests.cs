// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class ByteTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<byte, bool>{ Name = "Byte To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<byte, bool>{ Name = "Byte To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<byte, byte>{ Name = "Byte To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, byte[]>{ Name = "Byte To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<byte, char>{ Name = "Byte To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<byte, DateTime>{ Name = "Byte To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<byte, DateTimeOffset>{ Name = "Byte To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<byte, decimal>{ Name = "Byte To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, double>{ Name = "Byte To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, float>{ Name = "Byte To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, Guid>{ Name = "Byte To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, int>{ Name = "Byte To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, long>{ Name = "Byte To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, PrimaryColor>{ Name = "Byte To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<byte, PrimaryColor>{ Name = "Byte To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, sbyte>{ Name = "Byte To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, short>{ Name = "Byte To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, string>{ Name = "Byte To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<byte, TimeSpan>{ Name = "Byte To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, Type>{ Name = "Byte To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, uint>{ Name = "Byte To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, Ulid>{ Name = "Byte To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, ulong>{ Name = "Byte To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte, Uri>{ Name = "Byte To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, ushort>{ Name = "Byte To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<byte, byte?>{ Name = "Byte To Nullable Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new byte?(42) },
        new GenericCoerceTest<byte?, byte>{ Name = "Nullable Byte To Byte (null)", Input = new byte?(), ExpectedResult = false },
        new GenericCoerceTest<byte?, byte>{ Name = "Nullable Byte To Byte (42)", Input = new byte?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<byte?, byte?>{ Name = "Nullable Byte To Nullable Byte (null)", Input = new byte?(), ExpectedResult = true, ExpectedOutput = new byte?() },
        new GenericCoerceTest<byte?, byte?>{ Name = "Nullable Byte To Nullable Byte (42)", Input = new byte?(42), ExpectedResult = true, ExpectedOutput = new byte?(42) },

        new GenericCoerceTest<byte, bool?>{ Name = "Byte To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<byte?, bool>{ Name = "Nullable Byte To Bool (42)", Input = new byte?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<byte?, bool?>{ Name = "Nullable Byte To Nullable Bool (42)", Input = new byte?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<byte, IInterface>{ Name = "Byte To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, BaseClass>{ Name = "Byte To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<byte, DerivedClass>{ Name = "Byte To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<byte, bool>{ Name = "Byte To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<byte, bool>{ Name = "Byte To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<byte, byte>{ Name = "Byte To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, byte[]>{ Name = "Byte To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<byte, char>{ Name = "Byte To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<byte, DateTime>{ Name = "Byte To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<byte, DateTimeOffset>{ Name = "Byte To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<byte, decimal>{ Name = "Byte To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, double>{ Name = "Byte To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, float>{ Name = "Byte To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, Guid>{ Name = "Byte To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, int>{ Name = "Byte To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, long>{ Name = "Byte To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, PrimaryColor>{ Name = "Byte To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<byte, PrimaryColor>{ Name = "Byte To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, sbyte>{ Name = "Byte To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, short>{ Name = "Byte To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, string>{ Name = "Byte To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<byte, TimeSpan>{ Name = "Byte To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, Type>{ Name = "Byte To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, uint>{ Name = "Byte To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, Ulid>{ Name = "Byte To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, ulong>{ Name = "Byte To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte, Uri>{ Name = "Byte To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, ushort>{ Name = "Byte To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<byte, byte?>{ Name = "Byte To Nullable Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new byte?(42) },
        new NonGenericCoerceTest<byte?, byte>{ Name = "Nullable Byte To Byte (null)", Input = new byte?(), ExpectedResult = false },
        new NonGenericCoerceTest<byte?, byte>{ Name = "Nullable Byte To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<byte?, byte?>{ Name = "Nullable Byte To Nullable Byte (null)", Input = new byte?(), ExpectedResult = true, ExpectedOutput = new byte?() },
        new NonGenericCoerceTest<byte?, byte?>{ Name = "Nullable Byte To Nullable Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new byte?(42) },

        new NonGenericCoerceTest<byte, bool?>{ Name = "Byte To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<byte?, bool>{ Name = "Nullable Byte To Bool (42)", Input = new byte?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<byte?, bool?>{ Name = "Nullable Byte To Nullable Bool (42)", Input = new byte?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<byte, IInterface>{ Name = "Byte To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, BaseClass>{ Name = "Byte To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<byte, DerivedClass>{ Name = "Byte To DerivedClass (42)", Input = 42, ExpectedResult = false },
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