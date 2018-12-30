using System;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day2
{
    public class Day2Solver : AoC2018SolverBase
    {
        public override string Description => "Day 2: Inventory Management System";
        public override int SortOrder => 2;

        private string[] _input;

        public override void Initialize(string input)
        {
            _input = input.Split(Environment.NewLine);
        }

        public override string SolvePart1(bool silent = true)
        {
            var candidates = _input.Select(p => p.GroupBy(q => q));
            var checksum = candidates.Count(p => p.Any(q => q.Count() == 2))
                * candidates.Count(p => p.Any(q => q.Count() == 3));

            return checksum.ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            for (int i = 0; i < _input.Length - 1; i++)
            {
                for (int j = i + 1; j < _input.Length; j++)
                {
                    var differences = Enumerable.Range(0, _input[0].Length).Where(p => _input[i][p] != _input[j][p]);
                    if (differences.Count() == 1)
                    {
                        if (!silent)
                            Console.WriteLine($"{_input[i]}, {_input[j]} -> {_input[i].Substring(0, differences.Single())}_{_input[i].Substring(differences.Single() + 1)}");

                        return $"{_input[i].Substring(0, differences.Single())}{_input[i].Substring(differences.Single() + 1)}";
                    }
                }
            }
            return "Unknown!";
        }
    }
}
