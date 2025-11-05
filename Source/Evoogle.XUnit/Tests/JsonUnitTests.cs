// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extension;
using Evoogle.Extensions;

using FluentAssertions;

namespace Evoogle.XUnit;

/// <summary>
///     Provides reusable xUnit test harnesses for verifying System.Text.Json serialization, deserialization, and roundtrip behavior for models under test.
/// </summary>
public static class JsonUnitTests
{
    #region Test Classes

    /// <summary>
    ///     Base test harness for JSON converter tests targeting a model of type <typeparamref name="T"/>.
    ///     Supplies common configuration and helper utilities to derived test scenarios.
    /// </summary>
    /// <typeparam name="T">The model type under test for JSON operations.</typeparam>
    public abstract class JsonConverterTestBase<T> : XUnitTest
    {
        #region Default Properties

        /// <summary>
        ///     Gets the default <see cref="JsonSerializerOptions"/> used by tests when no explicit options are supplied.
        /// </summary>
        /// <remarks>
        ///     Defaults are:
        ///     <list type="bullet">
        ///         <item><description><see cref="JsonSerializerOptions.WriteIndented"/>: <see langword="false"/></description></item>
        ///         <item><description><see cref="JsonSerializerOptions.DefaultIgnoreCondition"/>: <see cref="JsonIgnoreCondition.WhenWritingNull"/></description></item>
        ///     </list>
        /// </remarks>
        private static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = new()
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        #endregion

        #region User Supplied Properties

        /// <summary>
        ///     Gets or initializes an optional extension type to attach to the object under test.
        ///     When specified, a new instance of this type is created and attached via <see cref="IExtensible.AttachExtension(Type, object)"/> during <c>Arrange</c>.
        /// </summary>
        /// <remarks>
        ///     The target object must implement <see cref="IExtensible"/> when an extension type is supplied.
        ///     The extension type should be instantiable via a public parameterless constructor.
        /// </remarks>
        public Type? ExtensionType1 { get; init; }

        /// <summary>
        ///     Gets or initializes a second optional extension type to attach to the object under test.
        ///     When specified, a new instance of this type is created and attached via <see cref="IExtensible.AttachExtension(Type, object)"/> during <c>Arrange</c>.
        /// </summary>
        /// <remarks>
        ///     The target object must implement <see cref="IExtensible"/> when an extension type is supplied.
        ///     The extension type should be instantiable via a public parameterless constructor.
        /// </remarks>
        public Type? ExtensionType2 { get; init; }

        /// <summary>
        ///     Gets or initializes the <see cref="JsonSerializerOptions"/> used by the test to serialize and/or deserialize.
        /// </summary>
        /// <value>
        ///     Defaults to <see cref="DefaultJsonSerializerOptions"/> if not explicitly provided.
        /// </value>
        public JsonSerializerOptions JsonSerializerOptions { get; init; } = DefaultJsonSerializerOptions;
        #endregion

        #region Helper Methods

        /// <summary>
        ///     Casts the supplied object to <see cref="IExtensible"/> or throws if not compatible.
        /// </summary>
        /// <param name="obj">The object instance expected to implement <see cref="IExtensible"/>.</param>
        /// <returns>The same instance cast to <see cref="IExtensible"/>.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="obj"/> is <see langword="null"/> or does not implement <see cref="IExtensible"/>.
        /// </exception>
        protected static IExtensible AsExtensible(T? obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Object cannot be null.");
            }

