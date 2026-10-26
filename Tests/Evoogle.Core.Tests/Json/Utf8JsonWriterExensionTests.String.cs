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
    public static TheoryDataRow<IXUnitTest>[] TryWriteDateTimePropertyAsStringTheoryData =>
    [.. (
        from value in new DateTime[]
        {
            DateTime.MinValue,
            new DateTime(2026, 10, 25, 12, 34, 56, DateTimeKind.Utc),
            new DateTime(2026, 10, 26, 1, 2, 3, DateTimeKind.Unspecified)
        }
        from defaultValue in new DateTime[]
        {
            DateTime.MinValue,
            new DateTime(2026, 10, 25, 12, 34, 56, DateTimeKind.Utc),
            new DateTime(2026, 10, 26, 1, 2, 3, DateTimeKind.Unspecified)
        }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = JsonSerializer.Serialize(value.ToString("O", CultureInfo.InvariantCulture))
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(DateTime)}, Value={value}, "
                    + $"Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteDateTimePropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteDateTimeOffsetPropertyAsStringTheoryData =>
    [.. (
        from value in new DateTimeOffset[]
        {
            default(DateTimeOffset),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.Zero),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.FromMinutes(330))
        }
        from defaultValue in new DateTimeOffset[]
        {
            default(DateTimeOffset),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.Zero),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.FromMinutes(330))
        }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = JsonSerializer.Serialize(value.ToString("O", CultureInfo.InvariantCulture))
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(DateTimeOffset)}, Value={value}, "
                    + $"Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteDateTimeOffsetPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteEnumPropertyAsStringTheoryData =>
    [.. (
        from value in new Utf8JsonWriterTestEnum[]
        {
            Utf8JsonWriterTestEnum.None,
            Utf8JsonWriterTestEnum.Green,
            Utf8JsonWriterTestEnum.Red
        }
        from defaultValue in new Utf8JsonWriterTestEnum[]
        {
            Utf8JsonWriterTestEnum.None,
            Utf8JsonWriterTestEnum.Green,
            Utf8JsonWriterTestEnum.Red
        }
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteEnumPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteGuidPropertyAsStringTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteGuidPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableDateTimePropertyAsStringTheoryData =>
    [.. (
        from value in new DateTime?[]
        {
            null,
            DateTime.MinValue,
            new DateTime(2026, 10, 25, 12, 34, 56, DateTimeKind.Utc),
            new DateTime(2026, 10, 26, 1, 2, 3, DateTimeKind.Unspecified)
        }
        from defaultValue in new DateTime?[]
        {
            null,
            DateTime.MinValue,
            new DateTime(2026, 10, 25, 12, 34, 56, DateTimeKind.Utc),
            new DateTime(2026, 10, 26, 1, 2, 3, DateTimeKind.Unspecified)
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
        let jsonValue = JsonSerializer.Serialize(value?.ToString("O", CultureInfo.InvariantCulture))
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(DateTime?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableDateTimePropertyAsString),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(DateTime?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(DateTime?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableDateTimeOffsetPropertyAsStringTheoryData =>
    [.. (
        from value in new DateTimeOffset?[]
        {
            null,
            default(DateTimeOffset),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.Zero),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.FromMinutes(330))
        }
        from defaultValue in new DateTimeOffset?[]
        {
            null,
            default(DateTimeOffset),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.Zero),
            new DateTimeOffset(2026, 10, 25, 12, 34, 56, TimeSpan.FromMinutes(330))
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
        let jsonValue = JsonSerializer.Serialize(value?.ToString("O", CultureInfo.InvariantCulture))
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(DateTimeOffset?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableDateTimeOffsetPropertyAsString),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(DateTimeOffset?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(DateTimeOffset?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableEnumPropertyAsStringTheoryData =>
    [.. (
        from value in new Utf8JsonWriterTestEnum?[]
        {
            null,
            Utf8JsonWriterTestEnum.None,
            Utf8JsonWriterTestEnum.Green,
            Utf8JsonWriterTestEnum.Red
        }
        from defaultValue in new Utf8JsonWriterTestEnum?[]
        {
            null,
            Utf8JsonWriterTestEnum.None,
            Utf8JsonWriterTestEnum.Green,
            Utf8JsonWriterTestEnum.Red
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableEnumPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableGuidPropertyAsStringTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableGuidPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableTimeSpanPropertyAsStringTheoryData =>
    [.. (
        from value in new TimeSpan?[]
        {
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(-1.5),
            TimeSpan.FromHours(25)
        }
        from defaultValue in new TimeSpan?[]
        {
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(-1.5),
            TimeSpan.FromHours(25)
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
        let jsonValue = JsonSerializer.Serialize(value?.ToString("c", CultureInfo.InvariantCulture))
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(TimeSpan?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableTimeSpanPropertyAsString),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(TimeSpan?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(TimeSpan?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableUlidPropertyAsStringTheoryData =>
    [.. (
        from value in new Ulid?[]
        {
            null,
            default(Ulid),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAV"),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAW")
        }
        from defaultValue in new Ulid?[]
        {
            null,
            default(Ulid),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAV"),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAW")
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
        let jsonValue = JsonSerializer.Serialize(value?.ToString())
        let expectedJson = isOmitted ? "{}" : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(Ulid?)}, Value={value.SafeToString()}, "
                    + $"Condition={condition}, DefaultValue={defaultValue.SafeToString()}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableUlidPropertyAsString),
                        Type.EmptyTypes,
                        writer,
                        Expression.Constant(value, typeof(Ulid?)),
                        Expression.Constant(condition),
                        Expression.Constant(defaultValue, typeof(Ulid?))
                    ),
                    writer
                ),
                ExpectedJson = expectedJson,
            }
        )
    )];

    public static TheoryDataRow<IXUnitTest>[] TryWriteStringPropertyAsStringTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteStringPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteTimeSpanPropertyAsStringTheoryData =>
    [.. (
        from value in new TimeSpan[]
        {
            TimeSpan.Zero,
            TimeSpan.FromSeconds(-1.5),
            TimeSpan.FromHours(25)
        }
        from defaultValue in new TimeSpan[]
        {
            TimeSpan.Zero,
            TimeSpan.FromSeconds(-1.5),
            TimeSpan.FromHours(25)
        }
        from condition in new[]
        {
            JsonIgnoreCondition.WhenWritingNull,
            JsonIgnoreCondition.WhenWritingDefault,
            JsonIgnoreCondition.Never
        }
        let jsonValue = JsonSerializer.Serialize(value.ToString("c", CultureInfo.InvariantCulture))
        let expectedJson = condition == JsonIgnoreCondition.WhenWritingDefault && value == defaultValue
            ? "{}"
            : $$"""{"value":{{jsonValue}}}"""
        let writer = Expression.Parameter(typeof(Utf8JsonWriter), "a")
        select new TheoryDataRow<IXUnitTest>
        (
            new WritePropertyTest
            {
                Name = $"Type={typeof(TimeSpan)}, Value={value}, "
                    + $"Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteTimeSpanPropertyAsString),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteUlidPropertyAsStringTheoryData =>
    [.. (
        from value in new Ulid[]
        {
            default(Ulid),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAV"),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAW")
        }
        from defaultValue in new Ulid[]
        {
            default(Ulid),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAV"),
            Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAW")
        }
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
                Name = $"Type={typeof(Ulid)}, Value={value}, "
                    + $"Condition={condition}, DefaultValue={defaultValue}",
                WritePropertyExpression = Expression.Lambda<Action<Utf8JsonWriter>>
                (
                    Expression.Call
                    (
                        typeof(Utf8JsonWriterTestHelper),
                        nameof(Utf8JsonWriterTestHelper.TryWriteUlidPropertyAsString),
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
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TryWriteDateTimePropertyAsStringTheoryData))]
    public void TryWriteDateTimePropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteDateTimeOffsetPropertyAsStringTheoryData))]
    public void TryWriteDateTimeOffsetPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteEnumPropertyAsStringTheoryData))]
    public void TryWriteEnumPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteGuidPropertyAsStringTheoryData))]
    public void TryWriteGuidPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableDateTimePropertyAsStringTheoryData))]
    public void TryWriteNullableDateTimePropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableDateTimeOffsetPropertyAsStringTheoryData))]
    public void TryWriteNullableDateTimeOffsetPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableEnumPropertyAsStringTheoryData))]
    public void TryWriteNullableEnumPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableGuidPropertyAsStringTheoryData))]
    public void TryWriteNullableGuidPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableTimeSpanPropertyAsStringTheoryData))]
    public void TryWriteNullableTimeSpanPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableUlidPropertyAsStringTheoryData))]
    public void TryWriteNullableUlidPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteStringPropertyAsStringTheoryData))]
    public void TryWriteStringPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteTimeSpanPropertyAsStringTheoryData))]
    public void TryWriteTimeSpanPropertyAsString(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteUlidPropertyAsStringTheoryData))]
    public void TryWriteUlidPropertyAsString(IXUnitTest test) => test.Execute(this);
    #endregion
}
