using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day06 : AdventOfCode2018SolverBase
    {
        private int _distanceThreshold = 1;
        private List<Coordinate> _coordinates = new List<Coordinate>();

        public Day06(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 6: Chronal Coordinates";

        public override void Initialize(string input)
        {
            string? distanceThreshold;
            do
            {
                distanceThreshold = IO.RequestInput("Distance threshold?");
            } while (distanceThreshold == default || !Int32.TryParse(distanceThreshold, out _distanceThreshold));
            
            _coordinates = ReadAndSplitInput<string>(input)
                .Select(p => p.Split(",").Select(q => int.Parse(q)).ToArray())
                .Select(p => new Coordinate(p[0], p[1])).ToList();
        }

        public override string ExecutePart1()
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

            IO.LogIfAttached(() => $"The largest area belongs to {bestCoordinate.X}, {bestCoordinate.Y} with area {bestArea}");

            return bestArea.ToString();
        }

        public override string ExecutePart2()
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

            IO.LogIfAttached(() => $"There are {hits} points with a distance of less than {_distanceThreshold}.");
            return hits.ToString();
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
}
