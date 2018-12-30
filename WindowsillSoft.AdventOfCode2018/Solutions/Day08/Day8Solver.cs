using System;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day8
{
    public class Day8Solver : IAdventOfCodeSolver
    {
        private Tree _tree;

        public string Description => "Day 8: Memory Maneuver";

        public int SortOrder => 8;

        public void Initialize(string input)
        {
            var numericInputs = input
                .Split(" ")
                .Select(p => int.Parse(p))
                .ToArray();

            _tree = new Tree(numericInputs, 0);
        }

        public string SolvePart1(bool silent = true)
        {
            return _tree.MetadataSum().ToString();
        }

        public string SolvePart2(bool silent = true)
        {
            return _tree.Part2MetadataSum().ToString();
        }
    }
}
