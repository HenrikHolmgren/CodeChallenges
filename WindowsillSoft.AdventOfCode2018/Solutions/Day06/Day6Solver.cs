using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day6
{
    public class Day6Solver : IAdventOfCodeSolver
    {
        private int _distanceThreshold = 1;
        private List<Coordinate> _coordinates;

        public string Description => "Day 6: Chronal Coordinates";

        public int SortOrder => 6;

        public void Initialize(string input)
        {
            _distanceThreshold = int.Parse(input.Split(Environment.NewLine)
                .First());

            _coordinates = input.Split(Environment.NewLine)
                .Skip(1)
                .Select(p => p.Split(",").Select(q => int.Parse(q)).ToArray())
                .Select(p => new Coordinate(p[0], p[1])).ToList();
        }

        public string SolvePart1(bool silent = true)
        {
            var voronoi = new int[_coordinates.Max(p => p.X) + 1, _coordinates.Max(p => p.Y) + 1];
            for (int x = 0; x < voronoi.GetLength(0); x++)
                for (int y = 0; y < voronoi.GetLength(1); y++)
                {
                    var coordinate = new Coordinate(x, y);
                    var nearestNeighbor = _coordinates.Select((p, i) => new
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

            var pointCounts = Enumerable.Range(0, _coordinates.Count).ToDictionary(p => p, _ => 0);
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
                    bestCoordinate = _coordinates[pointcount.Key];
                }
            }

            if (!silent)
                Console.WriteLine($"The largest area belongs to {bestCoordinate.X}, {bestCoordinate.Y} with area {bestArea}");

            return bestArea.ToString();
        }

        public string SolvePart2(bool silent = true)
        {
            int maxX = _coordinates.Max(p => p.X);
            int maxY = _coordinates.Max(p => p.Y);
            int hits = 0;
            for (int x = 0; x < maxX; x++)
                for (int y = 0; y < maxY; y++)
                {
                    var coordinate = new Coordinate(x, y);
                    var sumDist = _coordinates.Sum(p => coordinate.Distance(p));
                    if (sumDist < _distanceThreshold)
                        hits++;
                }

            if (!silent)
                Console.WriteLine($"There are {hits} points with a distance of less than {_distanceThreshold}.");
            return hits.ToString();
        }
    }
}


