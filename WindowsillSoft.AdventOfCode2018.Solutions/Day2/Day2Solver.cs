using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day2
{
    public class Day2Solver : IProblemSolver
    {
        public string Description => "Day 2";
        public int SortOrder => 2;

        public void Solve()
        {
            var candidates = File.ReadAllLines("Day2/Day2Input.txt")
                .Select(p => p.GroupBy(q => q));

            var checksum = candidates.Count(p => p.Any(q => q.Count() == 2))
                * candidates.Count(p => p.Any(q => q.Count() == 3));

            Console.WriteLine($"Part 1: {checksum}");

            var input = File.ReadAllLines("Day2/Day2Input.txt");
            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = i + 1; j < input.Length; j++)
                {
                    var differences = Enumerable.Range(0, input[0].Length).Where(p => input[i][p] != input[j][p]);
                    if (differences.Count() == 1)
                        Console.WriteLine($"Part 2: {input[i]}, {input[j]} -> {input[i].Substring(0, differences.Single())}_{input[i].Substring(differences.Single() + 1)}");
                }
            }
        }
    }
}
