using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day8
{
    public class Day8Solver : AoC2018SolverBase
    {
        private Tree _tree;

        public override string Description => "Day 8: Memory Maneuver";

        public override int SortOrder => 8;

        public override void Initialize(string input)
        {
            var numericInputs = input
                .Split(" ")
                .Select(p => int.Parse(p))
                .ToArray();

            _tree = new Tree(numericInputs, 0);
        }

        public override string SolvePart1(bool silent = true)
        {
            return _tree.MetadataSum().ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            return _tree.Part2MetadataSum().ToString();
        }
    }
}
