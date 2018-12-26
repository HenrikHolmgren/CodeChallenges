using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day23
{
    public class Day23Solver : IProblemSolver
    {
        public string Description => "Day 23: Experimental Emergency Teleportation";

        public int SortOrder => 23;

        public void Solve()
        {
            var matcher = new Regex(@"pos=<(?'x'-?\d+),(?'y'-?\d+),(?'z'-?\d+)>, r=(?'r'\d+)", RegexOptions.Compiled);

            var input = File.ReadAllLines("Day23/Day23Test2.txt")
                .Select(p => matcher.Match(p))
                .Select(p => new Drone(
                    long.Parse(p.Groups["x"].Value),
                    long.Parse(p.Groups["y"].Value),
                    long.Parse(p.Groups["z"].Value),
                    long.Parse(p.Groups["r"].Value)))
                .ToList();

            var strongestDrone = input.OrderByDescending(p => p.R).First();
            var inRangeOfStrongest = input.Count(p => p.Distance(strongestDrone) < strongestDrone.R);
            Console.WriteLine($"There are {inRangeOfStrongest} drones in range of {strongestDrone}");

            var searchspace = new SearchSpace(input);
            //(xMin: input.Min(p => p.X), xMax: input.Max(p => p.X),
            //    yMin: input.Min(p => p.Y), yMax: input.Max(p => p.Y),
            //    zMin: input.Min(p => p.Z), zMax: input.Max(p => p.Z));
            Console.WriteLine($"Initial best coverage: {searchspace.BestCoverageEstimate()}");

            List<SearchSpace> Fringe = new List<SearchSpace>() { searchspace };
            //var bestSpace = searchspace;
            (long x, long y, long z) bestPlacement = default;

            //'good' initial coverage guess
            long bestCoverage = inRangeOfStrongest;

            while (Fringe.Any())
            {
                var candidate = Fringe.First();
                Fringe.Remove(candidate);

                if (candidate.BestCoverageEstimate() < bestCoverage)
                    continue;

                if (candidate.LargestSize <= 2)
                {
                    var coord = candidate.BestCoverageLocation();
                    var coverage = candidate.GetCoverage(coord);
                    if (coverage > bestCoverage
                        || coverage == bestCoverage && Length(coord) < Length(bestPlacement))
                    {
                        bestCoverage = coverage;
                        bestPlacement = coord;

                        Console.WriteLine($"Better coord {bestPlacement} found with coverage {bestCoverage}");
                    }
                }
                else
                {
                    foreach (var part in candidate.Partition())
                        Fringe.Insert(0, part);
                    //Fringe.AddRange(searchspace.Partition());
                }
            }
            Console.WriteLine();
            Console.WriteLine(Length(bestPlacement));
            //Console.WriteLine($"Initial search space size: {searchspace.LargestSize}");
            //while (searchspace.LargestSize > 8)
            //{
            //    var partitions = searchspace.Partition()
            //        .Select(p => new { Space = p, Coverage = p.BestCoverageEstimate() }).ToList();
            //    Console.WriteLine($"  Candiates: {string.Join(", ", partitions.Select(p => p.Space + ": " + p.Coverage))}");
            //    searchspace = partitions.OrderByDescending(p => p.Coverage)
            //        .ThenBy(p => p.Space.X.from)
            //        .ThenBy(p => p.Space.Y.from)
            //        .ThenBy(p => p.Space.Z.from)
            //        .First().Space;
            //    Console.WriteLine($"Partitioned to new space: {searchspace} with best coverage {partitions.OrderByDescending(p => p.Coverage).First().Coverage}");
            //}
            //Console.WriteLine();
            //Console.WriteLine(searchspace.BestCoverageLocation());
        }

        private long Length((long x, long y, long z) coord)
          => Math.Abs(coord.x) + Math.Abs(coord.y) + Math.Abs(coord.z);
    }

    public class SearchSpace
    {
        private List<Drone> _drones;
        public (long from, long to) X;
        public (long from, long to) Y;
        public (long from, long to) Z;

        public SearchSpace(List<Drone> drones)
            : this(drones,
                  (drones.Min(p => p.X), drones.Max(p => p.X)),
                  (drones.Min(p => p.Y), drones.Max(p => p.Y)),
                  (drones.Min(p => p.Z), drones.Max(p => p.Z)))
        { }

        private SearchSpace(List<Drone> drones, (long from, long to) x, (long from, long to) y, (long from, long to) z)
        {
            _drones = drones;
            X = (x.from, Expand(x.to, x.from));
            Y = (y.from, Expand(y.to, y.from));
            Z = (z.from, Expand(z.to, z.from));
        }

        private long Expand(long to, long from)
        {
            return from + (long)Math.Pow(2, Math.Ceiling(Math.Log(to - from, 2)));
        }

        public long BestCoverageEstimate() =>
            _drones.Where(p => p.Overlaps(X, Y, Z))
                //(p.X + p.R >= X.from && X.to >= p.X - p.R) &&
                //(p.Y + p.R >= Y.from && Y.to >= p.Y - p.R) &&
                //(p.Z + p.R >= Z.from && Z.to >= p.Z - p.R))
                .Count();

        public IEnumerable<SearchSpace> Partition()
        {
            if ((X.to - X.from) >= (Y.to - Y.from) && (X.to - X.from) >= (Z.to - Z.from))
            {
                var mid = X.from + (long)Math.Floor((X.to - X.from) / 2.0);
                if (X.from < mid) yield return new SearchSpace(_drones, (X.from, mid), Y, Z);
                if (X.to > mid) yield return new SearchSpace(_drones, (mid, X.to), Y, Z);
            }
            else if (Y.to - Y.from > Z.to - Z.from)
            {
                var mid = Y.from + (long)Math.Floor((Y.to - Y.from) / 2.0);
                if (Y.from < mid) yield return new SearchSpace(_drones, X, (Y.from, mid), Z);
                if (Y.to > mid) yield return new SearchSpace(_drones, X, (mid, Y.to), Z);
            }
            else
            {
                var mid = Z.from + (long)Math.Floor((Z.to - Z.from) / 2.0);
                if (Z.from < mid) yield return new SearchSpace(_drones, X, Y, (Z.from, mid));
                if (Z.to > mid) yield return new SearchSpace(_drones, X, Y, (mid, Z.to));
            }
        }

        public IEnumerable<(long, long, long)> BuildSet(long from1, long to1, long from2, long to2, long from3, long to3)

        {
            for (long v1 = from1; v1 < to1; v1++)
                for (long v2 = from2; v2 < to2; v2++)
                    for (long v3 = from3; v3 < to3; v3++)
                        yield return (v1, v2, v3);
        }
        public (long x, long y, long z) BestCoverageLocation()
        {
            var res = BuildSet(X.from, X.to, Y.from, Y.to, Z.from, Z.to)
                .Select(p => new { coord = p, count = GetCoverage(p) })
                .ToList()
                .OrderByDescending(p => p.count)
                .ToList();

            //foreach (var result in res)
            //    Console.WriteLine($"{result.coord} -> {result.count}");

            return res.First().coord;

        }

        public long LargestSize => Math.Max(Math.Max((X.to - X.from), (Y.to - Y.from)), (Z.to - Z.from));

        public override string ToString()
            => $"({X.from}, {Y.from}, {Z.from}) - ({X.to},{Y.to},{Z.to})";

        internal long GetCoverage((long x, long y, long z) coordinate)
            => _drones.Count(q => q.Distance(coordinate) <= q.R);
    }

    public class Drone
    {
        public long X { get; }
        public long Y { get; }
        public long Z { get; }
        public long R { get; }

        public Drone(long x, long y, long z, long r)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
        }

        public long Distance(Drone other)
            => Distance((other.X, other.Y, other.Z));

        public override string ToString()
            => $"<{X},{Y},{Z}>,R{R}";

        internal long Distance((long x, long y, long z) other)
            => Math.Abs(other.x - X)
                + Math.Abs(other.y - Y)
                + Math.Abs(other.z - Z);

        internal bool Overlaps((long from, long to) x, (long from, long to) y, (long from, long to) z)
        {
            if (X + R < x.from) return false;
            if (X - R > x.to) return false;

            if (Y + R < y.from) return false;
            if (Y - R > y.to) return false;

            if (Z + R < z.from) return false;
            if (Z - R > z.to) return false;
            return true;
        }
    }
}
