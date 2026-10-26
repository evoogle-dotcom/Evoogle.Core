// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

[DynamicLinqType]
public static class Utf8JsonWriterTestHelper
{
    #region Fields
    private static readonly CultureInfoJsonConverter _cultureInfoJsonConverter = new();
    private static readonly EnumJsonConverter<Utf8JsonWriterTestEnum> _testEnumConverter = new();
    private static readonly TypeJsonConverter _typeConverter = new();
    #endregion

    #region Boolean Methods
    public static void TryWriteBoolPropertyAsBoolean(Utf8JsonWriter writer, bool value, JsonIgnoreCondition condition, bool defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsBoolean("value", value, options, defaultValue);
    }

    public static void TryWriteNullableBoolPropertyAsBoolean(Utf8JsonWriter writer, bool? value, JsonIgnoreCondition condition, bool? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsBoolean("value", value, options, defaultValue);
    }
    #endregion

    #region Converter Methods
    public static void WriteRequiredPropertyEnumWithConverter(Utf8JsonWriter writer, Utf8JsonWriterTestEnum value, JsonIgnoreCondition condition)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.WritePropertyWithConverter("value", value, options, _testEnumConverter);
    }

    public static void TryWriteCultureInfoPropertyWithConverter(Utf8JsonWriter writer, CultureInfo? value, JsonIgnoreCondition condition, CultureInfo? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _cultureInfoJsonConverter, defaultValue);
    }

    public static void TryWriteEnumPropertyWithConverter(Utf8JsonWriter writer, Utf8JsonWriterTestEnum value, JsonIgnoreCondition condition, Utf8JsonWriterTestEnum defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _testEnumConverter, defaultValue);
    }

    public static void TryWriteNullableEnumPropertyWithConverter(Utf8JsonWriter writer, Utf8JsonWriterTestEnum? value, JsonIgnoreCondition condition, Utf8JsonWriterTestEnum? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _testEnumConverter, defaultValue);
    }

    public static void TryWriteTypePropertyWithConverter(Utf8JsonWriter writer, Type value, JsonIgnoreCondition condition, Type? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _typeConverter, defaultValue);
    }
    #endregion

    #region Number Methods
    public static void TryWriteDecimalPropertyAsNumber(Utf8JsonWriter writer, decimal value, JsonIgnoreCondition condition, decimal defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteDoublePropertyAsNumber(Utf8JsonWriter writer, double value, JsonIgnoreCondition condition, double defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteFloatPropertyAsNumber(Utf8JsonWriter writer, float value, JsonIgnoreCondition condition, float defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteIntPropertyAsNumber(Utf8JsonWriter writer, int value, JsonIgnoreCondition condition, int defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteLongPropertyAsNumber(Utf8JsonWriter writer, long value, JsonIgnoreCondition condition, long defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteNullableDecimalPropertyAsNumber(Utf8JsonWriter writer, decimal? value, JsonIgnoreCondition condition, decimal? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteNullableDoublePropertyAsNumber(Utf8JsonWriter writer, double? value, JsonIgnoreCondition condition, double? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteNullableNullableFloatPropertyAsNumber(Utf8JsonWriter writer, float? value, JsonIgnoreCondition condition, float? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteNullableIntPropertyAsNumber(Utf8JsonWriter writer, int? value, JsonIgnoreCondition condition, int? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }

    public static void TryWriteNullableLongPropertyAsNumber(Utf8JsonWriter writer, long? value, JsonIgnoreCondition condition, long? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options, defaultValue);
    }
    #endregion

    #region Serializer Methods
    public static void TryWriteBoolPropertyWithSerializer(Utf8JsonWriter writer, bool value, JsonIgnoreCondition condition, bool defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }

    public static void TryWriteDecimalPropertyWithSerializer(Utf8JsonWriter writer, decimal value, JsonIgnoreCondition condition, decimal defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }

    public static void TryWriteGuidPropertyWithSerializer(Utf8JsonWriter writer, Guid value, JsonIgnoreCondition condition, Guid defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }

    public static void TryWriteNullableBoolPropertyWithSerializer(Utf8JsonWriter writer, bool? value, JsonIgnoreCondition condition, bool? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }

    public static void TryWriteNullableDecimalPropertyWithSerializer(Utf8JsonWriter writer, decimal? value, JsonIgnoreCondition condition, decimal? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }

    public static void TryWriteNullableGuidPropertyWithSerializer(Utf8JsonWriter writer, Guid? value, JsonIgnoreCondition condition, Guid? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }

    public static void TryWriteStringPropertyWithSerializer(Utf8JsonWriter writer, string? value, JsonIgnoreCondition condition, string? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options, defaultValue);
    }
    #endregion

    #region String Methods
    public static void TryWriteDateTimePropertyAsString(Utf8JsonWriter writer, DateTime value, JsonIgnoreCondition condition, DateTime defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue: defaultValue);
    }

    public static void TryWriteDateTimeOffsetPropertyAsString(Utf8JsonWriter writer, DateTimeOffset value, JsonIgnoreCondition condition, DateTimeOffset defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue: defaultValue);
    }

    public static void TryWriteEnumPropertyAsString(Utf8JsonWriter writer, Utf8JsonWriterTestEnum value, JsonIgnoreCondition condition, Utf8JsonWriterTestEnum defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }

    public static void TryWriteGuidPropertyAsString(Utf8JsonWriter writer, Guid value, JsonIgnoreCondition condition, Guid defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }

    public static void TryWriteNullableDateTimePropertyAsString(Utf8JsonWriter writer, DateTime? value, JsonIgnoreCondition condition, DateTime? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue: defaultValue);
    }

    public static void TryWriteNullableDateTimeOffsetPropertyAsString(Utf8JsonWriter writer, DateTimeOffset? value, JsonIgnoreCondition condition, DateTimeOffset? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue: defaultValue);
    }

    public static void TryWriteNullableEnumPropertyAsString(Utf8JsonWriter writer, Utf8JsonWriterTestEnum? value, JsonIgnoreCondition condition, Utf8JsonWriterTestEnum? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }

    public static void TryWriteNullableGuidPropertyAsString(Utf8JsonWriter writer, Guid? value, JsonIgnoreCondition condition, Guid? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }

    public static void TryWriteNullableTimeSpanPropertyAsString(Utf8JsonWriter writer, TimeSpan? value, JsonIgnoreCondition condition, TimeSpan? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue: defaultValue);
    }

    public static void TryWriteNullableUlidPropertyAsString(Utf8JsonWriter writer, Ulid? value, JsonIgnoreCondition condition, Ulid? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }

    public static void TryWriteStringPropertyAsString(Utf8JsonWriter writer, string? value, JsonIgnoreCondition condition, string? defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }

    public static void TryWriteTimeSpanPropertyAsString(Utf8JsonWriter writer, TimeSpan value, JsonIgnoreCondition condition, TimeSpan defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue: defaultValue);
    }

    public static void TryWriteUlidPropertyAsString(Utf8JsonWriter writer, Ulid value, JsonIgnoreCondition condition, Ulid defaultValue)
    {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options, defaultValue);
    }
    #endregion
}
