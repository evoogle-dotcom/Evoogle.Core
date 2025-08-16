// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

[DynamicLinqType]
public static class Utf8JsonWriterTestHelper
{
    #region Fields
    private static readonly EnumJsonConverter<Utf8JsonWriterTestEnum> _testEnumConverter = new();
    private static readonly TypeJsonConverter _typeConverter = new();
    #endregion

    #region Boolean Methods
    public static void WritePropertyBoolean(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = bool.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsBoolean("value", value, options);
    }

    public static void WritePropertyNullableBoolean(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? bool.Parse(valueAsString) : (bool?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsBoolean("value", value, options);
    }
    #endregion

    #region Converter Methods
    public static void WritePropertyEnumWithConverter(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = Enum.Parse<Utf8JsonWriterTestEnum>(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _testEnumConverter);
    }

    public static void WritePropertyNullableEnumWithConverter(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? Enum.Parse<Utf8JsonWriterTestEnum>(valueAsString) : (Utf8JsonWriterTestEnum?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _testEnumConverter);
    }

    public static void WritePropertyTypeWithConverter(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? Type.GetType(valueAsString, throwOnError: true) : null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithConverter("value", value, options, _typeConverter);
    }
    #endregion

    #region Number Methods
    public static void WritePropertyDecimal(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = decimal.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyDouble(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = double.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyFloat(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = float.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyInt(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = int.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyLong(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = long.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyNullableDecimal(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? decimal.Parse(valueAsString) : (decimal?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyNullableDouble(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? double.Parse(valueAsString) : (double?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyNullableFloat(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? float.Parse(valueAsString) : (float?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyNullableInt(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? int.Parse(valueAsString) : (int?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }

    public static void WritePropertyNullableLong(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? long.Parse(valueAsString) : (long?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsNumber("value", value, options);
    }
    #endregion

    #region Serializer Methods
    public static void WritePropertyBooleanWithSerializer(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = bool.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }

    public static void WritePropertyNullableBooleanWithSerializer(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? bool.Parse(valueAsString) : (bool?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }

    public static void WritePropertyDecimalWithSerializer(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = decimal.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }

    public static void WritePropertyNullableDecimalWithSerializer(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? decimal.Parse(valueAsString) : (decimal?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }

    public static void WritePropertyGuidWithSerializer(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = Guid.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }

    public static void WritePropertyNullableGuidWithSerializer(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? Guid.Parse(valueAsString) : (Guid?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }

    public static void WritePropertyStringWithSerializer(Utf8JsonWriter writer, string? value, string conditionAsString)
    {
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyWithSerializer("value", value, options);
    }
    #endregion

    #region String Methods
    public static void WritePropertyDateTime(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = DateTime.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyDateTimeOffset(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = DateTimeOffset.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyEnum(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = Enum.Parse<Utf8JsonWriterTestEnum>(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyGuid(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = Guid.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyNullableDateTime(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? DateTime.Parse(valueAsString) : (DateTime?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyNullableDateTimeOffset(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? DateTimeOffset.Parse(valueAsString) : (DateTimeOffset?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyNullableEnum(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? Enum.Parse<Utf8JsonWriterTestEnum>(valueAsString) : (Utf8JsonWriterTestEnum?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyNullableGuid(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? Guid.Parse(valueAsString) : (Guid?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyNullableTimeSpan(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? TimeSpan.Parse(valueAsString) : (TimeSpan?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyNullableUlid(Utf8JsonWriter writer, string? valueAsString, string conditionAsString)
    {
        var value = valueAsString != null ? Ulid.Parse(valueAsString) : (Ulid?)null;
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyString(Utf8JsonWriter writer, string? value, string conditionAsString)
    {
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyTimeSpan(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = TimeSpan.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }

    public static void WritePropertyUlid(Utf8JsonWriter writer, string valueAsString, string conditionAsString)
    {
        var value = Ulid.Parse(valueAsString);
        var condition = GetCondition(conditionAsString);

        var options = new JsonSerializerOptions { DefaultIgnoreCondition = condition };
        writer.TryWritePropertyAsString("value", value, options);
    }
    #endregion

    #region Implementation Methods
    private static JsonIgnoreCondition GetCondition(string conditionAsString)
    {
        return Enum.TryParse<JsonIgnoreCondition>(conditionAsString, out var condition)
            ? condition
            : throw new ArgumentException($"Invalid condition: {conditionAsString}", nameof(conditionAsString));
    }
    #endregion
}
