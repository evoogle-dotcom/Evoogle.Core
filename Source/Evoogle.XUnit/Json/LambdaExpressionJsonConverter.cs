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
///     JSON converter for the <see cref="LambdaExpression"/>> .NET class.
/// </summary>
public sealed class LambdaExpressionJsonConverter : JsonConverter<LambdaExpression>
{
    #region DTO
    private sealed class LambdaExpressionInfo
    {
        #region Fields
        private static readonly JsonConverter<Type> _typeJsonConverter = new TypeJsonConverter();
        #endregion

        #region Properties
        public Type? ResultType { get; set; }
        public Type[]? ParameterTypes { get; set; }
        public string? Body { get; set; }
        #endregion

        #region Methods
        public string GetBody() => this.Body ?? throw new JsonException($"{nameof(this.Body)} property is null.");

        public static string GetBodyJsonPropertyName() => "Body";

        public Type[] GetParameterTypes()
        {
            if (this.ParameterTypes is null)
            {
                throw new JsonException($"{nameof(this.ParameterTypes)} property is null.");
            }

            return this.ParameterTypes;
        }

        public static string GetParameterTypesJsonPropertyName() => "ParameterTypes";

        public Type GetResultType() => this.ResultType ?? throw new JsonException($"{nameof(this.ResultType)} property is null.");

        public static string GetResultTypeJsonPropertyName() => "ResultType";

        public static LambdaExpression? ReadFrom(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object for LambdaExpression.");
            }

            Type? resultType = null;
            List<Type>? parameterTypes = null;
            string? body = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name.");
                }

                var propertyName = reader.GetString();
                reader.Read();

                if (string.Equals(propertyName, GetResultTypeJsonPropertyName(), StringComparison.Ordinal))
                {
                    resultType = _typeJsonConverter.Read(ref reader, typeof(Type), options);
                }
                else if (string.Equals(propertyName, GetParameterTypesJsonPropertyName(), StringComparison.Ordinal))
                {
                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException("Expected start of array for ParameterTypes.");
                    }

                    parameterTypes = [];
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        var type = _typeJsonConverter.Read(ref reader, typeof(Type), options) ?? throw new JsonException("Unable to deserialize parameter type.");
                        parameterTypes.Add(type);
                    }
                }
                else if (string.Equals(propertyName, GetBodyJsonPropertyName(), StringComparison.Ordinal))
                {
                    body = reader.GetString();
                }
                else
                {
                    reader.Skip();
                }
            }

            if (resultType is null || parameterTypes is null || body is null)
            {
                return null;
            }

            // Reconstruct the lambda using your parameter-naming convention: 'a', 'b', 'c', ...
            var parameterNameChar = 'a';
            var parameterExpressions = parameterTypes
                .Select(t =>
                {
                    var name = parameterNameChar++.ToString();
                    return Expression.Parameter(t, name);
                })
                .ToArray();

            // Use the Dynamic LINQ overload that accepts (parameters, resultType, body).
            var reconstructed = DynamicExpressionParser.ParseLambda
            (
                ParsingConfig.Default,
                false,
                parameterExpressions,
                resultType,
                body
            ) ?? throw new JsonException($"Could not parse lambda body string {{Text={body}}} into {nameof(LambdaExpression)}.");

            return reconstructed;
        }

        public static void WriteTo(Utf8JsonWriter writer, LambdaExpression value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(GetResultTypeJsonPropertyName());
            _typeJsonConverter.Write(writer, value.ReturnType, options);

            writer.WritePropertyName(GetParameterTypesJsonPropertyName());
            writer.WriteStartArray();
            foreach (var p in value.Parameters)
            {
                _typeJsonConverter.Write(writer, p.Type, options);
            }

            writer.WriteEndArray();

            writer.WritePropertyName(GetBodyJsonPropertyName());
            var expressionBodyString = ExpressionUtils.GetExpressionBodyString(value.Body);
            writer.WriteStringValue(expressionBodyString);

            writer.WriteEndObject();
        }
        #endregion
    }
    #endregion

    /// <inheritdoc />
    public override LambdaExpression? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => LambdaExpressionInfo.ReadFrom(ref reader, options);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, LambdaExpression value, JsonSerializerOptions options)
        => LambdaExpressionInfo.WriteTo(writer, value, options);
}
