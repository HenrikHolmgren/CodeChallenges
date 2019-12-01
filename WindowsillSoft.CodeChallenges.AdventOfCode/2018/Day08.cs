using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day08 : AdventOfCode2018SolverBase
    {
        private Tree? _tree;

        public Day08(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 8: Memory Maneuver";

        public override void Initialize(string input)
        {
            var numericInputs = ReadAndSplitInput<int>(input).ToArray();

            _tree = new Tree(numericInputs, 0);
        }

        public override string ExecutePart1()
        {
            return _tree?.MetadataSum().ToString() ?? "Uninitialized!";
        }

        public override string ExecutePart2()
        {
            return _tree?.Part2MetadataSum().ToString() ?? "Uninitialized!";
        }

        public class Tree
        {
            public Tree(int[] input, int offset)
            {
                StartIndex = offset;
                var children = input[offset++];
                var metadata = input[offset++];
                for (int i = 0; i < children; i++)
                {
                    var child = new Tree(input, offset);
                    offset = child.EndIndex;
                    Children.Add(child);
                }
                for (int i = 0; i < metadata; i++)
                    Metadata.Add(input[offset++]);
                EndIndex = offset;
            }

            public int StartIndex { get; }
            public int EndIndex { get; }

            public List<Tree> Children { get; } = new List<Tree>();
            public List<int> Metadata { get; } = new List<int>();

            public int MetadataSum() =>
                Metadata.Sum()
                + Children.Sum(p => p.MetadataSum());

            public int Part2MetadataSum()
            {
                if (!Children.Any())
                    return Metadata.Sum();
                else
                {
                    return Metadata.Sum(p => p - 1 < Children.Count ? Children[p - 1].Part2MetadataSum() : 0);
                }
            }
        }
    }
}
