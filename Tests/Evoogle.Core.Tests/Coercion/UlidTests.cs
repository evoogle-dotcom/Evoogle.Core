// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class UlidTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<Ulid, bool>{ Name = $"Ulid To Bool ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, byte>{ Name = $"Ulid To Byte ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, byte[]>{ Name = $"Ulid To ByteArray ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestUlidByteArray },
        new GenericCoerceTest<Ulid, char>{ Name = $"Ulid To Char ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, DateTime>{ Name = $"Ulid To DateTime ({TestUlidString})", Input = TestUlid, ExpectedResult = false},
        new GenericCoerceTest<Ulid, DateTimeOffset>{ Name = $"Ulid To DateTimeOffset ({TestUlidString})", Input = TestUlid, ExpectedResult = false},
        new GenericCoerceTest<Ulid, decimal>{ Name = $"Ulid To Decimal ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, double>{ Name = $"Ulid To Double ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, float>{ Name = $"Ulid To Float ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, Guid>{ Name = $"Ulid To Guid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestGuid },
        new GenericCoerceTest<Ulid, int>{ Name = $"Ulid To Int ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, long>{ Name = $"Ulid To Long ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, ColorSet1>{ Name = $"Ulid To Enum ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, sbyte>{ Name = $"Ulid To SByte ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, short>{ Name = $"Ulid To Short ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, string>{ Name = $"Ulid To String ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestUlidString },
        new GenericCoerceTest<Ulid, TimeSpan>{ Name = $"Ulid To TimeSpan ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, Type>{ Name = $"Ulid To Type ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, uint>{ Name = $"Ulid To UInt ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, Ulid>{ Name = $"Ulid To Ulid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestUlid },
        new GenericCoerceTest<Ulid, ulong>{ Name = $"Ulid To ULong ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, Uri>{ Name = $"Ulid To Uri ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, ushort>{ Name = $"Ulid To UShort ({TestUlidString})", Input = TestUlid, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<Ulid, Ulid?>{ Name = $"Ulid To Nullable Ulid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },
        new GenericCoerceTest<Ulid?, Ulid>{ Name = "Nullable Ulid To Ulid (null)", Input = new Ulid?(), ExpectedResult = false },
        new GenericCoerceTest<Ulid?, Ulid>{ Name = $"Nullable Ulid To Ulid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = TestUlid },
        new GenericCoerceTest<Ulid?, Ulid?>{ Name = "Nullable Ulid To Nullable Ulid (null)", Input = new Ulid?(), ExpectedResult = true, ExpectedOutput = new Ulid?() },
        new GenericCoerceTest<Ulid?, Ulid?>{ Name = $"Nullable Ulid To Nullable Ulid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },

        new GenericCoerceTest<Ulid, Guid?>{ Name = $"Ulid To Nullable Guid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },
        new GenericCoerceTest<Ulid?, Guid>{ Name = $"Nullable Ulid To Guid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = TestGuid },
        new GenericCoerceTest<Ulid?, Guid?>{ Name = $"Nullable Ulid To Nullable Guid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },

        // Interface/Class Types
        new GenericCoerceTest<Ulid, IInterface>{ Name = $"Ulid To Interface ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, BaseClass>{ Name = $"Ulid To BaseClass ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new GenericCoerceTest<Ulid, DerivedClass>{ Name = $"Ulid To DerivedClass ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<Ulid, bool>{ Name = $"Ulid To Bool ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, byte>{ Name = $"Ulid To Byte ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, byte[]>{ Name = $"Ulid To ByteArray ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestUlidByteArray },
        new NonGenericCoerceTest<Ulid, char>{ Name = $"Ulid To Char ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, DateTime>{ Name = $"Ulid To DateTime ({TestUlidString})", Input = TestUlid, ExpectedResult = false},
        new NonGenericCoerceTest<Ulid, DateTimeOffset>{ Name = $"Ulid To DateTimeOffset ({TestUlidString})", Input = TestUlid, ExpectedResult = false},
        new NonGenericCoerceTest<Ulid, decimal>{ Name = $"Ulid To Decimal ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, double>{ Name = $"Ulid To Double ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, float>{ Name = $"Ulid To Float ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, Guid>{ Name = $"Ulid To Guid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestGuid },
        new NonGenericCoerceTest<Ulid, int>{ Name = $"Ulid To Int ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, long>{ Name = $"Ulid To Long ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, ColorSet1>{ Name = $"Ulid To Enum ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, sbyte>{ Name = $"Ulid To SByte ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, short>{ Name = $"Ulid To Short ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, string>{ Name = $"Ulid To String ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestUlidString },
        new NonGenericCoerceTest<Ulid, TimeSpan>{ Name = $"Ulid To TimeSpan ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, Type>{ Name = $"Ulid To Type ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, uint>{ Name = $"Ulid To UInt ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, Ulid>{ Name = $"Ulid To Ulid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = TestUlid },
        new NonGenericCoerceTest<Ulid, ulong>{ Name = $"Ulid To ULong ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, Uri>{ Name = $"Ulid To Uri ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, ushort>{ Name = $"Ulid To UShort ({TestUlidString})", Input = TestUlid, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<Ulid, Ulid?>{ Name = $"Ulid To Nullable Ulid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },
        new NonGenericCoerceTest<Ulid?, Ulid>{ Name = "Nullable Ulid To Ulid (null)", Input = new Ulid?(), ExpectedResult = false },
        new NonGenericCoerceTest<Ulid?, Ulid>{ Name = $"Nullable Ulid To Ulid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = TestUlid },
        new NonGenericCoerceTest<Ulid?, Ulid?>{ Name = "Nullable Ulid To Nullable Ulid (null)", Input = new Ulid?(), ExpectedResult = true, ExpectedOutput = new Ulid?() },
        new NonGenericCoerceTest<Ulid?, Ulid?>{ Name = $"Nullable Ulid To Nullable Ulid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },

        new NonGenericCoerceTest<Ulid, Guid?>{ Name = $"Ulid To Nullable Guid ({TestUlidString})", Input = TestUlid, ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },
        new NonGenericCoerceTest<Ulid?, Guid>{ Name = $"Nullable Ulid To Guid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = TestGuid },
        new NonGenericCoerceTest<Ulid?, Guid?>{ Name = $"Nullable Ulid To Nullable Guid ({TestUlidString})", Input = new Ulid?(TestUlid), ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },

        // Interface/Class Types
        new NonGenericCoerceTest<Ulid, IInterface>{ Name = $"Ulid To Interface ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, BaseClass>{ Name = $"Ulid To BaseClass ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
        new NonGenericCoerceTest<Ulid, DerivedClass>{ Name = $"Ulid To DerivedClass ({TestUlidString})", Input = TestUlid, ExpectedResult = false },
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
