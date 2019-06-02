using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day19 : AdventOfCode2017SolverBase
    {
        private char[,] _map;

        public Day19(IIOProvider provider) : base(provider) => _map = new char[0, 0];

        public override string Name => "Day 19: A Series of Tubes";

        public override void Initialize(string input)
        {
            var inputLines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            _map = new char[inputLines[0].Length, inputLines.Length];
            for (var y = 0; y < inputLines.Length; y++)
            {
                for (var x = 0; x < inputLines[y].Length; x++)
                    _map[x, y] = inputLines[y][x];
            }
        }

        public override string ExecutePart1()
            => RoutePacket().result;

        public override string ExecutePart2()
            => RoutePacket().stepCount.ToString();

        private (string result, int stepCount) RoutePacket()
        {
            var initialPosition = Enumerable.Range(0, _map.GetLength(0)).First(p => _map[p, 0] != ' ');

            var position = (x: initialPosition, y: 0);
            var direction = (dx: 0, dy: 1);
            var output = "";
            var steps = 0;

            while (true)
            {
                var currentValue = _map[position.x, position.y];
                if (currentValue == ' ')
                    break;

                if (currentValue == '+')
                {
                    foreach (var newDir in new (int dx, int dy)[] { (1, 0), (0, 1), (-1, 0), (0, -1) })
                    {
                        //Don't go back where you came from
                        if (newDir.dx == -direction.dx && newDir.dy == -direction.dy)
                            continue;

                        if (position.x + newDir.dx >= _map.GetLength(0) || position.x + newDir.dx < 0
                            || position.y + newDir.dy >= _map.GetLength(1) || position.y + newDir.dy < 0)
                        {
                            continue;
                        }

                        if (_map[position.x + newDir.dx, position.y + newDir.dy] != ' ')
                        {
                            direction = newDir;
                            break;
                        }
                    }
                }
                else
                {
                    if (currentValue != '|' && currentValue != '-')
                        output += currentValue;
                }
                steps++;
                position = (position.x + direction.dx, position.y + direction.dy);

            }

            return (output, steps);
        }
    }
}
