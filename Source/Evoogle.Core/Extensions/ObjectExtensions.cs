// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evoogle;

/// <summary>
///     Extension methods for .NET <see cref="object"> class.
/// </summary>
public static class ObjectExtensions
{
    #region Properties
    private static JsonSerializerOptions DefaultDeepCopyWithJsonOptions { get; } = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    private static JsonSerializerOptions DefaultToJsonOptions { get; } = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };
    #endregion

    #region Methods

    /// <summary>
    ///     Create a deep copy by using the serializing/deserializing to/from JSON idiom of making a deep copy of an object.
    /// </summary>
    /// <param name="sourceObject">
    ///     Source object to make a deep copy of by using the serializing/deserializing to/from JSON idiom of making a deep copy of an object.
    /// </param>
    /// <param name="sourceType">
    ///     Type of object to serialize into JSON.
    /// </param>
    /// <param name="options">
    ///     Optional JSON serializer options to use for the JSON serializing/deserializing roundtrip.
    ///     If null, default JSON serializer options will be used.
    /// </param>
    /// <returns>
    ///     Null if the source object is null, otherwise a deep copy of the source object.
    /// </returns>
    public static object? DeepCopyWithJson(this object? sourceObject, Type sourceType, JsonSerializerOptions? options = null)
    {
        if (sourceObject == null)
            return null;

        var sourceJson = JsonSerializer.Serialize(
            sourceObject,
            sourceType,
            options ?? DefaultDeepCopyWithJsonOptions);

        var resultObject = JsonSerializer.Deserialize(
            sourceJson,
            sourceType,
            options ?? DefaultDeepCopyWithJsonOptions);

        return resultObject;
    }

    /// <summary>
    ///     Create a deep copy by using the serializing/deserializing to/from JSON idiom of making a deep copy of an object.
    /// </summary>
    /// <param name="sourceObject">
    ///     Source object to make a deep copy of by using the serializing/deserializing to/from JSON idiom of making a deep copy of an object.
    /// </param>
    /// <param name="sourceType">
    ///     Type of object to serialize into JSON.
    /// </param>
    /// <param name="resultType">
    ///     Type of object to deserialize from JSON and return.
    /// </param>
    /// <param name="options">
    ///     Optional JSON serializer options to use for the JSON serializing/deserializing roundtrip.
    ///     If null, default JSON serializer options will be used.
    /// </param>
    /// <returns>
    ///     Null if the source object is null, otherwise a deep copy of the source object.
    /// </returns>
    public static object? DeepCopyWithJson(this object? sourceObject, Type sourceType, Type resultType, JsonSerializerOptions? options = null)
    {
        if (sourceObject == null)
            return null;

        var sourceJson = JsonSerializer.Serialize(
            sourceObject,
            sourceType,
            options ?? DefaultDeepCopyWithJsonOptions);

        var resultObject = JsonSerializer.Deserialize(
            sourceJson,
            resultType,
            options ?? DefaultDeepCopyWithJsonOptions);

        return resultObject;
    }

    /// <summary>
    ///     Create a deep copy by using the serializing/deserializing to/from JSON idiom of making a deep copy of a strong-typed object.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of object to serialize/deserialize to/from JSON.
    /// </typeparam>
    /// <param name="source">
    ///     Source object to make a deep copy of by using the serializing/deserializing to/from JSON idiom of making a deep copy of an object.
    /// </param>
    /// <param name="options">
    ///     Optional JSON serializer options to use for the JSON serializing/deserializing roundtrip.
    ///     If null, default JSON serializer options will be used.
    /// </param>
    /// <returns>
    ///     Null if the source object is null, otherwise a strongly-typed deep copy of the source object.
    /// </returns>
    public static T? DeepCopyWithJson<T>(this T? source, JsonSerializerOptions? options = null)
        where T : class
    {
        var sourceObject = (object?)source;
        var sourceType = typeof(T);
        var resultObject = sourceObject.DeepCopyWithJson(sourceType, options);
        var result = (T?)resultObject;
        return result;
    }

    /// <summary>
    ///     Create a deep copy by using the serializing/deserializing to/from JSON idiom of making a deep copy of a strong-typed object.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of object to serialize to JSON.
    /// </typeparam>
    /// <typeparam name="TResult">
    ///     Type of result to deserialize from JSON.
    /// </typeparam>
    /// <param name="source">
    ///     Source object to make a deep copy of by using the serializing/deserializing to/from JSON idiom of making a deep copy of an object.
    /// </param>
    /// <param name="options">
    ///     Optional JSON serializer options to use for the JSON serializing/deserializing roundtrip.
    ///     If null, default JSON serializer options will be used.
    /// </param>
    /// <returns>
    ///     Null if the source object is null, otherwise a strongly-typed deep copy of the source object.
    /// </returns>
    public static TResult? DeepCopyWithJson<T, TResult>(this T? source, JsonSerializerOptions? options = null)
        where T : class
        where TResult : class
    {
        var sourceObject = (object?)source;
        var sourceType = typeof(T);
        var resultType = typeof(TResult);
        var resultObject = sourceObject.DeepCopyWithJson(sourceType, resultType, options);
        var result = (TResult?)resultObject;
        return result;
    }

    /// <summary>
    ///     Gets a fully qualified method name for any .NET object.
    /// </summary>
    /// <param name="obj">.NET object to call extension method on.</param>
    /// <param name="callerMethodName">The .NET object method name typically resolved by using the .NET CallerMemberName compile type attribute.</param>
    /// <returns>The fully qualified method name: TypeName.MethodName</returns>
    public static string GetFullyQualifiedMethodName(this object obj, string callerMethodName)
    {
        var fullyQualifiedMethodName = $"{obj.GetType().Name}.{callerMethodName}";
        return fullyQualifiedMethodName;
    }

    /// <summary>
    ///     Gets a safe JSON representation of any .NET object even if the .NET object is null.
    ///     Provides optional <see cref="JsonSerializerOptions" /> to be used during JSON serialization.
    /// </summary>
    /// <typeparam name="T">Type of object to get a safe JSON representation of.</typeparam>
    /// <param name="obj">.NET object to call extension method on.</param>
    /// <param name="options">
    ///     Optional JSON serializer options to use for the JSON serializing.
    ///     If null, default JSON serializer options will be used.
    /// </param>
    /// <returns>JSON string of the .NET object.</returns>
    public static string SafeToJson<T>(this T? obj, JsonSerializerOptions? options = null)
    {
        var toJson = JsonSerializer.Serialize(obj, options ?? DefaultToJsonOptions);
        return toJson;
    }

    /// <summary>
    ///     Gets a safe ToString method invocation of any .NET object even if the .NET object is null.
    ///     Provides optional parameters to customize the text when the .NET object is null or the .NET object ToString method returned an empty string.
    /// </summary>
    /// <typeparam name="T">Type of object to get a safe <c>ToString</c> representation of.</typeparam>
    /// <param name="obj">.NET object to call extension method on.</param>
    /// <param name="emptyText">
    ///     Optional parameter to set what text should be used if the reference object ToString is indeed empty.
    ///     Defaults to the text '<empty>' if not supplied.
    /// </param>
    /// <param name="nullText">
    ///     Optional parameter to set what text should be used if the reference object or the ToString result is indeed null.
    ///     Defaults to the text '<null>' if not supplied.
    /// </param>
    /// <returns>
    ///     The actual <c>ToString</c> representation if available.
    ///     If the reference type is null or the actual <c>ToString</c> representation is null, then the parameter nullText is returned.
    ///     If the reference type <c>ToString</c> representation is empty, then the parameter emptyText is returned.
    /// </returns>
    public static string SafeToString<T>(this T? obj, string? emptyText = "<empty>", string? nullText = "<null>")
    {
        var toStringResult = obj?.ToString();

        if (toStringResult == null)
            return nullText ?? "<null>";

        if (string.IsNullOrWhiteSpace(toStringResult))
            return emptyText ?? "<empty>";

        return toStringResult;
    }
    #endregion
}
