using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day13 : AdventOfCode2017SolverBase
    {
        public override string Name => "Day 13: Packet Scanners";

        private readonly Dictionary<int, int> _ranges = new Dictionary<int, int>();
        private Dictionary<int, int> _periods = new Dictionary<int, int>();

        public Day13(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            foreach (var layer in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var stats = layer.Split(':').Select(p => Int32.Parse(p)).ToArray();
                _ranges[stats[0]] = stats[1];

                _periods = _ranges.ToDictionary(p => p.Key, p => 2 * p.Value - 2);
            }
        }

        public override string ExecutePart1() => (GetPenalty(0) - 1).ToString();

        public override string ExecutePart2()
            => Enumerable.Range(0, Int32.MaxValue)
                .First(p => GetPenalty(p) == 0)
                .ToString();

        private int GetPenalty(int delay)
        {
            return _periods.Where(p => (p.Key + delay) % p.Value == 0)
                .Select(p => _ranges[p.Key] * p.Key + (p.Key == 0 ? 1 : 0))
                .Sum();
        }
    }
}
