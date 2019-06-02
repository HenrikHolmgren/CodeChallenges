using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day02 : AdventOfCode2017SolverBase
    {
        private int[][] _sheet;
        public Day02(IIOProvider provider) : base(provider)
            => _sheet = new int[0][];
        public override string Name => "Day 2: Corruption Checksum";

        public override void Initialize(string input)
        {
            _sheet = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split("\t", StringSplitOptions.RemoveEmptyEntries).Select(q => Int32.Parse(q)).ToArray())
                .ToArray();
        }

        public override string ExecutePart1()
            => _sheet.Select(p => p.Max() - p.Min()).Sum().ToString();

        public override string ExecutePart2()
            => _sheet.Select(p => GetPart2Division(p)).Sum().ToString();

        private int GetPart2Division(int[] line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                for (var j = i + 1; j < line.Length; j++)
                {
                    if (line[i] % line[j] == 0)
                        return line[i] / line[j];
                    else if (line[j] % line[i] == 0)
                        return line[j] / line[i];
                }
            }

            throw new InvalidOperationException("No evenly divisible candidates found!");
        }
    }
}