            return obj as IExtensible ?? throw new InvalidOperationException("Object must implement IExtensible.");
        }
        #endregion
    }

    /// <summary>
    ///     Verifies that a JSON string deserializes into an object of type <typeparamref name="T"/> that is equivalent to an expected instance (optionally with extensions attached).
    /// </summary>
    /// <typeparam name="T">The model type under test for deserialization.</typeparam>
    public class JsonDeserializeTest<T> : JsonConverterTestBase<T>
    {
        #region User Supplied Properties

        /// <summary>
        ///     Gets or initializes the JSON source payload to deserialize.
        /// </summary>
        public string? Source { get; init; }

        /// <summary>
        ///     Gets or initializes the expected object resulting from deserialization.
        ///     Optional extensions (see <see cref="JsonConverterTestBase{T}.ExtensionType1"/> and <see cref="JsonConverterTestBase{T}.ExtensionType2"/>) are attached to this instance during <c>Arrange</c>.
        /// </summary>
        public T? Expected { get; init; }
        #endregion

        #region Calculated Properties

        /// <summary>
        ///     Gets or sets the actual deserialized instance created during <c>Act</c>.
        /// </summary>
        private T? Actual { get; set; }
        #endregion

        #region XUnitTest Methods

        /// <summary>
        ///     Prepares the expected instance by optionally attaching configured extensions.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when configured extensions are provided but <see cref="Expected"/> is <see langword="null"/> or does not implement <see cref="IExtensible"/>.
        /// </exception>
        protected override void Arrange()
        {
            this.WriteLine($"Source:   {this.Source.SafeToString().RemoveWhitespace()}");
            this.WriteLine($"Expected: {this.Expected.SafeToString()}");

            if (this.ExtensionType1 != null)
            {
                this.WriteLine();
                this.WriteLine($"Extension 1: {this.ExtensionType1.SafeToName()}");

                var extension1 = Activator.CreateInstance(this.ExtensionType1);
                var expectedAsExtensible = AsExtensible(this.Expected);

                expectedAsExtensible.AttachExtension(this.ExtensionType1, extension1!);
            }

            if (this.ExtensionType2 != null)
            {
                if (this.ExtensionType1 == null)
                {
                    this.WriteLine();
                }
                this.WriteLine($"Extension 2: {this.ExtensionType2.SafeToName()}");

                var extension2 = Activator.CreateInstance(this.ExtensionType2);
                var expectedAsExtensible = AsExtensible(this.Expected);

                expectedAsExtensible.AttachExtension(this.ExtensionType2, extension2!);
            }
        }

        /// <summary>
        ///     Deserializes <see cref="Source"/> into <see cref="Actual"/> using the configured <see cref="JsonConverterTestBase{T}.JsonSerializerOptions"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Source"/> is <see langword="null"/>.</exception>
        /// <exception cref="JsonException">Thrown when the JSON payload is invalid.</exception>
        /// <exception cref="NotSupportedException">Thrown when the target type <typeparamref name="T"/> is not supported.</exception>
        protected override void Act()
        {
            this.Actual = JsonSerializer.Deserialize<T>(this.Source!, this.JsonSerializerOptions);
            this.WriteLine();
            this.WriteLine($"Actual:   {this.Actual.SafeToString()}");
        }

        /// <summary>
        ///     Asserts that the deserialized <see cref="Actual"/> is equivalent to <see cref="Expected"/>.
        /// </summary>
        /// <exception cref="FluentAssertions.Execution.AssertionFailedException">
        ///     Thrown when the objects are not equivalent.
        /// </exception>
        protected override void Assert() => this.Actual.Should().BeEquivalentTo(this.Expected);
        #endregion
    }

    /// <summary>
    ///     Verifies that an object of type <typeparamref name="T"/> can be serialized and then deserialized back to a value equivalent to the original (a JSON roundtrip).
    /// </summary>
    /// <typeparam name="T">The model type under test for roundtrip serialization.</typeparam>
    public class JsonRoundtripTest<T> : JsonConverterTestBase<T>
    {
        #region User Supplied Properties

        /// <summary>
        ///     Gets or initializes the object to serialize and then deserialize.
        ///     Optional extensions (see <see cref="JsonConverterTestBase{T}.ExtensionType1"/> and
        ///     <see cref="JsonConverterTestBase{T}.ExtensionType2"/>) are attached to this instance during <c>Arrange</c>.
        /// </summary>
        public T? Expected { get; init; }
        #endregion

        #region Calculated Properties

        /// <summary>
        ///     Gets or sets the instance produced by the serialize-deserialize cycle during <c>Act</c>.
        /// </summary>
        private T? Actual { get; set; }
        #endregion

        #region XUnitTest Methods

        /// <summary>
        ///     Prepares the expected instance by optionally attaching configured extensions.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when configured extensions are provided but <see cref="Expected"/> is <see langword="null"/> or does not implement <see cref="IExtensible"/>.
        /// </exception>
        protected override void Arrange()
        {
            this.WriteLine($"Expected: {this.Expected.SafeToString()}");

            if (this.ExtensionType1 != null)
            {
                this.WriteLine();
                this.WriteLine($"Extension 1: {this.ExtensionType1.SafeToName()}");

                var extension1 = Activator.CreateInstance(this.ExtensionType1);
                var expectedAsExtensible = AsExtensible(this.Expected);

                expectedAsExtensible.AttachExtension(this.ExtensionType1, extension1!);
            }

            if (this.ExtensionType2 != null)
            {
                if (this.ExtensionType1 == null)
                {
                    this.WriteLine();
                }
                this.WriteLine($"Extension 2: {this.ExtensionType2.SafeToName()}");

                var extension2 = Activator.CreateInstance(this.ExtensionType2);
                var expectedAsExtensible = AsExtensible(this.Expected);

                expectedAsExtensible.AttachExtension(this.ExtensionType2, extension2!);
            }
        }

        /// <summary>
        ///     Serializes <see cref="Expected"/> to JSON and deserializes it back into <see cref="Actual"/> using the configured <see cref="JsonConverterTestBase{T}.JsonSerializerOptions"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the type <typeparamref name="T"/> cannot be serialized.</exception>
        /// <exception cref="JsonException">Thrown when deserialization of the serialized JSON fails.</exception>
        protected override void Act()
        {
            var json = JsonSerializer.Serialize(this.Expected, this.JsonSerializerOptions);
            this.Actual = JsonSerializer.Deserialize<T>(json, this.JsonSerializerOptions);
            this.WriteLine($"Actual:   {this.Actual.SafeToString()}");
        }

        /// <summary>
        ///     Asserts that the roundtripped <see cref="Actual"/> is equivalent to <see cref="Expected"/>.
        /// </summary>
        /// <exception cref="FluentAssertions.Execution.AssertionFailedException">
        ///     Thrown when the objects are not equivalent.
        /// </exception>
        protected override void Assert() => this.Actual.Should().BeEquivalentTo(this.Expected);
        #endregion
    }

    /// <summary>
    ///     Verifies that serializing an object of type <typeparamref name="T"/> yields the expected JSON payload.
    /// </summary>
    /// <typeparam name="T">The model type under test for serialization.</typeparam>
    public class JsonSerializeTest<T> : JsonConverterTestBase<T>
    {
        #region User Supplied Properties

        /// <summary>
        ///     Gets or initializes the source object to serialize.
        ///     Optional extensions (see <see cref="JsonConverterTestBase{T}.ExtensionType1"/> and
        ///     <see cref="JsonConverterTestBase{T}.ExtensionType2"/>) are attached to this instance during <c>Arrange</c>.
        /// </summary>
        public T? Source { get; init; }

        /// <summary>
        ///     Gets or initializes the expected JSON payload.
        /// </summary>
        /// <remarks>
        ///     Comparison ignores all whitespace characters (spaces, tabs, newlines) to reduce noise from formatting.
        /// </remarks>
        public string? Expected { get; init; }
        #endregion

        #region Calculated Properties

        /// <summary>
        ///     Gets or sets the JSON produced during <c>Act</c>.
        /// </summary>
        private string? ActualJson { get; set; }
        #endregion

        #region XUnitTest Methods

        /// <summary>
        ///     Prepares the source instance by optionally attaching configured extensions.
        ///     Writes source and expected payload information to the test output.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when configured extensions are provided but <see cref="Source"/> is <see langword="null"/> or does not implement <see cref="IExtensible"/>.
        /// </exception>
        protected override void Arrange()
        {
            this.WriteLine($"Source:   {this.Source.SafeToString()}");
            this.WriteLine($"Expected: {this.Expected.SafeToString().RemoveWhitespace()}");
            this.WriteLine();

            if (this.ExtensionType1 != null)
            {
                this.WriteLine();
                this.WriteLine($"Extension 1: {this.ExtensionType1.SafeToName()}");

                var extension1 = Activator.CreateInstance(this.ExtensionType1);
                var sourceAsExtensible = AsExtensible(this.Source);

                sourceAsExtensible.AttachExtension(this.ExtensionType1, extension1!);
            }

            if (this.ExtensionType2 != null)
            {
                if (this.ExtensionType1 == null)
                {
                    this.WriteLine();
                }
                this.WriteLine($"Extension 2: {this.ExtensionType2.SafeToName()}");

                var extension2 = Activator.CreateInstance(this.ExtensionType2);
                var sourceAsExtensible = AsExtensible(this.Source);

                sourceAsExtensible.AttachExtension(this.ExtensionType2, extension2!);
            }
        }

        /// <summary>
        ///     Serializes <see cref="Source"/> into <see cref="ActualJson"/> using the configured <see cref="JsonConverterTestBase{T}.JsonSerializerOptions"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the type <typeparamref name="T"/> cannot be serialized.</exception>
        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.Source, this.JsonSerializerOptions);
            this.WriteLine($"Actual:   {this.ActualJson.SafeToString().RemoveWhitespace()}");
        }

        /// <summary>
        ///     Asserts that the produced JSON equals <see cref="Expected"/> when both are normalized by removing all whitespace characters.
        /// </summary>
        /// <exception cref="FluentAssertions.Execution.AssertionFailedException">
        ///     Thrown when the normalized JSON strings are not equal.
        /// </exception>
        protected override void Assert()
        {
            var actualJsonMinusWhitespace = this.ActualJson.RemoveWhitespace();
            var expectedJsonMinusWhitespace = this.Expected.RemoveWhitespace();

            actualJsonMinusWhitespace.Should().Be(expectedJsonMinusWhitespace);
        }
        #endregion
    }
    #endregion
}
