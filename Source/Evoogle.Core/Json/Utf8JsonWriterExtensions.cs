// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     Provides extension methods for <see cref="Utf8JsonWriter"/> to conditionally write values only and properties/values based on null/default value handling defined by <see cref="JsonSerializerOptions.DefaultIgnoreCondition"/>.
/// </summary>
public static partial class Utf8JsonWriterExtensions
{
    #region Write Value/Reference Extension Methods

    /// <summary>
    ///     Writes a non-nullable value type using the provided <paramref name="writeAction"/> if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The value type to write.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options controlling whether to ignore default values.</param>
    /// <param name="writeAction">The delegate that performs the write operation.</param>
    /// <param name="equalityComparer">Optional comparer to determine if the value is the default.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>, <paramref name="options"/>, or <paramref name="writeAction"/> is null.</exception>
    public static void WriteConditionalValue<T>
    (
        this Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options,
        Action<T> writeAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, _) = ShouldWriteValue(value, options, equalityComparer);
        if (!shouldWrite)
        {
            return;
        }

        writeAction(value);
    }

    /// <summary>
    ///     Writes a nullable value type using the provided <paramref name="writeAction"/>, or writes a null token, if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="nullableValue">The nullable value to write.</param>
    /// <param name="options">The serializer options controlling when null/default values are ignored.</param>
    /// <param name="writeAction">The delegate that writes the non-null value.</param>
    /// <param name="equalityComparer">Optional comparer to determine if the value is default.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>, <paramref name="options"/>, or <paramref name="writeAction"/> is null.</exception>
    public static void WriteConditionalNullable<T>
    (
        this Utf8JsonWriter writer,
        T? nullableValue,
        JsonSerializerOptions options,
        Action<T> writeAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, valueIsNull) = ShouldWriteNullableValue(nullableValue, options, equalityComparer);
        if (!shouldWrite)
        {
            return;
        }

        if (valueIsNull)
        {
            writer.WriteNullValue();
        }
        else
        {
            var value = nullableValue!.Value;
            writeAction(value);
        }
    }

    /// <summary>
    ///     Writes a reference type using the provided <paramref name="writeAction"/>, or writes a null token, if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The reference value to write.</param>
    /// <param name="options">The serializer options controlling whether to ignore nulls.</param>
    /// <param name="writeAction">The delegate that writes the non-null value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>, <paramref name="options"/>, or <paramref name="writeAction"/> is null.</exception>
    public static void WriteConditionalReference<T>
    (
        this Utf8JsonWriter writer,
        T? value,
        JsonSerializerOptions options,
        Action<T> writeAction
    )
        where T : class
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, valueIsNull) = ShouldWriteReference(value, options);
        if (!shouldWrite)
        {
            return;
        }

        if (valueIsNull)
        {
            writer.WriteNullValue();
        }
        else
        {
            writeAction(value!);
        }
    }
    #endregion

    #region Write Property and Value/Reference Extension Methods
    /// <summary>
    ///     Writes a non-nullable value type as a named property using <paramref name="writeAction"/>, if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The JSON property name.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options to control property ignoring behavior.</param>
    /// <param name="writeAction">The delegate that writes the property name and value.</param>
    /// <param name="equalityComparer">Optional comparer to determine if the value is the default.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>, <paramref name="propertyName"/>, <paramref name="options"/>, or <paramref name="writeAction"/> is null.</exception>
    public static void WriteConditionalProperty<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T value,
        JsonSerializerOptions options,
        Action<string, T> writeAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, _) = ShouldWriteValue(value, options, equalityComparer);
        if (!shouldWrite)
        {
            return;
        }

        writeAction(propertyName, value);
    }

    /// <summary>
    ///     Writes a nullable value type as a named property using <paramref name="writeAction"/>, or writes a null property, if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The JSON property name.</param>
    /// <param name="nullableValue">The nullable value to write.</param>
    /// <param name="options">The serializer options controlling ignore conditions.</param>
    /// <param name="writeAction">The delegate that writes the property name and value.</param>
    /// <param name="equalityComparer">Optional comparer to determine if the value is the default.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>, <paramref name="propertyName"/>, <paramref name="options"/>, or <paramref name="writeAction"/> is null.</exception>
    public static void WriteConditionalNullableProperty<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? nullableValue,
        JsonSerializerOptions options,
        Action<string, T> writeAction,
        EqualityComparer<T>? equalityComparer = null
    )
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, valueIsNull) = ShouldWriteNullableValue(nullableValue, options, equalityComparer);
        if (!shouldWrite)
        {
            return;
        }

        if (valueIsNull)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            var value = nullableValue!.Value;
            writeAction(propertyName, value);
        }
    }

    /// <summary>
    ///     Writes a reference type as a named property using <paramref name="writeAction"/>, or writes a null property, if permitted by <paramref name="options"/>.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="propertyName">The JSON property name.</param>
    /// <param name="value">The reference value to write.</param>
    /// <param name="options">The serializer options controlling null-handling behavior.</param>
    /// <param name="writeAction">The delegate that writes the property name and non-null value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>, <paramref name="propertyName"/>, <paramref name="options"/>, or <paramref name="writeAction"/> is null.</exception>
    public static void WriteConditionalReferenceProperty<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? value,
        JsonSerializerOptions options,
        Action<string, T> writeAction
    )
        where T : class
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, valueIsNull) = ShouldWriteReference(value, options);
        if (!shouldWrite)
        {
            return;
        }

        if (valueIsNull)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            writeAction(propertyName, value!);
        }
    }
    #endregion

    #region Implementation Methods
    private static (bool ShouldWrite, bool IsDefault) ShouldWriteValue<T>(T value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(options);

        var comparer = equalityComparer ?? EqualityComparer<T>.Default;
        var isDefault = comparer.Equals(value, default);

        return options.DefaultIgnoreCondition switch
        {
            JsonIgnoreCondition.WhenWritingDefault =>
                isDefault ? (false, false) : (true, false),

            JsonIgnoreCondition.WhenWritingNull or JsonIgnoreCondition.Never =>
                (true, false),

            _ => throw new ArgumentOutOfRangeException(nameof(options.DefaultIgnoreCondition))
        };
    }

    private static (bool ShouldWrite, bool IsNull) ShouldWriteNullableValue<T>(T? nullableValue, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer)
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
                {
                    return (false, true);
                }

                var value = nullableValue!.Value; // value is not null => safe to access .Value

                var comparer = equalityComparer ?? EqualityComparer<T>.Default;
                var isDefault = comparer.Equals(value, default);

                return isDefault ? (false, false) : (true, false);

            case JsonIgnoreCondition.Never:
                return (true, isNull);

            default:
                throw new ArgumentOutOfRangeException(nameof(options.DefaultIgnoreCondition));
        }
    }

    private static (bool ShouldWrite, bool IsNull) ShouldWriteReference<T>(T? value, JsonSerializerOptions options)
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
    #endregion
}
