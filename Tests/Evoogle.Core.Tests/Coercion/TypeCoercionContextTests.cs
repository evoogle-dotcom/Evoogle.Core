// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Collections.Frozen;
using System.Globalization;
using System.Reflection;

using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Coercion;

public class TypeCoercionContextTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Types
    private sealed class ImmutableContextTest : XUnitTest
    {
        private TypeCoercionContextBuilder? Builder { get; set; }

        private TypeCoercionContext? Context { get; set; }

        private TypeCoercionContext? ClearedContext { get; set; }

        private CultureInfo? SourceCulture { get; set; }

        private bool? ConcurrentCoercionSucceeded { get; set; }

        private object? Definitions { get; set; }

        protected override void Arrange()
        {
            this.SourceCulture = new CultureInfo("fr-FR");
            this.Builder = new TypeCoercionContextBuilder()
                .SetFormat(typeof(DateTime), "F")
                .SetFormatProvider(typeof(DateTime), this.SourceCulture)
                .SetDateTimeStyles(typeof(DateTime), DateTimeStyles.AssumeUniversal);
        }

        protected override void Act()
        {
            this.Context = this.Builder!.Build();
            this.Builder
                .SetFormat(typeof(DateTime), "G")
                .RemoveFormatProvider(typeof(DateTime))
                .RemoveDateTimeStyles(typeof(DateTime));
            this.SourceCulture!.DateTimeFormat.ShortDatePattern = "yyyy";

            this.ClearedContext = new TypeCoercionContextBuilder()
                .ClearFormats()
                .ClearFormatProviders()
                .ClearDateTimeStyles()
                .Build();

            var coercion = new TypeCoercion();
            this.ConcurrentCoercionSucceeded = Task.WhenAll
            (
                Enumerable.Range(0, 64).Select
                (
                    _ => Task.Run
                    (
                        () => Enumerable.Range(0, 1000).All
                        (
                            value => coercion.Coerce<int, string>(value, this.Context) ==
                                value.ToString(CultureInfo.InvariantCulture)
                        )
                    )
                )
            ).GetAwaiter().GetResult().All(result => result);

            this.Definitions = typeof(TypeCoercion)
                .GetField("_definitions", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(coercion);
        }

        protected override void Assert()
        {
            TypeCoercionContext.Default.FormatMapping.Should().NotBeNull();
            TypeCoercionContext.Default.FormatMapping.Should().BeAssignableTo<FrozenDictionary<Type, string>>();

            this.Context!.GetFormat(typeof(DateTime)).Should().Be("F");
            this.Context.GetDateTimeStyles(typeof(DateTime)).Should().Be(DateTimeStyles.AssumeUniversal);
            var culture = this.Context.GetFormatProvider(typeof(DateTime)).Should().BeOfType<CultureInfo>().Subject;
            culture.IsReadOnly.Should().BeTrue();
            culture.Should().NotBeSameAs(this.SourceCulture);
            culture.DateTimeFormat.ShortDatePattern.Should().NotBe("yyyy");

            this.ClearedContext!.FormatMapping.Should().BeEmpty();
            this.ClearedContext.FormatProviderMapping.Should().BeEmpty();
            this.ClearedContext.DateTimeStylesMapping.Should().BeEmpty();
            this.ConcurrentCoercionSucceeded.Should().BeTrue();
            this.Definitions.Should().BeAssignableTo
            <
                FrozenDictionary<Tuple<Type, Type>, ITypeCoercionDefinition>
            >();
        }
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] ImmutableContextTheoryData =>
    [
        new ImmutableContextTest
        {
            Name = "Coercion Context And Definitions Are Frozen For Concurrent Reads"
        }
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(ImmutableContextTheoryData))]
    public void ImmutableContext(IXUnitTest test) => test.Execute(this);
    #endregion
}
