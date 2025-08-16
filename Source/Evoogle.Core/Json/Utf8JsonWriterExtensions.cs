// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.Json;

/// <summary>
///     Provides helper methods for <see cref="Utf8JsonWriter"/> that only write values when
///     the current <see cref="JsonSerializerOptions"/> indicate they should be included.
/// </summary>
public static partial class Utf8JsonWriterExtensions
{
    #region TryWrite Methods

    /// <summary>
    ///     Attempts to write a value using the specified <paramref name="writeAction"/> when the
    ///     current <paramref name="options"/> permit it.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeAction">The action that writes the value to the writer.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to determine whether the value is the default and should be
    ///     ignored.
    /// </param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWrite<T>
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

        var (shouldWrite, _) = ShouldWrite(value, options, equalityComparer);
        if (!shouldWrite)
        {
            return false;
        }

        writeAction(value);

        return true;
    }

    /// <summary>
    ///     Attempts to write a nullable value using the specified <paramref name="writeAction"/>
    ///     when the provided <paramref name="options"/> allow it.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="nullableValue">The value to write, if present.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeAction">The action that writes the value to the writer.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to determine whether the value is the default and should be
    ///     ignored.
    /// </param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWrite<T>
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

        var (shouldWrite, isNull) = ShouldWrite(nullableValue, options, equalityComparer);
        if (!shouldWrite)
        {
            return false;
        }

        if (isNull)
        {
            writer.WriteNullValue();
        }
        else
        {
            var value = nullableValue!.Value;
            writeAction(value);
        }

        return true;
    }

    /// <summary>
    ///     Attempts to write a reference type value using the specified
    ///     <paramref name="writeAction"/> when the supplied <paramref name="options"/> permit it.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="obj">The object to write.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeAction">The action that writes the value to the writer.</param>
    /// <returns><c>true</c> if the value was written; otherwise, <c>false</c>.</returns>
    public static bool TryWrite<T>
    (
        this Utf8JsonWriter writer,
        T? obj,
        JsonSerializerOptions options,
        Action<T> writeAction
    )
        where T : class
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, isNull) = ShouldWrite(obj, options);
        if (!shouldWrite)
        {
            return false;
        }

        if (isNull)
        {
            writer.WriteNullValue();
        }
        else
        {
            writeAction(obj!);
        }

        return true;
    }
    #endregion

    #region TryWriteProperty Extension Methods

    /// <summary>
    ///     Attempts to write a property using the supplied <paramref name="writeAction"/> when
    ///     the provided <paramref name="options"/> indicate the value should be included.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeAction">The action that writes the property and value.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to determine whether the value is the default and should be
    ///     ignored.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteProperty<T>
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

        var (shouldWrite, _) = ShouldWrite(value, options, equalityComparer);
        if (!shouldWrite)
        {
            return false;
        }

        writeAction(propertyName, value);
        return true;
    }

    /// <summary>
    ///     Attempts to write a nullable property using the supplied
    ///     <paramref name="writeAction"/> when the
    ///     <paramref name="options"/> allow it.
    /// </summary>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="nullableValue">The value to write, if present.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeAction">The action that writes the property and value.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to determine whether the value is the default and should be
    ///     ignored.
    /// </param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteProperty<T>
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

        var (shouldWrite, isNull) = ShouldWrite(nullableValue, options, equalityComparer);
        if (!shouldWrite)
        {
            return false;
        }

        if (isNull)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            var value = nullableValue!.Value;
            writeAction(propertyName, value);
        }

        return true;
    }

    /// <summary>
    ///     Attempts to write a property for a reference type using the supplied
    ///     <paramref name="writeAction"/> when the <paramref name="options"/> permit it.
    /// </summary>
    /// <typeparam name="T">The type of the object to write.</typeparam>
    /// <param name="writer">The writer to which the value will be written.</param>
    /// <param name="propertyName">The name of the JSON property.</param>
    /// <param name="obj">The object to write.</param>
    /// <param name="options">The serializer options that control when values are ignored.</param>
    /// <param name="writeAction">The action that writes the property and value.</param>
    /// <returns><c>true</c> if the property was written; otherwise, <c>false</c>.</returns>
    public static bool TryWriteProperty<T>
    (
        this Utf8JsonWriter writer,
        string propertyName,
        T? obj,
        JsonSerializerOptions options,
        Action<string, T> writeAction
    )
        where T : class
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(writeAction);

        var (shouldWrite, isNull) = ShouldWrite(obj, options);
        if (!shouldWrite)
        {
            return false;
        }

        if (isNull)
        {
            writer.WriteNull(propertyName);
        }
        else
        {
            writeAction(propertyName, obj!);
        }

        return true;
    }
    #endregion

    #region Implementation Methods
    /// <summary>
    ///     Determines whether a struct value should be written based on the serializer options
    ///     and an optional equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value being evaluated.</typeparam>
    /// <param name="value">The value to inspect.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns>
    ///     A tuple indicating whether the value should be written and whether it was the default
    ///     value.
    /// </returns>
    private static (bool ShouldWrite, bool IsDefault) ShouldWrite<T>(T value, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer)
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

            _ => throw new ArgumentOutOfRangeException(nameof(options))
        };
    }

    /// <summary>
    ///     Determines whether a nullable struct value should be written based on the serializer
    ///     options and an optional equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value being evaluated.</typeparam>
    /// <param name="nullableValue">The value to inspect.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <param name="equalityComparer">
    ///     Optional comparer used to detect default values.
    /// </param>
    /// <returns>
    ///     A tuple indicating whether the value should be written and whether it was <c>null</c>.
    /// </returns>
    private static (bool ShouldWrite, bool IsNull) ShouldWrite<T>(T? nullableValue, JsonSerializerOptions options, EqualityComparer<T>? equalityComparer)
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
                throw new ArgumentOutOfRangeException(nameof(options));
        }
    }

    /// <summary>
    ///     Determines whether a reference type value should be written based on the serializer
    ///     options.
    /// </summary>
    /// <typeparam name="T">The type of the value being evaluated.</typeparam>
    /// <param name="obj">The object to inspect.</param>
    /// <param name="options">The serializer options that control ignore conditions.</param>
    /// <returns>
    ///     A tuple indicating whether the value should be written and whether it was
    ///     <c>null</c>.
    /// </returns>
    private static (bool ShouldWrite, bool IsNull) ShouldWrite<T>(T? obj, JsonSerializerOptions options)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(options);

        var isNull = obj is null;

        return options.DefaultIgnoreCondition switch
        {
            JsonIgnoreCondition.WhenWritingNull or JsonIgnoreCondition.WhenWritingDefault =>
                isNull ? (false, true) : (true, false),

            JsonIgnoreCondition.Never =>
                (true, isNull),

            _ => throw new ArgumentOutOfRangeException(nameof(options))
        };
    }
    #endregion
}
