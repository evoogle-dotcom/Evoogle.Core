// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extensions;
using Evoogle.XUnit;

namespace Evoogle.Json;

public partial class Utf8JsonWriterExensionTests
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] WriteRequiredPropertyEnumWithConverterTheoryData =>
    [.. (
        from value in Enum.GetValues<Utf8JsonWriterTestEnum>()
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = JsonSerializer.Serialize(value.ToString())
        let expectedJson = $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Utf8JsonWriterTestEnum)}, Value={value}, "
                    + $"Condition={condition}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.WriteRequiredPropertyEnumWithConverter),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value),
                        Expression.Constant(condition)
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteCultureInfoPropertyWithConverterTheoryData =>
    [.. (
        from value in new CultureInfo?[]
        {
            null,
            CultureInfo.InvariantCulture,
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("fr-FR")
        }
        from defaultValue in new CultureInfo?[]
        {
            null,
            CultureInfo.InvariantCulture,
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("fr-FR")
        }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let isOmitted = condition switch
        {
            JsonIgnoreCondition.WhenWritingNull => value is null,
            JsonIgnoreCondition.WhenWritingDefault => Equals(value, defaultValue),
            _ => false
        }
        let jsonValue = JsonSerializer.Serialize(value?.Name)
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(CultureInfo)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteCultureInfoPropertyWithConverter),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(CultureInfo)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(CultureInfo))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteEnumPropertyWithConverterTheoryData =>
    [.. (
        from value in Enum.GetValues<Utf8JsonWriterTestEnum>()
        from defaultValue in Enum.GetValues<Utf8JsonWriterTestEnum>()
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = JsonSerializer.Serialize(value.ToString())
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Utf8JsonWriterTestEnum)}, Value={value}, "
                    + $"Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteEnumPropertyWithConverter),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue)
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableEnumPropertyWithConverterTheoryData =>
    [.. (
        from value in Enum.GetValues<Utf8JsonWriterTestEnum>()
            .Select(enumValue => (Utf8JsonWriterTestEnum?)enumValue).Prepend(null)
        from defaultValue in Enum.GetValues<Utf8JsonWriterTestEnum>()
            .Select(enumValue => (Utf8JsonWriterTestEnum?)enumValue).Prepend(null)
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let isOmitted = condition switch
        {
            JsonIgnoreCondition.WhenWritingNull => value is null,
            JsonIgnoreCondition.WhenWritingDefault => value == defaultValue,
            _ => false
        }
        let jsonValue = JsonSerializer.Serialize(value?.ToString())
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Utf8JsonWriterTestEnum?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableEnumPropertyWithConverter),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(Utf8JsonWriterTestEnum?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(Utf8JsonWriterTestEnum?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteTypePropertyWithConverterTheoryData =>
    [.. (
        from value in new Type?[]
        {
            null,
            typeof(string),
            typeof(int),
            typeof(Guid)
        }
        from defaultValue in new Type?[]
        {
            null,
            typeof(string),
            typeof(int),
            typeof(Guid)
        }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let isOmitted = condition switch
        {
            JsonIgnoreCondition.WhenWritingNull => value is null,
            JsonIgnoreCondition.WhenWritingDefault => value == defaultValue,
            _ => false
        }
        let jsonValue = JsonSerializer.Serialize(value is null ? null : $"{value.FullName}, System.Private.CoreLib")
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Type)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteTypePropertyWithConverter),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(Type)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(Type))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(WriteRequiredPropertyEnumWithConverterTheoryData))]
    public void WriteRequiredPropertyEnumWithConverter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteCultureInfoPropertyWithConverterTheoryData))]
    public void TryWriteCultureInfoPropertyWithConverter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteEnumPropertyWithConverterTheoryData))]
    public void TryWriteEnumPropertyWithConverter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableEnumPropertyWithConverterTheoryData))]
    public void TryWriteNullableEnumPropertyWithConverter(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteTypePropertyWithConverterTheoryData))]
    public void TryWriteTypePropertyWithConverter(IXUnitTest test) => test.Execute(this);
    #endregion
}
