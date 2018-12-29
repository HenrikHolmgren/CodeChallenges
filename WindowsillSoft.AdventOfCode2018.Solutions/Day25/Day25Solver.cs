using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;
using WindowsillSoft.AdventOfCode2018.Core.Geometry;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day25
{
    public class Day25Solver : IProblemSolver
    {
        public string Description => "Day 25: Four-Dimensional Adventure";

        public int SortOrder => 25;

        public void Solve()
        {
            var input = File.ReadAllLines("Day25/Day25Input.txt")
                .Select(p => p.Split(',').Select(q => int.Parse(q)).ToArray())
                .Select(p => new ManhattanPointNInt(p))
                .ToArray();

            List<Constellation> constellations = new List<Constellation>();
            foreach (var point in input)
            {
                var matchingConstellations = constellations.Where(p => p.Contains(point)).ToList();
                Constellation target;

                if (matchingConstellations.Count == 0)
                {
                    target = new Constellation(new[] { point });
                    constellations.Add(target);
                    continue;
                }

                if (matchingConstellations.Count == 1)
                    target = matchingConstellations.Single();
                else
                {
                    target = matchingConstellations.Aggregate((p, q) => p.Merge(q));
                    foreach (var constellation in matchingConstellations)
                        constellations.Remove(constellation);
                    constellations.Add(target);
                }
                target.Add(point);
            }

            Console.WriteLine($"After adding {input.Length} fix-points, there are a total of {constellations.Count} constellations");
        }
    }

    public class Constellation
    {
        private List<ManhattanPointNInt> _points;
        private readonly ManhattanPointNInt[] _boundingCube = new ManhattanPointNInt[2];

        public Constellation(IEnumerable<ManhattanPointNInt> points)
        {
            _boundingCube = new[] { points.First(), points.First() };
            _points = new List<ManhattanPointNInt>();
            foreach (var newpoint in points)
                Add(newpoint);
        }

        public void Add(ManhattanPointNInt point)
        {
            _boundingCube[0] = _boundingCube[0].Zip(point, (p, q) => Math.Min(p, q));
            _boundingCube[1] = _boundingCube[1].Zip(point, (p, q) => Math.Max(p, q));
            _points.Add(point);
        }

        public Constellation Merge(Constellation other)
            => new Constellation(_points.Concat(other._points));

        public bool Contains(ManhattanPointNInt point)
            => point.DistanceToCube(_boundingCube[0], _boundingCube[1]) <= 3
                && _points.Any(p => p.Distance(point) <= 3);
    }
}
