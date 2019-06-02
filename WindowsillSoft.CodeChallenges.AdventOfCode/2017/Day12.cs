using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day12 : AdventOfCode2017SolverBase
    {
        private readonly Dictionary<int, List<int>> _edges = new Dictionary<int, List<int>>();

        public Day12(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 12: Digital Plumber";

        public override void Initialize(string input)
        {
            foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var value = line.Split("<->");
                var source = Int32.Parse(value[0]);
                var targets = value[1].Split(',').Select(p => Int32.Parse(p)).ToArray();

                foreach (var target in targets)
                    AddEdge(source, target);
            }
        }

        private void AddEdge(int source, int target)
        {
            if (!_edges.ContainsKey(source))
                _edges[source] = new List<int>();
            if (!_edges.ContainsKey(target))
                _edges[target] = new List<int>();

            _edges[source].Add(target);
            _edges[target].Add(source);
        }

        public override string ExecutePart1()
        {
            var reachable = GetReachableNodes(0);
            return reachable.Count().ToString();
        }

        private HashSet<int> GetReachableNodes(int start)
        {
            var reachable = new HashSet<int> { start };
            var fringe = new Queue<int>();
            fringe.Enqueue(start);
            while (fringe.Any())
            {
                var current = fringe.Dequeue();
                foreach (var neighbour in _edges[current])
                {
                    if (!reachable.Contains(neighbour))
                    {
                        reachable.Add(neighbour);
                        fringe.Enqueue(neighbour);
                    }
                }
            }

            return reachable;
        }

        public override string ExecutePart2()
        {
            var allNodes = _edges.Keys.ToHashSet();

            var cliques = new List<HashSet<int>>();
            while (allNodes.Any())
            {
                var nodeClique = GetReachableNodes(allNodes.First());
                allNodes.RemoveWhere(p => nodeClique.Contains(p));
                cliques.Add(nodeClique);
            }

            return cliques.Count.ToString();
        }
    }
}
