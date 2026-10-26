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
    public static TheoryDataRow<IXUnitTest>[] TryWriteBoolPropertyWithSerializerTheoryData =>
    [.. (
        from value in new[] { false, true }
        from defaultValue in new[] { false, true }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : value ? """{"value":true}""" : """{"value":false}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "writer")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(bool)}, Value={value}, Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteBoolPropertyWithSerializer),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteDecimalPropertyWithSerializerTheoryData =>
    [.. (
        from value in new[] { -1.5m, 0m, 1.5m }
        from defaultValue in new[] { -1.5m, 0m, 1.5m }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = value.ToString(CultureInfo.InvariantCulture)
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(decimal)}, Value={value}, Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteDecimalPropertyWithSerializer),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteGuidPropertyWithSerializerTheoryData =>
    [.. (
        from value in new Guid[]
        {
            Guid.Empty,
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222")
        }
        from defaultValue in new Guid[]
        {
            Guid.Empty,
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222")
        }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = JsonSerializer.Serialize(value.ToString("D"))
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Guid)}, Value={value}, "
                    + $"Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteGuidPropertyWithSerializer),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableBoolPropertyWithSerializerTheoryData =>
    [.. (
        from value in new bool?[] { null, false, true }
        from defaultValue in new bool?[] { null, false, true }
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
        let expectedJson = isOmitted
            ? "{}"
            : value switch
            {
                null => """{"value":null}""",
                false => """{"value":false}""",
                true => """{"value":true}"""
            }
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "writer")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(bool?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableBoolPropertyWithSerializer),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(bool?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(bool?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableDecimalPropertyWithSerializerTheoryData =>
    [.. (
        from value in new decimal?[] { null, -1.5m, 0m, 1.5m }
        from defaultValue in new decimal?[] { null, -1.5m, 0m, 1.5m }
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
        let jsonValue = value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "null"
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(decimal?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableDecimalPropertyWithSerializer),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(decimal?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(decimal?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableGuidPropertyWithSerializerTheoryData =>
    [.. (
        from value in new Guid?[]
        {
            null,
            Guid.Empty,
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222")
        }
        from defaultValue in new Guid?[]
        {
            null,
            Guid.Empty,
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222")
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
        let jsonValue = JsonSerializer.Serialize(value?.ToString("D"))
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Guid?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableGuidPropertyWithSerializer),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(Guid?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(Guid?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteStringPropertyWithSerializerTheoryData =>
    [.. (
        from value in new string?[]
        {
            null,
            string.Empty,
            "text",
            "quote\" slash\\ newline\n",
            "caf\u00E9"
        }
        from defaultValue in new string?[]
        {
            null,
            string.Empty,
            "text",
            "quote\" slash\\ newline\n",
            "caf\u00E9"
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
        let jsonValue = JsonSerializer.Serialize(value)
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(string)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteStringPropertyWithSerializer),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(string)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(string))
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
    [MemberData(nameof(TryWriteBoolPropertyWithSerializerTheoryData))]
    public void TryWriteBoolPropertyWithSerializer(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteDecimalPropertyWithSerializerTheoryData))]
    public void TryWriteDecimalPropertyWithSerializer(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteGuidPropertyWithSerializerTheoryData))]
    public void TryWriteGuidPropertyWithSerializer(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableBoolPropertyWithSerializerTheoryData))]
    public void TryWriteNullableBoolPropertyWithSerializer(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableDecimalPropertyWithSerializerTheoryData))]
    public void TryWriteNullableDecimalPropertyWithSerializer(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableGuidPropertyWithSerializerTheoryData))]
    public void TryWriteNullableGuidPropertyWithSerializer(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteStringPropertyWithSerializerTheoryData))]
    public void TryWriteStringPropertyWithSerializer(IXUnitTest test) => test.Execute(this);
    #endregion
}
