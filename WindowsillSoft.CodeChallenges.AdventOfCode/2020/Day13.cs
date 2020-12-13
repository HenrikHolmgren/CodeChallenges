using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day13 : AdventOfCode2020SolverBase
    {
        private int _departureTime;
        private int[] _busses = new int[0];

        public Day13(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 13: Shuttle Search";
        public override string ExecutePart1()
        {
            var bestBus = _busses
                .Where(p => p != -1)
                .OrderBy(p => p - _departureTime % p)
                .First();
            return (bestBus * (bestBus - _departureTime % bestBus)).ToString();
        }

        public override string ExecutePart2()
        {
            long increment = _busses[0];
            long result = 0;

            for (int i = 1; i < _busses.Length; i++)
            {
                if (_busses[i] == -1) continue;

                while ((result + i) % _busses[i] != 0)
                    result += increment;

                increment *= _busses[i];
            }
            return result.ToString();
        }

        public override void Initialize(string input)
        {
            var split = input.Split('\r', '\n');
            _departureTime = int.Parse(split[0]);
            _busses = split[1].Split(',')
                .Select(p => p == "x" ? -1 : int.Parse(p))
                .ToArray();
        }
    }
}
