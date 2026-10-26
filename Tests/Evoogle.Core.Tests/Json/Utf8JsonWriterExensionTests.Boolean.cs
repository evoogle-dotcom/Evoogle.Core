// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extensions;
using Evoogle.XUnit;

namespace Evoogle.Json;

public partial class Utf8JsonWriterExensionTests
{
    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] TryWriteBoolPropertyAsBooleanTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteBoolPropertyAsBoolean),
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

    public static TheoryDataRow<IXUnitTest>[] TryWriteNullableBoolPropertyAsBooleanTheoryData =>
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
                        nameof(Utf8JsonWriterTestHelper.TryWriteNullableBoolPropertyAsBoolean),
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
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TryWriteBoolPropertyAsBooleanTheoryData))]
    public void TryWriteBoolPropertyAsBoolean(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TryWriteNullableBoolPropertyAsBooleanTheoryData))]
    public void TryWriteNullableBoolPropertyAsBoolean(IXUnitTest test) => test.Execute(this);
    #endregion
}
