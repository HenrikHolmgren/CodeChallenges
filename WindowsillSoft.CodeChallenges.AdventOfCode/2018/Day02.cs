using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day02 : AdventOfCode2018SolverBase
    {
        public override string Name => "Day 2: Inventory Management System";

        private string[] _input = new string[0];

        public Day02(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            _input = ReadAndSplitInput<string>(input).ToArray();
        }

        public override string ExecutePart1()
        {
            var candidates = _input.Select(p => p.GroupBy(q => q));
            var checksum = candidates.Count(p => p.Any(q => q.Count() == 2))
                * candidates.Count(p => p.Any(q => q.Count() == 3));

            return checksum.ToString();
        }

        public override string ExecutePart2()
        {
            for (int i = 0; i < _input.Length - 1; i++)
            {
                for (int j = i + 1; j < _input.Length; j++)
                {
                    var differences = Enumerable.Range(0, _input[0].Length).Where(p => _input[i][p] != _input[j][p]);
                    if (differences.Count() == 1)
                    {
                        IO.LogIfAttached(() =>
                            $"{_input[i]}, {_input[j]} -> {_input[i].Substring(0, differences.Single())}_{_input[i].Substring(differences.Single() + 1)}");

                        return $"{_input[i].Substring(0, differences.Single())}{_input[i].Substring(differences.Single() + 1)}";
                    }
                }
            }
            return "Unknown!";
        }
    }
}
