// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class BaseClassTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<BaseClass, bool>{ Name = $"BaseClass To Bool ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, byte>{ Name = $"BaseClass To Byte ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, byte[]>{ Name = $"BaseClass To ByteArray ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new GenericCoerceTest<BaseClass, char>{ Name = $"BaseClass To Char ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, DateTime>{ Name = $"BaseClass To DateTime ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new GenericCoerceTest<BaseClass, DateTimeOffset>{ Name = $"BaseClass To DateTimeOffset ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new GenericCoerceTest<BaseClass, decimal>{ Name = $"BaseClass To Decimal ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, double>{ Name = $"BaseClass To Double ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, float>{ Name = $"BaseClass To Float ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, Guid>{ Name = $"BaseClass To Guid ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, int>{ Name = $"BaseClass To Int ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, long>{ Name = $"BaseClass To Long ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, PrimaryColor>{ Name = $"BaseClass To Enum ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, sbyte>{ Name = $"BaseClass To SByte ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, short>{ Name = $"BaseClass To Short ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, string>{ Name = $"BaseClass To String ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new GenericCoerceTest<BaseClass, TimeSpan>{ Name = $"BaseClass To TimeSpan ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, Type>{ Name = $"BaseClass To Type ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, uint>{ Name = $"BaseClass To UInt ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, Ulid>{ Name = $"BaseClass To Ulid ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, ulong>{ Name = $"BaseClass To ULong ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, Uri>{ Name = $"BaseClass To Uri ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new GenericCoerceTest<BaseClass, ushort>{ Name = $"BaseClass To UShort ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<BaseClass, BaseClass?>{ Name = $"BaseClass To Nullable BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new GenericCoerceTest<BaseClass?, BaseClass>{ Name = "Nullable BaseClass To BaseClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<BaseClass?, BaseClass>{ Name = $"Nullable BaseClass To BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new GenericCoerceTest<BaseClass?, BaseClass?>{ Name = "Nullable BaseClass To Nullable BaseClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<BaseClass?, BaseClass?>{ Name = $"Nullable BaseClass To Nullable BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },

        // Interface/Class Types
        new GenericCoerceTest<BaseClass, IInterface>{ Name = $"BaseClass To Interface ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new GenericCoerceTest<BaseClass, BaseClass>{ Name = $"BaseClass To BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new GenericCoerceTest<BaseClass, DerivedClass>{ Name = $"BaseClass To DerivedClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<BaseClass, bool>{ Name = $"BaseClass To Bool ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, byte>{ Name = $"BaseClass To Byte ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, byte[]>{ Name = $"BaseClass To ByteArray ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new NonGenericCoerceTest<BaseClass, char>{ Name = $"BaseClass To Char ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, DateTime>{ Name = $"BaseClass To DateTime ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new NonGenericCoerceTest<BaseClass, DateTimeOffset>{ Name = $"BaseClass To DateTimeOffset ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new NonGenericCoerceTest<BaseClass, decimal>{ Name = $"BaseClass To Decimal ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, double>{ Name = $"BaseClass To Double ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, float>{ Name = $"BaseClass To Float ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, Guid>{ Name = $"BaseClass To Guid ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, int>{ Name = $"BaseClass To Int ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, long>{ Name = $"BaseClass To Long ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, PrimaryColor>{ Name = $"BaseClass To Enum ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, sbyte>{ Name = $"BaseClass To SByte ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, short>{ Name = $"BaseClass To Short ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, string>{ Name = $"BaseClass To String ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false},
        new NonGenericCoerceTest<BaseClass, TimeSpan>{ Name = $"BaseClass To TimeSpan ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, Type>{ Name = $"BaseClass To Type ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, uint>{ Name = $"BaseClass To UInt ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, Ulid>{ Name = $"BaseClass To Ulid ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, ulong>{ Name = $"BaseClass To ULong ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, Uri>{ Name = $"BaseClass To Uri ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
        new NonGenericCoerceTest<BaseClass, ushort>{ Name = $"BaseClass To UShort ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<BaseClass, BaseClass?>{ Name = $"BaseClass To Nullable BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new NonGenericCoerceTest<BaseClass?, BaseClass>{ Name = "Nullable BaseClass To BaseClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<BaseClass?, BaseClass>{ Name = $"Nullable BaseClass To BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new NonGenericCoerceTest<BaseClass?, BaseClass?>{ Name = "Nullable BaseClass To Nullable BaseClass (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<BaseClass?, BaseClass?>{ Name = $"Nullable BaseClass To Nullable BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },

        // Interface/Class Types
        new NonGenericCoerceTest<BaseClass, IInterface>{ Name = $"BaseClass To Interface ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new NonGenericCoerceTest<BaseClass, BaseClass>{ Name = $"BaseClass To BaseClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = true, ExpectedOutput = TestBaseClass },
        new NonGenericCoerceTest<BaseClass, DerivedClass>{ Name = $"BaseClass To DerivedClass ({nameof(BaseClass)})", Input = TestBaseClass, ExpectedResult = false },
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
