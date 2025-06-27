// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class DecimalTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<decimal, bool>{ Name = "Decimal To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<decimal, bool>{ Name = "Decimal To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<decimal, byte>{ Name = "Decimal To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, byte[]>{ Name = "Decimal To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<decimal, char>{ Name = "Decimal To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<decimal, DateTime>{ Name = "Decimal To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<decimal, DateTimeOffset>{ Name = "Decimal To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<decimal, decimal>{ Name = "Decimal To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, double>{ Name = "Decimal To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, float>{ Name = "Decimal To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, Guid>{ Name = "Decimal To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, int>{ Name = "Decimal To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, long>{ Name = "Decimal To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, ColorSet1>{ Name = "Decimal To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new GenericCoerceTest<decimal, ColorSet1>{ Name = "Decimal To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, sbyte>{ Name = "Decimal To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, short>{ Name = "Decimal To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, string>{ Name = "Decimal To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<decimal, TimeSpan>{ Name = "Decimal To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, Type>{ Name = "Decimal To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, uint>{ Name = "Decimal To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, Ulid>{ Name = "Decimal To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, ulong>{ Name = "Decimal To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal, Uri>{ Name = "Decimal To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, ushort>{ Name = "Decimal To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<decimal, decimal?>{ Name = "Decimal To Nullable Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new decimal?(42) },
        new GenericCoerceTest<decimal?, decimal>{ Name = "Nullable Decimal To Decimal (null)", Input = new decimal?(), ExpectedResult = false },
        new GenericCoerceTest<decimal?, decimal>{ Name = "Nullable Decimal To Decimal (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<decimal?, decimal?>{ Name = "Nullable Decimal To Nullable Decimal (null)", Input = new decimal?(), ExpectedResult = true, ExpectedOutput = new decimal?() },
        new GenericCoerceTest<decimal?, decimal?>{ Name = "Nullable Decimal To Nullable Decimal (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = new decimal?(42) },

        new GenericCoerceTest<decimal, bool?>{ Name = "Decimal To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<decimal?, bool>{ Name = "Nullable Decimal To Bool (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<decimal?, bool?>{ Name = "Nullable Decimal To Nullable Bool (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<decimal, IInterface>{ Name = "Decimal To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, BaseClass>{ Name = "Decimal To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<decimal, DerivedClass>{ Name = "Decimal To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<decimal, bool>{ Name = "Decimal To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<decimal, bool>{ Name = "Decimal To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<decimal, byte>{ Name = "Decimal To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, byte[]>{ Name = "Decimal To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<decimal, char>{ Name = "Decimal To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<decimal, DateTime>{ Name = "Decimal To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<decimal, DateTimeOffset>{ Name = "Decimal To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<decimal, decimal>{ Name = "Decimal To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, double>{ Name = "Decimal To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, float>{ Name = "Decimal To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, Guid>{ Name = "Decimal To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, int>{ Name = "Decimal To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, long>{ Name = "Decimal To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, ColorSet1>{ Name = "Decimal To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = ColorSet1.Red },
        new NonGenericCoerceTest<decimal, ColorSet1>{ Name = "Decimal To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, sbyte>{ Name = "Decimal To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, short>{ Name = "Decimal To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, string>{ Name = "Decimal To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<decimal, TimeSpan>{ Name = "Decimal To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, Type>{ Name = "Decimal To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, uint>{ Name = "Decimal To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, Ulid>{ Name = "Decimal To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, ulong>{ Name = "Decimal To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal, Uri>{ Name = "Decimal To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, ushort>{ Name = "Decimal To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<decimal, decimal?>{ Name = "Decimal To Nullable Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new decimal?(42) },
        new NonGenericCoerceTest<decimal?, decimal>{ Name = "Nullable Decimal To Decimal (null)", Input = new decimal?(), ExpectedResult = false },
        new NonGenericCoerceTest<decimal?, decimal>{ Name = "Nullable Decimal To Decimal (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<decimal?, decimal?>{ Name = "Nullable Decimal To Nullable Decimal (null)", Input = new decimal?(), ExpectedResult = true, ExpectedOutput = new decimal?() },
        new NonGenericCoerceTest<decimal?, decimal?>{ Name = "Nullable Decimal To Nullable Decimal (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = new decimal?(42) },

        new NonGenericCoerceTest<decimal, bool?>{ Name = "Decimal To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<decimal?, bool>{ Name = "Nullable Decimal To Bool (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<decimal?, bool?>{ Name = "Nullable Decimal To Nullable Bool (42)", Input = new decimal?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<decimal, IInterface>{ Name = "Decimal To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, BaseClass>{ Name = "Decimal To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<decimal, DerivedClass>{ Name = "Decimal To DerivedClass (42)", Input = 42, ExpectedResult = false },
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
