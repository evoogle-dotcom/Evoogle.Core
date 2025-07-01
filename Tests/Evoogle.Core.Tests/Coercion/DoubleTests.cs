// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class DoubleTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<double, bool>{ Name = "Double To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<double, bool>{ Name = "Double To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<double, byte>{ Name = "Double To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, byte[]>{ Name = "Double To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<double, char>{ Name = "Double To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<double, DateTime>{ Name = "Double To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<double, DateTimeOffset>{ Name = "Double To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<double, decimal>{ Name = "Double To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, double>{ Name = "Double To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, float>{ Name = "Double To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, Guid>{ Name = "Double To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, int>{ Name = "Double To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, long>{ Name = "Double To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, ColorSet1>{ Name = "Double To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new GenericCoerceTest<double, ColorSet1>{ Name = "Double To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, sbyte>{ Name = "Double To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, short>{ Name = "Double To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, string>{ Name = "Double To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<double, TimeSpan>{ Name = "Double To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, Type>{ Name = "Double To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, uint>{ Name = "Double To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, Ulid>{ Name = "Double To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, ulong>{ Name = "Double To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double, Uri>{ Name = "Double To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, ushort>{ Name = "Double To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<double, double?>{ Name = "Double To Nullable Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new double?(42) },
        new GenericCoerceTest<double?, double>{ Name = "Nullable Double To Double (null)", Input = new double?(), ExpectedResult = false },
        new GenericCoerceTest<double?, double>{ Name = "Nullable Double To Double (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<double?, double?>{ Name = "Nullable Double To Nullable Double (null)", Input = new double?(), ExpectedResult = true, ExpectedOutput = new double?() },
        new GenericCoerceTest<double?, double?>{ Name = "Nullable Double To Nullable Double (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = new double?(42) },

        new GenericCoerceTest<double, bool?>{ Name = "Double To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<double?, bool>{ Name = "Nullable Double To Bool (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<double?, bool?>{ Name = "Nullable Double To Nullable Bool (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<double, IInterface>{ Name = "Double To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, BaseClass>{ Name = "Double To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<double, DerivedClass>{ Name = "Double To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<double, bool>{ Name = "Double To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<double, bool>{ Name = "Double To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<double, byte>{ Name = "Double To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, byte[]>{ Name = "Double To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<double, char>{ Name = "Double To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<double, DateTime>{ Name = "Double To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<double, DateTimeOffset>{ Name = "Double To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<double, decimal>{ Name = "Double To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, double>{ Name = "Double To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, float>{ Name = "Double To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, Guid>{ Name = "Double To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, int>{ Name = "Double To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, long>{ Name = "Double To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, ColorSet1>{ Name = "Double To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new NonGenericCoerceTest<double, ColorSet1>{ Name = "Double To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, sbyte>{ Name = "Double To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, short>{ Name = "Double To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, string>{ Name = "Double To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<double, TimeSpan>{ Name = "Double To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, Type>{ Name = "Double To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, uint>{ Name = "Double To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, Ulid>{ Name = "Double To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, ulong>{ Name = "Double To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double, Uri>{ Name = "Double To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, ushort>{ Name = "Double To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<double, double?>{ Name = "Double To Nullable Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new double?(42) },
        new NonGenericCoerceTest<double?, double>{ Name = "Nullable Double To Double (null)", Input = new double?(), ExpectedResult = false },
        new NonGenericCoerceTest<double?, double>{ Name = "Nullable Double To Double (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<double?, double?>{ Name = "Nullable Double To Nullable Double (null)", Input = new double?(), ExpectedResult = true, ExpectedOutput = new double?() },
        new NonGenericCoerceTest<double?, double?>{ Name = "Nullable Double To Nullable Double (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = new double?(42) },

        new NonGenericCoerceTest<double, bool?>{ Name = "Double To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<double?, bool>{ Name = "Nullable Double To Bool (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<double?, bool?>{ Name = "Nullable Double To Nullable Bool (42)", Input = new double?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<double, IInterface>{ Name = "Double To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, BaseClass>{ Name = "Double To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<double, DerivedClass>{ Name = "Double To DerivedClass (42)", Input = 42, ExpectedResult = false },
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
