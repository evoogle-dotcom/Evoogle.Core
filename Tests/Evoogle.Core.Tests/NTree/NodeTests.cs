// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Expressions;
using System.Text.Json.Serialization;

using Evoogle.Extensions;
using Evoogle.XUnit;
using Evoogle.XUnit.Json;

using FluentAssertions;

namespace Evoogle.NTree;

public class NodeTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class TreeMutateWithParentTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode>))]
        public Expression<Func<TestNode>> TreeFactoryExpression { get; init; } = null!;

        [JsonConverter(typeof(ExpressionActionJsonConverter<TestNode>))]
        public Expression<Action<TestNode>> TreeMutateExpression { get; init; } = null!;

        public string ExpectedBeforeTraversal { get; init; } = null!;
        public string ExpectedAfterTraversal { get; init; } = null!;
        #endregion

        #region Calculated Properties
        private string ActualAfterTraversal { get; set; } = null!;
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            var expectedBeforeTraversalString = this.ExpectedBeforeTraversal.SafeToString();
            this.WriteLine($"Expected Before Traversal: {expectedBeforeTraversalString}");
            this.WriteLine();

            var expectedAfterTraversalString = this.ExpectedAfterTraversal.SafeToString();
            this.WriteLine($"Expected After Traversal:  {expectedAfterTraversalString}");
            this.WriteLine();
        }

        protected override void Act()
        {
            // Create tree
            var treeFactoryFunc = this.TreeFactoryExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeFactoryExpression)} into a function object.");
            var tree = treeFactoryFunc();

            // Create breadth first enumerator
            var enumerator = tree.CreateBreadthFirstEnumerator();
            var nameCollection = new List<string>();

            // Mutate the tree
            var treeMutateAction = this.TreeMutateExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeMutateExpression)} into a function object.");
            treeMutateAction(tree);

            // Traverse the tree with a breadth first enumerator
            tree.Traverse(
                enumerator,
                node =>
                {
                    nameCollection.Add(node.Name);
                    return true;
                });

            var actualAfterTraversal = nameCollection.SafeToDelimitedString('|');
            this.ActualAfterTraversal = actualAfterTraversal;

            var actualAfterTraversalString = this.ActualAfterTraversal.SafeToString();
            this.WriteLine($"Actual After Traversal:    {actualAfterTraversalString}");
        }

        protected override void Assert() => this.ActualAfterTraversal.Should().BeEquivalentTo(this.ExpectedAfterTraversal);
        #endregion
    }

    public class TreeMutateWithParentAndChildTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode>))]
        public Expression<Func<TestNode>> TreeFactoryExpression { get; init; } = null!;

        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode>))]
        public Expression<Func<TestNode>> ChildFactoryExpression { get; init; } = null!;

        [JsonConverter(typeof(ExpressionActionJsonConverter<TestNode, TestNode>))]
        public Expression<Action<TestNode, TestNode>> TreeMutateExpression { get; init; } = null!;

        #region Calculated Properties
        private string ActualAfterTraversal { get; set; } = null!;
        #endregion

        public string ExpectedBeforeTraversal { get; init; } = null!;
        public string ExpectedAfterTraversal { get; init; } = null!;
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            var expectedBeforeTraversalString = this.ExpectedBeforeTraversal.SafeToString();
            this.WriteLine($"Expected Before Traversal: {expectedBeforeTraversalString}");
            this.WriteLine();

            var expectedAfterTraversalString = this.ExpectedAfterTraversal.SafeToString();
            this.WriteLine($"Expected After Traversal:  {expectedAfterTraversalString}");
            this.WriteLine();
        }

        protected override void Act()
        {
            // Create tree
            var treeFactoryFunc = this.TreeFactoryExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeFactoryExpression)} into a function object.");
            var tree = treeFactoryFunc();

            // Create child
            var childFactoryFunc = this.ChildFactoryExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeFactoryExpression)} into a function object.");
            var child = childFactoryFunc();

            // Create breadth first enumerator
            var enumerator = tree.CreateBreadthFirstEnumerator();
            var nameCollection = new List<string>();

            // Mutate the tree
            var treeMutateAction = this.TreeMutateExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeMutateExpression)} into a function object.");
            treeMutateAction(tree, child);

            // Traverse the tree with a breadth first enumerator
            tree.Traverse(
                enumerator,
                node =>
                {
                    nameCollection.Add(node.Name);
                    return true;
                });

            var actualAfterTraversal = nameCollection.SafeToDelimitedString('|');
            this.ActualAfterTraversal = actualAfterTraversal;

            var actualAfterTraversalString = this.ActualAfterTraversal.SafeToString();
            this.WriteLine($"Actual After Traversal:    {actualAfterTraversalString}");
        }

        protected override void Assert() => this.ActualAfterTraversal.Should().BeEquivalentTo(this.ExpectedAfterTraversal);
        #endregion
    }

    public class TraversalTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode>))]
        public Expression<Func<TestNode>> TreeFactoryExpression { get; init; } = null!;

        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode, IEnumerator<TestNode>>))]
        public Expression<Func<TestNode, IEnumerator<TestNode>>> EnumeratorFactoryExpression { get; init; } = null!;

        public string ExpectedTraversal { get; init; } = null!;
        #endregion

        #region Calculated Properties
        private string ActualTraversal { get; set; } = null!;
        #endregion

        #region XUnitTest Methods
        protected override void Arrange()
        {
            var expectedTraversalString = this.ExpectedTraversal.SafeToString();
            this.WriteLine($"Expected Traversal: {expectedTraversalString}");
            this.WriteLine();
        }

        protected override void Act()
        {
            // Create tree
            var treeFactoryFunc = this.TreeFactoryExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeFactoryExpression)} into a function object.");
            var tree = treeFactoryFunc();

            // Create enumerator
            var enumeratorFactoryFunc = this.EnumeratorFactoryExpression.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.EnumeratorFactoryExpression)} into a function object.");
            var enumerator = enumeratorFactoryFunc(tree);
            var nameCollection = new List<string>();

            // Traverse the tree
            tree.Traverse(
                enumerator,
                node =>
                {
                    nameCollection.Add(node.Name);
                    return true;
                });

            var actualNameTraversal = nameCollection.SafeToDelimitedString('|');
            this.ActualTraversal = actualNameTraversal;

            var actualTraversalString = this.ActualTraversal.SafeToString();
            this.WriteLine($"Actual Traversal:   {actualTraversalString}");
        }

        protected override void Assert() => this.ActualTraversal.Should().BeEquivalentTo(this.ExpectedTraversal);
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] ReplaceChildTheoryData =>
    [
        // ReplaceFirstChild
        new TreeMutateWithParentAndChildTest
        {
            Name = "ReplaceFirstChild Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "ReplaceFirstChild Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1|Z"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|12",
            ExpectedAfterTraversal = "1|Z|12"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13",
            ExpectedAfterTraversal = "1|Z|12|13"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|111",
            ExpectedAfterTraversal = "1|Z"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122",
            ExpectedAfterTraversal = "1|Z|12|121|122"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133",
            ExpectedAfterTraversal = "1|Z|12|13|121|122|123|131|132|133"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|111|1111",
            ExpectedAfterTraversal = "1|Z"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222",
            ExpectedAfterTraversal = "1|Z|12|121|122|1211|1212|1221|1222"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveFirstChild Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceFirstChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333",
            ExpectedAfterTraversal = "1|Z|12|13|121|122|123|131|132|133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333"
        },

        // ReplaceSecondChild
        new TreeMutateWithParentAndChildTest
        {
            Name = "ReplaceSecondChild Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "ReplaceSecondChild Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1|11"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|12",
            ExpectedAfterTraversal = "1|11|Z"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13",
            ExpectedAfterTraversal = "1|11|Z|13"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|111",
            ExpectedAfterTraversal = "1|11|111"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122",
            ExpectedAfterTraversal = "1|11|Z|111|112"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133",
            ExpectedAfterTraversal = "1|11|Z|13|111|112|113|131|132|133"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|111|1111",
            ExpectedAfterTraversal = "1|11|111|1111"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222",
            ExpectedAfterTraversal = "1|11|Z|111|112|1111|1112|1121|1122"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveSecondChild Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceSecondChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333",
            ExpectedAfterTraversal = "1|11|Z|13|111|112|113|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1311|1312|1313|1321|1322|1323|1331|1332|1333"
        },

        // ReplaceThirdChild
        new TreeMutateWithParentAndChildTest
        {
            Name = "ReplaceThirdChild Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "ReplaceThirdChild Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1|11"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|12",
            ExpectedAfterTraversal = "1|11|12"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13",
            ExpectedAfterTraversal = "1|11|12|Z"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|111",
            ExpectedAfterTraversal = "1|11|111"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122",
            ExpectedAfterTraversal = "1|11|12|111|112|121|122"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133",
            ExpectedAfterTraversal = "1|11|12|Z|111|112|113|121|122|123"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|111|1111",
            ExpectedAfterTraversal = "1|11|111|1111"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222",
            ExpectedAfterTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222"
        },

        new TreeMutateWithParentAndChildTest
        {
            Name = "RemoveThirdChild Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            ChildFactoryExpression = () => TestNode.CreateNode("Z"),
            TreeMutateExpression = (a,b) => TestNode.ReplaceThirdChild(a,b),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333",
            ExpectedAfterTraversal = "1|11|12|Z|111|112|113|121|122|123|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233"
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] RemoveChildTheoryData =>
    [
        // RemoveFirstChild
        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|12",
            ExpectedAfterTraversal = "1|12"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|12|13",
            ExpectedAfterTraversal = "1|12|13"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|111",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122",
            ExpectedAfterTraversal = "1|12|121|122"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133",
            ExpectedAfterTraversal = "1|12|13|121|122|123|131|132|133"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|111|1111",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222",
            ExpectedAfterTraversal = "1|12|121|122|1211|1212|1221|1222"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveFirstChild Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            TreeMutateExpression = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333",
            ExpectedAfterTraversal = "1|12|13|121|122|123|131|132|133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333"
        },

        // RemoveSecondChild
        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1|11"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|12",
            ExpectedAfterTraversal = "1|11"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|12|13",
            ExpectedAfterTraversal = "1|11|13"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|111",
            ExpectedAfterTraversal = "1|11|111"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122",
            ExpectedAfterTraversal = "1|11|111|112"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133",
            ExpectedAfterTraversal = "1|11|13|111|112|113|131|132|133"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|111|1111",
            ExpectedAfterTraversal = "1|11|111|1111"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222",
            ExpectedAfterTraversal = "1|11|111|112|1111|1112|1121|1122"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveSecondChild Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            TreeMutateExpression = (a) => TestNode.RemoveSecondChild(a),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333",
            ExpectedAfterTraversal = "1|11|13|111|112|113|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1311|1312|1313|1321|1322|1323|1331|1332|1333"
        },

        // RemoveThirdChild
        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1|11"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|12",
            ExpectedAfterTraversal = "1|11|12"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|12|13",
            ExpectedAfterTraversal = "1|11|12"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|111",
            ExpectedAfterTraversal = "1|11|111"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122",
            ExpectedAfterTraversal = "1|11|12|111|112|121|122"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133",
            ExpectedAfterTraversal = "1|11|12|111|112|113|121|122|123"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|111|1111",
            ExpectedAfterTraversal = "1|11|111|1111"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222",
            ExpectedAfterTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222"
        },

        new TreeMutateWithParentTest
        {
            Name = "RemoveThirdChild Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            TreeMutateExpression = (a) => TestNode.RemoveThirdChild(a),
            ExpectedBeforeTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333",
            ExpectedAfterTraversal = "1|11|12|111|112|113|121|122|123|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233"
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TraversalTheoryData =>
    [
        // BFS Traversal
        new TraversalTest
        {
            Name = "BFS Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1"
        },

        new TraversalTest
        {
            Name = "BFS Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11"
        },

        new TraversalTest
        {
            Name = "BFS Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12"
        },

        new TraversalTest
        {
            Name = "BFS Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13"
        },

        new TraversalTest
        {
            Name = "BFS Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111"
        },

        new TraversalTest
        {
            Name = "BFS Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|111|112|121|122"
        },

        new TraversalTest
        {
            Name = "BFS Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133"
        },

        new TraversalTest
        {
            Name = "BFS Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111"
        },

        new TraversalTest
        {
            Name = "BFS Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222"
        },

        new TraversalTest
        {
            Name = "BFS Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            EnumeratorFactoryExpression = (a) => TestNode.CreateBreadthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333"
        },

        // DFS Traversal
        new TraversalTest
        {
            Name = "DFS Depth=0 Children=0",
            TreeFactoryExpression = () => TestNode.CreateTree(0, 0),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1"
        },

        new TraversalTest
        {
            Name = "DFS Depth=1 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 1),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11"
        },

        new TraversalTest
        {
            Name = "DFS Depth=1 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 2),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12"
        },

        new TraversalTest
        {
            Name = "DFS Depth=1 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(1, 3),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13"
        },

        new TraversalTest
        {
            Name = "DFS Depth=2 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 1),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111"
        },

        new TraversalTest
        {
            Name = "DFS Depth=2 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 2),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|112|12|121|122"
        },

        new TraversalTest
        {
            Name = "DFS Depth=2 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(2, 3),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|112|113|12|121|122|123|13|131|132|133"
        },

        new TraversalTest
        {
            Name = "DFS Depth=3 Children=1",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 1),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111"
        },

        new TraversalTest
        {
            Name = "DFS Depth=3 Children=2",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 2),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111|1112|112|1121|1122|12|121|1211|1212|122|1221|1222"
        },

        new TraversalTest
        {
            Name = "DFS Depth=3 Children=3",
            TreeFactoryExpression = () => TestNode.CreateTree(3, 3),
            EnumeratorFactoryExpression = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111|1112|1113|112|1121|1122|1123|113|1131|1132|1133|12|121|1211|1212|1213|122|1221|1222|1223|123|1231|1232|1233|13|131|1311|1312|1313|132|1321|1322|1323|133|1331|1332|1333"
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(ReplaceChildTheoryData))]
    public void ReplaceChild(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(RemoveChildTheoryData))]
    public void RemoveChild(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(TraversalTheoryData))]
    public void Traversal(IXUnitTest test) => test.Execute(this);
    #endregion
}
