using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day14 : AdventOfCode2017SolverBase
    {
        private string[] _hashes;

        public Day14(IIOProvider provider) : base(provider) => _hashes = new string[0];

        public override string Name => "Day 14: Disk Defragmentation";

        public override void Initialize(string input)
        {
            var hashes = new List<string>();
            for (var i = 0; i < 128; i++)
            {
                //TODO: Refactor to common library class for KnotHashing
                var knotHasher = new Day10(IO);
                knotHasher.Initialize($"256;{input}-{i}");
                var hash = knotHasher.ExecutePart2();
                hashes.Add(hash);
            }
            _hashes = hashes.ToArray();
        }

        public override string ExecutePart1()
        {
            return _hashes.SelectMany(p => p.Select(q => Convert.ToInt32($"{q}", 16)))
                .Select(p => Convert.ToString(p, 2).Count(q => q == '1'))
                .Sum().ToString();
        }

        public override string ExecutePart2()
        {
            var regionMarkers = new int[128, 128];

            var hashValues = _hashes.Select(p => p.Select(q => Convert.ToInt32($"{q}", 16)).ToArray())
                .ToArray();

            var highestRegion = 0;

            for (var y = 0; y < 128; y++)
            {
                for (var x = 0; x < 128; x++)
                {
                    if (!IsOccupied(x, y, hashValues)
                        || regionMarkers[x, y] != 0)
                    {
                        continue;
                    }

                    highestRegion++;

                    var fringe = new Queue<(int x, int y)>();
                    fringe.Enqueue((x, y));
                    while (fringe.Any())
                    {
                        var current = fringe.Dequeue();
                        regionMarkers[current.x, current.y] = highestRegion;

                        foreach (var neighbour in GetNeighbours(current.x, current.y)
                            .Where(p => IsOccupied(p.x, p.y, hashValues)
                            && regionMarkers[p.x, p.y] == 0))
                        {
                            fringe.Enqueue(neighbour);
                        }
                    }
                }
            }
            /*
            for (var y = 0; y < 20; y++)
            {
                var tmp = new StringBuilder();
                for (var x = 0; x < 20; x++)
                {
                    tmp.Append(regionMarkers[x, y] == 0 ? " . " : regionMarkers[x, y].ToString("000"));
                    tmp.Append(" ");
                }
                IO.LogLine(tmp);
            }
            */
            return highestRegion.ToString();
        }

        private IEnumerable<(int x, int y)> GetNeighbours(int x, int y)
        {
            if (x > 0)
                yield return (x - 1, y);
            if (y > 0)
                yield return (x, y - 1);
            if (x < 127)
                yield return (x + 1, y);
            if (y < 127)
                yield return (x, y + 1);
        }

        private bool IsOccupied(int x, int y, int[][] hashValues)
            => (hashValues[y][x / 4] & (1 << (3 - (x % 4)))) != 0;
    }
}
