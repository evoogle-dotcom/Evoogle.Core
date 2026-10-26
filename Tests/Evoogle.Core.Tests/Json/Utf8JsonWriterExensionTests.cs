// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Evoogle.Extensions;
using Evoogle.XUnit;
using Evoogle.XUnit.Json;

using FluentAssertions;

namespace Evoogle.Json;

[DynamicLinqType]
public partial class Utf8JsonWriterExensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class WritePropertyTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionActionJsonConverter<Utf8JsonWriter>))]
        public Expression<Action<Utf8JsonWriter>> WritePropertyExpression { get; init; } = null!;

        public string ExpectedJson { get; init; } = null!;
        #endregion

        #region Calculated Properties
        private string ActualJson { get; set; } = null!;
        #endregion

        #region XUnitTest Methods
        protected override void Arrange() => this.WriteLine($"Expected JSON: {this.ExpectedJson.SafeToString()}");

        protected override void Act()
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false });

            writer.WriteStartObject();

            var writeProperty = this.WritePropertyExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.WritePropertyExpression)} into a function object.");
            writeProperty(writer);

            writer.WriteEndObject();
            writer.Flush();

            this.ActualJson = Encoding.UTF8.GetString(stream.ToArray());
            this.WriteLine($"Actual   JSON: {this.ActualJson.SafeToString()}");
        }

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
