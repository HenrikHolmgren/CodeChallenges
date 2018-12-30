using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day1
{
    public class Day1Solver : IAdventOfCodeSolver
    {
        public string Description => "Day 1: Chronal Calibration";
        public int SortOrder => 1;

        private int[] _input;

        public void Initialize(string input)
        {
            _input = input.Split(Environment.NewLine).Select(p => int.Parse(p)).ToArray();
        }

        public string SolvePart1(bool silent = true)
        {
            var result = _input.Sum();

            return result.ToString();
        }

        public string SolvePart2(bool silent = true)
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
