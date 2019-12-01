using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day23 : AdventOfCode2018SolverBase
    {
        private List<Drone> _drones = new List<Drone>();

        public Day23(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 23: Experimental Emergency Teleportation";

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"pos=<(?'x'-?\d+),(?'y'-?\d+),(?'z'-?\d+)>, r=(?'r'\d+)", RegexOptions.Compiled);

            _drones = ReadAndSplitInput<string>(input)
             .Select(p => matcher.Match(p))
             .Select(p => new Drone(
                 int.Parse(p.Groups["x"].Value),
                 int.Parse(p.Groups["y"].Value),
                 int.Parse(p.Groups["z"].Value),
                 int.Parse(p.Groups["r"].Value)))
             .ToList();
        }

        public override string ExecutePart1()
        {
            var strongestDrone = _drones.OrderByDescending(p => p.R).First();
            var inRangeOfStrongest = _drones.Count(p => p.Distance(strongestDrone) <= strongestDrone.R);
            return inRangeOfStrongest.ToString();
        }

        public override string ExecutePart2()
        {
            var searchspace = new SearchSpace(_drones);

            List<SearchSpace> Fringe = new List<SearchSpace>() { searchspace };

            (int x, int y, int z) bestPlacement = default;
            int bestCoverage = 0;

            while (Fringe.Any())
            {
                var candidate = Fringe.First();
                Fringe.Remove(candidate);
                if (candidate.BestCoverageEstimate() < bestCoverage)
                    continue;

                if (candidate.Size == 0)
                {
                    var coord = candidate.Location;
                    var coverage = _drones.Count(p => p.Distance(coord) <= p.R);
                    if (coverage > bestCoverage
                        || coverage == bestCoverage && Length(coord) < Length(bestPlacement))
                    {
                        bestCoverage = coverage;
                        bestPlacement = coord;

                        IO.LogIfAttached(() => $"Better coord {bestPlacement} found with coverage {bestCoverage} (distance {Length(bestPlacement)})");

                    }
                }
                else
                {
                    foreach (var part in candidate.Partition())
                        Fringe.Add(part);
                    Fringe = Fringe.OrderByDescending(p => p.BestCoverageEstimate())
                        .ThenBy(p => Length(p.Location))
                        .ThenBy(p => p.Size).ToList();
                }

            }

            IO.LogIfAttached(() => $"Best location found was at {bestPlacement} at distance {Length(bestPlacement)}");

            return Length(bestPlacement).ToString();
        }

        private long Length((int x, int y, int z) coord)
          => Math.Abs(coord.x) + Math.Abs(coord.y) + Math.Abs(coord.z);

        public class Drone
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int R { get; }

            public Drone(int x, int y, int z, int r)
            {
                X = x;
                Y = y;
                Z = z;
                R = r;
            }

            public int Distance(Drone other)
                => Distance((other.X, other.Y, other.Z));

            public override string ToString()
                => $"<{X},{Y},{Z}>,R{R}";

            public int Distance((int x, int y, int z) other)
                => Math.Abs(other.x - X)
                    + Math.Abs(other.y - Y)
                    + Math.Abs(other.z - Z);

            internal bool OverlapsCube(int x, int y, int z, int size)
            {
                var d = 0;
                if (X > x + size)
                    d += X - (x + size);
                if (X < x)
                    d += x - X;
                if (Y > y + size)
                    d += Y - (y + size);
                if (Y < y)
                    d += y - Y;
                if (Z > z + size)
                    d += Z - (z + size);
                if (Z < z)
                    d += z - Z;
                return d <= R;
            }
        }

        public class SearchSpace
        {
            private Drone[] _drones;
            private int _X, _Y, _Z;
            public int Size { get; }
            public (int x, int y, int z) Location => (_X, _Y, _Z);

            public SearchSpace(List<Drone> drones)
            {
                _drones = drones.ToArray();
                _X = drones.Min(p => p.X);
                _Y = drones.Min(p => p.Y);
                _Z = drones.Min(p => p.Z);

                var maxWidth = new[] {
                drones.Max(p => p.X) - _X,
                drones.Max(p => p.Y) - _Y,
                drones.Max(p => p.Z) - _Z }
                    .Max();

                //var size = Math.Pow(2, Math.Ceiling(Math.Log(maxWidth, 2)));
                //potential overflow due to Double conversion - switching to silly version instead.
                Size = 1;
                while (Size < maxWidth)
                    Size <<= 1;
            }

            private SearchSpace(Drone[] drones, int x, int y, int z, int size)
            {
                (_X, _Y, _Z, Size) = (x, y, z, size);
                _drones = drones.Where(p => p.OverlapsCube(x, y, z, size)).ToArray();
            }

            internal int BestCoverageEstimate()
            {
                return _drones.Count();
            }

            internal IEnumerable<SearchSpace> Partition()
            {
                yield return new SearchSpace(_drones, _X, _Y, _Z, Size / 2);
                yield return new SearchSpace(_drones, _X + Size / 2, _Y, _Z, Size / 2);
                yield return new SearchSpace(_drones, _X, _Y + Size / 2, _Z, Size / 2);
                yield return new SearchSpace(_drones, _X, _Y, _Z + Size / 2, Size / 2);
                yield return new SearchSpace(_drones, _X + Size / 2, _Y + Size / 2, _Z, Size / 2);
                yield return new SearchSpace(_drones, _X + Size / 2, _Y, _Z + Size / 2, Size / 2);
                yield return new SearchSpace(_drones, _X, _Y + Size / 2, _Z + Size / 2, Size / 2);
                yield return new SearchSpace(_drones, _X + Size / 2, _Y + Size / 2, _Z + Size / 2, Size / 2);
            }

            public override string ToString()
                => $"{(_X, _Y, _Z)},{Size};{_drones.Length}";
        }
    }
}

