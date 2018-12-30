using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode2018.Core.Geometry;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day25
{
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
