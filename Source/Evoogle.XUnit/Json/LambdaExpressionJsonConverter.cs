// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Json;
using Evoogle.XUnit.Json.Internal;

namespace Evoogle.XUnit.Json;

/// <summary>
///     JSON converter for the <see cref="LambdaExpression"/> .NET class.
///     Stores a structured payload: result type, parameter types, and the string body.
///     NOTE: Intended for trusted inputs only (Dynamic LINQ parsing).
/// </summary>
public sealed class LambdaExpressionJsonConverter : JsonConverter<LambdaExpression>
{
    #region DTO
    private sealed class LambdaExpressionInfo
    {
        public Type? ResultType { get; set; }
        public Type[]? ParameterTypes { get; set; }
        public string? Body { get; set; }

        private static readonly JsonConverter<Type> TypeJsonConverter = new TypeJsonConverter();

        public string GetBody() => Body ?? throw new NullReferenceException($"{nameof(Body)} property is null.");

        public Type[] GetParameterTypes()
        {
            if (ParameterTypes is null)
                throw new NullReferenceException($"{nameof(ParameterTypes)} property is null.");
            return ParameterTypes;
        }

        public Type GetResultType()
            => ResultType ?? throw new NullReferenceException($"{nameof(ResultType)} property is null.");

        public static string GetParameterTypesJsonPropertyName() => "ParameterTypes";
        public static string GetResultTypeJsonPropertyName() => "ResultType";
        public static string GetBodyJsonPropertyName() => "Body";

        public static void WriteTo(Utf8JsonWriter writer, LambdaExpression value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(GetResultTypeJsonPropertyName());
            TypeJsonConverter.Write(writer, value.ReturnType, options);

            writer.WritePropertyName(GetParameterTypesJsonPropertyName());
            writer.WriteStartArray();
            foreach (var p in value.Parameters)
                TypeJsonConverter.Write(writer, p.Type, options);
            writer.WriteEndArray();

            writer.WritePropertyName(GetBodyJsonPropertyName());
            var expressionBodyString = ExpressionUtils.GetExpressionBodyString(value.Body);
            writer.WriteStringValue(expressionBodyString);

            writer.WriteEndObject();
        }

        public static LambdaExpression? ReadFrom(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected start of object for LambdaExpression.");

            Type? resultType = null;
            List<Type>? parameterTypes = null;
            string? body = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) break;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected property name.");

                var propName = reader.GetString();
                reader.Read();

                if (string.Equals(propName, GetResultTypeJsonPropertyName(), StringComparison.Ordinal))
                {
                    resultType = TypeJsonConverter.Read(ref reader, typeof(Type), options);
                }
                else if (string.Equals(propName, GetParameterTypesJsonPropertyName(), StringComparison.Ordinal))
                {
                    if (reader.TokenType != JsonTokenType.StartArray)
                        throw new JsonException("Expected start of array for ParameterTypes.");

                    parameterTypes = new List<Type>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        var t = TypeJsonConverter.Read(ref reader, typeof(Type), options)
                                ?? throw new JsonException("Unable to deserialize parameter type.");
                        parameterTypes.Add(t);
                    }
                }
                else if (string.Equals(propName, GetBodyJsonPropertyName(), StringComparison.Ordinal))
                {
                    body = reader.GetString();
                }
                else
                {
                    reader.Skip();
                }
            }

            if (resultType is null || parameterTypes is null || body is null)
                return null;

            // Reconstruct the lambda using your parameter-naming convention: 'a', 'b', 'c', ...
            var parameterNameChar = 'a';
            var parameterExpressions = parameterTypes
                .Select(t =>
                {
                    var name = (parameterNameChar++).ToString();
                    return Expression.Parameter(t, name);
                })
                .ToArray();

            // Use the Dynamic LINQ overload that accepts (parameters, resultType, body).
            var reconstructed = DynamicExpressionParser.ParseLambda(
                                     ParsingConfig.Default,
                                     false,
                                     parameterExpressions,
                                     resultType,
                                     body)
                                 ?? throw new InvalidOperationException(
                                     $"Could not parse lambda body string {{Text={body}}} into {nameof(LambdaExpression)}.");

            return (LambdaExpression)reconstructed;
        }
    }
    #endregion

    public override LambdaExpression? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => LambdaExpressionInfo.ReadFrom(ref reader, options);

    public override void Write(Utf8JsonWriter writer, LambdaExpression value, JsonSerializerOptions options)
        => LambdaExpressionInfo.WriteTo(writer, value, options);
}
