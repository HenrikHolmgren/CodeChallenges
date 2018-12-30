using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day5
{
    public class Day5Solver : AoC2018SolverBase
    {
        private HashSet<char> _uniqueUnits;
        private LinkedList<char> _list;

        public override string Description => "Day 5: Alchemical Reduction";
        public override int SortOrder => 5;

        public override void Initialize(string input)
        {
            LinkedList<char> list = null;

            _uniqueUnits = new HashSet<char>();

            foreach (var unit in input.Reverse())
            {
                list = new LinkedList<char>(unit, list);
                _uniqueUnits.Add(char.ToLower(unit));
            }
            _list = new LinkedList<char>('¤', list);
        }

        public override string SolvePart1(bool silent = true)
        {
            var reducedList = _list.Clone().Reduce(IsMatch);
            return (reducedList.Length() - 1).ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            var bestUnit = '¤';
            var bestLength = int.MaxValue;

            foreach (var unit in _uniqueUnits)
            {
                var filteredList = _list.Filter(p => p == unit || p == char.ToUpper(unit));

                filteredList = filteredList.Reduce(IsMatch);
                var filteredLength = filteredList.Length();
                if (filteredLength < bestLength)
                {
                    bestLength = filteredLength;
                    bestUnit = unit;
                }
                if (!silent)
                    Console.WriteLine($"After removing {unit} units, it reduces to {filteredList.Length() - 1}.");
            }
            return (bestLength - 1).ToString();
        }

        private bool IsMatch(char value1, char value2)
            => value1 != value2 &&
                (char.ToUpper(value1) == value2 || char.ToUpper(value2) == value1);
    }
}
