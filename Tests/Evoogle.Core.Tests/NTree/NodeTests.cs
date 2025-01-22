// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Expressions;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text.Json.Serialization;

using Evoogle.Json;
using Evoogle.XUnit;

using FluentAssertions;

namespace Evoogle.NTree;

public class NodeTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    [DynamicLinqType]
    public class TestNode : Node<TestNode>
    {
        public override string Name { get; }

        public TestNode(string name)
        {
            this.Name = name;
        }

        public TestNode(string name, params TestNode[] nodeCollection)
            : base(nodeCollection.AsEnumerable())
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static IEnumerator<TestNode> CreateBreadFirstEnumerator(TestNode testNode)
        {
            return testNode.CreateBreadFirstEnumerator();
        }

        public static IEnumerator<TestNode> CreateDepthFirstEnumerator(TestNode testNode)
        {
            return testNode.CreateDepthFirstEnumerator();
        }

        public static TestNode CreateTree(int maxDepth, int maxChildren)
        {
            var root = new TestNode("1");
            if (maxDepth == 0)
                return root;

            BuildTree(maxDepth, maxChildren, 1, root);

            return root;
        }

        public static void RemoveFirstChild(TestNode tree)
        {
            var node = tree.FirstChild;
            if (node == null)
                return;

            tree.RemoveChild(node);
        }

        private static void BuildTree(int maxDepth, int maxChildren, int currentDepth, TestNode parent)
        {
            // Create child nodes
            var childNodes = new List<TestNode>();
            var parentName = parent.Name;
            for (var childNumber = 1; childNumber <= maxChildren; ++childNumber)
            {
                var childName = $"{parentName}{childNumber}";
                var childNode = new TestNode(childName);
                childNodes.Add(childNode);

                parent.AddChild(childNode);
            }

            // If we are at maximum depth, then return.
            if (currentDepth == maxDepth)
                return;

            for (var i = 0; i < maxChildren; ++i)
            {
                var childNode = childNodes[i];
                BuildTree(maxDepth, maxChildren, currentDepth + 1, childNode);
            }
        }
    }

    public class TreeActionTest : XUnitTest
    {
        #region Calculated Properties
        private string ActualAfterTraversal { get; set; } = null!;
        #endregion

        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode>))]
        public Expression<Func<TestNode>> TreeFactory { get; set; } = null!;

        [JsonConverter(typeof(ExpressionActionJsonConverter<TestNode>))]
        public Expression<Action<TestNode>> TreeAction { get; set; } = null!;

        public string ExpectedBeforeTraversal { get; set; } = null!;
        public string ExpectedAfterTraversal { get; set; } = null!;
        #endregion

        #region Methods
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
            var treeFactory = this.TreeFactory.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeFactory)} into a function object.");
            var tree = treeFactory();

            // Create tree enumerator
            var enumerator = tree.CreateBreadFirstEnumerator();
            var nameCollection = new List<string>();

            // Perform action on tree
            var treeAction = this.TreeAction.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeAction)} into a function object.");
            treeAction(tree);

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

        protected override void Assert()
        {
            this.ActualAfterTraversal.Should().BeEquivalentTo(this.ExpectedAfterTraversal);
        }
        #endregion
    }

    public class TraversalTest : XUnitTest
    {
        #region Calculated Properties
        private string ActualTraversal { get; set; } = null!;
        #endregion

        #region User Supplied Properties
        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode>))]
        public Expression<Func<TestNode>> TreeFactory { get; set; } = null!;

        [JsonConverter(typeof(ExpressionFuncJsonConverter<TestNode, IEnumerator<TestNode>>))]
        public Expression<Func<TestNode, IEnumerator<TestNode>>> EnumeratorFactory { get; set; } = null!;

        public string ExpectedTraversal { get; set; } = null!;
        #endregion

        #region Methods
        protected override void Arrange()
        {
            var expectedTraversalString = this.ExpectedTraversal.SafeToString();
            this.WriteLine($"Expected Traversal: {expectedTraversalString}");
            this.WriteLine();
        }

        protected override void Act()
        {
            // Create tree
            var treeFactory = this.TreeFactory.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.TreeFactory)} into a function object.");
            var tree = treeFactory();

            // Create tree enumerator
            var enumeratorFactory = this.EnumeratorFactory.Compile() ?? throw new InvalidOperationException($"Unable to compile {nameof(this.EnumeratorFactory)} into a function object.");
            var enumerator = enumeratorFactory(tree);
            var nameCollection = new List<string>();

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

        protected override void Assert()
        {
            this.ActualTraversal.Should().BeEquivalentTo(this.ExpectedTraversal);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] RemoveChildTheoryData =>
    [
        new TreeActionTest
        {
            Name = "RemoveChild Child #1 Depth=0 Children=0",
            TreeFactory = () => TestNode.CreateTree(0, 0),
            TreeAction = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1",
            ExpectedAfterTraversal = "1"
        },

        new TreeActionTest
        {
            Name = "RemoveChild Child #1 Depth=1 Children=1",
            TreeFactory = () => TestNode.CreateTree(1, 1),
            TreeAction = (a) => TestNode.RemoveFirstChild(a),
            ExpectedBeforeTraversal = "1|11",
            ExpectedAfterTraversal = "1"
        },
    ];

    public static TheoryDataRow<IXUnitTest>[] TraversalTheoryData =>
    [
        // BFS Traversal
        new TraversalTest
        {
            Name = "BFS Depth=0 Children=0",
            TreeFactory = () => TestNode.CreateTree(0, 0),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1"
        },

        new TraversalTest
        {
            Name = "BFS Depth=1 Children=1",
            TreeFactory = () => TestNode.CreateTree(1, 1),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11"
        },

        new TraversalTest
        {
            Name = "BFS Depth=1 Children=2",
            TreeFactory = () => TestNode.CreateTree(1, 2),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|12"
        },

        new TraversalTest
        {
            Name = "BFS Depth=1 Children=3",
            TreeFactory = () => TestNode.CreateTree(1, 3),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13"
        },

        new TraversalTest
        {
            Name = "BFS Depth=2 Children=1",
            TreeFactory = () => TestNode.CreateTree(2, 1),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|111"
        },

        new TraversalTest
        {
            Name = "BFS Depth=2 Children=2",
            TreeFactory = () => TestNode.CreateTree(2, 2),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|111|112|121|122"
        },

        new TraversalTest
        {
            Name = "BFS Depth=2 Children=3",
            TreeFactory = () => TestNode.CreateTree(2, 3),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133"
        },

        new TraversalTest
        {
            Name = "BFS Depth=3 Children=1",
            TreeFactory = () => TestNode.CreateTree(3, 1),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111"
        },

        new TraversalTest
        {
            Name = "BFS Depth=3 Children=2",
            TreeFactory = () => TestNode.CreateTree(3, 2),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222"
        },

        new TraversalTest
        {
            Name = "BFS Depth=3 Children=3",
            TreeFactory = () => TestNode.CreateTree(3, 3),
            EnumeratorFactory = (a) => TestNode.CreateBreadFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333"
        },

        // DFS Traversal
        new TraversalTest
        {
            Name = "DFS Depth=0 Children=0",
            TreeFactory = () => TestNode.CreateTree(0, 0),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1"
        },

        new TraversalTest
        {
            Name = "DFS Depth=1 Children=1",
            TreeFactory = () => TestNode.CreateTree(1, 1),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11"
        },

        new TraversalTest
        {
            Name = "DFS Depth=1 Children=2",
            TreeFactory = () => TestNode.CreateTree(1, 2),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12"
        },

        new TraversalTest
        {
            Name = "DFS Depth=1 Children=3",
            TreeFactory = () => TestNode.CreateTree(1, 3),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|12|13"
        },

        new TraversalTest
        {
            Name = "DFS Depth=2 Children=1",
            TreeFactory = () => TestNode.CreateTree(2, 1),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111"
        },

        new TraversalTest
        {
            Name = "DFS Depth=2 Children=2",
            TreeFactory = () => TestNode.CreateTree(2, 2),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|112|12|121|122"
        },

        new TraversalTest
        {
            Name = "DFS Depth=2 Children=3",
            TreeFactory = () => TestNode.CreateTree(2, 3),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|112|113|12|121|122|123|13|131|132|133"
        },

        new TraversalTest
        {
            Name = "DFS Depth=3 Children=1",
            TreeFactory = () => TestNode.CreateTree(3, 1),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111"
        },

        new TraversalTest
        {
            Name = "DFS Depth=3 Children=2",
            TreeFactory = () => TestNode.CreateTree(3, 2),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111|1112|112|1121|1122|12|121|1211|1212|122|1221|1222"
        },

        new TraversalTest
        {
            Name = "DFS Depth=3 Children=3",
            TreeFactory = () => TestNode.CreateTree(3, 3),
            EnumeratorFactory = (a) => TestNode.CreateDepthFirstEnumerator(a),
            ExpectedTraversal = "1|11|111|1111|1112|1113|112|1121|1122|1123|113|1131|1132|1133|12|121|1211|1212|1213|122|1221|1222|1223|123|1231|1232|1233|13|131|1311|1312|1313|132|1321|1322|1323|133|1331|1332|1333"
        },
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(RemoveChildTheoryData))]
    public void RemoveChild(IXUnitTest test)
    {
        test.Execute(this);
    }

    [Theory]
    [MemberData(nameof(TraversalTheoryData))]
    public void Traversal(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}