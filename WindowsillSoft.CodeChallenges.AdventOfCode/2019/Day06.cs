using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day06 : AdventOfCode2019SolverBase
    {
        private OrbitTree _orbits;

        public Day06(IIOProvider provider) : base(provider) { _orbits = new OrbitTree("COM"); }

        public override string Name => "Day 6: Universal Orbit Map";

        public override string ExecutePart1()
        {
            return _orbits.CountOrbits().ToString();
        }

        public override string ExecutePart2()
        {
            return _orbits.MinimumTransferCount("SAN", "YOU").ToString();
        }

        public override void Initialize(string input) => _orbits = new OrbitTree(ReadAndSplitInput<string>(input));

        public class OrbitTree
        {
            public string Label { get; }
            public IReadOnlyList<OrbitTree> Children { get; }

            public OrbitTree(string label)
            {
                Label = label;
                Children = Enumerable.Empty<OrbitTree>().ToList();
            }

            public OrbitTree(IEnumerable<string> input)
            {
                var relations = input
                    .Select(p => p.Split(")"))
                    .ToLookup(p => p[0], p => p[1]);

                Label = "COM";
                var commonChildren = relations["COM"];

                Children = commonChildren
                    .Select(p => new OrbitTree(p, relations))
                    .ToList();
            }

            private OrbitTree(string node, ILookup<string, string> relations)
            {
                Label = node;
                Children = relations[node]
                    .Select(p => new OrbitTree(p, relations))
                    .ToList();
            }

            public override string ToString()
                => $"{Label} {{{String.Join(", ", Children)}}}";

            public int CountOrbits(int rootOfset = 0) => rootOfset + Children.Sum(p => p.CountOrbits(rootOfset + 1));

            public int MinimumTransferCount(string origin, string destination)
            {
                var originPath = LookupChildPath(origin);
                var destinationPath = LookupChildPath(destination);

                var commonNodes = originPath.Intersect(destinationPath);
                return originPath.Count()
                    + destinationPath.Count()
                    - 2 * commonNodes.Count() //Each node common between paths are counted twice
                    - 2; //source and destination nodes should not be counted
            }

            private IEnumerable<OrbitTree> LookupChildPath(string destination)
            {
                if (Label.Equals(destination))
                    yield return this;
                else
                {
                    foreach (var child in Children)
                    {
                        var childpath = child.LookupChildPath(destination).ToArray();
                        if (childpath.Any())
                        {
                            foreach (var node in childpath)
                                yield return node;
                            yield return this;
                        }
                    }
                }
            }
        }
    }
}
