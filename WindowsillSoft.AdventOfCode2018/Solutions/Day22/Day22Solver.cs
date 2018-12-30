using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day22
{
    public class Day22Solver : IAdventOfCodeSolver
    {
        public string Description => "Day 22: Mode Maze";

        public int SortOrder => 22;

        Dictionary<(int x, int y), int> ErosionValues = new Dictionary<(int x, int y), int>();

        private int _depth;
        private (int x, int y) _target;

        public void Initialize(string input)
        {
            var inputLines = input.Split(Environment.NewLine)
                .Select(p => p.Split(':')[1])
                .ToArray();

            _depth = int.Parse(inputLines[0]);
            _target = (int.Parse(inputLines[1].Split(',')[0]),
                int.Parse(inputLines[1].Split(',')[1]));
        }

        public string SolvePart1(bool silent = true)
        {
            var sum = (from x in Enumerable.Range(0, _target.x + 1)
                       from y in Enumerable.Range(0, _target.y + 1)
                       select GetErosionLevel((x, y)) % 3).Sum();

            return sum.ToString();
        }

        public string SolvePart2(bool silent = true)
        {
            var Distances = new Dictionary<(int x, int y, Tool t), int>();

            //Todo: replace with priority queue!
            var Fringe = new List<(int x, int y, Tool t, int d)>();
            Fringe.Add((0, 0, Tool.Torch, 0));
            Distances[(0, 0, Tool.Torch)] = 0;

            Dictionary<int, Tool[]> allowedTools = new Dictionary<int, Tool[]>
            {
                { 0, new[] { Tool.ClimbingGear, Tool.Torch} }, //rocky
                { 1, new[] { Tool.ClimbingGear, Tool.Neither } }, //wet
                { 2, new[] { Tool.Torch, Tool.Neither } } //narrow
            };
            while (Fringe.Any())
            {
                var candidate = Fringe.OrderBy(p => p.d + (Math.Abs(p.x - _target.x) + Math.Abs(p.y - _target.y) + p.t == Tool.Torch ? 0 : 7)).First();
                Fringe.Remove(candidate);

                //We know we are breadth-first searching so any visited location will be visited in shorter moves than this.
                if (Distances.ContainsKey((candidate.x, candidate.y, candidate.t))
                    && !(candidate.x == 0 && candidate.y == 0 && candidate.t == Tool.Torch))
                    continue;

                if (candidate.x == _target.x && candidate.y == _target.y && candidate.t == Tool.Torch)
                    return candidate.d.ToString();

                Distances[(candidate.x, candidate.y, candidate.t)] = candidate.d;

                foreach (var tool in allowedTools[GetTerrain((candidate.x, candidate.y))].Where(p => p != candidate.t))
                    Fringe.Add((candidate.x, candidate.y, tool, candidate.d + 7));
                foreach (var neighbour in new(int x, int y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                {
                    if (candidate.x + neighbour.x < 0) continue;
                    if (candidate.y + neighbour.y < 0) continue;
                    if (candidate.x + neighbour.x > _target.x * 50) continue;
                    if (candidate.y + neighbour.y > _target.y * 100) continue;

                    var neighbourTerrain = GetTerrain((candidate.x + neighbour.x, candidate.y + neighbour.y));
                    if (allowedTools[neighbourTerrain].Contains(candidate.t))
                        Fringe.Add((candidate.x + neighbour.x, candidate.y + neighbour.y, candidate.t, candidate.d + 1));
                }
            }

            return "No solution found!";
        }

        private int GetTerrain((int, int y) coords)
            => GetErosionLevel(coords) % 3;
        public class Comparer : IComparer<(int x, int y, Tool t, int d)>
        {
            public int Compare((int x, int y, Tool t, int d) x, (int x, int y, Tool t, int d) y)
                => x.d.CompareTo(y.d);
        }

        private void PrintMap()
        {
            for (int y = 0; y <= _target.y; y++)
            {
                for (int x = 0; x <= _target.x; x++)
                    Console.Write(Map(GetErosionLevel((x, y))));
                Console.WriteLine();
            }
        }

        public int GetErosionLevel((int x, int y) coords)
        {
            if (ErosionValues.ContainsKey(coords))
                return ErosionValues[coords];

            int erosionLevel;
            if (coords == (0, 0) || coords == _target)
                erosionLevel = _depth;
            else if (coords.y == 0)
                erosionLevel = (coords.x * 16807) + _depth;
            else if (coords.x == 0)
                erosionLevel = (coords.y * 48271) + _depth;
            else
                erosionLevel = (GetErosionLevel((coords.x - 1, coords.y))) * (GetErosionLevel((coords.x, coords.y - 1))) + _depth;

            ErosionValues[coords] = erosionLevel % 20183;

            return ErosionValues[coords];
        }

        public char Map(int val)
        {
            switch (val % 3)
            {
                case 0: return '.';
                case 1: return '=';
                case 2: return '|';
            }
            return '?';
        }
    }
}
