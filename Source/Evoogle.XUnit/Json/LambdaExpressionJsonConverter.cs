// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Json.Internal;

namespace Evoogle.Json;

/// <summary>
///     JSON converter for the <see cref="LambdaExpression"/>> .NET class.
/// </summary>
public class LambdaExpressionJsonConverter : JsonConverter<LambdaExpression>
{
    #region Types
    private class LambdaExpressionInfo
    {
        #region Properties
        public Type? ResultType { get; set; }
        public Type[]? ParameterTypes { get; set; }
        public string? Body { get; set; }

        private static JsonConverter<Type> TypeJsonConverter = new TypeJsonConverter();
        #endregion

        #region Accessor Methods
        public string GetBody()
        {
            return this.Body ?? throw new NullReferenceException($"{nameof(Body)} property is null.");
        }

        public Type[] GetParameterTypes()
        {
            if (this.ParameterTypes == null)
                throw new NullReferenceException($"{nameof(ParameterTypes)} property is null.");

            return this.ParameterTypes;
        }

        public Type GetResultType()
        {
            return this.ResultType ?? throw new NullReferenceException($"{nameof(ResultType)} property is null.");
        }

        public bool HasParameterTypes() => this.ParameterTypes != null && this.ParameterTypes.Any();
        #endregion

        #region JSON Methods
        public static LambdaExpressionInfo JsonRead(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"{nameof(LambdaExpressionInfo)} JSON deserilization error, expected JSON token type enumeration {{Value={JsonTokenType.StartObject}}}.");
            }

            var lambdaExpressionInfo = new LambdaExpressionInfo();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return lambdaExpressionInfo;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"{nameof(LambdaExpressionInfo)} JSON deserilization error, expected JSON token type enumeration {{Value={JsonTokenType.PropertyName}}}.");
                }

                var propertyName = reader.GetString();
                if (propertyName == GetResultTypeJsonPropertyName(options))
                {
                    reader.Read();
                    var resultType = TypeJsonConverter.Read(ref reader, typeof(Type), options) ?? throw new JsonException($"{nameof(LambdaExpressionInfo)} JSON deserilization error, deserilization of result type return null.");
                    lambdaExpressionInfo.ResultType = resultType;
                }
                else if (propertyName == GetParameterTypesJsonPropertyName(options))
                {
                    reader.Read();
                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        lambdaExpressionInfo.ParameterTypes = null;
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        var parameterTypeList = new List<Type>();
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndArray)
                            {
                                break;
                            }

                            var parameterType = TypeJsonConverter.Read(ref reader, typeof(Type), options) ?? throw new JsonException($"{nameof(LambdaExpressionInfo)} JSON deserilization error, deserilization of parameter type return null.");
                            parameterTypeList.Add(parameterType);
                        }

                        var parameterTypes = parameterTypeList.ToArray();
                        lambdaExpressionInfo.ParameterTypes = parameterTypes;
                    }
                }
                else if (propertyName == GetBodyJsonPropertyName(options))
                {
                    reader.Read();
                    var body = reader.GetString();
                    lambdaExpressionInfo.Body = body;
                }
                else
                {
                    throw new JsonException($"{nameof(LambdaExpressionInfo)} JSON deserilization error, do not know how to deserialize JSON property {{Name={propertyName}}}.");
                }
            }

            throw new JsonException($"{nameof(LambdaExpressionInfo)} JSON deserilization error, unexpected end of JSON read method.");
        }

        public static void JsonWrite(Utf8JsonWriter writer, LambdaExpressionInfo lambdaExpressionInfo, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(GetResultTypeJsonPropertyName(options));
            TypeJsonConverter.Write(writer, lambdaExpressionInfo.GetResultType(), options);

            writer.WritePropertyName(GetParameterTypesJsonPropertyName(options));
            var parameterTypes = lambdaExpressionInfo.ParameterTypes;
            if (parameterTypes == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartArray();
                foreach (var parameterType in parameterTypes)
                {
                    TypeJsonConverter.Write(writer, parameterType, options);
                }
                writer.WriteEndArray();
            }

            writer.WritePropertyName(GetBodyJsonPropertyName(options));
            writer.WriteStringValue(lambdaExpressionInfo.GetBody());

            writer.WriteEndObject();
        }

        private static string GetBodyJsonPropertyName(JsonSerializerOptions options)
        {
            var bodyPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(Body)) ?? nameof(Body);
            return bodyPropertyName;
        }

        private static string GetParameterTypesJsonPropertyName(JsonSerializerOptions options)
        {
            var resultTypePropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(ParameterTypes)) ?? nameof(ParameterTypes);
            return resultTypePropertyName;
        }

        private static string GetResultTypeJsonPropertyName(JsonSerializerOptions options)
        {
            var resultTypePropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(ResultType)) ?? nameof(ResultType);
            return resultTypePropertyName;
        }
        #endregion   
    }
    #endregion

    #region JsonConverter Overrides
    /// <summary>
    ///     Override of <see cref="JsonConverter{LambdaExpression}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/> method.
    /// </summary>
    public override LambdaExpression? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var lambdaExpressionInfo = LambdaExpressionInfo.JsonRead(ref reader, options);

        var resultType = lambdaExpressionInfo.GetResultType();
        var expressionBodyString = lambdaExpressionInfo.GetBody();

        var reconstructedExpressionUntyped = default(LambdaExpression);

        if (!lambdaExpressionInfo.HasParameterTypes())
        {
            reconstructedExpressionUntyped = DynamicExpressionParser.ParseLambda(ParsingConfig.Default, false, resultType, expressionBodyString) ?? throw new InvalidOperationException($"Could not parse lambda body string {{Text={expressionBodyString}}} into {nameof(LambdaExpression)}.");
        }
        else
        {
            var parameterNameChar = 'a';
            var parameterTypes = lambdaExpressionInfo.GetParameterTypes();
            var parameterExpressions = parameterTypes
                .Select(x =>
                {
                    var parameterType = x;
                    var parameterName = parameterNameChar++.ToString();

                    var expressionParameter = Expression.Parameter(parameterType, parameterName);
                    return expressionParameter;
                })
                .ToArray();

            reconstructedExpressionUntyped = DynamicExpressionParser.ParseLambda(ParsingConfig.Default, false, parameterExpressions, resultType, expressionBodyString) ?? throw new InvalidOperationException($"Could not parse lambda body string {{Text={expressionBodyString}}} into {nameof(LambdaExpression)}.");
        }

        if (reconstructedExpressionUntyped == null)
            return null;

        return reconstructedExpressionUntyped;
    }

    /// <summary>
    ///     Override of <see cref="JsonConverter{LambdaExpression}.Write(Utf8JsonWriter, LambdaExpression, JsonSerializerOptions)"/> method.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, LambdaExpression expression, JsonSerializerOptions options)
    {
        var expressionParameterTypes = ExpressionUtils.GetParameterTypes(expression);

        var expressionBody = expression.Body;
        var expressionBodyResultType = expressionBody.Type;
        var expressionBodyString = ExpressionUtils.GetExpressionBodyString(expressionBody);

        var lambdaExpressionInfo = new LambdaExpressionInfo
        {
            ResultType = expressionBodyResultType,
            ParameterTypes = expressionParameterTypes,
            Body = expressionBodyString
        };

        LambdaExpressionInfo.JsonWrite(writer, lambdaExpressionInfo, options);
    }
    #endregion
}
