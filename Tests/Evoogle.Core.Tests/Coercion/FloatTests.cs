// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class FloatTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<float, bool>{ Name = "Float To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<float, bool>{ Name = "Float To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<float, byte>{ Name = "Float To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, byte[]>{ Name = "Float To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<float, char>{ Name = "Float To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<float, DateTime>{ Name = "Float To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<float, DateTimeOffset>{ Name = "Float To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<float, decimal>{ Name = "Float To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, double>{ Name = "Float To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, float>{ Name = "Float To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, Guid>{ Name = "Float To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, int>{ Name = "Float To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, long>{ Name = "Float To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, ColorSet1>{ Name = "Float To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new GenericCoerceTest<float, ColorSet1>{ Name = "Float To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, sbyte>{ Name = "Float To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, short>{ Name = "Float To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, string>{ Name = "Float To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<float, TimeSpan>{ Name = "Float To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, Type>{ Name = "Float To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, uint>{ Name = "Float To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, Ulid>{ Name = "Float To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, ulong>{ Name = "Float To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float, Uri>{ Name = "Float To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, ushort>{ Name = "Float To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<float, float?>{ Name = "Float To Nullable Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new float?(42) },
        new GenericCoerceTest<float?, float>{ Name = "Nullable Float To Float (null)", Input = new float?(), ExpectedResult = false },
        new GenericCoerceTest<float?, float>{ Name = "Nullable Float To Float (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<float?, float?>{ Name = "Nullable Float To Nullable Float (null)", Input = new float?(), ExpectedResult = true, ExpectedOutput = new float?() },
        new GenericCoerceTest<float?, float?>{ Name = "Nullable Float To Nullable Float (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = new float?(42) },

        new GenericCoerceTest<float, bool?>{ Name = "Float To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<float?, bool>{ Name = "Nullable Float To Bool (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<float?, bool?>{ Name = "Nullable Float To Nullable Bool (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<float, IInterface>{ Name = "Float To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, BaseClass>{ Name = "Float To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<float, DerivedClass>{ Name = "Float To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<float, bool>{ Name = "Float To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<float, bool>{ Name = "Float To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<float, byte>{ Name = "Float To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, byte[]>{ Name = "Float To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<float, char>{ Name = "Float To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<float, DateTime>{ Name = "Float To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<float, DateTimeOffset>{ Name = "Float To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<float, decimal>{ Name = "Float To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, double>{ Name = "Float To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, float>{ Name = "Float To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, Guid>{ Name = "Float To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, int>{ Name = "Float To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, long>{ Name = "Float To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, ColorSet1>{ Name = "Float To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new NonGenericCoerceTest<float, ColorSet1>{ Name = "Float To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, sbyte>{ Name = "Float To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, short>{ Name = "Float To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, string>{ Name = "Float To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<float, TimeSpan>{ Name = "Float To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, Type>{ Name = "Float To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, uint>{ Name = "Float To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, Ulid>{ Name = "Float To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, ulong>{ Name = "Float To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float, Uri>{ Name = "Float To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, ushort>{ Name = "Float To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<float, float?>{ Name = "Float To Nullable Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new float?(42) },
        new NonGenericCoerceTest<float?, float>{ Name = "Nullable Float To Float (null)", Input = new float?(), ExpectedResult = false },
        new NonGenericCoerceTest<float?, float>{ Name = "Nullable Float To Float (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<float?, float?>{ Name = "Nullable Float To Nullable Float (null)", Input = new float?(), ExpectedResult = true, ExpectedOutput = new float?() },
        new NonGenericCoerceTest<float?, float?>{ Name = "Nullable Float To Nullable Float (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = new float?(42) },

        new NonGenericCoerceTest<float, bool?>{ Name = "Float To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<float?, bool>{ Name = "Nullable Float To Bool (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<float?, bool?>{ Name = "Nullable Float To Nullable Bool (42)", Input = new float?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<float, IInterface>{ Name = "Float To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, BaseClass>{ Name = "Float To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<float, DerivedClass>{ Name = "Float To DerivedClass (42)", Input = 42, ExpectedResult = false },
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
