using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day18
{
    public class Day18Solver : AoC2018SolverBase
    {
        private char[,] _map;

        public override string Description => "Day 18: Settlers of the North Pole";

        public override int SortOrder => 18;

        public override void Initialize(string input)
        {
            var mapLines = input.Split(Environment.NewLine);
            _map = new char[mapLines[0].Length, mapLines.Length];
            for (int y = 0; y < mapLines.Length; y++)
                for (int x = 0; x < mapLines[0].Length; x++)
                    _map[x, y] = mapLines[y][x];
        }

        public override string SolvePart1(bool silent = true)
        {
            var map = _map;

            var backBuffer = new char[map.GetLength(0), map.GetLength(1)];

            for (int i = 0; i < 10; i++)
            {
                foreach (var (x, y) in
                from x in Enumerable.Range(0, map.GetLength(0))
                from y in Enumerable.Range(0, map.GetLength(1))
                select (x, y))
                    backBuffer[x, y] = GenerateNextState(map, x, y);

                var tmp = backBuffer;
                backBuffer = map;
                map = tmp;
            }

            return Score(map).ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            var history = new Dictionary<string, long>();

            var map = this._map;
            var backBuffer = new char[map.GetLength(0), map.GetLength(1)];

            long index = 0;
            while (true)
            {
                var stringMap = Stringify(map);
                if (history.ContainsKey(stringMap))
                    break;
                else
                    history[stringMap] = index;

                foreach (var (x, y) in
                from x in Enumerable.Range(0, map.GetLength(0))
                from y in Enumerable.Range(0, map.GetLength(1))
                select (x, y))
                {
                    backBuffer[x, y] = GenerateNextState(map, x, y);
                }

                var tmp = backBuffer;
                backBuffer = map;
                map = tmp;
                index++;
            }

            var repeatStart = history[Stringify(map)];
            if (!silent)
                Console.WriteLine($"Pattern detected after {history.Count} rounds ({index}), repeating a state from {repeatStart} ago.");

            var target = 1_000_000_000;
            var repeatDuration = index - repeatStart;
            var targetIndex = (target - repeatStart) % repeatDuration + repeatStart;

            return Score(history.Single(p => p.Value == targetIndex).Key).ToString();
        }

        private int Score(string map)
            => map.Count(p => p == '|') * map.Count(p => p == '#');

        private int Score(char[,] map)
        {
            int lumber = 0, trees = 0;

            foreach (var set in
                from x in Enumerable.Range(0, map.GetLength(0))
                from y in Enumerable.Range(0, map.GetLength(1))
                select (x, y))
            {
                if (map[set.x, set.y] == '|') trees++;
                else if (map[set.x, set.y] == '#') lumber++;
            }
            return lumber * trees;
        }

        private char GenerateNextState(char[,] map, int x, int y)
        {
            var neighbours = GetNeighbourhood(map, x, y).Select(p => map[p.x, p.y]);
            if (map[x, y] == '.' && neighbours.Count(p => p == '|') >= 3)
                return '|';
            if (map[x, y] == '|' && neighbours.Count(p => p == '#') >= 3)
                return '#';
            if (map[x, y] == '#' &&
                (neighbours.Count(p => p == '|') == 0 || neighbours.Count(p => p == '#') == 0))
                return '.';
            return map[x, y];
        }

        private IEnumerable<(int x, int y)> GetNeighbourhood(char[,] map, int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((x, y) == (i, j)) continue;
                    if (i < 0 || i >= map.GetLength(0)) continue;
                    if (j < 0 || j >= map.GetLength(1)) continue;
                    yield return (i, j);
                }
            }
        }

        private static void WriteState(char[,] input)
        {
            (Console.CursorLeft, Console.CursorTop) = (0, 0);

            for (int y = 0; y < input.GetLength(1); y++)
            {
                for (int x = 0; x < input.GetLength(0); x++)
                {
                    if (input[x, y] != default)
                        Console.Write(input[x, y]);
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        private string Stringify(char[,] input)
        {
            StringBuilder builder = new StringBuilder();
            for (int y = 0; y < input.GetLength(1); y++)
                for (int x = 0; x < input.GetLength(0); x++)
                    builder.Append(input[x, y]);
            return builder.ToString();
        }
    }
}
