using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day17 : AdventOfCode2018SolverBase
    {
        private int _minY;
        private char[,] _field = new char[0, 0];

        public Day17(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 17: Reservoir Research";

        public override void Initialize(string input)
        {
            var parser = new Regex(@"(?'c1'[x|y])=(?'v1'\d+), (?'c2'[x|y])=(?'v2a'\d+)\.\.(?'v2b'\d+)", RegexOptions.Compiled);
            var lines = input.Split('\n')
                .Select(p => parser.Match(p))
                .Select(p => new
                {
                    c1 = p.Groups["c1"].Value[0],
                    v1 = int.Parse(p.Groups["v1"].Value),
                    c2 = p.Groups["c2"].Value[0],
                    v2a = int.Parse(p.Groups["v2a"].Value),
                    v2b = int.Parse(p.Groups["v2b"].Value)
                })
                .ToList();

            var maxX = Math.Max(lines.Where(p => p.c1 == 'x').Max(p => p.v1),
                lines.Where(p => p.c2 == 'x').Max(p => p.v2b)) + 1;
            var minX = Math.Min(lines.Where(p => p.c1 == 'x').Min(p => p.v1),
                lines.Where(p => p.c2 == 'x').Min(p => p.v2a)) - 1;

            var maxY = Math.Max(lines.Where(p => p.c1 == 'y').Max(p => p.v1),
                lines.Where(p => p.c2 == 'y').Max(p => p.v2b));
            var minY = Math.Min(lines.Where(p => p.c1 == 'y').Min(p => p.v1),
                lines.Where(p => p.c2 == 'y').Min(p => p.v2a)) - 1;

            Console.WriteLine($"Playing field is ({minX}, {minY}) to ({maxX}, {maxY}).");

            var field = new char[maxX - minX + 1, maxY - minY + 1];
            foreach (var line in lines)
            {
                foreach (var rangeVar in Enumerable.Range(line.v2a, line.v2b - line.v2a + 1))
                {
                    if (line.c1 == 'x')
                        field[line.v1 - minX, rangeVar - minY] = '#';
                    else
                        field[rangeVar - minX, line.v1 - minY] = '#';
                }
            }

            field[500 - minX, 0] = '+';
            _minY = minY;
            _field = field;
        }

        public override string ExecutePart1()
        {
            int spoutPos = 0;

            var field = (char[,])_field.Clone();

            for (int i = 0; i < field.GetLength(0); i++)
                if (field[i, 0] == '+')
                    spoutPos = i;

            var waterCount = CountWaters(field, false);
            var prevWaterCount = -1;

            int iterations = 0;
            while (prevWaterCount != waterCount)
            {
                iterations++;
                AddFall(field, spoutPos, 1);

                prevWaterCount = waterCount;
                waterCount = CountWaters(field, false);
            }
            return CountWaters(field, true).ToString();
        }

        public override string ExecutePart2()
        {
            int spoutPos = 0;

            var field = (char[,])_field.Clone();

            for (int i = 0; i < field.GetLength(0); i++)
                if (field[i, 0] == '+')
                    spoutPos = i;

            var waterCount = CountWaters(field, false);
            var prevWaterCount = -1;

            int iterations = 0;
            while (prevWaterCount != waterCount)
            {
                iterations++;
                AddFall(field, spoutPos, 1);

                prevWaterCount = waterCount;
                waterCount = CountWaters(field, false);
            }
            return CountWaters(field, false).ToString();
        }

        private int CountWaters(char[,] input, bool includeWet)
        {
            int waterCount = 0;
            foreach (var (x, y) in
                from x in Enumerable.Range(0, input.GetLength(0))
                from y in Enumerable.Range(0, input.GetLength(1))
                select (x, y))
                if ((includeWet && input[x, y] == '|') || input[x, y] == '~')
                    waterCount++;

            return waterCount;
        }

        private void AddFall(char[,] input, int x, int y)
        {
            int waterPos = y;
            for (; waterPos < input.GetLength(1); waterPos++)
            {
                if (input[x, waterPos] == '#' || input[x, waterPos] == '~')
                    break;
                else
                    input[x, waterPos] = '|';
            }

            if (waterPos >= input.GetLength(1))
                return;

            bool flood = true;
            int floodStart = x;
            while (floodStart >= 0)
            {
                if (input[floodStart, waterPos] != '#' &&
                    input[floodStart, waterPos] != '~')
                {
                    flood = false;
                    break;
                }
                else if (input[floodStart, waterPos - 1] == '#')
                    break;

                input[floodStart, waterPos - 1] = '|';
                floodStart--;
            }

            int floodEnd = x;
            while (floodEnd < input.GetLength(0))
            {
                if (input[floodEnd, waterPos] != '#' &&
                    input[floodEnd, waterPos] != '~')
                {
                    flood = false;
                    break;
                }
                else if (input[floodEnd, waterPos - 1] == '#')
                    break;
                input[floodEnd, waterPos - 1] = '|';
                floodEnd++;
            }

            if (flood)
            {
                for (int i = floodStart + 1; i < floodEnd; i++)
                    input[i, waterPos - 1] = '~';
            }
            else
            {
                if (floodStart >= 0 && input[floodStart, waterPos - 1] != '#')
                    AddFall(input, floodStart, waterPos - 1);
                if (floodEnd < input.GetLength(0) && input[floodEnd, waterPos - 1] != '#')
                    AddFall(input, floodEnd, waterPos - 1);
            }
        }

        private static void WriteStateToFile(char[,] input, string outFile)
        {
            var lines = new List<string>();

            for (int y = 0; y < input.GetLength(1); y++)
            {
                string buff = string.Empty;
                for (int x = 0; x < input.GetLength(0); x++)
                {
                    if (input[x, y] == default)
                        buff += ' ';
                    else
                        buff += input[x, y];
                }
                lines.Add(buff);
            }

            File.WriteAllLines(outFile, lines);
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
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
        }
    }
}
