using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day23
{
    public class Day23Solver : AoC2018SolverBase
    {
        private List<Drone> _drones;

        public override string Description => "Day 23: Experimental Emergency Teleportation";

        public override int SortOrder => 23;

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"pos=<(?'x'-?\d+),(?'y'-?\d+),(?'z'-?\d+)>, r=(?'r'\d+)", RegexOptions.Compiled);

            _drones = input.Split(Environment.NewLine)
             .Select(p => matcher.Match(p))
             .Select(p => new Drone(
                 int.Parse(p.Groups["x"].Value),
                 int.Parse(p.Groups["y"].Value),
                 int.Parse(p.Groups["z"].Value),
                 int.Parse(p.Groups["r"].Value)))
             .ToList();
        }

        public override string SolvePart1(bool silent = true)
        {
            var strongestDrone = _drones.OrderByDescending(p => p.R).First();
            var inRangeOfStrongest = _drones.Count(p => p.Distance(strongestDrone) <= strongestDrone.R);
            return inRangeOfStrongest.ToString();
        }

        public override string SolvePart2(bool silent = true)
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
                        if (!silent)
                        {
                            Console.CursorLeft = 0;
                            Console.WriteLine($"Better coord {bestPlacement} found with coverage {bestCoverage} (distance {Length(bestPlacement)})");
                        }
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
            if (!silent)
            {
                Console.WriteLine();
                Console.WriteLine($"Best location found was at {bestPlacement} at distance {Length(bestPlacement)}");
            }
            return Length(bestPlacement).ToString();
        }

        private long Length((int x, int y, int z) coord)
          => Math.Abs(coord.x) + Math.Abs(coord.y) + Math.Abs(coord.z);
    }
}
