// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.XUnit.Json.Internal;

namespace Evoogle.XUnit.Json;

/// <summary>
///     JSON converter for the <see cref="Expression{Action{T}}"/> .NET class.
/// </summary>
/// <typeparam name="T">Type of the single parameter.</typeparam>
public sealed class ExpressionActionJsonConverter<T> : JsonConverter<Expression<Action<T>>>
{
    private static readonly ParameterExpression[] ParameterExpressions =
    [
        Expression.Parameter(typeof(T), "a")
    ];

    public override Expression<Action<T>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
            return null;

        var lambda = DynamicExpressionParser.ParseLambda(
            typeof(Action<T>),
            ParsingConfig.Default,
            false,
            ParameterExpressions,
            null,
            body);

        return (Expression<Action<T>>)lambda;
    }

    public override void Write(Utf8JsonWriter writer, Expression<Action<T>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
}

/// <summary>
///     JSON converter for the <see cref="Expression{Action{T1,T2}}"/> .NET class.
/// </summary>
/// <typeparam name="T1">Type of the first parameter.</typeparam>
/// <typeparam name="T2">Type of the second parameter.</typeparam>
public sealed class ExpressionActionJsonConverter<T1, T2> : JsonConverter<Expression<Action<T1, T2>>>
{
    private static readonly ParameterExpression[] ParameterExpressions =
    [
        Expression.Parameter(typeof(T1), "a"),
        Expression.Parameter(typeof(T2), "b")
    ];

    public override Expression<Action<T1, T2>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
            return null;

        var lambda = DynamicExpressionParser.ParseLambda(
            typeof(Action<T1, T2>),
            ParsingConfig.Default,
            false,
            ParameterExpressions,
            null,
            body);

        return (Expression<Action<T1, T2>>)lambda;
    }

    public override void Write(Utf8JsonWriter writer, Expression<Action<T1, T2>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
}
