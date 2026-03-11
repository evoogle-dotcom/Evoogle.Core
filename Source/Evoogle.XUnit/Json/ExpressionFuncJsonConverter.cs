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
///     JSON converter for <c>Expression&lt;Func&lt;TResult&gt;&gt;</c> .NET lambda expressions.
/// </summary>
/// <typeparam name="TResult">Type of the lambda return value.</typeparam>
public class ExpressionFuncJsonConverter<TResult> : JsonConverter<Expression<Func<TResult>>>
{
    #region JsonConverter Methods
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Func<TResult>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
        {
            return null;
        }

        var lambda = DynamicExpressionParser.ParseLambda(
            ParsingConfig.Default,
            false,
            typeof(TResult),
            body);

        return (Expression<Func<TResult>>)lambda;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Func<TResult>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
    #endregion
}

/// <summary>
///     JSON converter for <c>Expression&lt;Func&lt;T, TResult&gt;&gt;</c> .NET lambda expressions.
///     As a convention, the lambda expression argument must have the 'a' name, i.e. <c>a =&gt; a.Foo()</c>.
/// </summary>
/// <typeparam name="T">Type of the single lambda argument.</typeparam>
/// <typeparam name="TResult">Type of the lambda return value.</typeparam>
public class ExpressionFuncJsonConverter<T, TResult> : JsonConverter<Expression<Func<T, TResult>>>
{
    #region Properties
    private static ParameterExpression[] ParameterExpressions { get; } = [Expression.Parameter(typeof(T), "a")];
    #endregion

    #region JsonConverter Methods
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Func<T, TResult>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
        {
            return null;
        }

        var lambda = DynamicExpressionParser.ParseLambda(
            ParsingConfig.Default,
            false,
            ParameterExpressions,
            typeof(TResult),
            body);

        return (Expression<Func<T, TResult>>)lambda;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Func<T, TResult>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
    #endregion
}
