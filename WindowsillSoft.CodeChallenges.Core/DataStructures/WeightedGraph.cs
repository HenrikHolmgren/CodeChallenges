using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsillSoft.CodeChallenges.Core.DataStructures
{
    public class WeightedGraph<TContent, TEdgeWeight> where TContent : IComparable
    {
        public Node[] Nodes { get; }

        public static WeightedGraph<TContent, TEdgeWeight> Build<TElement>(
              IEnumerable<TElement> items,
              Func<TElement, TContent> selectContent,
              Func<TElement, IEnumerable<KeyValuePair<TContent, TEdgeWeight>>> selectChildren)
            where TElement : notnull
        {
            var nodes = items.ToDictionary(p => p, p => new Node(selectContent(p)));
            var contentLookup = nodes.ToDictionary(p => p.Value.Content, p => p.Value);

            foreach (var node in nodes)
            {
                var children = selectChildren(node.Key);
                foreach (var child in children)
                    node.Value.AddChild(contentLookup[child.Key], child.Value);
            }

            return new WeightedGraph<TContent, TEdgeWeight>(nodes.Values.ToArray());
        }

        protected WeightedGraph(Node[] nodes)
        {
            Nodes = nodes;
        }

        public class Node
        {
            private List<Edge> _edges { get; set; } = new();
            private List<Node> _parents { get; set; } = new();

            internal Node(TContent content)
                => Content = content;

            public TContent Content { get; protected set; }

            public IReadOnlyList<Edge> Edges => _edges;
            public IReadOnlyList<Node> Parents => _parents;

            internal void AddChild(Node child, TEdgeWeight weight)
            {
                _edges.Add(new Edge(weight, this, child));
                child._parents.Add(this);
            }

            public IEnumerable<Node> AllParents()
                => Parents.Concat(Parents.SelectMany(p => p.AllParents()));

        }

        public class Edge
        {
            internal Edge(TEdgeWeight weight,
                Node parent,
                Node child)
            {
                Weight = weight;
                Source = parent;
                Target = child;
            }

            public TEdgeWeight Weight { get; }
            public Node Source { get; }
            public Node Target { get; }
        }
    }
}
