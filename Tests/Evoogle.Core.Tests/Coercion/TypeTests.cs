// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class TypeTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<Type, bool>{ Name = $"Type To Bool ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, byte>{ Name = $"Type To Byte ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, byte[]>{ Name = $"Type To ByteArray ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, char>{ Name = $"Type To Char ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, DateTime>{ Name = $"Type To DateTime ({TestTypeString})", Input = TestType, ExpectedResult = false},
        new GenericCoerceTest<Type, DateTimeOffset>{ Name = $"Type To DateTimeOffset ({TestTypeString})", Input = TestType, ExpectedResult = false},
        new GenericCoerceTest<Type, decimal>{ Name = $"Type To Decimal ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, double>{ Name = $"Type To Double ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, float>{ Name = $"Type To Float ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, Guid>{ Name = $"Type To Guid ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, int>{ Name = $"Type To Int ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, long>{ Name = $"Type To Long ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, ColorSet1>{ Name = $"Type To Enum ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, sbyte>{ Name = $"Type To SByte ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, short>{ Name = $"Type To Short ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, string>{ Name = $"Type To String ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestTypeString },
        new GenericCoerceTest<Type, TimeSpan>{ Name = $"Type To TimeSpan ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, Type>{ Name = $"Type To Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },
        new GenericCoerceTest<Type, uint>{ Name = $"Type To UInt ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, Ulid>{ Name = $"Type To Ulid ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, ulong>{ Name = $"Type To ULong ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, Uri>{ Name = $"Type To Uri ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, ushort>{ Name = $"Type To UShort ({TestTypeString})", Input = TestType, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<Type, Type?>{ Name = $"Type To Nullable Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },
        new GenericCoerceTest<Type?, Type>{ Name = "Nullable Type To Type (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<Type?, Type>{ Name = $"Nullable Type To Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },
        new GenericCoerceTest<Type?, Type?>{ Name = "Nullable Type To Nullable Type (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<Type?, Type?>{ Name = $"Nullable Type To Nullable Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },

        // Interface/Class Types
        new GenericCoerceTest<Type, IInterface>{ Name = $"Type To Interface ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, BaseClass>{ Name = $"Type To BaseClass ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new GenericCoerceTest<Type, DerivedClass>{ Name = $"Type To DerivedClass ({TestTypeString})", Input = TestType, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<Type, bool>{ Name = $"Type To Bool ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, byte>{ Name = $"Type To Byte ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, byte[]>{ Name = $"Type To ByteArray ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, char>{ Name = $"Type To Char ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, DateTime>{ Name = $"Type To DateTime ({TestTypeString})", Input = TestType, ExpectedResult = false},
        new NonGenericCoerceTest<Type, DateTimeOffset>{ Name = $"Type To DateTimeOffset ({TestTypeString})", Input = TestType, ExpectedResult = false},
        new NonGenericCoerceTest<Type, decimal>{ Name = $"Type To Decimal ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, double>{ Name = $"Type To Double ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, float>{ Name = $"Type To Float ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, Guid>{ Name = $"Type To Guid ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, int>{ Name = $"Type To Int ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, long>{ Name = $"Type To Long ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, ColorSet1>{ Name = $"Type To Enum ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, sbyte>{ Name = $"Type To SByte ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, short>{ Name = $"Type To Short ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, string>{ Name = $"Type To String ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestTypeString },
        new NonGenericCoerceTest<Type, TimeSpan>{ Name = $"Type To TimeSpan ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, Type>{ Name = $"Type To Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },
        new NonGenericCoerceTest<Type, uint>{ Name = $"Type To UInt ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, Ulid>{ Name = $"Type To Ulid ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, ulong>{ Name = $"Type To ULong ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, Uri>{ Name = $"Type To Uri ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, ushort>{ Name = $"Type To UShort ({TestTypeString})", Input = TestType, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<Type, Type?>{ Name = $"Type To Nullable Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },
        new NonGenericCoerceTest<Type?, Type>{ Name = "Nullable Type To Type (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<Type?, Type>{ Name = $"Nullable Type To Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },
        new NonGenericCoerceTest<Type?, Type?>{ Name = "Nullable Type To Nullable Type (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<Type?, Type?>{ Name = $"Nullable Type To Nullable Type ({TestTypeString})", Input = TestType, ExpectedResult = true, ExpectedOutput = TestType },

        // Interface/Class Types
        new NonGenericCoerceTest<Type, IInterface>{ Name = $"Type To Interface ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, BaseClass>{ Name = $"Type To BaseClass ({TestTypeString})", Input = TestType, ExpectedResult = false },
        new NonGenericCoerceTest<Type, DerivedClass>{ Name = $"Type To DerivedClass ({TestTypeString})", Input = TestType, ExpectedResult = false },
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
