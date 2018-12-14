using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day11
{
    public class Day11Solver : IProblemSolver
    {
        public string Description => "Day 11";

        public int SortOrder => 11;

        static object lockObj = new object();

        public void Solve()
        {
            int serial, gridSize;
            do Console.Write("Please enter serial: ");
            while (!int.TryParse(Console.ReadLine(), out serial));

            do Console.Write("Please enter grid size: ");
            while (!int.TryParse(Console.ReadLine(), out gridSize));

            if (gridSize * gridSize * 9L * 2 > Int32.MaxValue)
                Console.WriteLine($"Warning! Potential overflow! Grids of {gridSize} x {gridSize} will potentially sum to an integer overflow.");
            var sw = Stopwatch.StartNew();

            int[,] sumSet = new int[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int sum = 0;
                    if (i != 0) sum += sumSet[i - 1, j];
                    if (j != 0) sum += sumSet[i, j - 1];
                    if (i != 0 && j != 0) sum -= sumSet[i - 1, j - 1];
                    sum += GetLevel(serial, i + 1, j + 1);
                    sumSet[i, j] = sum;
                }
            }

            int max = int.MinValue;
            int bestX = 0, bestY = 0, bestSize = 0;

            Parallel.For(0, gridSize - 3, i =>
              {
                  for (int j = 0; j < gridSize - 3; j++)
                  {
                      var sum = GetSum(sumSet, i, j, 3);
                      if (sum > max)
                      {
                          lock (lockObj)
                          {
                              if (sum > max)
                              {
                                  max = sum;
                                  bestX = i + 2;
                                  bestY = j + 2;
                              }
                          }
                      }
                  }
              });

            Console.WriteLine($"Best 3x3 for puzzle {serial} is at {bestX},{bestY} with value {max}");
            max = 0;

            Parallel.For(0, gridSize, i =>
            {
                for (int j = 0; j < gridSize; j++)
                    for (int sq = 0; sq < gridSize - (Math.Max(i, j)); sq++)
                    {
                        var sum = GetSum(sumSet, i, j, sq);
                        if (sum > max)
                        {
                            lock (lockObj)
                            {
                                if (sum > max)
                                {
                                    max = sum;

                                    bestX = i + 2;
                                    bestY = j + 2;
                                    bestSize = sq;
                                }
                            }
                        }
                    }
            });

            Console.WriteLine($"Best nxn for puzzle {serial} is at {bestX},{bestY},{bestSize} with value {max}");

            Console.WriteLine($"Elapsed: {sw.Elapsed}");
        }

        private static int GetLevel(int serial, int x, int y)
        {
            var level = ((x + 10) * y + serial) * (x + 10);
            level = (level / 100) % 10 - 5;
            return level;
        }

        private int GetSum(int[,] sumset, int left, int top, int squareSize)
        {
            var sum = sumset[left + squareSize, top + squareSize];
            if (squareSize == 0)
                return sum;
            sum -= sumset[left, top + squareSize];
            sum -= sumset[left + squareSize, top];
            sum += sumset[left, top];
            return sum;
        }
    }
}
