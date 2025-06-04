// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class GuidTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<Guid, bool>{ Name = $"Guid To Bool ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, byte>{ Name = $"Guid To Byte ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, byte[]>{ Name = $"Guid To ByteArray ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestGuidByteArray },
        new GenericCoerceTest<Guid, char>{ Name = $"Guid To Char ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, DateTime>{ Name = $"Guid To DateTime ({TestGuidString})", Input = TestGuid, ExpectedResult = false},
        new GenericCoerceTest<Guid, DateTimeOffset>{ Name = $"Guid To DateTimeOffset ({TestGuidString})", Input = TestGuid, ExpectedResult = false},
        new GenericCoerceTest<Guid, decimal>{ Name = $"Guid To Decimal ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, double>{ Name = $"Guid To Double ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, float>{ Name = $"Guid To Float ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, Guid>{ Name = $"Guid To Guid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestGuid },
        new GenericCoerceTest<Guid, int>{ Name = $"Guid To Int ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, long>{ Name = $"Guid To Long ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, PrimaryColor>{ Name = $"Guid To Enum ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, sbyte>{ Name = $"Guid To SByte ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, short>{ Name = $"Guid To Short ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, string>{ Name = $"Guid To String ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestGuidString },
        new GenericCoerceTest<Guid, TimeSpan>{ Name = $"Guid To TimeSpan ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, Type>{ Name = $"Guid To Type ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, uint>{ Name = $"Guid To UInt ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, Ulid>{ Name = $"Guid To Ulid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestUlid },
        new GenericCoerceTest<Guid, ulong>{ Name = $"Guid To ULong ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, Uri>{ Name = $"Guid To Uri ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, ushort>{ Name = $"Guid To UShort ({TestGuidString})", Input = TestGuid, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<Guid, Guid?>{ Name = $"Guid To Nullable Guid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },
        new GenericCoerceTest<Guid?, Guid>{ Name = "Nullable Guid To Guid (null)", Input = new Guid?(), ExpectedResult = false },
        new GenericCoerceTest<Guid?, Guid>{ Name = $"Nullable Guid To Guid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = TestGuid },
        new GenericCoerceTest<Guid?, Guid?>{ Name = "Nullable Guid To Nullable Guid (null)", Input = new Guid?(), ExpectedResult = true, ExpectedOutput = new Guid?() },
        new GenericCoerceTest<Guid?, Guid?>{ Name = $"Nullable Guid To Nullable Guid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },

        new GenericCoerceTest<Guid, Ulid?>{ Name = $"Guid To Nullable Ulid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },
        new GenericCoerceTest<Guid?, Ulid>{ Name = $"Nullable Guid To Ulid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = TestUlid },
        new GenericCoerceTest<Guid?, Ulid?>{ Name = $"Nullable Guid To Nullable Ulid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },

        // Interface/Class Types
        new GenericCoerceTest<Guid, IInterface>{ Name = $"Guid To Interface ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, BaseClass>{ Name = $"Guid To BaseClass ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new GenericCoerceTest<Guid, DerivedClass>{ Name = $"Guid To DerivedClass ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<Guid, bool>{ Name = $"Guid To Bool ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, byte>{ Name = $"Guid To Byte ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, byte[]>{ Name = $"Guid To ByteArray ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestGuidByteArray },
        new NonGenericCoerceTest<Guid, char>{ Name = $"Guid To Char ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, DateTime>{ Name = $"Guid To DateTime ({TestGuidString})", Input = TestGuid, ExpectedResult = false},
        new NonGenericCoerceTest<Guid, DateTimeOffset>{ Name = $"Guid To DateTimeOffset ({TestGuidString})", Input = TestGuid, ExpectedResult = false},
        new NonGenericCoerceTest<Guid, decimal>{ Name = $"Guid To Decimal ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, double>{ Name = $"Guid To Double ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, float>{ Name = $"Guid To Float ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, Guid>{ Name = $"Guid To Guid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestGuid },
        new NonGenericCoerceTest<Guid, int>{ Name = $"Guid To Int ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, long>{ Name = $"Guid To Long ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, PrimaryColor>{ Name = $"Guid To Enum ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, sbyte>{ Name = $"Guid To SByte ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, short>{ Name = $"Guid To Short ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, string>{ Name = $"Guid To String ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestGuidString },
        new NonGenericCoerceTest<Guid, TimeSpan>{ Name = $"Guid To TimeSpan ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, Type>{ Name = $"Guid To Type ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, uint>{ Name = $"Guid To UInt ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, Ulid>{ Name = $"Guid To Ulid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = TestUlid },
        new NonGenericCoerceTest<Guid, ulong>{ Name = $"Guid To ULong ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, Uri>{ Name = $"Guid To Uri ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, ushort>{ Name = $"Guid To UShort ({TestGuidString})", Input = TestGuid, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<Guid, Guid?>{ Name = $"Guid To Nullable Guid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },
        new NonGenericCoerceTest<Guid?, Guid>{ Name = "Nullable Guid To Guid (null)", Input = new Guid?(), ExpectedResult = false },
        new NonGenericCoerceTest<Guid?, Guid>{ Name = $"Nullable Guid To Guid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = TestGuid },
        new NonGenericCoerceTest<Guid?, Guid?>{ Name = "Nullable Guid To Nullable Guid (null)", Input = new Guid?(), ExpectedResult = true, ExpectedOutput = new Guid?() },
        new NonGenericCoerceTest<Guid?, Guid?>{ Name = $"Nullable Guid To Nullable Guid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = new Guid?(TestGuid) },

        new NonGenericCoerceTest<Guid, Ulid?>{ Name = $"Guid To Nullable Ulid ({TestGuidString})", Input = TestGuid, ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },
        new NonGenericCoerceTest<Guid?, Ulid>{ Name = $"Nullable Guid To Ulid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = TestUlid },
        new NonGenericCoerceTest<Guid?, Ulid?>{ Name = $"Nullable Guid To Nullable Ulid ({TestGuidString})", Input = new Guid?(TestGuid), ExpectedResult = true, ExpectedOutput = new Ulid?(TestUlid) },

        // Interface/Class Types
        new NonGenericCoerceTest<Guid, IInterface>{ Name = $"Guid To Interface ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, BaseClass>{ Name = $"Guid To BaseClass ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
        new NonGenericCoerceTest<Guid, DerivedClass>{ Name = $"Guid To DerivedClass ({TestGuidString})", Input = TestGuid, ExpectedResult = false },
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
