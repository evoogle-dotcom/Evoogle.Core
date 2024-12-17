// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;

using Xunit.Abstractions;

namespace Evoogle.NTree;

public class NodeTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
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

        public static TestNode CreateTree(int maxDepth, int maxChildren)
        {
            var root = new TestNode("1");
            if (maxDepth == 0)
                return root;

            BuildTree(maxDepth, maxChildren, 1, root);

            return root;
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

    public class TraversalTest : XUnitTest
    {
        #region Calculated Properties
        private string ActualNameTraversal { get; set; } = null!;
        #endregion

        #region User Supplied Properties
        public Func<TestNode, IEnumerator<TestNode>> EnumeratorFactory { get; set; } = null!;
        public Func<TestNode> TreeFactory { get; set; } = null!;
        public string ExpectedNameTraversal { get; set; } = null!;
        #endregion

        #region Methods
        protected override void Arrange()
        {
            var expectedTraversalString = this.ExpectedNameTraversal.SafeToString();
            this.WriteLine($"Expected Traversal: {expectedTraversalString}");
            this.WriteLine();
        }

        protected override void Act()
        {
            var tree = this.TreeFactory();
            var enumerator = this.EnumeratorFactory(tree);
            var nameCollection = new List<string>();

            tree.Traverse(
                enumerator,
                node =>
                {
                    nameCollection.Add(node.Name);
                    return true;
                });

            var actualNameTraversal = nameCollection.SafeToDelimitedString('|');
            this.ActualNameTraversal = actualNameTraversal;

            var actualTraversalString = this.ActualNameTraversal.SafeToString();
            this.WriteLine($"Actual Traversal:   {actualTraversalString}");
        }

        protected override void Assert()
        {
            this.ActualNameTraversal.Should().BeEquivalentTo(this.ExpectedNameTraversal);
        }
        #endregion
    }
    #endregion

    #region Theory Data
    public static TheoryData<IXUnitTest> TraversalTheoryData => new()
    {
        // BFS Traversal
        {
            new TraversalTest
            {
                Name = "BFS Depth=0 Children=0",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(0, 0),
                ExpectedNameTraversal = "1"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=1 Children=1",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(1, 1),
                ExpectedNameTraversal = "1|11"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=1 Children=2",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(1, 2),
                ExpectedNameTraversal = "1|11|12"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=1 Children=3",
                EnumeratorFactory = node => node.CreateBreadFirstEnumerator(),
                TreeFactory = ()=> TestNode.CreateTree(1, 3),
                ExpectedNameTraversal = "1|11|12|13"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=2 Children=1",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(2, 1),
                ExpectedNameTraversal = "1|11|111"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=2 Children=2",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(2, 2),
                ExpectedNameTraversal = "1|11|12|111|112|121|122"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=2 Children=3",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(2, 3),
                ExpectedNameTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=3 Children=1",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(3, 1),
                ExpectedNameTraversal = "1|11|111|1111"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=3 Children=2",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(3, 2),
                ExpectedNameTraversal = "1|11|12|111|112|121|122|1111|1112|1121|1122|1211|1212|1221|1222"
            }
        },
        {
            new TraversalTest
            {
                Name = "BFS Depth=3 Children=3",
                EnumeratorFactory = (node) => node.CreateBreadFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(3, 3),
                ExpectedNameTraversal = "1|11|12|13|111|112|113|121|122|123|131|132|133|1111|1112|1113|1121|1122|1123|1131|1132|1133|1211|1212|1213|1221|1222|1223|1231|1232|1233|1311|1312|1313|1321|1322|1323|1331|1332|1333"
            }
        },

        // DFS Traversal
        {
            new TraversalTest
            {
                Name = "DFS Depth=0 Children=0",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(0, 0),
                ExpectedNameTraversal = "1"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=1 Children=1",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(1, 1),
                ExpectedNameTraversal = "1|11"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=1 Children=2",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(1, 2),
                ExpectedNameTraversal = "1|11|12"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=1 Children=3",
                EnumeratorFactory = node => node.CreateDepthFirstEnumerator(),
                TreeFactory = ()=> TestNode.CreateTree(1, 3),
                ExpectedNameTraversal = "1|11|12|13"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=2 Children=1",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(2, 1),
                ExpectedNameTraversal = "1|11|111"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=2 Children=2",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(2, 2),
                ExpectedNameTraversal = "1|11|111|112|12|121|122"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=2 Children=3",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(2, 3),
                ExpectedNameTraversal = "1|11|111|112|113|12|121|122|123|13|131|132|133"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=3 Children=1",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(3, 1),
                ExpectedNameTraversal = "1|11|111|1111"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=3 Children=2",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(3, 2),
                ExpectedNameTraversal = "1|11|111|1111|1112|112|1121|1122|12|121|1211|1212|122|1221|1222"
            }
        },
        {
            new TraversalTest
            {
                Name = "DFS Depth=3 Children=3",
                EnumeratorFactory = (node) => node.CreateDepthFirstEnumerator(),
                TreeFactory = () => TestNode.CreateTree(3, 3),
                ExpectedNameTraversal = "1|11|111|1111|1112|1113|112|1121|1122|1123|113|1131|1132|1133|12|121|1211|1212|1213|122|1221|1222|1223|123|1231|1232|1233|13|131|1311|1312|1313|132|1321|1322|1323|133|1331|1332|1333"
            }
        },
    };
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(TraversalTheoryData))]
    public void Traversal(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}