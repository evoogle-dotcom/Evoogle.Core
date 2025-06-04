// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     Provides extension methods for <see cref="Utf8JsonWriter"/> to conditionally write properties based on null/default value handling defined by <see cref="JsonSerializerOptions.DefaultIgnoreCondition"/>.
/// </summary>
public static partial class Utf8JsonWriterExtensions
{
    #region Core Extension Methods
    /// <summary>
    ///     Conditionally writes a nullable value type property.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="nullableValue">The nullable value to write.</param>
    /// <param name="options">The serializer options to respect ignore conditions.</param>
    /// <param name="writeNonNullPropertyAction">The delegate to write the non-null value.</param>
    /// <param name="equalityComparer">An optional custom equality comparer.</param>
    public static void WritePropertyNullableValueType<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? nullableValue,
        JsonSerializerOptions options,
        Action<string, T> writeNonNullPropertyAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeNonNullPropertyAction);

        var (shouldWrite, valueIsNull) = ShouldWriteNullableValueType(nullableValue, options, equalityComparer);
        if (!shouldWrite)
            return;

        if (valueIsNull)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            var value = nullableValue!.Value;
            writeNonNullPropertyAction(propertyName, value);
        }
    }

    /// <summary>
    ///     Conditionally writes a reference type property.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options to respect ignore conditions.</param>
    /// <param name="writeNonNullPropertyAction">The delegate to write the non-null value.</param>
    public static void WritePropertyReferenceType<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? value,
        JsonSerializerOptions options,
        Action<string, T> writeNonNullPropertyAction
    )
        where T : class
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeNonNullPropertyAction);

        var (shouldWrite, valueIsNull) = ShouldWriteReferenceType(value, options);
        if (!shouldWrite)
            return;

        if (valueIsNull)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            writeNonNullPropertyAction(propertyName, value!);
        }
    }

    /// <summary>
    ///     Conditionally writes a non-nullable value type property.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options to respect ignore conditions.</param>
    /// <param name="writeNonNullPropertyAction">The delegate to write the value.</param>
    /// <param name="equalityComparer">An optional custom equality comparer.</param>
    public static void WritePropertyValueType<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T value,
        JsonSerializerOptions options,
        Action<string, T> writeNonNullPropertyAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeNonNullPropertyAction);

        var (shouldWrite, _) = ShouldWriteValueType(value, options, equalityComparer);
        if (!shouldWrite)
            return;

        writeNonNullPropertyAction(propertyName, value);
    }
    #endregion

    #region Implementation Methods
    /// <summary>Determines whether a nullable value type should be written.</summary>
    private static (bool ShouldWrite, bool IsNull) ShouldWriteNullableValueType<T>(T? nullableValue, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(options);

        var isNull = !nullableValue.HasValue;

        switch (options.DefaultIgnoreCondition)
        {
            case JsonIgnoreCondition.WhenWritingNull:
                return isNull ? (false, true) : (true, false);

            case JsonIgnoreCondition.WhenWritingDefault:
                if (isNull)
                    return (false, true);

                var value = nullableValue!.Value; // value is not null => safe to access .Value

                bool isDefault;
                if (equalityComparer is null || ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
                {
                    // Fast path using direct comparison
                    isDefault = EqualityComparer<T>.Default.Equals(value, default);
                }
                else
                {
                    // Slower but supports custom comparison logic
                    isDefault = equalityComparer.Equals(value, default);
                }

                return isDefault ? (false, false) : (true, false);

            case JsonIgnoreCondition.Never:
                return (true, isNull);

            default:
                throw new ArgumentOutOfRangeException(nameof(options.DefaultIgnoreCondition));
        }
    }

    /// <summary>Determines whether a reference type value should be written.</summary>
    private static (bool ShouldWrite, bool IsNull) ShouldWriteReferenceType<T>(T? value, JsonSerializerOptions options)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(options);

        var isNull = value is null;

        return options.DefaultIgnoreCondition switch
        {
            JsonIgnoreCondition.WhenWritingNull or JsonIgnoreCondition.WhenWritingDefault =>
                isNull ? (false, true) : (true, false),

            JsonIgnoreCondition.Never =>
                (true, isNull),

            _ => throw new ArgumentOutOfRangeException(nameof(options.DefaultIgnoreCondition))
        };
    }

    /// <summary>Determines whether a non-nullable value type should be written.</summary>
    private static (bool ShouldWrite, bool IsDefault) ShouldWriteValueType<T>(T value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(options);

        bool isDefault;
        if (equalityComparer is null || ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
        {
            // Fast path using direct comparison
            isDefault = EqualityComparer<T>.Default.Equals(value, default);
        }
        else
        {
            // Slower but supports custom comparison logic
            isDefault = equalityComparer.Equals(value, default);
        }

        return options.DefaultIgnoreCondition switch
        {
            JsonIgnoreCondition.WhenWritingDefault =>
                isDefault ? (false, false) : (true, false),

            JsonIgnoreCondition.WhenWritingNull or JsonIgnoreCondition.Never =>
                (true, false),

            _ => throw new ArgumentOutOfRangeException(nameof(options.DefaultIgnoreCondition))
        };
    }
    #endregion
}
