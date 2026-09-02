// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

using Evoogle.XUnit;
using Evoogle.XUnit.Json;

using FluentAssertions;

namespace Evoogle.Extension;

public class ExtensibleTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    [DynamicLinqType]
    public class TestExtensible : ExtensibleBase
    {
        public static TestExtensible Create() => new();

        public static void AttachExtension(TestExtensible testExtensible, string? name) =>
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            testExtensible.AttachExtension(name != null ? new TestExtension(name) : null);
#pragma warning restore CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.


        public static void AttachAndDetachExtension(TestExtensible testExtensible, string? name)
        {
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            testExtensible.AttachExtension(name != null ? new TestExtension(name) : null);
#pragma warning restore CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            testExtensible.DetachExtension<TestExtension>();
        }

        public void Freeze() => this.FreezeExtensions();
    }

    public class TestExtension
    {
        public TestExtension()
            : this("default")
        {
        }

        public TestExtension(string name) => this.Name = name;

        public string Name { get; set; }
    }

    public class SecondTestExtension(string name)
    {
        public string Name { get; set; } = name;
    }

    public class FrozenExtensibleTest : XUnitTest
    {
        private TestExtensible? Extensible { get; set; }

        private IReadOnlyList<Type>? ExtensionTypes { get; set; }

        private bool? EmptyExtensionsWasNonNull { get; set; }

        private bool? ConcurrentLookupSucceeded { get; set; }

        private List<Exception> MutationExceptions { get; } = [];

        protected override void Arrange()
        {
            this.Extensible = new TestExtensible();
            this.EmptyExtensionsWasNonNull = this.Extensible.Extensions is not null &&
                this.Extensible.ExtensionCount == 0;
            this.Extensible.AttachExtension(typeof(TestExtension), new TestExtension("first"));
            this.Extensible.AttachExtension(typeof(SecondTestExtension), new SecondTestExtension("second"));
            this.Extensible.Freeze();
        }

        protected override void Act()
        {
            this.ExtensionTypes = [.. this.Extensible!.Extensions.Keys];
            this.ConcurrentLookupSucceeded = Task.WhenAll
            (
                Enumerable.Range(0, 64).Select
                (
                    _ => Task.Run
                    (
                        () => Enumerable.Range(0, 1000).All
                        (
                            _ => this.Extensible.TryGetExtension<TestExtension>(out var extension) &&
                                extension.Name == "first"
                        )
                    )
                )
            ).GetAwaiter().GetResult().All(result => result);

            Capture(() => this.Extensible.AttachExtension(typeof(Uri), new Uri("https://example.com")));
            Capture(() => this.Extensible.DetachExtension<TestExtension>());
            Capture(() => this.Extensible.CreateExtension<TestExtension>());
            Capture(() => this.Extensible.GetOrAttachExtension<TestExtension>());
            Capture(() => this.Extensible.ModifyExtension<TestExtension>(extension => extension.Name = "changed"));

            void Capture(Action action)
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    this.MutationExceptions.Add(exception);
                }
            }
        }

        protected override void Assert()
        {
            this.EmptyExtensionsWasNonNull.Should().BeTrue();
            this.Extensible!.Extensions.Should().NotBeNull();
            this.Extensible.ExtensionCount.Should().Be(2);
            this.ExtensionTypes.Should().Equal(typeof(TestExtension), typeof(SecondTestExtension));
            this.ConcurrentLookupSucceeded.Should().BeTrue();
            this.MutationExceptions.Should().HaveCount(5)
                .And.OnlyContain(exception => exception is InvalidOperationException);
            this.Extensible.TryGetExtension<TestExtension>(out var extension).Should().BeTrue();
            extension!.Name.Should().Be("first");
        }
    }

    public class ExtensibleMutateTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestExtensible>))]
        public Expression<Func<TestExtensible>> ExtensibleFactoryExpression { get; init; } = null!;

        [JsonConverter(typeof(ExpressionActionJsonConverter<TestExtensible>))]
        public Expression<Action<TestExtensible>>? ExtensibleMutateExpression { get; init; }

        public bool ExpectedResult { get; init; }
        public bool ExpectedArgumentNullExceptionThrown { get; init; }

        public TestExtension? ExpectedExtension { get; init; }
        #endregion

        #region Calculated Properties
        private bool? ActualResult { get; set; }
        private bool? ActualArgumentNullExceptionThrown { get; set; }

        private TestExtension? ActualExtension { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
        }

        protected override void Act()
        {
            // Create extensible object
            var extensibleFactoryFunc = this.ExtensibleFactoryExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.ExtensibleFactoryExpression)} into a function object.");
            var extensible = extensibleFactoryFunc();

            // Try and get extension
            try
            {
                // Mutate extensible object
                if (this.ExtensibleMutateExpression != null)
                {
                    var extensibleMutateAction = this.ExtensibleMutateExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.ExtensibleMutateExpression)} into a function object.");
                    extensibleMutateAction(extensible);
                }

                var actualResult = extensible.TryGetExtension<TestExtension>(out var actualExtension);

                this.ActualResult = actualResult;
                this.ActualExtension = actualResult ? actualExtension : null;
            }
            catch (ArgumentNullException)
            {
                this.ActualArgumentNullExceptionThrown = true;
            }
        }

        protected override void Assert()
        {
            if (this.ActualArgumentNullExceptionThrown.HasValue)
            {
                this.ActualArgumentNullExceptionThrown.Value.Should().Be(this.ExpectedArgumentNullExceptionThrown);
                return;
            }

            this.ActualResult.Should().Be(this.ExpectedResult);
            if (this.ActualResult == false)
            {
                return;
            }

            this.AssertOutputForSuccessResult();
        }
        #endregion

        #region Implementation Methods
        private void AssertOutputForSuccessResult()
        {
            if (this.ExpectedExtension == null)
            {
                this.ActualExtension.Should().BeNull();
                return;
            }

            this.ActualExtension.Should().BeEquivalentTo(this.ExpectedExtension);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] TryGetExtensionTheoryData =>
    [
        new ExtensibleMutateTest
        {
            Name = "Nothing Attached TryGetExtension Returns False",
            ExtensibleFactoryExpression = () => TestExtensible.Create(),
            ExtensibleMutateExpression = null,
            ExpectedResult = false,
            ExpectedExtension = null
        },

        new ExtensibleMutateTest
        {
            Name = "Extension Attached TryGetExtension Returns True",
            ExtensibleFactoryExpression = () => TestExtensible.Create(),
            ExtensibleMutateExpression = (a) => TestExtensible.AttachExtension(a, "42"),
            ExpectedResult = true,
            ExpectedExtension = new TestExtension("42")
        },

        new ExtensibleMutateTest
        {
            Name = "Extension Attached/Detached TryGetExtension Returns False",
            ExtensibleFactoryExpression = () => TestExtensible.Create(),
            ExtensibleMutateExpression = (a) => TestExtensible.AttachAndDetachExtension(a, "42"),
            ExpectedResult = false,
            ExpectedExtension = null
        },

        new ExtensibleMutateTest
        {
            Name = "Null Extension Attached TryGetExtension Throws NullArgumentException",
            ExtensibleFactoryExpression = () => TestExtensible.Create(),
            ExtensibleMutateExpression = (a) => TestExtensible.AttachExtension(a, null),
            ExpectedArgumentNullExceptionThrown = true
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] FrozenExtensibleTheoryData =>
    [
        new FrozenExtensibleTest
        {
            Name = "Frozen Extensions Preserve Order And Support Concurrent Reads"
        }
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TryGetExtensionTheoryData))]
    public void TryGetExtension(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(FrozenExtensibleTheoryData))]
    public void FrozenExtensible(IXUnitTest test) => test.Execute(this);
    #endregion
}
