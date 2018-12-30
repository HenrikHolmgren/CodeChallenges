using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;
using WindowsillSoft.AdventOfCode2018.Core.Geometry;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day25
{
    public class Day25Solver : IAdventOfCodeSolver
    {
        private ManhattanPointNInt[] _anomalies;

        public string Description => "Day 25: Four-Dimensional Adventure";

        public int SortOrder => 25;

        public void Initialize(string input)
        {
            _anomalies = input.Split(Environment.NewLine)
                .Select(p => p.Split(',').Select(q => int.Parse(q)).ToArray())
                .Select(p => new ManhattanPointNInt(p))
                .ToArray();
        }

        public string SolvePart1(bool silent = true)
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

        public string SolvePart2(bool silent = true)
        {
            return "Have a reindeer";
        }
    }
}
