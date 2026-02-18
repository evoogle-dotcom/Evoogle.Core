// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extensions;

using FluentAssertions;

namespace Evoogle.XUnit.Tests;

/// <summary>
///     Provides reusable xUnit test harnesses for verifying System.Text.Json serialization, deserialization, and roundtrip behavior for models under test.
/// </summary>
public static class JsonUnitTests
{
    #region Test Classes
    /// <summary>
    ///     Base class for JSON serialization/deserialization tests providing common configuration and infrastructure.
    /// </summary>
    /// <typeparam name="T">The type being tested for JSON serialization and/or deserialization.</typeparam>
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
        ///     Gets or initializes the <see cref="JsonSerializerOptions"/> used by the test to serialize and/or deserialize.
        /// </summary>
        /// <value>
        ///     Defaults to <see cref="DefaultJsonSerializerOptions"/> if not explicitly provided.
        /// </value>
        public JsonSerializerOptions JsonSerializerOptions { get; init; } = DefaultJsonSerializerOptions;
        #endregion
    }

    /// <summary>
    ///     Test harness for verifying JSON deserialization behavior.
    ///     Deserializes a JSON string and compares the result against an expected object created via a factory expression.
    /// </summary>
    /// <typeparam name="T">The type to deserialize from JSON.</typeparam>
    /// <typeparam name="TFactoryArg">The type of argument passed to the factory expression to create the expected object.</typeparam>
    public class JsonDeserializeTest<T, TFactoryArg> : JsonConverterTestBase<T>
    {
        #region User Supplied Properties
        /// <summary>
        ///     Gets the JSON string to deserialize.
        /// </summary>
        public required string? SourceJson { get; init; }

        /// <summary>
        ///     Gets the argument to pass to the factory expression that creates the expected object.
        /// </summary>
        public required TFactoryArg? ExpectedFactoryArgument { get; init; }

        /// <summary>
        ///     Gets the factory expression that creates the expected object for comparison.
        /// </summary>
        public required Expression<Func<TFactoryArg?, T?>> ExpectedFactoryExpression { get; init; }

        /// <summary>
        ///     Gets an optional list of member names to exclude from the equivalence comparison.
        ///     If <see langword="null"/> or empty, all members will be compared.
        /// </summary>
        public List<string>? ExcludeMembers { get; init; } = null;
        #endregion

        #region Calculated Properties
        private T? Expected { get; set; }
        private T? Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            var expectedFactoryFunc = this.ExpectedFactoryExpression.Compile();
            var expected = expectedFactoryFunc(this.ExpectedFactoryArgument);
            this.Expected = expected;

            this.WriteLine($"Source JSON:\n{this.SourceJson.SafeToString().RemoveWhitespace()}");
            this.WriteLine();
            this.WriteLine($"Expected: {this.Expected.SafeToString()}");
        }

        /// <summary>
        ///     Acts on the test by deserializing the source JSON string.
        /// </summary>
        protected override void Act()
        {
            this.Actual = JsonSerializer.Deserialize<T>(this.SourceJson!, this.JsonSerializerOptions);
            this.WriteLine();
            this.WriteLine($"Actual:   {this.Actual.SafeToString()}");
        }

        /// <summary>
        ///     Asserts that the actual deserialized object is equivalent to the expected object,
        ///     optionally excluding specified members from the comparison.
        /// </summary>
        protected override void Assert()
        {
            if (this.ExcludeMembers == null || this.ExcludeMembers.Count == 0)
            {
                this.Actual.Should().BeEquivalentTo(this.Expected);
                return;
            }

            var excludeMembersSet = new HashSet<string>(this.ExcludeMembers);
            this.Actual.Should().BeEquivalentTo(this.Expected, opt => opt.Excluding(info => excludeMembersSet.Contains(info.Path)));
        }
        #endregion
    }

    /// <summary>
    ///     Test harness for verifying JSON roundtrip serialization and deserialization behavior.
    ///     Serializes an object to JSON, then deserializes it back and compares the result to the original.
    /// </summary>
    /// <typeparam name="T">The type to serialize and deserialize.</typeparam>
    /// <typeparam name="TFactoryArg">The type of argument passed to the factory expression to create the original object.</typeparam>
    public class JsonRoundtripTest<T, TFactoryArg> : JsonConverterTestBase<T>
    {
        #region User Supplied Properties
        /// <summary>
        ///     Gets the argument to pass to the factory expression that creates the original object.
        /// </summary>
        public required TFactoryArg? ExpectedFactoryArgument { get; init; }

        /// <summary>
        ///     Gets the factory expression that creates the original object for serialization and comparison.
        /// </summary>
        public required Expression<Func<TFactoryArg?, T?>> ExpectedFactoryExpression { get; init; }

        /// <summary>
        ///     Gets an optional list of member names to exclude from the equivalence comparison.
        ///     If <see langword="null"/> or empty, all members will be compared.
        /// </summary>
        public List<string>? ExcludeMembers { get; init; } = null;
        #endregion

        #region Calculated Properties
        /// <summary>
        ///     Gets or sets the expected object (the original before roundtrip).
        /// </summary>
        private T? Expected { get; set; }

        /// <summary>
        ///     Gets or sets the actual object after roundtrip serialization and deserialization.
        /// </summary>
        private T? Actual { get; set; }
        #endregion

        #region XUnitTest Methods
        /// <summary>
        ///     Arranges the test by creating the original object using the factory expression.
        /// </summary>
        protected override void Arrange()
        {
            var expectedFactoryFunc = this.ExpectedFactoryExpression.Compile();
            var expected = expectedFactoryFunc(this.ExpectedFactoryArgument);
            this.Expected = expected;

            this.WriteLine($"Expected: {this.Expected.SafeToString()}");
            this.WriteLine();
        }

        /// <summary>
        ///     Acts on the test by performing a roundtrip: serializing the original object to JSON,
        ///     then deserializing it back to an object.
        /// </summary>
        protected override void Act()
        {
            var json = JsonSerializer.Serialize(this.Expected, this.JsonSerializerOptions);
            this.Actual = JsonSerializer.Deserialize<T>(json, this.JsonSerializerOptions);
            this.WriteLine($"Actual:   {this.Actual.SafeToString()}");
        }

        /// <summary>
        ///     Asserts that the roundtripped object is equivalent to the original,
        ///     optionally excluding specified members from the comparison.
        /// </summary>
        protected override void Assert()
        {
            if (this.ExcludeMembers == null || this.ExcludeMembers.Count == 0)
            {
                this.Actual.Should().BeEquivalentTo(this.Expected);
                return;
            }

            var excludeMembersSet = new HashSet<string>(this.ExcludeMembers);
            this.Actual.Should().BeEquivalentTo(this.Expected, opt => opt.Excluding(info => excludeMembersSet.Contains(info.Path)));
        }
        #endregion
    }

    /// <summary>
    ///     Test harness for verifying JSON serialization behavior.
    ///     Serializes an object to JSON and compares the result against an expected JSON string.
    /// </summary>
    /// <typeparam name="T">The type to serialize to JSON.</typeparam>
    /// <typeparam name="TFactoryArg">The type of argument passed to the factory expression to create the source object.</typeparam>
    public class JsonSerializeTest<T, TFactoryArg> : JsonConverterTestBase<T>
    {
        #region User Supplied Properties
        /// <summary>
        ///     Gets the argument to pass to the factory expression that creates the source object to serialize.
        /// </summary>
        public required TFactoryArg? SourceFactoryArgument { get; init; }

        /// <summary>
        ///     Gets the factory expression that creates the source object to serialize.
        /// </summary>
        public required Expression<Func<TFactoryArg?, T?>> SourceFactoryExpression { get; init; }

        /// <summary>
        ///     Gets the expected JSON string after serialization.
        /// </summary>
        public required string? ExpectedJson { get; init; }
        #endregion

        #region Calculated Properties
        /// <summary>
        ///     Gets or sets the source object to serialize.
        /// </summary>
        private T? Source { get; set; }

        /// <summary>
        ///     Gets or sets the actual JSON string produced by serialization.
        /// </summary>
        private string? ActualJson { get; set; }
        #endregion

        #region XUnitTest Methods
        /// <summary>
        ///     Arranges the test by creating the source object using the factory expression.
        /// </summary>
        protected override void Arrange()
        {
            var sourceFactoryFunc = this.SourceFactoryExpression.Compile();
            var source = sourceFactoryFunc(this.SourceFactoryArgument);
            this.Source = source;

            this.WriteLine($"Source: {this.Source.SafeToString()}");
            this.WriteLine();
            this.WriteLine($"Expected JSON:\n{this.ExpectedJson.SafeToString().RemoveWhitespace()}");
            this.WriteLine();
        }

        /// <summary>
        ///     Acts on the test by serializing the source object to JSON.
        /// </summary>
        protected override void Act()
        {
            this.ActualJson = JsonSerializer.Serialize(this.Source, this.JsonSerializerOptions);
            this.WriteLine($"Actual JSON:\n{this.ActualJson.SafeToString().RemoveWhitespace()}");
        }

        /// <summary>
        ///     Asserts that the actual serialized JSON matches the expected JSON,
        ///     comparing without whitespace.
        /// </summary>
        protected override void Assert()
        {
            var actualJsonMinusWhitespace = this.ActualJson.RemoveWhitespace();
            var expectedJsonMinusWhitespace = this.ExpectedJson.RemoveWhitespace();

            actualJsonMinusWhitespace.Should().Be(expectedJsonMinusWhitespace);
        }
        #endregion
    }
    #endregion
}
