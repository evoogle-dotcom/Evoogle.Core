// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

using Xunit.Abstractions;

namespace Evoogle;

public class DotNetDictionaryExtensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class GetValueUnitTest<TKey, TValue> : XUnitTest
    {
        #region User Supplied Properties
        public IDictionary<TKey, TValue> Dictionary { get; set; } = null!;
        public TKey Key { get; set; } = default!;
        public TValue? ExpectedValue { get; set; }
        public bool ExpectedExceptionThrown { get; set; }
        #endregion

        #region Calculated Properties
        private TValue? ActualValue { get; set; }
        private bool ActualExceptionThrown { get; set; }
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            this.WriteLine("Expected");
            this.WriteLine($"  Value           = {this.ExpectedValue.SafeToString()}");
            this.WriteLine($"  ExceptionThrown = {this.ExpectedExceptionThrown}");
            this.WriteLine();
        }

        protected override void Act()
        {
            this.ActualExceptionThrown = false;
            try
            {
                this.ActualValue = this.Dictionary.GetValue(this.Key);
            }
            catch (Exception)
            {
                this.ActualExceptionThrown = true;
                this.ActualValue = default;
            }

            this.WriteLine("Actual");
            this.WriteLine($"  Value           = {this.ActualValue.SafeToString()}");
            this.WriteLine($"  ExceptionThrown = {this.ActualExceptionThrown}");
        }

        protected override void Assert()
        {
            this.ActualExceptionThrown.Should().Be(this.ExpectedExceptionThrown);
            this.ActualValue.Should().BeEquivalentTo(this.ExpectedValue);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryData<IXUnitTest> GetValueTheoryData => new()
    {
        { new GetValueUnitTest<int, int> { Name = "With IntToInt Dictionary And Existing Key", Dictionary = new Dictionary<int, int> { { 24, 42 } }, Key = 24, ExpectedValue = 42, ExpectedExceptionThrown = false } },
        { new GetValueUnitTest<int, int> { Name = "With IntToInt Dictionary And Non Existing Key", Dictionary = new Dictionary<int, int> { { 24, 42 } }, Key = 68, ExpectedValue = default, ExpectedExceptionThrown = true } },

        { new GetValueUnitTest<string, string> { Name = "With StringToString Dictionary And Existing Key", Dictionary = new Dictionary<string, string> { { "24", "42" } }, Key = "24", ExpectedValue = "42", ExpectedExceptionThrown = false } },
        { new GetValueUnitTest<string, string> { Name = "With StringToString Dictionary And Non Existing Key", Dictionary = new Dictionary<string, string> { { "24", "42" } }, Key = "68", ExpectedValue = default, ExpectedExceptionThrown = true } },
    };
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(GetValueTheoryData))]
    public void TestGetValue(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}