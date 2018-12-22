using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day17
{
    public class Day17Solver : IProblemSolver
    {
        public string Description => "Day 17: Reservoir Research";

        public int SortOrder => 17;

        public void Solve()
        {
            var (miny, input) = GetInput("Day17/Day17Input.txt");
            //var input = GetInput("Day17/Day17Test2.txt");

            //WriteState(input);

            int spoutPos = 0;
            for (int i = 0; i < input.GetLength(0); i++)
                if (input[i, 0] == '+') spoutPos = i;

            var waterCount = CountWaters(input, false);
            var prevWaterCount = -1;

            int iterations = 0;
            while (prevWaterCount != waterCount)
            {
                iterations++;
                AddFall(input, spoutPos, 1);
                //WriteState(input);
                //Console.ReadKey(true);
                prevWaterCount = waterCount;
                waterCount = CountWaters(input, false);
            }

            WriteStateToFile(input, "C:\\temp\\Day17Out.txt");
            Console.WriteLine($"Steady state reached with {waterCount} tiles of water ({CountWaters(input, true) - miny} wet tiles) at iteration {iterations}.");

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
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        private (int, char[,]) GetInput(string inputFile)
        {
            var parser = new Regex(@"(?'c1'[x|y])=(?'v1'\d+), (?'c2'[x|y])=(?'v2a'\d+)\.\.(?'v2b'\d+)", RegexOptions.Compiled);
            var lines = File.ReadAllLines(inputFile)
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
                lines.Where(p => p.c2 == 'y').Min(p => p.v2a))-1;

            //minY = Math.Min(0, minY);
            Console.WriteLine($"Playing field is ({minX}, {minY}) to ({maxX}, {maxY}).");

            var field = new char[maxX - minX + 1, maxY - minY + 1];
            foreach (var input in lines)
            {
                foreach (var rangeVar in Enumerable.Range(input.v2a, input.v2b - input.v2a + 1))
                {
                    if (input.c1 == 'x')
                        field[input.v1 - minX, rangeVar - minY] = '#';
                    else
                        field[rangeVar - minX, input.v1 - minY] = '#';
                }
            }

            field[500 - minX, 0] = '+';
            return (minY, field);
        }
    }
}
