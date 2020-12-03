using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day03 : AdventOfCode2020SolverBase
    {
        protected bool[][] _map = new bool[0][];

        protected record Position(int X, int Y);
        public Day03(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 3: Toboggan Trajectory";

        public override string ExecutePart1()
        {
            return GetCollisionsForCourse(3, 1).ToString();
        }

        public override string ExecutePart2()
        {
            long accumulator = 1;
            foreach (var candidateDirection in new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) })
                accumulator *= GetCollisionsForCourse(candidateDirection.Item1, candidateDirection.Item2);
            return accumulator.ToString();
        }

        private long GetCollisionsForCourse(int xIncrement, int yIncrement)
        {
            int x = 0, y = 0;
            long count = 0;
            while (y < _map.Length)
            {
                if (_map[y][x])
                    count++;
                x = (x + xIncrement) % _map[0].Length;
                y += yIncrement;
            }
            return count;
        }

        public override void Initialize(string input)
        {
            _map = ReadAndSplitInput<string>(input)
                .Select(p => p.Select(
                    q => q == '#').ToArray())
                    .ToArray();
        }
    }
}
