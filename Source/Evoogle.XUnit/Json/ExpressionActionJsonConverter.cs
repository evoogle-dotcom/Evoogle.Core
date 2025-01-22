// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Json.Internal;

namespace Evoogle.Json;

/// <summary>
///     JSON converter for the <see cref="Expression{Action{T}}"/>> .NET class.
///     As a convention, the lambda expression argument must have the 'a' name, i.e. a => Foo(a);
/// </summary>
/// <typeparam name="T">Type of the lambda argument.</typeparam>
public class ExpressionActionJsonConverter<T> : JsonConverter<Expression<Action<T>>>
{
    #region Properties
    private static ParameterExpression[] ParameterExpressions { get; } = [Expression.Parameter(typeof(T), "a")];
    #endregion

    #region JsonConverter Overrides
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Action<T>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var serializedExpression = reader.GetString();
        if (serializedExpression == null)
            return null;

        var deserializedExpression = JsonSerializer.Deserialize<string>(serializedExpression);
        if (deserializedExpression == null)
            return null;

        var reconstructedExpressionUntyped = DynamicExpressionParser.ParseLambda(typeof(Action<T>), ParsingConfig.Default, false, ParameterExpressions, null, deserializedExpression) ?? throw new InvalidOperationException($"Could not parse string {{Text={deserializedExpression}}} into {nameof(Action<T>)}.");
        if (reconstructedExpressionUntyped == null)
            return null;

        var reconstructedExpressionTyped = (Expression<Action<T>>)reconstructedExpressionUntyped;
        return reconstructedExpressionTyped;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Action<T>> expression, JsonSerializerOptions options)
    {
        var expressionBody = expression.Body;
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expressionBody);

        var serializedExpression = JsonSerializer.Serialize(expressionBodyString);
        writer.WriteStringValue(serializedExpression);
    }
    #endregion
}

/// <summary>
///     JSON converter for the <see cref="Expression{Action{T1,T2}}"/>> .NET class.
///     As a convention, the lambda expression argument must have the 'a' and 'b' names, i.e. (a,b) => Foo(a,b);
/// </summary>
/// <typeparam name="T">Type of the lambda argument.</typeparam>
public class ExpressionActionJsonConverter<T1, T2> : JsonConverter<Expression<Action<T1, T2>>>
{
    #region Properties
    private static ParameterExpression[] ParameterExpressions { get; } = [Expression.Parameter(typeof(T1), "a"), Expression.Parameter(typeof(T2), "b")];
    #endregion

    #region JsonConverter Overrides
    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override Expression<Action<T1, T2>>? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var serializedExpression = reader.GetString();
        if (serializedExpression == null)
            return null;

        var deserializedExpression = JsonSerializer.Deserialize<string>(serializedExpression);
        if (deserializedExpression == null)
            return null;

        var reconstructedExpressionUntyped = DynamicExpressionParser.ParseLambda(typeof(Action<T1, T2>), ParsingConfig.Default, false, ParameterExpressions, null, deserializedExpression) ?? throw new InvalidOperationException($"Could not parse string {{Text={deserializedExpression}}} into {nameof(Action<T1, T2>)}.");
        if (reconstructedExpressionUntyped == null)
            return null;

        var reconstructedExpressionTyped = (Expression<Action<T1, T2>>)reconstructedExpressionUntyped;
        return reconstructedExpressionTyped;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Expression<Action<T1, T2>> expression, JsonSerializerOptions options)
    {
        var expressionBody = expression.Body;
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expressionBody);

        var serializedExpression = JsonSerializer.Serialize(expressionBodyString);
        writer.WriteStringValue(serializedExpression);
    }
    #endregion
}
