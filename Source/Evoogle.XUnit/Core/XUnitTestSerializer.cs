// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle.XUnit;

/// <summary>
///     Serialization helpers for saving and restoring test classes.
/// </summary>
public static class XUnitTestSerializer
{
    #region Properties
    /// <summary>
    ///     Global JSON options used by this serializer. Callers may register additional converters.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = BuildDefault();
    #endregion

    #region Methods
    /// <summary>Serialize a test instance to JSON.</summary>
    public static string Serialize(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var t = instance.GetType();
        if (!IsSerializable(t))
        {
            throw new InvalidOperationException(
                $"Type '{t.FullName}' cannot be serialized by {nameof(XUnitTestSerializer)} " +
                "because it is not a subclass of XUnitTest or XUnitTestAsync.");
        }

        return JsonSerializer.Serialize(instance, Options);
    }

    /// <summary>Deserialize JSON into a test instance.</summary>
    public static T Deserialize<T>(string json)
        where T : class
    {
        ArgumentException.ThrowIfNullOrEmpty(json);

        var value = JsonSerializer.Deserialize<T>(json, Options);
        if (value is null)
            throw new InvalidOperationException($"Deserialization produced null for target type '{typeof(T).FullName}'.");

        var t = value.GetType();
        if (!IsSerializable(t))
        {
            throw new InvalidOperationException(
                $"Type '{t.FullName}' cannot be deserialized by {nameof(XUnitTestSerializer)} " +
                "because it is not a subclass of XUnitTest or XUnitTestAsync.");
        }

        return value;
    }

    /// <summary>
    ///     Returns true if the type derives from <see cref="XUnitTest"/> or <see cref="XUnitTestAsync"/>.
    /// </summary>
    public static bool IsSerializable(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return typeof(XUnitTest).IsAssignableFrom(type) || typeof(XUnitTestAsync).IsAssignableFrom(type);
    }

    private static JsonSerializerOptions BuildDefault()
    {
        var o = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            WriteIndented = false
        };

        // If you have custom converters, register them here or from callers:
        // o.Converters.Add(new TypeJsonConverter());
        // o.Converters.Add(new LambdaExpressionJsonConverter());

        return o;
    }
    #endregion
}
