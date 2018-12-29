using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day6
{
    public class Day6Solver : IProblemSolver
    {
        private const int DISTANCE_THRESHOLD = 10000;

        public string Description => "Day 6";

        public int SortOrder => 6;

        public void Solve()
        {
            var input = File.ReadAllLines("Day6/Day6Input.txt")
                .Select(p => p.Split(",").Select(q => int.Parse(q)).ToArray())
                .Select(p => new Coordinate(p[0], p[1])).ToList();

            var voronoi = new int[input.Max(p => p.X) + 1, input.Max(p => p.Y) + 1];
            for (int x = 0; x < voronoi.GetLength(0); x++)
                for (int y = 0; y < voronoi.GetLength(1); y++)
                {
                    var coordinate = new Coordinate(x, y);
                    var nearestNeighbor = input.Select((p, i) => new
                    {
                        item = p,
                        index = i,
                        dist = coordinate.Distance(p)
                    }).OrderBy(p => p.dist).ToList();
                    if (nearestNeighbor[0].dist == nearestNeighbor[1].dist)
                        voronoi[x, y] = -1;
                    else
                        voronoi[x, y] = nearestNeighbor[0].index;
                }

            var pointCounts = Enumerable.Range(0, input.Count).ToDictionary(p => p, _ => 0);
            pointCounts[-1] = 0;
            var tabuPoints = new HashSet<int> { -1 };

            for (int x = 0; x < voronoi.GetLength(0); x++)
                for (int y = 0; y < voronoi.GetLength(1); y++)
                {
                    if (x == 0 || y == 0 || x == voronoi.GetLength(0) - 1 || y == voronoi.GetLength(1) - 1)
                        tabuPoints.Add(voronoi[x, y]);
                    pointCounts[voronoi[x, y]]++;
                }

            int bestArea = 0;
            var bestCoordinate = default(Coordinate);

            foreach (var pointcount in pointCounts)
            {
                if (tabuPoints.Contains(pointcount.Key))
                    continue;
                if (pointcount.Value > bestArea)
                {
                    bestArea = pointcount.Value;
                    bestCoordinate = input[pointcount.Key];
                }
            }

            Console.WriteLine($"The largest area belongs to {bestCoordinate.X}, {bestCoordinate.Y} with area {bestArea}");

            int maxX = input.Max(p => p.X);
            int maxY = input.Max(p => p.Y);
            int hits = 0;
            for (int x = 0; x < maxX; x++)
                for (int y = 0; y < maxY; y++)
                {
                    var coordinate = new Coordinate(x, y);
                    var sumDist = input.Sum(p => coordinate.Distance(p));
                    if (sumDist < DISTANCE_THRESHOLD)
                        hits++;
                }

            Console.WriteLine($"There are {hits} points with a distance of less than {DISTANCE_THRESHOLD}.");
        }
    }

    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance(Coordinate c2)
            => Math.Abs(X - c2.X) + Math.Abs(Y - c2.Y);
        public Coordinate(int x, int y)
            => (X, Y) = (x, y);
    }
}


