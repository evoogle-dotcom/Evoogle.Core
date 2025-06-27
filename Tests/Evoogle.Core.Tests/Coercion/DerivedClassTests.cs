// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class DerivedClassTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<DerivedClass, bool>{ Name = $"DerivedClass To Bool ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, byte>{ Name = $"DerivedClass To Byte ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, byte[]>{ Name = $"DerivedClass To ByteArray ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new GenericCoerceTest<DerivedClass, char>{ Name = $"DerivedClass To Char ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, DateTime>{ Name = $"DerivedClass To DateTime ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new GenericCoerceTest<DerivedClass, DateTimeOffset>{ Name = $"DerivedClass To DateTimeOffset ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new GenericCoerceTest<DerivedClass, decimal>{ Name = $"DerivedClass To Decimal ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, double>{ Name = $"DerivedClass To Double ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, float>{ Name = $"DerivedClass To Float ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, Guid>{ Name = $"DerivedClass To Guid ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, int>{ Name = $"DerivedClass To Int ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, long>{ Name = $"DerivedClass To Long ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, ColorSet1>{ Name = $"DerivedClass To Enum ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, sbyte>{ Name = $"DerivedClass To SByte ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, short>{ Name = $"DerivedClass To Short ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, string>{ Name = $"DerivedClass To String ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new GenericCoerceTest<DerivedClass, TimeSpan>{ Name = $"DerivedClass To TimeSpan ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, Type>{ Name = $"DerivedClass To Type ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, uint>{ Name = $"DerivedClass To UInt ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, Ulid>{ Name = $"DerivedClass To Ulid ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, ulong>{ Name = $"DerivedClass To ULong ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, Uri>{ Name = $"DerivedClass To Uri ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new GenericCoerceTest<DerivedClass, ushort>{ Name = $"DerivedClass To UShort ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<DerivedClass, DerivedClass?>{ Name = $"DerivedClass To Nullable DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new GenericCoerceTest<DerivedClass?, DerivedClass>{ Name = "Nullable DerivedClass To DerivedClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<DerivedClass?, DerivedClass>{ Name = $"Nullable DerivedClass To DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new GenericCoerceTest<DerivedClass?, DerivedClass?>{ Name = "Nullable DerivedClass To Nullable DerivedClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<DerivedClass?, DerivedClass?>{ Name = $"Nullable DerivedClass To Nullable DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },

        // Interface/Class Types
        new GenericCoerceTest<DerivedClass, IInterface>{ Name = $"DerivedClass To Interface ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new GenericCoerceTest<DerivedClass, BaseClass>{ Name = $"DerivedClass To BaseClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new GenericCoerceTest<DerivedClass, DerivedClass>{ Name = $"DerivedClass To DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<DerivedClass, bool>{ Name = $"DerivedClass To Bool ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, byte>{ Name = $"DerivedClass To Byte ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, byte[]>{ Name = $"DerivedClass To ByteArray ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new NonGenericCoerceTest<DerivedClass, char>{ Name = $"DerivedClass To Char ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, DateTime>{ Name = $"DerivedClass To DateTime ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new NonGenericCoerceTest<DerivedClass, DateTimeOffset>{ Name = $"DerivedClass To DateTimeOffset ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new NonGenericCoerceTest<DerivedClass, decimal>{ Name = $"DerivedClass To Decimal ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, double>{ Name = $"DerivedClass To Double ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, float>{ Name = $"DerivedClass To Float ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, Guid>{ Name = $"DerivedClass To Guid ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, int>{ Name = $"DerivedClass To Int ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, long>{ Name = $"DerivedClass To Long ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, ColorSet1>{ Name = $"DerivedClass To Enum ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, sbyte>{ Name = $"DerivedClass To SByte ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, short>{ Name = $"DerivedClass To Short ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, string>{ Name = $"DerivedClass To String ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false},
        new NonGenericCoerceTest<DerivedClass, TimeSpan>{ Name = $"DerivedClass To TimeSpan ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, Type>{ Name = $"DerivedClass To Type ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, uint>{ Name = $"DerivedClass To UInt ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, Ulid>{ Name = $"DerivedClass To Ulid ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, ulong>{ Name = $"DerivedClass To ULong ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, Uri>{ Name = $"DerivedClass To Uri ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },
        new NonGenericCoerceTest<DerivedClass, ushort>{ Name = $"DerivedClass To UShort ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<DerivedClass, DerivedClass?>{ Name = $"DerivedClass To Nullable DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new NonGenericCoerceTest<DerivedClass?, DerivedClass>{ Name = "Nullable DerivedClass To DerivedClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<DerivedClass?, DerivedClass>{ Name = $"Nullable DerivedClass To DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new NonGenericCoerceTest<DerivedClass?, DerivedClass?>{ Name = "Nullable DerivedClass To Nullable DerivedClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<DerivedClass?, DerivedClass?>{ Name = $"Nullable DerivedClass To Nullable DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },

        // Interface/Class Types
        new NonGenericCoerceTest<DerivedClass, IInterface>{ Name = $"DerivedClass To Interface ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new NonGenericCoerceTest<DerivedClass, BaseClass>{ Name = $"DerivedClass To BaseClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
        new NonGenericCoerceTest<DerivedClass, DerivedClass>{ Name = $"DerivedClass To DerivedClass ({nameof(DerivedClass)})", Input = TestDerivedClass, ExpectedResult = true, ExpectedOutput = TestDerivedClass },
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
