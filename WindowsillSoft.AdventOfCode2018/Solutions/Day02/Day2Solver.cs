using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day2
{
    public class Day2Solver : IAdventOfCodeSolver
    {
        public string Description => "Day 2: Inventory Management System";
        public int SortOrder => 2;

        private string[] _input;

        public void Initialize(string input)
        {
            _input = input.Split(Environment.NewLine);
        }

        public string SolvePart1(bool silent = true)
        {
            var candidates = _input.Select(p => p.GroupBy(q => q));
            var checksum = candidates.Count(p => p.Any(q => q.Count() == 2))
                * candidates.Count(p => p.Any(q => q.Count() == 3));

            return checksum.ToString();
        }

        public string SolvePart2(bool silent = true)
        {
            for (int i = 0; i < _input.Length - 1; i++)
            {
                for (int j = i + 1; j < _input.Length; j++)
                {
                    var differences = Enumerable.Range(0, _input[0].Length).Where(p => _input[i][p] != _input[j][p]);
                    if (differences.Count() == 1)
                    {
                        if (!silent)
                            Console.WriteLine($"{_input[i]}, {_input[j]} -> {_input[i].Substring(0, differences.Single())}_{_input[i].Substring(differences.Single() + 1)}");

                        return $"{_input[i].Substring(0, differences.Single())}{_input[i].Substring(differences.Single() + 1)}";
                    }
                }
            }
            return "Unknown!";
        }
    }
}
