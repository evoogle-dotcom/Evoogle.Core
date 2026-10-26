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
    public static TheoryDataRow<IXUnitTest>[] TryWriteDecimalPropertyAsNumberTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteDecimalPropertyAsNumber),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteDoublePropertyAsNumberTheoryData =>
    [.. (
        from value in new[] { -1.5d, 0d, 1.5d }
        from defaultValue in new[] { -1.5d, 0d, 1.5d }
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
                Name = $"Type={typeof(double)}, Value={value}, Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteDoublePropertyAsNumber),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteFloatPropertyAsNumberTheoryData =>
    [.. (
        from value in new[] { -1.5f, 0f, 1.5f }
        from defaultValue in new[] { -1.5f, 0f, 1.5f }
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
                Name = $"Type={typeof(float)}, Value={value}, Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteFloatPropertyAsNumber),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteIntPropertyAsNumberTheoryData =>
    [.. (
        from value in new[] { -1, 0, 1 }
        from defaultValue in new[] { -1, 0, 1 }
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
                Name = $"Type={typeof(int)}, Value={value}, Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteIntPropertyAsNumber),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteLongPropertyAsNumberTheoryData =>
    [.. (
        from value in new[] { -1L, 0L, 1L }
        from defaultValue in new[] { -1L, 0L, 1L }
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
                Name = $"Type={typeof(long)}, Value={value}, Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteLongPropertyAsNumber),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableDecimalPropertyAsNumberTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableDecimalPropertyAsNumber),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableDoublePropertyAsNumberTheoryData =>
    [.. (
        from value in new double?[] { null, -1.5d, 0d, 1.5d }
        from defaultValue in new double?[] { null, -1.5d, 0d, 1.5d }
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
                Name = $"Type={typeof(double?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableDoublePropertyAsNumber),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(double?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(double?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableFloatPropertyAsNumberTheoryData =>
    [.. (
        from value in new float?[] { null, -1.5f, 0f, 1.5f }
        from defaultValue in new float?[] { null, -1.5f, 0f, 1.5f }
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
                Name = $"Type={typeof(float?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableNullableFloatPropertyAsNumber),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(float?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(float?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableIntPropertyAsNumberTheoryData =>
    [.. (
        from value in new int?[] { null, -1, 0, 1 }
        from defaultValue in new int?[] { null, -1, 0, 1 }
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
                Name = $"Type={typeof(int?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableIntPropertyAsNumber),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(int?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(int?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableLongPropertyAsNumberTheoryData =>
    [.. (
        from value in new long?[] { null, -1L, 0L, 1L }
        from defaultValue in new long?[] { null, -1L, 0L, 1L }
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
                Name = $"Type={typeof(long?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableLongPropertyAsNumber),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(long?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(long?))
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
    [MemberData(nameof(TryWriteDecimalPropertyAsNumberTheoryData))]
    public void TryWriteDecimalPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteDoublePropertyAsNumberTheoryData))]
    public void TryWriteDoublePropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteFloatPropertyAsNumberTheoryData))]
    public void TryWriteFloatPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteIntPropertyAsNumberTheoryData))]
    public void TryWriteIntPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteLongPropertyAsNumberTheoryData))]
    public void TryWriteLongPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableDecimalPropertyAsNumberTheoryData))]
    public void TryWriteNullableDecimalPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableDoublePropertyAsNumberTheoryData))]
    public void TryWriteNullableDoublePropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableFloatPropertyAsNumberTheoryData))]
    public void TryWriteNullableFloatPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableIntPropertyAsNumberTheoryData))]
    public void TryWriteNullableIntPropertyAsNumber(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableLongPropertyAsNumberTheoryData))]
    public void TryWriteNullableLongPropertyAsNumber(IXUnitTest test) => test.Execute(this);
    #endregion
}
