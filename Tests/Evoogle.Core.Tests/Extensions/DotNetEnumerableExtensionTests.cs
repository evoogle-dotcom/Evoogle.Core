// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle;

public class DotNetEnumerableExtensionTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Types
    private class EmptyIfNullUnitTest<T> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<T>? Original { get; set; }
        public IEnumerable<T> Expected { get; set; } = null!;
        #endregion

        #region Calculated Properties
        private IEnumerable<T> Actual { get; set; } = null!;
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            var expectedJson = this.Expected.SafeToJson();
            this.WriteLine($"Expected IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(expectedJson);
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.EmptyIfNull();

            var actualJson = this.Actual.SafeToJson();
            this.WriteLine($"Actual IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(actualJson);
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }

    private class IsNullOrEmptyUnitTest<T> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<T>? Original { get; set; }
        public bool Expected { get; set; }
        #endregion

        #region Calculated Properties
        private bool Actual { get; set; }
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            this.WriteLine($"Expected IsNullOrEmpty: {this.Expected}");
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.IsNullOrEmpty();

            this.WriteLine($"Actual IsNullOrEmpty: {this.Actual}");
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().Be(expected);
        }
        #endregion
    }

    private class SafeCastUnitTest<TFrom, TTo> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<TFrom>? Original { get; set; }
        public IEnumerable<TTo> Expected { get; set; } = null!;
        #endregion

        #region Calculated Properties
        private IEnumerable<TTo> Actual { get; set; } = null!;
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(TFrom).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            var expectedJson = this.Expected.SafeToJson();
            this.WriteLine($"Expected IEnumerable<{typeof(TTo).Name}> as JSON");
            this.WriteLine(expectedJson);
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.SafeCast<TTo>();

            var actualJson = this.Actual.SafeToJson();
            this.WriteLine($"Actual IEnumerable<{typeof(TTo).Name}> as JSON");
            this.WriteLine(actualJson);
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }

    private class SafeToArrayUnitTest<T> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<T>? Original { get; set; }
        public T[] Expected { get; set; } = null!;
        #endregion

        #region Calculated Properties
        private T[] Actual { get; set; } = null!;
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            var expectedJson = this.Expected.SafeToJson();
            this.WriteLine($"Expected <{typeof(T).Name}> [] as JSON");
            this.WriteLine(expectedJson);
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.SafeToArray();

            var actualJson = this.Actual.SafeToJson();
            this.WriteLine($"Actual {typeof(T).Name} [] as JSON");
            this.WriteLine(actualJson);
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }

    private class SafeToListUnitTest<T> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<T>? Original { get; set; }
        public IList<T> Expected { get; set; } = null!;
        #endregion

        #region Calculated Properties
        private IList<T> Actual { get; set; } = null!;
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            var expectedJson = this.Expected.SafeToJson();
            this.WriteLine($"Expected List<{typeof(T).Name}> as JSON");
            this.WriteLine(expectedJson);
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.SafeToList();

            var actualJson = this.Actual.SafeToJson();
            this.WriteLine($"Actual List<{typeof(T).Name}> as JSON");
            this.WriteLine(actualJson);
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }

    private class SafeToReadOnlyCollectionUnitTest<T> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<T>? Original { get; set; }
        public IReadOnlyCollection<T> Expected { get; set; } = null!;
        #endregion

        #region Calculated Properties
        private IReadOnlyCollection<T> Actual { get; set; } = null!;
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            var expectedJson = this.Expected.SafeToJson();
            this.WriteLine($"Expected IReadOnlyCollection<{typeof(T).Name}> as JSON");
            this.WriteLine(expectedJson);
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.SafeToReadOnlyCollection();

            var actualJson = this.Actual.SafeToJson();
            this.WriteLine($"Actual IReadOnlyCollection<{typeof(T).Name}> as JSON");
            this.WriteLine(actualJson);
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }

    private class SafeToReadOnlyListUnitTest<T> : XUnitTest
    {
        #region User Supplied Properties
        public IEnumerable<T>? Original { get; set; }
        public IReadOnlyList<T> Expected { get; set; } = null!;
        #endregion

        #region Calculated Properties
        private IReadOnlyList<T> Actual { get; set; } = null!;
        #endregion

        #region XUnitTest Overrides
        protected override void Arrange()
        {
            var originalJson = this.Original.SafeToJson();
            this.WriteLine($"Original IEnumerable<{typeof(T).Name}> as JSON");
            this.WriteLine(originalJson);
            this.WriteLine();

            var expectedJson = this.Expected.SafeToJson();
            this.WriteLine($"Expected IReadOnlyList<{typeof(T).Name}> as JSON");
            this.WriteLine(expectedJson);
            this.WriteLine();
        }

        protected override void Act()
        {
            this.Actual = this.Original!.SafeToReadOnlyList();

            var actualJson = this.Actual.SafeToJson();
            this.WriteLine($"Actual IReadOnlyList<{typeof(T).Name}> as JSON");
            this.WriteLine(actualJson);
            this.WriteLine();
        }

        protected override void Assert()
        {
            var expected = this.Expected;
            var actual = this.Actual;

            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] EmptyIfNullTheoryData =>
    [
        new EmptyIfNullUnitTest<string> { Name = "With Null Collection",      Original = default,                              Expected = Enumerable.Empty<string>() },
        new EmptyIfNullUnitTest<string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = Enumerable.Empty<string>() },
        new EmptyIfNullUnitTest<string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = ["String 1", "String 2", "String 3"] },
    ];

    public static TheoryDataRow<IXUnitTest>[] IsNullOrEmptyTheoryData =>
    [
        new IsNullOrEmptyUnitTest<string> { Name = "With Null Collection",      Original = default,                              Expected = true },
        new IsNullOrEmptyUnitTest<string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = true },
        new IsNullOrEmptyUnitTest<string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = false },
    ];

    public static TheoryDataRow<IXUnitTest>[] SafeCastTheoryData =>
    [
        new SafeCastUnitTest<string, string> { Name = "With Null Collection",      Original = default,                              Expected = Enumerable.Empty<string>() },
        new SafeCastUnitTest<string, string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = Enumerable.Empty<string>() },
        new SafeCastUnitTest<string, string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = ["String 1", "String 2", "String 3"] },
    ];

    public static TheoryDataRow<IXUnitTest>[] SafeToArrayTheoryData =>
    [
        new SafeToArrayUnitTest<string> { Name = "With Null Collection",      Original = default,                              Expected = [] },
        new SafeToArrayUnitTest<string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = [] },
        new SafeToArrayUnitTest<string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = ["String 1", "String 2", "String 3"] },
    ];

    public static TheoryDataRow<IXUnitTest>[] SafeToListTheoryData =>
    [
        new SafeToListUnitTest<string> { Name = "With Null Collection",      Original = default,                              Expected = new List<string>() },
        new SafeToListUnitTest<string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = new List<string>() },
        new SafeToListUnitTest<string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = new List<string> { "String 1", "String 2", "String 3" } },
    ];

    public static TheoryDataRow<IXUnitTest>[] SafeToReadOnlyCollectionTheoryData =>
    [
        new SafeToReadOnlyCollectionUnitTest<string> { Name = "With Null Collection",      Original = default,                              Expected = new List<string>() },
        new SafeToReadOnlyCollectionUnitTest<string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = new List<string>() },
        new SafeToReadOnlyCollectionUnitTest<string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = new List<string> { "String 1", "String 2", "String 3" } },
    ];

    public static TheoryDataRow<IXUnitTest>[] SafeToReadOnlyListTheoryData =>
    [
        new SafeToReadOnlyListUnitTest<string> { Name = "With Null Collection",      Original = default,                              Expected = new List<string>() },
        new SafeToReadOnlyListUnitTest<string> { Name = "With Empty Collection",     Original = Enumerable.Empty<string>(),           Expected = new List<string>() },
        new SafeToReadOnlyListUnitTest<string> { Name = "With Non Empty Collection", Original = ["String 1", "String 2", "String 3"], Expected = new List<string> { "String 1", "String 2", "String 3" } },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(EmptyIfNullTheoryData))]
    public void TestEnumerableEmptyIfNull(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(IsNullOrEmptyTheoryData))]
    public void TestEnumerableIsNullOrEmpty(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(SafeCastTheoryData))]
    public void TestEnumerableSafeCast(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(SafeToArrayTheoryData))]
    public void TestEnumerableSafeToArray(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(SafeToListTheoryData))]
    public void TestEnumerableSafeToList(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(SafeToReadOnlyCollectionTheoryData))]
    public void TestEnumerableSafeToReadOnlyCollection(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(SafeToReadOnlyListTheoryData))]
    public void TestEnumerableSafeToReadOnlyList(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}