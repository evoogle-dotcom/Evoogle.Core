// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class CharTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<char, bool>{ Name = "Char To Bool ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<char, byte>{ Name = "Char To Byte ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (byte)'*' },
        new GenericCoerceTest<char, byte[]>{ Name = "Char To ByteArray ('*')", Input = '*', ExpectedResult = false},
        new GenericCoerceTest<char, char>{ Name = "Char To Char ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = '*' },
        new GenericCoerceTest<char, DateTime>{ Name = "Char To DateTime ('*')", Input = '*', ExpectedResult = false},
        new GenericCoerceTest<char, DateTimeOffset>{ Name = "Char To DateTimeOffset ('*')", Input = '*', ExpectedResult = false},
        new GenericCoerceTest<char, decimal>{ Name = "Char To Decimal ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (decimal)'*' },
        new GenericCoerceTest<char, double>{ Name = "Char To Double ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (double)'*' },
        new GenericCoerceTest<char, float>{ Name = "Char To Float ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (float)'*' },
        new GenericCoerceTest<char, Guid>{ Name = "Char To Guid ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, int>{ Name = "Char To Int ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (int)'*' },
        new GenericCoerceTest<char, long>{ Name = "Char To Long ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (long)'*' },
        new GenericCoerceTest<char, ColorSet1>{ Name = "Char To Enum ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, sbyte>{ Name = "Char To SByte ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (sbyte)'*' },
        new GenericCoerceTest<char, short>{ Name = "Char To Short ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (short)'*' },
        new GenericCoerceTest<char, string>{ Name = "Char To String ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = "*" },
        new GenericCoerceTest<char, TimeSpan>{ Name = "Char To TimeSpan ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, Type>{ Name = "Char To Type ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, uint>{ Name = "Char To UInt ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (uint)'*' },
        new GenericCoerceTest<char, Ulid>{ Name = "Char To Ulid ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, ulong>{ Name = "Char To ULong ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (ulong)'*' },
        new GenericCoerceTest<char, Uri>{ Name = "Char To Uri ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, ushort>{ Name = "Char To UShort ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (ushort)'*' },

        // Nullable Types
        new GenericCoerceTest<char, char?>{ Name = "Char To Nullable Char ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = new char?('*') },
        new GenericCoerceTest<char?, char>{ Name = "Nullable Char To Char (null)", Input = new char?(), ExpectedResult = false },
        new GenericCoerceTest<char?, char>{ Name = "Nullable Char To Char ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = '*' },
        new GenericCoerceTest<char?, char?>{ Name = "Nullable Char To Nullable Char (null)", Input = new char?(), ExpectedResult = true, ExpectedOutput = new char?() },
        new GenericCoerceTest<char?, char?>{ Name = "Nullable Char To Nullable Char ('*')", Input = new char?('*'), ExpectedResult = true, ExpectedOutput = new char?('*') },

        new GenericCoerceTest<char, byte?>{ Name = "Char To Nullable Byte ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = new byte?((byte)'*') },
        new GenericCoerceTest<char?, byte>{ Name = "Nullable Char To Byte ('*')", Input = new char?('*'), ExpectedResult = true, ExpectedOutput = (byte)'*' },
        new GenericCoerceTest<char?, byte?>{ Name = "Nullable Char To Nullable Byte ('*')", Input = new char?('*'), ExpectedResult = true, ExpectedOutput = new byte?((byte)'*') },

        // Interface/Class Types
        new GenericCoerceTest<char, IInterface>{ Name = "Char To Interface ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, BaseClass>{ Name = "Char To BaseClass ('*')", Input = '*', ExpectedResult = false },
        new GenericCoerceTest<char, DerivedClass>{ Name = "Char To DerivedClass ('*')", Input = '*', ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<char, bool>{ Name = "Char To Bool ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<char, byte>{ Name = "Char To Byte ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (byte)'*' },
        new NonGenericCoerceTest<char, byte[]>{ Name = "Char To ByteArray ('*')", Input = '*', ExpectedResult = false},
        new NonGenericCoerceTest<char, char>{ Name = "Char To Char ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = '*' },
        new NonGenericCoerceTest<char, DateTime>{ Name = "Char To DateTime ('*')", Input = '*', ExpectedResult = false},
        new NonGenericCoerceTest<char, DateTimeOffset>{ Name = "Char To DateTimeOffset ('*')", Input = '*', ExpectedResult = false},
        new NonGenericCoerceTest<char, decimal>{ Name = "Char To Decimal ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (decimal)'*' },
        new NonGenericCoerceTest<char, double>{ Name = "Char To Double ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (double)'*' },
        new NonGenericCoerceTest<char, float>{ Name = "Char To Float ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (float)'*' },
        new NonGenericCoerceTest<char, Guid>{ Name = "Char To Guid ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, int>{ Name = "Char To Int ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (int)'*' },
        new NonGenericCoerceTest<char, long>{ Name = "Char To Long ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (long)'*' },
        new NonGenericCoerceTest<char, ColorSet1>{ Name = "Char To Enum ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, sbyte>{ Name = "Char To SByte ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (sbyte)'*' },
        new NonGenericCoerceTest<char, short>{ Name = "Char To Short ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (short)'*' },
        new NonGenericCoerceTest<char, string>{ Name = "Char To String ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = "*" },
        new NonGenericCoerceTest<char, TimeSpan>{ Name = "Char To TimeSpan ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, Type>{ Name = "Char To Type ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, uint>{ Name = "Char To UInt ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (uint)'*' },
        new NonGenericCoerceTest<char, Ulid>{ Name = "Char To Ulid ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, ulong>{ Name = "Char To ULong ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (ulong)'*' },
        new NonGenericCoerceTest<char, Uri>{ Name = "Char To Uri ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, ushort>{ Name = "Char To UShort ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = (ushort)'*' },

        // Nullable Types
        new NonGenericCoerceTest<char, char?>{ Name = "Char To Nullable Char ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = new char?('*') },
        new NonGenericCoerceTest<char?, char>{ Name = "Nullable Char To Char (null)", Input = new char?(), ExpectedResult = false },
        new NonGenericCoerceTest<char?, char>{ Name = "Nullable Char To Char ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = '*' },
        new NonGenericCoerceTest<char?, char?>{ Name = "Nullable Char To Nullable Char (null)", Input = new char?(), ExpectedResult = true, ExpectedOutput = new char?() },
        new NonGenericCoerceTest<char?, char?>{ Name = "Nullable Char To Nullable Char ('*')", Input = new char?('*'), ExpectedResult = true, ExpectedOutput = new char?('*') },

        new NonGenericCoerceTest<char, byte?>{ Name = "Char To Nullable Byte ('*')", Input = '*', ExpectedResult = true, ExpectedOutput = new byte?((byte)'*') },
        new NonGenericCoerceTest<char?, byte>{ Name = "Nullable Char To Byte ('*')", Input = new char?('*'), ExpectedResult = true, ExpectedOutput = (byte)'*' },
        new NonGenericCoerceTest<char?, byte?>{ Name = "Nullable Char To Nullable Byte ('*')", Input = new char?('*'), ExpectedResult = true, ExpectedOutput = new byte?((byte)'*') },

        // Interface/Class Types
        new NonGenericCoerceTest<char, IInterface>{ Name = "Char To Interface ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, BaseClass>{ Name = "Char To BaseClass ('*')", Input = '*', ExpectedResult = false },
        new NonGenericCoerceTest<char, DerivedClass>{ Name = "Char To DerivedClass ('*')", Input = '*', ExpectedResult = false },
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
