// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class EnumTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<ColorSet2, bool>{ Name = $"Enum To Bool ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = true },
        new GenericCoerceTest<ColorSet2, byte>{ Name = $"Enum To Byte ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, byte[]>{ Name = $"Enum To ByteArray ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, char>{ Name = $"Enum To Char ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = (char)TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, DateTime>{ Name = $"Enum To DateTime ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, DateTimeOffset>{ Name = $"Enum To DateTimeOffset ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, decimal>{ Name = $"Enum To Decimal ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, double>{ Name = $"Enum To Double ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, float>{ Name = $"Enum To Float ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, Guid>{ Name = $"Enum To Guid ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, int>{ Name = $"Enum To Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, long>{ Name = $"Enum To Long ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Unspecified)})", Input = ColorSet2.Unspecified, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = ColorSet1.Green },
        new GenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Yellow)})", Input = ColorSet2.Yellow, ExpectedResult = true, ExpectedOutput = ColorSet1.Blue },
        new GenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Purple)})", Input = ColorSet2.Purple, ExpectedResult = true, ExpectedOutput = ColorSet1.Purple },
        new GenericCoerceTest<ColorSet2, sbyte>{ Name = $"Enum To SByte ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, short>{ Name = $"Enum To Short ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, string>{ Name = $"Enum To String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },
        new GenericCoerceTest<ColorSet2, TimeSpan>{ Name = $"Enum To TimeSpan ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, Type>{ Name = $"Enum To Type ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, uint>{ Name = $"Enum To UInt ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, Ulid>{ Name = $"Enum To Ulid ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, ulong>{ Name = $"Enum To ULong ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2, Uri>{ Name = $"Enum To Uri ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, ushort>{ Name = $"Enum To UShort ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },

        // Nullable Types
        new GenericCoerceTest<ColorSet2, int?>{ Name = $"Enum To Nullable Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2?, int>{ Name = $"Nullable Enum To Int (null)", Input = null, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2?, int>{ Name = $"Nullable Enum To Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new GenericCoerceTest<ColorSet2?, int?>{ Name = $"Nullable Enum To Nullable Int (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<ColorSet2?, int?>{ Name = $"Nullable Enum To Nullable Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },

        new GenericCoerceTest<ColorSet2, string?>{ Name = $"Enum To Nullable String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },
        new GenericCoerceTest<ColorSet2?, string>{ Name = $"Nullable Enum To String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<ColorSet2?, string>{ Name = $"Nullable Enum To String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },
        new GenericCoerceTest<ColorSet2?, string?>{ Name = $"Nullable Enum To Nullable String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<ColorSet2?, string?>{ Name = $"Nullable Enum To Nullable String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },

        // Interface/Class Types
        new GenericCoerceTest<ColorSet2, IInterface>{ Name = $"Enum To Interface ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, BaseClass>{ Name = $"Enum To BaseClass ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new GenericCoerceTest<ColorSet2, DerivedClass>{ Name = $"Enum To DerivedClass ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<ColorSet2, bool>{ Name = $"Enum To Bool ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = true },
        new NonGenericCoerceTest<ColorSet2, byte>{ Name = $"Enum To Byte ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, byte[]>{ Name = $"Enum To ByteArray ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, char>{ Name = $"Enum To Char ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = (char)TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, DateTime>{ Name = $"Enum To DateTime ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, DateTimeOffset>{ Name = $"Enum To DateTimeOffset ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, decimal>{ Name = $"Enum To Decimal ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, double>{ Name = $"Enum To Double ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, float>{ Name = $"Enum To Float ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, Guid>{ Name = $"Enum To Guid ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, int>{ Name = $"Enum To Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, long>{ Name = $"Enum To Long ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Unspecified)})", Input = ColorSet2.Unspecified, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = ColorSet1.Green },
        new NonGenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Yellow)})", Input = ColorSet2.Yellow, ExpectedResult = true, ExpectedOutput = ColorSet1.Blue },
        new NonGenericCoerceTest<ColorSet2, ColorSet1>{ Name = $"Enum To Enum ({nameof(ColorSet2)}.{nameof(ColorSet2.Purple)})", Input = ColorSet2.Purple, ExpectedResult = true, ExpectedOutput = ColorSet1.Purple },
        new NonGenericCoerceTest<ColorSet2, sbyte>{ Name = $"Enum To SByte ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, short>{ Name = $"Enum To Short ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, string>{ Name = $"Enum To String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },
        new NonGenericCoerceTest<ColorSet2, TimeSpan>{ Name = $"Enum To TimeSpan ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, Type>{ Name = $"Enum To Type ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, uint>{ Name = $"Enum To UInt ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, Ulid>{ Name = $"Enum To Ulid ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, ulong>{ Name = $"Enum To ULong ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2, Uri>{ Name = $"Enum To Uri ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, ushort>{ Name = $"Enum To UShort ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },

        // Nullable Types
        new NonGenericCoerceTest<ColorSet2, int?>{ Name = $"Enum To Nullable Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2?, int>{ Name = $"Nullable Enum To Int (null)", Input = null, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2?, int>{ Name = $"Nullable Enum To Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },
        new NonGenericCoerceTest<ColorSet2?, int?>{ Name = $"Nullable Enum To Nullable Int (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<ColorSet2?, int?>{ Name = $"Nullable Enum To Nullable Int ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = TestColorSet2GreenOrdinal },

        new NonGenericCoerceTest<ColorSet2, string?>{ Name = $"Enum To Nullable String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },
        new NonGenericCoerceTest<ColorSet2?, string>{ Name = $"Nullable Enum To String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<ColorSet2?, string>{ Name = $"Nullable Enum To String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },
        new NonGenericCoerceTest<ColorSet2?, string?>{ Name = $"Nullable Enum To Nullable String (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<ColorSet2?, string?>{ Name = $"Nullable Enum To Nullable String ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = true, ExpectedOutput = nameof(ColorSet2.Green) },

        // Interface/Class Types
        new NonGenericCoerceTest<ColorSet2, IInterface>{ Name = $"Enum To Interface ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, BaseClass>{ Name = $"Enum To BaseClass ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
        new NonGenericCoerceTest<ColorSet2, DerivedClass>{ Name = $"Enum To DerivedClass ({nameof(ColorSet2)}.{nameof(ColorSet2.Green)})", Input = ColorSet2.Green, ExpectedResult = false },
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
