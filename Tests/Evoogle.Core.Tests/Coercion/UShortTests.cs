// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class UShortTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<ushort, bool>{ Name = "UShort To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<ushort, bool>{ Name = "UShort To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<ushort, byte>{ Name = "UShort To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, byte[]>{ Name = "UShort To ByteArray (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<ushort, char>{ Name = "UShort To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new GenericCoerceTest<ushort, DateTime>{ Name = "UShort To DateTime (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<ushort, DateTimeOffset>{ Name = "UShort To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new GenericCoerceTest<ushort, decimal>{ Name = "UShort To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, double>{ Name = "UShort To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, float>{ Name = "UShort To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, Guid>{ Name = "UShort To Guid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, int>{ Name = "UShort To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, long>{ Name = "UShort To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, PrimaryColor>{ Name = "UShort To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<ushort, PrimaryColor>{ Name = "UShort To Enum (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, sbyte>{ Name = "UShort To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, short>{ Name = "UShort To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, string>{ Name = "UShort To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new GenericCoerceTest<ushort, TimeSpan>{ Name = "UShort To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, Type>{ Name = "UShort To Type (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, uint>{ Name = "UShort To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, Ulid>{ Name = "UShort To Ulid (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, ulong>{ Name = "UShort To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort, Uri>{ Name = "UShort To Uri (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, ushort>{ Name = "UShort To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new GenericCoerceTest<ushort, ushort?>{ Name = "UShort To Nullable UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new ushort?(42) },
        new GenericCoerceTest<ushort?, ushort>{ Name = "Nullable UShort To UShort (null)", Input = new ushort?(), ExpectedResult = false },
        new GenericCoerceTest<ushort?, ushort>{ Name = "Nullable UShort To UShort (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new GenericCoerceTest<ushort?, ushort?>{ Name = "Nullable UShort To Nullable UShort (null)", Input = new ushort?(), ExpectedResult = true, ExpectedOutput = new ushort?() },
        new GenericCoerceTest<ushort?, ushort?>{ Name = "Nullable UShort To Nullable UShort (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = new ushort?(42) },

        new GenericCoerceTest<ushort, bool?>{ Name = "UShort To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<ushort?, bool>{ Name = "Nullable UShort To Bool (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<ushort?, bool?>{ Name = "Nullable UShort To Nullable Bool (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new GenericCoerceTest<ushort, IInterface>{ Name = "UShort To Interface (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, BaseClass>{ Name = "UShort To BaseClass (42)", Input = 42, ExpectedResult = false },
        new GenericCoerceTest<ushort, DerivedClass>{ Name = "UShort To DerivedClass (42)", Input = 42, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<ushort, bool>{ Name = "UShort To Bool (0)", Input = 0, ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<ushort, bool>{ Name = "UShort To Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<ushort, byte>{ Name = "UShort To Byte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, byte[]>{ Name = "UShort To ByteArray (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<ushort, char>{ Name = "UShort To Char (42)", Input = 42, ExpectedResult = true, ExpectedOutput = (char)42 },
        new NonGenericCoerceTest<ushort, DateTime>{ Name = "UShort To DateTime (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<ushort, DateTimeOffset>{ Name = "UShort To DateTimeOffset (42)", Input = 42, ExpectedResult = false},
        new NonGenericCoerceTest<ushort, decimal>{ Name = "UShort To Decimal (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, double>{ Name = "UShort To Double (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, float>{ Name = "UShort To Float (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, Guid>{ Name = "UShort To Guid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, int>{ Name = "UShort To Int (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, long>{ Name = "UShort To Long (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, PrimaryColor>{ Name = "UShort To Enum (1)", Input = 1, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<ushort, PrimaryColor>{ Name = "UShort To Enum (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, sbyte>{ Name = "UShort To SByte (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, short>{ Name = "UShort To Short (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, string>{ Name = "UShort To String (42)", Input = 42, ExpectedResult = true, ExpectedOutput = "42" },
        new NonGenericCoerceTest<ushort, TimeSpan>{ Name = "UShort To TimeSpan (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, Type>{ Name = "UShort To Type (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, uint>{ Name = "UShort To UInt (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, Ulid>{ Name = "UShort To Ulid (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, ulong>{ Name = "UShort To ULong (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort, Uri>{ Name = "UShort To Uri (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, ushort>{ Name = "UShort To UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = 42 },

        // Nullable Types
        new NonGenericCoerceTest<ushort, ushort?>{ Name = "UShort To Nullable UShort (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new ushort?(42) },
        new NonGenericCoerceTest<ushort?, ushort>{ Name = "Nullable UShort To UShort (null)", Input = new ushort?(), ExpectedResult = false },
        new NonGenericCoerceTest<ushort?, ushort>{ Name = "Nullable UShort To UShort (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = 42 },
        new NonGenericCoerceTest<ushort?, ushort?>{ Name = "Nullable UShort To Nullable UShort (null)", Input = new ushort?(), ExpectedResult = true, ExpectedOutput = new ushort?() },
        new NonGenericCoerceTest<ushort?, ushort?>{ Name = "Nullable UShort To Nullable UShort (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = new ushort?(42) },

        new NonGenericCoerceTest<ushort, bool?>{ Name = "UShort To Nullable Bool (42)", Input = 42, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<ushort?, bool>{ Name = "Nullable UShort To Bool (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<ushort?, bool?>{ Name = "Nullable UShort To Nullable Bool (42)", Input = new ushort?(42), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        // Interface/Class Types
        new NonGenericCoerceTest<ushort, IInterface>{ Name = "UShort To Interface (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, BaseClass>{ Name = "UShort To BaseClass (42)", Input = 42, ExpectedResult = false },
        new NonGenericCoerceTest<ushort, DerivedClass>{ Name = "UShort To DerivedClass (42)", Input = 42, ExpectedResult = false },
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