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
///     JSON converter for the <see cref="Expression{Action{T}}"/>> .NET class.
///     As a convention, the lambda expression argument must have the 'a' name, i.e. a => Foo(a);
/// </summary>
/// <typeparam name="T">Type of the lambda argument.</typeparam>
public sealed class ExpressionActionJsonConverter<T> : JsonConverter<Expression<Action<T>>>
{
    #region Properties
    private static ParameterExpression[] ParameterExpressions { get; } = [Expression.Parameter(typeof(T), "a")];
    #endregion

    #region JsonConverter Methods
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Action<T>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
        {
            return null;
        }

        var lambda = DynamicExpressionParser.ParseLambda(
            typeof(Action<T>),
            ParsingConfig.Default,
            false,
            ParameterExpressions,
            null,
            body);

        return (Expression<Action<T>>)lambda;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Action<T>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
    #endregion
}

/// <summary>
///     JSON converter for the <see cref="Expression{Action{T1,T2}}"/>> .NET class.
///     As a convention, the lambda expression argument must have the 'a' and 'b' names, i.e. (a,b) => Foo(a,b);
/// </summary>
/// <typeparam name="T">Type of the lambda argument.</typeparam>
public sealed class ExpressionActionJsonConverter<T1, T2> : JsonConverter<Expression<Action<T1, T2>>>
{
    #region Properties
    private static ParameterExpression[] ParameterExpressions { get; } = [Expression.Parameter(typeof(T1), "a"), Expression.Parameter(typeof(T2), "b")];
    #endregion

    #region JsonConverter Methods
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Action<T1, T2>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var body = reader.GetString();
        if (string.IsNullOrEmpty(body))
        {
            return null;
        }

        var lambda = DynamicExpressionParser.ParseLambda(
            typeof(Action<T1, T2>),
            ParsingConfig.Default,
            false,
            ParameterExpressions,
            null,
            body);

        return (Expression<Action<T1, T2>>)lambda;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Action<T1, T2>> expression, JsonSerializerOptions options)
    {
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expression.Body);
        writer.WriteStringValue(expressionBodyString);
    }
    #endregion
}
