// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Json.Internal;

namespace Evoogle.Json;

/// <summary>
///     JSON converter for the <see cref="Expression{Func{TResult}}"/>> .NET class.
/// </summary>
/// <typeparam name="TResult">Type of the lambda result.</typeparam>
public class ExpressionFuncJsonConverter<TResult> : JsonConverter<Expression<Func<TResult>>>
{
    #region JsonConverter Overrides
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Func<TResult>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var serializedExpression = reader.GetString();
        if (serializedExpression == null)
            return null;

        var deserializedExpression = JsonSerializer.Deserialize<string>(serializedExpression);
        if (deserializedExpression == null)
            return null;

        var reconstructedExpressionUntyped = DynamicExpressionParser.ParseLambda(ParsingConfig.Default, false, typeof(TResult), deserializedExpression) ?? throw new InvalidOperationException($"Could not parse string {{Text={deserializedExpression}}} into {nameof(Func<TResult>)}.");
        if (reconstructedExpressionUntyped == null)
            return null;

        var reconstructedExpressionTyped = (Expression<Func<TResult>>)reconstructedExpressionUntyped;
        return reconstructedExpressionTyped;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Func<TResult>> expression, JsonSerializerOptions options)
    {
        var expressionBody = expression.Body;
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expressionBody);

        var serializedExpression = JsonSerializer.Serialize(expressionBodyString);
        writer.WriteStringValue(serializedExpression);
    }
    #endregion
}

/// <summary>
///     JSON converter for the <see cref="Expression{Func{T, TResult}}"/>> .NET class.
///     As a convention, the lambda expression argument must have the 'a' name, i.e. a => a.Foo();
/// </summary>
/// <typeparam name="T">Type of the lambda argument.</typeparam>
/// <typeparam name="TResult">Type of the lambda result.</typeparam>
public class ExpressionFuncJsonConverter<T, TResult> : JsonConverter<Expression<Func<T, TResult>>>
{
    #region Properties
    private static ParameterExpression[] ParameterExpressions { get; } = [Expression.Parameter(typeof(T), "a")];
    #endregion

    #region JsonConverter Overrides
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Func<T, TResult>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var serializedExpression = reader.GetString();
        if (serializedExpression == null)
            return null;

        var deserializedExpression = JsonSerializer.Deserialize<string>(serializedExpression);
        if (deserializedExpression == null)
            return null;

        var reconstructedExpressionUntyped = DynamicExpressionParser.ParseLambda(ParsingConfig.Default, false, ParameterExpressions, typeof(TResult), deserializedExpression) ?? throw new InvalidOperationException($"Could not parse string {{Text={deserializedExpression}}} into {nameof(Func<T, TResult>)}.");
        if (reconstructedExpressionUntyped == null)
            return null;

        var reconstructedExpressionTyped = (Expression<Func<T, TResult>>)reconstructedExpressionUntyped;
        return reconstructedExpressionTyped;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Func<T, TResult>> expression, JsonSerializerOptions options)
    {
        var expressionBody = expression.Body;
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expressionBody);

        var serializedExpression = JsonSerializer.Serialize(expressionBodyString);
        writer.WriteStringValue(serializedExpression);
    }
    #endregion
}
