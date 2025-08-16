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
public static partial class Utf8JsonWriterExtensions2
{
    #region TryWrite Methods
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
