// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class UriTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<Uri, bool>{ Name = $"Uri To Bool ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, byte>{ Name = $"Uri To Byte ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, byte[]>{ Name = $"Uri To ByteArray ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, char>{ Name = $"Uri To Char ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, DateTime>{ Name = $"Uri To DateTime ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, DateTimeOffset>{ Name = $"Uri To DateTimeOffset ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, decimal>{ Name = $"Uri To Decimal ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, double>{ Name = $"Uri To Double ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, float>{ Name = $"Uri To Float ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, Guid>{ Name = $"Uri To Guid ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, int>{ Name = $"Uri To Int ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, long>{ Name = $"Uri To Long ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, ColorSet1>{ Name = $"Uri To Enum ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, sbyte>{ Name = $"Uri To SByte ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, short>{ Name = $"Uri To Short ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, string>{ Name = $"Uri To String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },
        new GenericCoerceTest<Uri, TimeSpan>{ Name = $"Uri To TimeSpan ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, Type>{ Name = $"Uri To Type ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, uint>{ Name = $"Uri To UInt ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, Ulid>{ Name = $"Uri To Ulid ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, ulong>{ Name = $"Uri To ULong ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, Uri>{ Name = $"Uri To Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },
        new GenericCoerceTest<Uri, ushort>{ Name = $"Uri To UShort ({TestUriString})", Input = TestUri, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<Uri, Uri?>{ Name = $"Uri To Nullable Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },
        new GenericCoerceTest<Uri?, Uri>{ Name = "Nullable Uri To Uri (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<Uri?, Uri>{ Name = $"Nullable Uri To Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },
        new GenericCoerceTest<Uri?, Uri?>{ Name = "Nullable Uri To Nullable Uri (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<Uri?, Uri?>{ Name = $"Nullable Uri To Nullable Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },

        new GenericCoerceTest<Uri, string?>{ Name = $"Uri To Nullable String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },
        new GenericCoerceTest<Uri?, string>{ Name = $"Nullable Uri To String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },
        new GenericCoerceTest<Uri?, string?>{ Name = $"Nullable Uri To Nullable String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },

        // Interface/Class Types
        new GenericCoerceTest<Uri, IInterface>{ Name = $"Uri To Interface ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, BaseClass>{ Name = $"Uri To BaseClass ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new GenericCoerceTest<Uri, DerivedClass>{ Name = $"Uri To DerivedClass ({TestUriString})", Input = TestUri, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<Uri, bool>{ Name = $"Uri To Bool ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, byte>{ Name = $"Uri To Byte ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, byte[]>{ Name = $"Uri To ByteArray ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, char>{ Name = $"Uri To Char ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, DateTime>{ Name = $"Uri To DateTime ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, DateTimeOffset>{ Name = $"Uri To DateTimeOffset ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, decimal>{ Name = $"Uri To Decimal ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, double>{ Name = $"Uri To Double ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, float>{ Name = $"Uri To Float ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, Guid>{ Name = $"Uri To Guid ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, int>{ Name = $"Uri To Int ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, long>{ Name = $"Uri To Long ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, ColorSet1>{ Name = $"Uri To Enum ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, sbyte>{ Name = $"Uri To SByte ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, short>{ Name = $"Uri To Short ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, string>{ Name = $"Uri To String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },
        new NonGenericCoerceTest<Uri, TimeSpan>{ Name = $"Uri To TimeSpan ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, Type>{ Name = $"Uri To Type ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, uint>{ Name = $"Uri To UInt ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, Ulid>{ Name = $"Uri To Ulid ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, ulong>{ Name = $"Uri To ULong ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, Uri>{ Name = $"Uri To Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },
        new NonGenericCoerceTest<Uri, ushort>{ Name = $"Uri To UShort ({TestUriString})", Input = TestUri, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<Uri, Uri?>{ Name = $"Uri To Nullable Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },
        new NonGenericCoerceTest<Uri?, Uri>{ Name = "Nullable Uri To Uri (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<Uri?, Uri>{ Name = $"Nullable Uri To Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },
        new NonGenericCoerceTest<Uri?, Uri?>{ Name = "Nullable Uri To Nullable Uri (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<Uri?, Uri?>{ Name = $"Nullable Uri To Nullable Uri ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUri },

        new NonGenericCoerceTest<Uri, string?>{ Name = $"Uri To Nullable String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },
        new NonGenericCoerceTest<Uri?, string>{ Name = $"Nullable Uri To String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },
        new NonGenericCoerceTest<Uri?, string?>{ Name = $"Nullable Uri To Nullable String ({TestUriString})", Input = TestUri, ExpectedResult = true, ExpectedOutput = TestUriString },

        // Interface/Class Types
        new NonGenericCoerceTest<Uri, IInterface>{ Name = $"Uri To Interface ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, BaseClass>{ Name = $"Uri To BaseClass ({TestUriString})", Input = TestUri, ExpectedResult = false },
        new NonGenericCoerceTest<Uri, DerivedClass>{ Name = $"Uri To DerivedClass ({TestUriString})", Input = TestUri, ExpectedResult = false },
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
