// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.NTree;

public class NodeExtensionsTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Types
    public enum InterfaceTraversalTestCase
    {
        Children,
        BreadthFirstDescendants,
        BreadthFirstSelfAndDescendants,
        DepthFirstSelfAndDescendants,
        PathFromRoot,
        PathToRoot,
        Root,
        IsDescendant,
        IsNotDescendant,
        DelegateTraversal,
        VisitorTraversal,
        EnumeratorVisitorTraversal,
        SelectorPathString,
        NamedPathString
    }

    public enum ArgumentValidationTestCase
    {
        NullBreadthFirstNode,
        NullDepthFirstNode,
        NullChildrenNode,
        NullDescendantsEnumerator,
        NullSelfAndDescendantsEnumerator,
        NullDelegateEnumerator,
        NullDelegateVisitor,
        NullObjectVisitor,
        NullNameSelector,
        NullPotentialAncestor,
        InvalidDescendantsStrategy,
        InvalidSelfAndDescendantsStrategy,
        InvalidDelegateTraversalStrategy,
        InvalidVisitorTraversalStrategy
    }

    public sealed class InterfaceTraversalTest : XUnitTest
    {
        #region User Supplied Properties
        public required string ExpectedValue { get; init; }

        public required InterfaceTraversalTestCase TestCase { get; init; }
        #endregion

        #region Calculated Properties
        private string? ActualValue { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        { }

        protected override void Act()
        {
            var root = TestInterfaceNode.CreateTree();
            var child11 = root.FirstChild!;
            var child12 = child11.NextSibling!;
            var child112 = child11.LastChild!;

            this.ActualValue = this.TestCase switch
            {
                InterfaceTraversalTestCase.Children => JoinLabels(root.Children()),
                InterfaceTraversalTestCase.BreadthFirstDescendants =>
                    JoinLabels(root.Descendants()),
                InterfaceTraversalTestCase.BreadthFirstSelfAndDescendants =>
                    JoinLabels(root.SelfAndDescendants()),
                InterfaceTraversalTestCase.DepthFirstSelfAndDescendants => JoinLabels
                (
                    root.SelfAndDescendants(TraversalStrategy.DepthFirst)
                ),
                InterfaceTraversalTestCase.PathFromRoot =>
                    JoinLabels(child112.GetPathFromRoot()),
                InterfaceTraversalTestCase.PathToRoot =>
                    JoinLabels(child112.GetPathToRoot()),
                InterfaceTraversalTestCase.Root =>
                    ReferenceEquals(child112.Root, root).ToString(),
                InterfaceTraversalTestCase.IsDescendant =>
                    child112.IsDescendantOf(root).ToString(),
                InterfaceTraversalTestCase.IsNotDescendant =>
                    child112.IsDescendantOf(child12).ToString(),
                InterfaceTraversalTestCase.DelegateTraversal =>
                    TraverseWithDelegate(root),
                InterfaceTraversalTestCase.VisitorTraversal =>
                    TraverseWithVisitor(root, useEnumerator: false),
                InterfaceTraversalTestCase.EnumeratorVisitorTraversal =>
                    TraverseWithVisitor(root, useEnumerator: true),
                InterfaceTraversalTestCase.SelectorPathString =>
                    child112.GetPathString(static node => node.Label, "/"),
                InterfaceTraversalTestCase.NamedPathString => GetNamedPathString(),
                _ => throw new InvalidOperationException
                (
                    $"Unsupported {nameof(InterfaceTraversalTestCase)} value '{this.TestCase}'."
                )
            };
        }

        protected override void Assert()
        {
            this.ActualValue.Should().Be(this.ExpectedValue);
        }
        #endregion

        #region Implementation Methods
        private static string GetNamedPathString()
        {
            var root = TestNode.CreateTree(maxDepth: 2, maxChildren: 2);
            var child112 = root.FirstChild!.LastChild!;
            return child112.GetPathString("/");
        }

        private static string JoinLabels(IEnumerable<TestInterfaceNode> nodes)
            => string.Join('|', nodes.Select(static node => node.Label));

        private static string TraverseWithDelegate(TestInterfaceNode root)
        {
            var labels = new List<string>();
            root.Traverse
            (
                TraversalStrategy.BreadthFirst,
                node =>
                {
                    labels.Add(node.Label);
                    return node.Label != "111";
                }
            );

            return string.Join('|', labels);
        }

        private static string TraverseWithVisitor
        (
            TestInterfaceNode root,
            bool useEnumerator
        )
        {
            var visitor = new StopAtLabelVisitor("111");
            if (useEnumerator)
            {
                root.Traverse(root.CreateBreadthFirstEnumerator(), visitor);
            }
            else
            {
                root.Traverse(TraversalStrategy.BreadthFirst, visitor);
            }

            return string.Join('|', visitor.Labels);
        }
        #endregion
    }

    public sealed class ArgumentValidationTest : XUnitTest
    {
        #region User Supplied Properties
        public required Type ExpectedExceptionType { get; init; }

        public required string ExpectedParameterName { get; init; }

        public required ArgumentValidationTestCase TestCase { get; init; }
        #endregion

        #region Calculated Properties
        private Exception? ActualException { get; set; }
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        { }

        protected override void Act()
        {
            try
            {
                Execute(this.TestCase);
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }
        }

        protected override void Assert()
        {
            this.ActualException.Should().NotBeNull();
            this.ActualException!.GetType().Should().Be(this.ExpectedExceptionType);
            this.ActualException.Should().BeAssignableTo<ArgumentException>();

            var argumentException = (ArgumentException)this.ActualException;
            argumentException.ParamName.Should().Be(this.ExpectedParameterName);
        }
        #endregion

        #region Implementation Methods
        private static void Execute(ArgumentValidationTestCase testCase)
        {
            var root = TestInterfaceNode.CreateTree();
            var invalidStrategy = (TraversalStrategy)int.MaxValue;

            switch (testCase)
            {
                case ArgumentValidationTestCase.NullBreadthFirstNode:
                    _ = NodeExtensions.CreateBreadthFirstEnumerator<TestInterfaceNode>(null!);
                    break;
                case ArgumentValidationTestCase.NullDepthFirstNode:
                    _ = NodeExtensions.CreateDepthFirstEnumerator<TestInterfaceNode>(null!);
                    break;
                case ArgumentValidationTestCase.NullChildrenNode:
                    _ = NodeExtensions.Children<TestInterfaceNode>(null!);
                    break;
                case ArgumentValidationTestCase.NullDescendantsEnumerator:
                    _ = root.Descendants((IEnumerator<TestInterfaceNode>)null!);
                    break;
                case ArgumentValidationTestCase.NullSelfAndDescendantsEnumerator:
                    _ = root.SelfAndDescendants((IEnumerator<TestInterfaceNode>)null!);
                    break;
                case ArgumentValidationTestCase.NullDelegateEnumerator:
                    root.Traverse
                    (
                        (IEnumerator<TestInterfaceNode>)null!,
                        static _ => true
                    );
                    break;
                case ArgumentValidationTestCase.NullDelegateVisitor:
                    root.Traverse
                    (
                        TraversalStrategy.BreadthFirst,
                        (Func<TestInterfaceNode, bool>)null!
                    );
                    break;
                case ArgumentValidationTestCase.NullObjectVisitor:
                    root.Traverse
                    (
                        TraversalStrategy.BreadthFirst,
                        (INodeVisitor<TestInterfaceNode>)null!
                    );
                    break;
                case ArgumentValidationTestCase.NullNameSelector:
                    _ = root.GetPathString((Func<TestInterfaceNode, string>)null!);
                    break;
                case ArgumentValidationTestCase.NullPotentialAncestor:
                    _ = root.IsDescendantOf(null!);
                    break;
                case ArgumentValidationTestCase.InvalidDescendantsStrategy:
                    _ = root.Descendants(invalidStrategy);
                    break;
                case ArgumentValidationTestCase.InvalidSelfAndDescendantsStrategy:
                    _ = root.SelfAndDescendants(invalidStrategy);
                    break;
                case ArgumentValidationTestCase.InvalidDelegateTraversalStrategy:
                    root.Traverse(invalidStrategy, static _ => true);
                    break;
                case ArgumentValidationTestCase.InvalidVisitorTraversalStrategy:
                    root.Traverse(invalidStrategy, new StopAtLabelVisitor("111"));
                    break;
                default:
                    throw new InvalidOperationException
                    (
                        $"Unsupported {nameof(ArgumentValidationTestCase)} value '{testCase}'."
                    );
            }
        }
        #endregion
    }

    private sealed class StopAtLabelVisitor(string stopLabel) :
        INodeVisitor<TestInterfaceNode>
    {
        #region Properties
        public List<string> Labels { get; } = [];
        #endregion

        #region INodeVisitor<TestInterfaceNode> Methods
        public VisitResult Visit(TestInterfaceNode node)
        {
            this.Labels.Add(node.Label);
            return node.Label == stopLabel ? VisitResult.Done : VisitResult.Continue;
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] InterfaceTraversalTheoryData =>
    [
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.Children,
            "11|12"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.BreadthFirstDescendants,
            "11|12|111|112|121|122"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.BreadthFirstSelfAndDescendants,
            "1|11|12|111|112|121|122"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.DepthFirstSelfAndDescendants,
            "1|11|111|112|12|121|122"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.PathFromRoot,
            "1|11|112"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.PathToRoot,
            "112|11|1"
        ),
        CreateTraversalRow(InterfaceTraversalTestCase.Root, bool.TrueString),
        CreateTraversalRow(InterfaceTraversalTestCase.IsDescendant, bool.TrueString),
        CreateTraversalRow(InterfaceTraversalTestCase.IsNotDescendant, bool.FalseString),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.DelegateTraversal,
            "1|11|12|111"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.VisitorTraversal,
            "1|11|12|111"
        ),
        CreateTraversalRow
        (
            InterfaceTraversalTestCase.EnumeratorVisitorTraversal,
            "1|11|12|111"
        ),
        CreateTraversalRow(InterfaceTraversalTestCase.SelectorPathString, "1/11/112"),
        CreateTraversalRow(InterfaceTraversalTestCase.NamedPathString, "1/11/112")
    ];

    public static TheoryDataRow<IXUnitTest>[] ArgumentValidationTheoryData =>
    [
        CreateValidationRow(ArgumentValidationTestCase.NullBreadthFirstNode, "node"),
        CreateValidationRow(ArgumentValidationTestCase.NullDepthFirstNode, "node"),
        CreateValidationRow(ArgumentValidationTestCase.NullChildrenNode, "node"),
        CreateValidationRow
        (
            ArgumentValidationTestCase.NullDescendantsEnumerator,
            "enumerator"
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.NullSelfAndDescendantsEnumerator,
            "enumerator"
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.NullDelegateEnumerator,
            "enumerator"
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.NullDelegateVisitor,
            "visitorFunction"
        ),
        CreateValidationRow(ArgumentValidationTestCase.NullObjectVisitor, "visitor"),
        CreateValidationRow(ArgumentValidationTestCase.NullNameSelector, "nameSelector"),
        CreateValidationRow
        (
            ArgumentValidationTestCase.NullPotentialAncestor,
            "potentialAncestor"
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.InvalidDescendantsStrategy,
            "strategy",
            typeof(ArgumentOutOfRangeException)
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.InvalidSelfAndDescendantsStrategy,
            "strategy",
            typeof(ArgumentOutOfRangeException)
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.InvalidDelegateTraversalStrategy,
            "strategy",
            typeof(ArgumentOutOfRangeException)
        ),
        CreateValidationRow
        (
            ArgumentValidationTestCase.InvalidVisitorTraversalStrategy,
            "strategy",
            typeof(ArgumentOutOfRangeException)
        )
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(InterfaceTraversalTheoryData))]
    public void InterfaceTraversal(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(ArgumentValidationTheoryData))]
    public void ArgumentValidation(IXUnitTest test) => test.Execute(this);
    #endregion

    #region Factory Methods
    private static TheoryDataRow<IXUnitTest> CreateTraversalRow
    (
        InterfaceTraversalTestCase testCase,
        string expectedValue
    )
        => new InterfaceTraversalTest
        {
            Name = testCase.ToString(),
            TestCase = testCase,
            ExpectedValue = expectedValue
        };

    private static TheoryDataRow<IXUnitTest> CreateValidationRow
    (
        ArgumentValidationTestCase testCase,
        string expectedParameterName,
        Type? expectedExceptionType = null
    )
        => new ArgumentValidationTest
        {
            Name = testCase.ToString(),
            TestCase = testCase,
            ExpectedExceptionType = expectedExceptionType ?? typeof(ArgumentNullException),
            ExpectedParameterName = expectedParameterName
        };
    #endregion
}
