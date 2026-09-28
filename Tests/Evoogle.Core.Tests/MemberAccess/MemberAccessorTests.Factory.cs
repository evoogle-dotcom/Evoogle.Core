// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;

using Evoogle.XUnit;

namespace Evoogle.MemberAccess;

public partial class MemberAccessorTests
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] FactoryTheoryData =>
    [
        // Happy path tests for MemberAccessor factory methods

        // Field
        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readable/writeable field",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long),
            MemberName = nameof(MemberAccessShape.FieldValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = false,
            IsField = true,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readable/writeable field with nullable value",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long?),
            MemberName = nameof(MemberAccessShape.FieldNullableValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = false,
            IsField = true,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readable/writeable field with nullable string",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.FieldNullableString),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = false,
            IsField = true,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readonly field",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long),
            MemberName = nameof(MemberAccessShape.ReadonlyField),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = false,
            IsField = true,
            IsStatic = false,
            CanRead = true,
            CanWrite = false,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public static readable/writeable field",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.StaticTextField),
            BindingFlags = BindingFlags.Public | BindingFlags.Static,
            IsProperty = false,
            IsField = true,
            IsStatic = true,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with non-public instance readable/writeable field",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = "_privateField",
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            IsProperty = false,
            IsField = true,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with non-public static readonly field",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = "_privateStaticReadonlyField",
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Static,
            IsProperty = false,
            IsField = true,
            IsStatic = true,
            CanRead = true,
            CanWrite = false,
        },

        // Property
        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readable/writeable property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.TextValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            IsField = false,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readable/writeable property nullable value",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long?),
            MemberName = nameof(MemberAccessShape.CacheNullableValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            IsField = false,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readable/writeable property with nullable string",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.TextNullableValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            IsField = false,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance readonly property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long),
            MemberName = nameof(MemberAccessShape.ReadonlyProperty),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            IsField = false,
            IsStatic = false,
            CanRead = true,
            CanWrite = false,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public instance writeonly property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.WriteOnlyValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            IsField = false,
            IsStatic = false,
            CanRead = false,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public static readable/writeable property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.StaticTextValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Static,
            IsProperty = true,
            IsField = false,
            IsStatic = true,
            CanRead = true,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with public static write-only property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(MemberAccessShape.SecretToken),
            BindingFlags = BindingFlags.Public | BindingFlags.Static,
            IsProperty = true,
            IsField = false,
            IsStatic = true,
            CanRead = false,
            CanWrite = true,
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods succeed with non-public instance readable/writeable property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = "PrivateValue",
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Instance,
            IsProperty = true,
            IsField = false,
            IsStatic = false,
            CanRead = true,
            CanWrite = true,
        },

        // Exception path tests for MemberAccessor factory methods
        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods throw with null member info",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = string.Empty,
            ExceptionType = typeof(ArgumentNullException),
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods throw with unsupported member info",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(string),
            MemberName = nameof(ToString),
            ExceptionType = typeof(MemberAccessException),
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods throw with indexer property info",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(int),
            MemberName = "Item",
            ExceptionType = typeof(MemberAccessException),
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods throw with blank property name",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long),
            MemberName = string.Empty,
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            ExceptionType = typeof(ArgumentException),
        },

        new FactoryTest
        {
            Name = $"{nameof(MemberAccessor)} factory methods throw with ambiguous property",
            DeclaringType = typeof(MemberAccessShape),
            MemberType = typeof(long),
            MemberName = nameof(MemberAccessShape.AmbiguousValue),
            BindingFlags = BindingFlags.Public | BindingFlags.Instance,
            IsProperty = true,
            ExceptionType = typeof(MemberAccessException),
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(FactoryTheoryData))]
    public void Factory(IXUnitTest test) => test.Execute(this);
    #endregion
}
