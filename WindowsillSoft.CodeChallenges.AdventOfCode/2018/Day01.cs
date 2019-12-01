using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day01 : AdventOfCode2018SolverBase
    {
        public override string Name => "Day 1: Chronal Calibration";

        private int[] _input = new int[0];

        public Day01(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            _input = ReadAndSplitInput<int>(input).ToArray();
        }

        public override string ExecutePart1()
        {
            var result = _input.Sum();

            return result.ToString();
        }

        public override string ExecutePart2()
        {
            var frequencies = new HashSet<int>();
            int currentFrequency = 0;
            frequencies.Add(currentFrequency);

            while (true)
            {
                for (int i = 0; i < _input.Length; i++)
                {
                    currentFrequency += _input[i];
                    if (frequencies.Contains(currentFrequency))
                        return currentFrequency.ToString();

                    frequencies.Add(currentFrequency);
                }
            }
        }
    }
}
