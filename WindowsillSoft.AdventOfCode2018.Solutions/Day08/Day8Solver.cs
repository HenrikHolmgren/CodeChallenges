using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day8
{
    public class Day8Solver : IProblemSolver
    {
        public string Description => "Day 8";

        public int SortOrder => 8;

        public void Solve()
        {
            //var input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2"
            var input = File.ReadAllText("Day8/Day8Input.txt")
                .Split(" ")
                .Select(p => int.Parse(p))
                .ToArray();

            var root = new Tree(input, 0);

            Console.WriteLine(root.MetadataSum());

            Console.WriteLine(root.Part2MetadataSum());
        }
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
                return Metadata.Sum(p => p-1 < Children.Count ? Children[p-1].Part2MetadataSum() : 0);
            }
        }
    }
}
