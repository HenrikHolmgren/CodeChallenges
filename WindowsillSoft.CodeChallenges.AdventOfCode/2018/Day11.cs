using System;
using System.Threading.Tasks;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day11 : AdventOfCode2018SolverBase
    {
        public override string Name => "Day 11: Chronal Charge";

        static object lockObj = new object();

        private int[,] _sumSet = new int[0, 0];
        private int _gridSerial;

        public Day11(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            int gridSize;
            string? gridSizeStr;
            do
            {
                gridSizeStr = IO.RequestInput("Grid size?");
            } while (gridSizeStr == default || !Int32.TryParse(gridSizeStr, out gridSize));


            _gridSerial = int.Parse(input);

            if (gridSize * gridSize * 9L * 2 > Int32.MaxValue)
                Console.WriteLine($"Warning! Potential overflow! Grids of {gridSize} x {gridSize} will potentially sum to an integer overflow.");

            _sumSet = new int[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int sum = 0;
                    if (i != 0)
                        sum += _sumSet[i - 1, j];
                    if (j != 0)
                        sum += _sumSet[i, j - 1];
                    if (i != 0 && j != 0)
                        sum -= _sumSet[i - 1, j - 1];
                    sum += GetLevel(_gridSerial, i + 1, j + 1);
                    _sumSet[i, j] = sum;
                }
            }
        }

        public override string ExecutePart1()
        {
            int max = int.MinValue;
            int bestX = 0, bestY = 0;

            Parallel.For(0, _sumSet.GetLength(0) - 3, i =>
            {
                for (int j = 0; j < _sumSet.GetLength(0) - 3; j++)
                {
                    var sum = GetSum(_sumSet, i, j, 3);
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

            IO.LogIfAttached(() => $"Best 3x3 for puzzle {_gridSerial} is at {bestX},{bestY} with value {max}");

            return $"{bestX},{bestY}";
        }

        public override string ExecutePart2()
        {
            var max = 0;
            int bestX = 0, bestY = 0, bestSize = 0;

            Parallel.For(0, _sumSet.GetLength(0), i =>
            {
                for (int j = 0; j < _sumSet.GetLength(0); j++)
                    for (int sq = 0; sq < _sumSet.GetLength(0) - (Math.Max(i, j)); sq++)
                    {
                        var sum = GetSum(_sumSet, i, j, sq);
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

            IO.LogIfAttached(() => $"Best nxn for puzzle {_gridSerial} is at {bestX},{bestY},{bestSize} with value {max}");

            return $"{bestX},{bestY},{bestSize}";
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
