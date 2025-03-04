// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

namespace Evoogle.Coercion;

public class ByteArrayTests(ITestOutputHelper output) : CoerceTests(output)
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] GenericCoerceTheoryData =>
    [
        // Simple Types
        new GenericCoerceTest<byte[], bool>{ Name = $"ByteArray To Bool ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], byte>{ Name = $"ByteArray To Byte ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], byte[]>{ Name = $"ByteArray To ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new GenericCoerceTest<byte[], char>{ Name = $"ByteArray To Char ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], DateTime>{ Name = $"ByteArray To DateTime ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false},
        new GenericCoerceTest<byte[], DateTimeOffset>{ Name = $"ByteArray To DateTimeOffset ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false},
        new GenericCoerceTest<byte[], decimal>{ Name = $"ByteArray To Decimal ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], double>{ Name = $"ByteArray To Double ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], float>{ Name = $"ByteArray To Float ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], Guid>{ Name = $"ByteArray To Guid ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], int>{ Name = $"ByteArray To Int ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], long>{ Name = $"ByteArray To Long ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], PrimaryColor>{ Name = $"ByteArray To Enum ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], sbyte>{ Name = $"ByteArray To SByte ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], short>{ Name = $"ByteArray To Short ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], string>{ Name = $"ByteArray To String ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArrayString },
        new GenericCoerceTest<byte[], TimeSpan>{ Name = $"ByteArray To TimeSpan ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], Type>{ Name = $"ByteArray To Type ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], uint>{ Name = $"ByteArray To UInt ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], Ulid>{ Name = $"ByteArray To Ulid ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], ulong>{ Name = $"ByteArray To ULong ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], Uri>{ Name = $"ByteArray To Uri ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], ushort>{ Name = $"ByteArray To UShort ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },

        // Nullable Types
        new GenericCoerceTest<byte[], byte[]?>{ Name = $"ByteArray To Nullable ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new GenericCoerceTest<byte[]?, byte[]>{ Name = "Nullable ByteArray To ByteArray (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<byte[]?, byte[]>{ Name = $"Nullable ByteArray To ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new GenericCoerceTest<byte[]?, byte[]?>{ Name = "Nullable ByteArray To Nullable ByteArray (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new GenericCoerceTest<byte[]?, byte[]?>{ Name = $"Nullable ByteArray To Nullable ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },

        // Interface/Class Types
        new GenericCoerceTest<byte[], IInterface>{ Name = $"ByteArray To Interface ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], BaseClass>{ Name = $"ByteArray To BaseClass ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new GenericCoerceTest<byte[], DerivedClass>{ Name = $"ByteArray To DerivedClass ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] NonGenericCoerceTheoryData =>
    [
        // Simple Types
        new NonGenericCoerceTest<byte[], bool>{ Name = $"ByteArray To Bool ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], byte>{ Name = $"ByteArray To Byte ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], byte[]>{ Name = $"ByteArray To ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new NonGenericCoerceTest<byte[], char>{ Name = $"ByteArray To Char ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], DateTime>{ Name = $"ByteArray To DateTime ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false},
        new NonGenericCoerceTest<byte[], DateTimeOffset>{ Name = $"ByteArray To DateTimeOffset ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false},
        new NonGenericCoerceTest<byte[], decimal>{ Name = $"ByteArray To Decimal ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], double>{ Name = $"ByteArray To Double ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], float>{ Name = $"ByteArray To Float ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], Guid>{ Name = $"ByteArray To Guid ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], int>{ Name = $"ByteArray To Int ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], long>{ Name = $"ByteArray To Long ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], PrimaryColor>{ Name = $"ByteArray To Enum ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], sbyte>{ Name = $"ByteArray To SByte ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], short>{ Name = $"ByteArray To Short ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], string>{ Name = $"ByteArray To String ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArrayString },
        new NonGenericCoerceTest<byte[], TimeSpan>{ Name = $"ByteArray To TimeSpan ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], Type>{ Name = $"ByteArray To Type ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], uint>{ Name = $"ByteArray To UInt ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], Ulid>{ Name = $"ByteArray To Ulid ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], ulong>{ Name = $"ByteArray To ULong ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], Uri>{ Name = $"ByteArray To Uri ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], ushort>{ Name = $"ByteArray To UShort ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },

        // Nullable Types
        new NonGenericCoerceTest<byte[], byte[]?>{ Name = $"ByteArray To Nullable ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new NonGenericCoerceTest<byte[]?, byte[]>{ Name = "Nullable ByteArray To ByteArray (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<byte[]?, byte[]>{ Name = $"Nullable ByteArray To ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },
        new NonGenericCoerceTest<byte[]?, byte[]?>{ Name = "Nullable ByteArray To Nullable ByteArray (null)", Input = null, ExpectedResult = true, ExpectedOutput = null },
        new NonGenericCoerceTest<byte[]?, byte[]?>{ Name = $"Nullable ByteArray To Nullable ByteArray ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = true, ExpectedOutput = TestByteArray },

        // Interface/Class Types
        new NonGenericCoerceTest<byte[], IInterface>{ Name = $"ByteArray To Interface ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], BaseClass>{ Name = $"ByteArray To BaseClass ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
        new NonGenericCoerceTest<byte[], DerivedClass>{ Name = $"ByteArray To DerivedClass ({TestByteArrayString})", Input = TestByteArray, ExpectedResult = false },
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