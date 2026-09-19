// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

namespace Evoogle.Json;

/// <inheritdoc cref="Utf8JsonWriterExtensions"/>
public static partial class Utf8JsonWriterExtensions
{
    #region WriteJsonObject Extension Methods
    /// <summary>
    ///     Writes a JSON object using the provided writer, state, and object-writing action.
    /// </summary>
    /// <typeparam name="TState">The type of the state passed to the writing action.</typeparam>
    /// <param name="writer">The writer to which the object is written.</param>
    /// <param name="state">The state passed to the writing action.</param>
    /// <param name="writeObject">An action that writes the object properties.</param>
    /// <remarks>
    ///     The state parameter allows the callback to receive context without capturing
    ///     variables from the calling scope.
    ///
    ///     <code>
    ///     writer.WriteJsonObject(state, static (w, s) =>
    ///     {
    ///         // Write object properties using the writer and state
    ///     });
    ///     </code>
    /// </remarks>
    public static void WriteJsonObject<TState>
    (
        this Utf8JsonWriter writer,
        TState state,
        Action<Utf8JsonWriter, TState> writeObject
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(writeObject);

        writer.WriteStartObject();
        writeObject(writer, state);
        writer.WriteEndObject();
    }
    #endregion
}
