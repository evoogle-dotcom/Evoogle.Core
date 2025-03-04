// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class BoolTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<bool, bool>{ Name = "Bool To Bool (true)", Input = true, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<bool, byte>{ Name = "Bool To Byte (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, byte>{ Name = "Bool To Byte (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, byte[]>{ Name = "Bool To ByteArray (true)", Input = true, ExpectedResult = false, ExpectedOutput = null },
        new GenericCoerceTest<bool, char>{ Name = "Bool To Char (false)", Input = false, ExpectedResult = true, ExpectedOutput = (char)0 },
        new GenericCoerceTest<bool, char>{ Name = "Bool To Char (true)", Input = true, ExpectedResult = true, ExpectedOutput = (char)1 },
        new GenericCoerceTest<bool, DateTime>{ Name = "Bool To DateTime (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, DateTimeOffset>{ Name = "Bool To DateTimeOffset (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, decimal>{ Name = "Bool To Decimal (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, decimal>{ Name = "Bool To Decimal (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, double>{ Name = "Bool To Double (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, double>{ Name = "Bool To Double (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, float>{ Name = "Bool To Float (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, float>{ Name = "Bool To Float (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, Guid>{ Name = "Bool To Guid (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, int>{ Name = "Bool To Int (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, int>{ Name = "Bool To Int (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, long>{ Name = "Bool To Long (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, long>{ Name = "Bool To Long (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, PrimaryColor>{ Name = "Bool To Enum (true)", Input = true, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new GenericCoerceTest<bool, sbyte>{ Name = "Bool To SByte (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, sbyte>{ Name = "Bool To SByte (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, short>{ Name = "Bool To Short (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, short>{ Name = "Bool To Short (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, string>{ Name = "Bool To String (false)", Input = false, ExpectedResult = true, ExpectedOutput = "false" },
        new GenericCoerceTest<bool, string>{ Name = "Bool To String (true)", Input = true, ExpectedResult = true, ExpectedOutput = "true" },
        new GenericCoerceTest<bool, TimeSpan>{ Name = "Bool To TimeSpan (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, Type>{ Name = "Bool To Type (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, uint>{ Name = "Bool To UInt (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, uint>{ Name = "Bool To UInt (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, Ulid>{ Name = "Bool To Ulid (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, ulong>{ Name = "Bool To ULong (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, ulong>{ Name = "Bool To ULong (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool, Uri>{ Name = "Bool To Uri (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, ushort>{ Name = "Bool To UShort (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new GenericCoerceTest<bool, ushort>{ Name = "Bool To UShort (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },

        // Nullable Types
        new GenericCoerceTest<bool, bool?>{ Name = "Bool To Nullable Bool (false)", Input = false, ExpectedResult = true, ExpectedOutput = new bool?(false) },
        new GenericCoerceTest<bool, bool?>{ Name = "Bool To Nullable Bool (true)", Input = true, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new GenericCoerceTest<bool?, bool>{ Name = "Nullable Bool To Bool (null)", Input = new bool?(), ExpectedResult = false },
        new GenericCoerceTest<bool?, bool>{ Name = "Nullable Bool To Bool (false)", Input = new bool?(false), ExpectedResult = true, ExpectedOutput = false },
        new GenericCoerceTest<bool?, bool>{ Name = "Nullable Bool To Bool (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<bool?, bool?>{ Name = "Nullable Bool To Nullable Bool (null)", Input = new bool?(), ExpectedResult = true, ExpectedOutput = new bool?() },
        new GenericCoerceTest<bool?, bool?>{ Name = "Nullable Bool To Nullable Bool (false)", Input = new bool?(false), ExpectedResult = true, ExpectedOutput = new bool?(false) },
        new GenericCoerceTest<bool?, bool?>{ Name = "Nullable Bool To Nullable Bool (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        new GenericCoerceTest<bool, byte?>{ Name = "Bool To Nullable Byte (true)", Input = true, ExpectedResult = true, ExpectedOutput = new byte?(1) },
        new GenericCoerceTest<bool?, byte>{ Name = "Nullable Bool To Byte (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = 1 },
        new GenericCoerceTest<bool?, byte?>{ Name = "Nullable Bool To Nullable Byte (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = new byte?(1) },

        // Interface/Class Types
        new GenericCoerceTest<bool, IInterface>{ Name = "Bool To Interface (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, BaseClass>{ Name = "Bool To BaseClass (true)", Input = true, ExpectedResult = false },
        new GenericCoerceTest<bool, DerivedClass>{ Name = "Bool To DerivedClass (true)", Input = true, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<bool, bool>{ Name = "Bool To Bool (true)", Input = true, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<bool, byte>{ Name = "Bool To Byte (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, byte>{ Name = "Bool To Byte (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, byte[]>{ Name = "Bool To ByteArray (true)", Input = true, ExpectedResult = false, ExpectedOutput = null },
        new NonGenericCoerceTest<bool, char>{ Name = "Bool To Char (false)", Input = false, ExpectedResult = true, ExpectedOutput = (char)0 },
        new NonGenericCoerceTest<bool, char>{ Name = "Bool To Char (true)", Input = true, ExpectedResult = true, ExpectedOutput = (char)1 },
        new NonGenericCoerceTest<bool, DateTime>{ Name = "Bool To DateTime (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, DateTimeOffset>{ Name = "Bool To DateTimeOffset (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, decimal>{ Name = "Bool To Decimal (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, decimal>{ Name = "Bool To Decimal (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, double>{ Name = "Bool To Double (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, double>{ Name = "Bool To Double (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, float>{ Name = "Bool To Float (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, float>{ Name = "Bool To Float (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, Guid>{ Name = "Bool To Guid (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, int>{ Name = "Bool To Int (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, int>{ Name = "Bool To Int (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, long>{ Name = "Bool To Long (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, long>{ Name = "Bool To Long (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, PrimaryColor>{ Name = "Bool To Enum (true)", Input = true, ExpectedResult = true, ExpectedOutput = PrimaryColor.Green },
        new NonGenericCoerceTest<bool, sbyte>{ Name = "Bool To SByte (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, sbyte>{ Name = "Bool To SByte (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, short>{ Name = "Bool To Short (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, short>{ Name = "Bool To Short (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, string>{ Name = "Bool To String (false)", Input = false, ExpectedResult = true, ExpectedOutput = "false" },
        new NonGenericCoerceTest<bool, string>{ Name = "Bool To String (true)", Input = true, ExpectedResult = true, ExpectedOutput = "true" },
        new NonGenericCoerceTest<bool, TimeSpan>{ Name = "Bool To TimeSpan (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, Type>{ Name = "Bool To Type (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, uint>{ Name = "Bool To UInt (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, uint>{ Name = "Bool To UInt (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, Ulid>{ Name = "Bool To Ulid (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, ulong>{ Name = "Bool To ULong (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, ulong>{ Name = "Bool To ULong (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool, Uri>{ Name = "Bool To Uri (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, ushort>{ Name = "Bool To UShort (false)", Input = false, ExpectedResult = true, ExpectedOutput = 0 },
        new NonGenericCoerceTest<bool, ushort>{ Name = "Bool To UShort (true)", Input = true, ExpectedResult = true, ExpectedOutput = 1 },

        // Nullable Types
        new NonGenericCoerceTest<bool, bool?>{ Name = "Bool To Nullable Bool (false)", Input = false, ExpectedResult = true, ExpectedOutput = new bool?(false) },
        new NonGenericCoerceTest<bool, bool?>{ Name = "Bool To Nullable Bool (true)", Input = true, ExpectedResult = true, ExpectedOutput = new bool?(true) },
        new NonGenericCoerceTest<bool?, bool>{ Name = "Nullable Bool To Bool (null)", Input = new bool?(), ExpectedResult = false },
        new NonGenericCoerceTest<bool?, bool>{ Name = "Nullable Bool To Bool (false)", Input = new bool?(false), ExpectedResult = true, ExpectedOutput = false },
        new NonGenericCoerceTest<bool?, bool>{ Name = "Nullable Bool To Bool (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<bool?, bool?>{ Name = "Nullable Bool To Nullable Bool (null)", Input = new bool?(), ExpectedResult = true, ExpectedOutput = new bool?() },
        new NonGenericCoerceTest<bool?, bool?>{ Name = "Nullable Bool To Nullable Bool (false)", Input = new bool?(false), ExpectedResult = true, ExpectedOutput = new bool?(false) },
        new NonGenericCoerceTest<bool?, bool?>{ Name = "Nullable Bool To Nullable Bool (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = new bool?(true) },

        new NonGenericCoerceTest<bool, byte?>{ Name = "Bool To Nullable Byte (true)", Input = true, ExpectedResult = true, ExpectedOutput = new byte?(1) },
        new NonGenericCoerceTest<bool?, byte>{ Name = "Nullable Bool To Byte (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = 1 },
        new NonGenericCoerceTest<bool?, byte?>{ Name = "Nullable Bool To Nullable Byte (true)", Input = new bool?(true), ExpectedResult = true, ExpectedOutput = new byte?(1) },

        // Interface/Class Types
        new NonGenericCoerceTest<bool, IInterface>{ Name = "Bool To Interface (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, BaseClass>{ Name = "Bool To BaseClass (true)", Input = true, ExpectedResult = false },
        new NonGenericCoerceTest<bool, DerivedClass>{ Name = "Bool To DerivedClass (true)", Input = true, ExpectedResult = false },
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