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
///     JSON converter for the <see cref="Expression{Func{TResult}}"/> .NET class.
/// </summary>
/// <typeparam name="TResult">Type of the lambda result.</typeparam>
public sealed class ExpressionFuncJsonConverter<TResult> : JsonConverter<Expression<Func<TResult>>>
{
    public override Expression<Func<TResult>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
            return null;

        var lambda = DynamicExpressionParser.ParseLambda(
            ParsingConfig.Default,
            false,
            typeof(TResult),
            body);

        return (Expression<Func<TResult>>)lambda;
    }

    public override void Write(Utf8JsonWriter writer, Expression<Func<TResult>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
}

/// <summary>
///     JSON converter for the <see cref="Expression{Func{T,TResult}}"/> .NET class.
/// </summary>
/// <typeparam name="T">Type of the single input parameter.</typeparam>
/// <typeparam name="TResult">Type of the lambda result.</typeparam>
public sealed class ExpressionFuncJsonConverter<T, TResult> : JsonConverter<Expression<Func<T, TResult>>>
{
    private static readonly ParameterExpression[] ParameterExpressions =
    [
        Expression.Parameter(typeof(T), "a")
    ];

    public override Expression<Func<T, TResult>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
            return null;

        var lambda = DynamicExpressionParser.ParseLambda(
            ParsingConfig.Default,
            false,
            ParameterExpressions,
            typeof(TResult),
            body);

        return (Expression<Func<T, TResult>>)lambda;
    }

    public override void Write(Utf8JsonWriter writer, Expression<Func<T, TResult>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
}
