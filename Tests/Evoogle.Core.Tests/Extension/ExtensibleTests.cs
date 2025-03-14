// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

using Evoogle.Json;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.Extension;

public class ExtensibleTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    [DynamicLinqType]
    public class TestExtensible : ExtensibleBase
    {
        public static TestExtensible Create()
        {
            return new TestExtensible();
        }

        public static void AttachExtension(TestExtensible testExtensible, string? name)
        {
            testExtensible.AttachExtension(name != null ? new TestExtension(name) : null);
        }

        public static void AttachAndDetachExtension(TestExtensible testExtensible, string? name)
        {
            testExtensible.AttachExtension(name != null ? new TestExtension(name) : null);
            testExtensible.DetachExtension<TestExtension>();
        }
    }

    public class TestExtension(string name)
    {
        public string Name { get; set; } = name;
    }

    public class ExtensibleMutateTest : XUnitTest
    {
        #region Calculated Properties
        private bool ActualResult { get; set; }
        private bool ActualArgumentNullExceptionThrown { get; set; }

        private TestExtension? ActualExtension { get; set; }
        #endregion

        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestExtensible>))]
        public Expression<Func<TestExtensible>> ExtensibleFactoryExpression { get; set; } = null!;

        [JsonConverter(typeof(ExpressionActionJsonConverter<TestExtensible>))]
        public Expression<Action<TestExtensible>>? ExtensibleMutateExpression { get; set; }

        public bool ExpectedResult { get; set; }
        public bool ExpectedArgumentNullExceptionThrown { get; set; }

        public TestExtension? ExpectedExtension { get; set; }
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

                this.ActualResult = extensible.TryGetExtension<TestExtension>(out var actualExtension);
                this.ActualExtension = this.ActualResult ? actualExtension : null;
            }
            catch (ArgumentNullException)
            {
                this.ActualArgumentNullExceptionThrown = true;
            }
        }

        protected override void Assert()
        {
            this.ActualArgumentNullExceptionThrown.Should().Be(this.ExpectedArgumentNullExceptionThrown);
            if (this.ActualArgumentNullExceptionThrown == true)
                return;

            this.ActualResult.Should().Be(this.ExpectedResult);
            if (this.ActualResult == false)
                return;

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
    public void TryGetExtension(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}