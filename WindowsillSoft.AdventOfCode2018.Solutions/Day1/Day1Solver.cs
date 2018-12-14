using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day1
{
    public class Day1Solver : IProblemSolver
    {
        public string Description => "Day 1";
        public int SortOrder => 1;

        public void Solve()
        {
            var changes = File.ReadAllLines("Day1/Day1Input.txt")
                .Select(p => int.Parse(p))
                .ToArray();

            Console.WriteLine($"Part 1: {changes.Sum()}");

            var frequencies = new HashSet<int>();
            int currentFrequency = 0;
            while (true)
            {
                for (int i = 0; i < changes.Length; i++)
                {
                    currentFrequency += changes[i];
                    if (frequencies.Contains(currentFrequency))
                    {
                        Console.WriteLine($"Part 2: {currentFrequency}");
                        return;
                    }
                    frequencies.Add(currentFrequency);
                }
            }
        }
    }
}
