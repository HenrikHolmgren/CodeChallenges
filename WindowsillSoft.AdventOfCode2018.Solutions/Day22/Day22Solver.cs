using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day22
{
    public class Day22Solver : IProblemSolver
    {
        public string Description => "Day 22: Mode Maze";



        public int SortOrder => 22;
        Dictionary<(int x, int y), int> ErosionValues = new Dictionary<(int x, int y), int>();
        (int x, int y) Target = (6, 797);
        //(int x, int y) Target = (10, 10);
        int Depth = 11991;
        //int Depth = 510;

        public void Solve()
        {
            //PrintMap();

            Part1();
            Part2();
        }

        private void Part1()
        {
            var sum = (from x in Enumerable.Range(0, Target.x + 1)
                       from y in Enumerable.Range(0, Target.y + 1)
                       select GetErosionLevel((x, y)) % 3).Sum();
            Console.WriteLine(sum);
        }

        private void Part2()
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
                var candidate = Fringe.OrderBy(p => p.d).First();
                Fringe.Remove(candidate);

                //We know we are breadth-first searching so any visited location will be visited in shorter moves than this.
                if (Distances.ContainsKey((candidate.x, candidate.y, candidate.t))
                    && !(candidate.x == 0 && candidate.y == 0 && candidate.t == Tool.Torch))
                    continue;

                if (candidate.x == Target.x && candidate.y == Target.y && candidate.t == Tool.Torch)
                {
                    Console.WriteLine($"Path found to target in {candidate.d} steps!");
                    break;
                }
                Distances[(candidate.x, candidate.y, candidate.t)] = candidate.d;

                foreach (var tool in allowedTools[GetTerrain((candidate.x, candidate.y))].Where(p => p != candidate.t))
                    Fringe.Add((candidate.x, candidate.y, tool, candidate.d + 7));
                foreach (var neighbour in new (int x, int y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                {
                    if (candidate.x + neighbour.x < 0) continue;
                    if (candidate.y + neighbour.y < 0) continue;
                    if (candidate.x + neighbour.x > Target.x * 50) continue;
                    if (candidate.y + neighbour.y > Target.y * 100) continue;

                    var neighbourTerrain = GetTerrain((candidate.x + neighbour.x, candidate.y + neighbour.y));
                    if (allowedTools[neighbourTerrain].Contains(candidate.t))
                        Fringe.Add((candidate.x + neighbour.x, candidate.y + neighbour.y, candidate.t, candidate.d + 1));
                }
            }
        }

        private int GetTerrain((int, int y) coords)
            => GetErosionLevel(coords) % 3;
        public class Comparer : IComparer<(int x, int y, Tool t, int d)>
        {
            public int Compare((int x, int y, Tool t, int d) x, (int x, int y, Tool t, int d) y)
                => x.d.CompareTo(y.d);
        }

        public enum Tool
        {
            Torch,
            ClimbingGear,
            Neither,
        }

        private void PrintMap()
        {
            for (int y = 0; y <= Target.y; y++)
            {
                for (int x = 0; x <= Target.x; x++)
                    Console.Write(Map(GetErosionLevel((x, y))));
                Console.WriteLine();
            }
        }

        public int GetErosionLevel((int x, int y) coords)
        {
            if (ErosionValues.ContainsKey(coords))
                return ErosionValues[coords];

            int erosionLevel;
            if (coords == (0, 0) || coords == Target)
                erosionLevel = Depth;
            else if (coords.y == 0)
                erosionLevel = (coords.x * 16807) + Depth;
            else if (coords.x == 0)
                erosionLevel = (coords.y * 48271) + Depth;
            else
                erosionLevel = (GetErosionLevel((coords.x - 1, coords.y))) * (GetErosionLevel((coords.x, coords.y - 1))) + Depth;

            ErosionValues[coords] = erosionLevel % 20183;

            return ErosionValues[coords];
        }
        //public void Solve()
        //{
        //    int depth = 510;
        //    var target = (X: 10, Y: 10);
        //    int[] currentRow = new int[target.X + 1];
        //    int[] prevRow = new int[target.X + 1];

        //    //Initialize ground row
        //    for (int x = 0; x < target.X; x++)
        //        prevRow[x] = (x * 16807) % 20183;
        //    int riskSum = prevRow.Sum(p => (p + depth) % 3);

        //    Console.WriteLine(String.Join("", prevRow.Select(p => Map((p + depth) % 20183))));
        //    for (int y = 1; y <= target.Y; y++)
        //    {
        //        currentRow[0] = (48271 * y) % 20183;
        //        Console.Write(Map((currentRow[0] + depth) % 20183));
        //        for (int x = 1; x <= target.X; x++)
        //        {
        //            currentRow[x] = ((currentRow[x - 1] + 510) * (prevRow[x] + 510)) % 20183;
        //            if (target.X == x && target.Y == y)
        //                currentRow[x] = 0;

        //            Console.Write(Map((currentRow[x] + depth) % 20183));
        //        }
        //        riskSum += currentRow.Sum(p => (p + depth) % 3);
        //        var tmp = currentRow;
        //        currentRow = prevRow;
        //        prevRow = tmp;
        //        Console.WriteLine();
        //    }

        //    Console.WriteLine(riskSum - (prevRow[target.X] + depth) % 3);
        //    Console.ReadKey(true);
        //}

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
