using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.DataStructures;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day07 : AdventOfCode2020SolverBase
    {
        public Day07(IIOProvider provider) : base(provider)
        { }

        protected WeightedGraph<string, int>? _graph;

        public override string Name => "Day 7: Handy Haversacks";

        public override string ExecutePart1()
            => ((_graph!.Nodes.Single(p => p.Content == "shiny gold"))
            .AllParents().Distinct().Count()).ToString();

        public override string ExecutePart2()
            => (GetChildWeights(_graph!.Nodes.Single(p => p.Content == "shiny gold")) - 1).ToString();

        private long GetChildWeights(WeightedGraph<string, int>.Node node)
            => node.Edges.Sum(p => p.Weight * GetChildWeights(p.Target)) + 1;

        public override void Initialize(string input)
        {
            Dictionary<string, List<(int count, string contentColour)>> nodes = new();
            var contentMatch = new Regex(@"(?'count'\d+) (?'description'\w+ \w+) bag(s?)");

            foreach (var node in input.Split('\r', '\n'))
            {
                var split = node.Split("bags contain");
                var bagColour = split[0].Trim();
                var content = split[1];
                if (content.Trim().Equals("no other bags."))
                    nodes[bagColour] = new();
                else
                {
                    var subNodes = content.Split(',')
                        .Select(p =>
                        {
                            var match = contentMatch.Match(p);
                            return (int.Parse(match.Groups["count"].Value), match.Groups["description"].Value);
                        }).ToList();
                    nodes[bagColour] = subNodes;
                }
            }

            _graph = WeightedGraph<string, int>
                .Build(nodes, p => p.Key, p => p.Value.Select(q => new KeyValuePair<string, int>(q.contentColour, q.count)));
        }
    }
}
