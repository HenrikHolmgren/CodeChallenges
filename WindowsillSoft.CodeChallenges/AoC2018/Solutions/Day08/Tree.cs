using System.Collections.Generic;
using System.Linq;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day8
{
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
