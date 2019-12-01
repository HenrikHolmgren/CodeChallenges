using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Geometry;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day25 : AdventOfCode2018SolverBase
    {
        private ManhattanPointNInt[] _anomalies = new ManhattanPointNInt[0];

        public Day25(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 25: Four-Dimensional Adventure";

        public override void Initialize(string input)
        {
            _anomalies = ReadAndSplitInput<string>(input)
                .Select(p => p.Split(',').Select(q => int.Parse(q)).ToArray())
                .Select(p => new ManhattanPointNInt(p))
                .ToArray();
        }

        public override string ExecutePart1()
        {
            List<Constellation> constellations = new List<Constellation>();
            foreach (var point in _anomalies)
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

            return constellations.Count.ToString();
        }

        public override string ExecutePart2()
        {
            return "Have a reindeer";
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
}

