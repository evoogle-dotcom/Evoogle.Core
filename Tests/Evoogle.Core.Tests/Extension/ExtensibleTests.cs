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
        public static TestExtensible Create() => new TestExtensible();

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
    }

    public class TestExtension(string name)
    {
        public string Name { get; set; } = name;
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
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TryGetExtensionTheoryData))]
    public void TryGetExtension(IXUnitTest test) => test.Execute(this);
    #endregion
}
