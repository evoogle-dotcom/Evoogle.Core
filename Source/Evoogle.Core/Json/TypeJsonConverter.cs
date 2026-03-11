// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Reflection;

namespace Evoogle.Json;

/// <summary>
///     A custom JSON converter for serializing and deserializing .NET <see cref="Type"/> objects.
/// </summary>
/// <remarks>
///     Supports filtering deserialization using per-instance namespace and assembly whitelists.
///     This converter allows for the conversion of <see cref="Type"/> instances to and from JSON strings, using a compact, fully-qualified type name representation.
/// </remarks>
public class TypeJsonConverter : JsonConverter<Type>
{
    #region Fields
    private readonly HashSet<Assembly> _allowedAssemblies = [];

    private readonly List<string> _allowedNamespacePrefixes = [];
    #endregion

    #region Properties
    /// <summary>
    ///     Gets the set of allowed <see cref="Assembly"/> instances for deserialization filtering.
    ///     If empty, no assembly filtering is applied.
    /// </summary>
    public IEnumerable<Assembly> AllowedAssemblies => _allowedAssemblies;

    /// <summary>
    ///     Gets the list of allowed namespace prefixes for deserialization filtering.
    ///     If empty, no namespace filtering is applied.
    /// </summary>
    public IEnumerable<string> AllowedNamespaces => _allowedNamespacePrefixes;
    #endregion

    #region Fluent Configuration Methods
    /// <summary>
    ///     Adds an allowed assembly to the deserialization filter.
    /// </summary>
    /// <param name="assembly">The assembly to allow.</param>
    /// <returns>The current <see cref="TypeJsonConverter"/> instance.</returns>
    public TypeJsonConverter AddAllowedAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        _allowedAssemblies.Add(assembly);
        return this;
    }

    /// <summary>
    ///     Adds the assembly of a given type <typeparamref name="T"/> to the allowlist.
    /// </summary>
    public TypeJsonConverter AddAllowedAssemblyOf<T>() =>
        this.AddAllowedAssembly(typeof(T).Assembly);

    /// <summary>
    ///     Replaces the set of allowed assemblies with the given collection.
    /// </summary>
    public TypeJsonConverter SetAllowedAssemblies(IEnumerable<Assembly> assemblies)
    {
        _allowedAssemblies.Clear();
        foreach (var asm in assemblies)
        {
            this.AddAllowedAssembly(asm);
        }

        return this;
    }

    /// <summary>
    ///     Adds an allowed namespace prefix for deserialization filtering.
    /// </summary>
    /// <param name="prefix">The namespace prefix (e.g., "MyApp.Domain.").</param>
    /// <returns>The current <see cref="TypeJsonConverter"/> instance.</returns>
    public TypeJsonConverter AddAllowedNamespace(string prefix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);
        _allowedNamespacePrefixes.Add(prefix);
        return this;
    }

    /// <summary>
    ///     Replaces the set of allowed namespace prefixes.
    /// </summary>
    /// <param name="prefixes">The new list of prefixes.</param>
    public TypeJsonConverter SetAllowedNamespaces(IEnumerable<string> prefixes)
    {
        _allowedNamespacePrefixes.Clear();
        _allowedNamespacePrefixes.AddRange(prefixes);
        return this;
    }
    #endregion

    #region JsonConverter Methods
    /// <summary>
    ///     Deserializes a JSON string into a .NET <see cref="Type"/> object.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader providing the string value.</param>
    /// <param name="typeToConvert">The type of object to convert to (in this case, <see cref="Type"/>).</param>
    /// <param name="options">Options for the JSON serializer.</param>
    /// <returns>The deserialized <see cref="Type"/> object.</returns>
    /// <exception cref="JsonException">Thrown when the JSON string is null or cannot be resolved to a valid .NET type.</exception>
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new JsonException("Cannot deserialize .NET type because the input string was null or empty.");
        }

        var type = GetDeserializeType(typeName);

        if (!this.IsAllowed(type))
        {
            throw new JsonException($"Type '{type.FullName}' is not allowed for deserialization.");
        }

        return type;
    }

    /// <summary>
    ///     Serializes a .NET <see cref="Type"/> object into a JSON string.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer to output the string.</param>
    /// <param name="type">The <see cref="Type"/> object to serialize.</param>
    /// <param name="options">Options for the JSON serializer.</param>
    public override void Write(Utf8JsonWriter writer, Type type, JsonSerializerOptions options)
    {
        var typeName = GetSerializeTypeName(type);
        writer.WriteStringValue(typeName);
    }
    #endregion

    #region Utility Methods
    /// <summary>
    ///     Deserializes a string representation of a .NET type into its corresponding <see cref="Type"/> object.
    /// </summary>
    /// <param name="typeName">The string representation of the type, typically a compact qualified name (e.g., "System.String").</param>
    /// <returns>The <see cref="Type"/> object corresponding to the provided <paramref name="typeName"/>.</returns>
    /// <exception cref="JsonException">Thrown when the <paramref name="typeName"/> cannot be resolved to a valid .NET type.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeName"/> is null (handled internally by <see cref="Type.GetType(string)"/>).</exception>
    /// <remarks>
    ///     This method uses <see cref="Type.GetType(string, bool)"/> to resolve the type name. The <paramref name="typeName"/>
    ///     should match the format produced by <see cref="GetSerializeTypeName(Type)"/> to ensure successful roundtrip conversion.
    /// </remarks>
    public static Type GetDeserializeType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new JsonException("The JSON type name was null or whitespace.");
        }

        var resolvedType = Type.GetType(typeName, throwOnError: false) ?? throw new JsonException($"Unable to resolve .NET type from name: '{typeName}'.");
        return resolvedType;
    }

    /// <summary>
    ///     Serializes a .NET <see cref="Type"/> object into a string representation suitable for roundtrip deserialization.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to serialize into a string.</param>
    /// <returns>A compact qualified name of the <paramref name="type"/> (e.g., "System.String").</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
    /// <remarks>
    ///     This method relies on <see cref="TypeReflection.GetCompactQualifiedName(Type)"/> to produce a compact,
    ///     fully-qualified type name that can be deserialized back into the original <see cref="Type"/> using <see cref="GetDeserializeType(string)"/>.
    /// </remarks>
    public static string GetSerializeTypeName(Type type)
    {
        var typeCompactQualilfiedName = TypeReflection.GetCompactQualifiedName(type);
        return typeCompactQualilfiedName;
    }

    /// <summary>
    ///     Determines if a type passes the namespace and assembly allowlists.
    /// </summary>
    /// <param name="type">The type to evaluate.</param>
    /// <returns><c>true</c> if allowed; otherwise, <c>false</c>.</returns>
    private bool IsAllowed(Type type)
    {
        var ns = type.Namespace ?? string.Empty;
        var assembly = type.Assembly;

        var namespaceMatch = _allowedNamespacePrefixes.Count == 0 ||
            _allowedNamespacePrefixes.Any(prefix =>
                ns.StartsWith(prefix, StringComparison.Ordinal));

        var assemblyMatch = _allowedAssemblies.Count == 0 ||
            _allowedAssemblies.Contains(assembly);

        return namespaceMatch && assemblyMatch;
    }
    #endregion
}
