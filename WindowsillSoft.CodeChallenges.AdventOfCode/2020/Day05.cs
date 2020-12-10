using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day05 : AdventOfCode2020SolverBase
    {
        protected List<string> _boardingPasses = new();
        public Day05(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 5: Binary Boarding";

        public override string ExecutePart1() => _boardingPasses.Max(ToSeatId).ToString();

        public override string ExecutePart2()
        {
            var seatIds = _boardingPasses.Select(ToSeatId).ToList();
            var expectedIds = Enumerable.Range(seatIds.Min(), seatIds.Count());
            var myId = expectedIds.Except(seatIds).Single();

            return myId.ToString();
        }

        private int ToSeatId(string arg)
        {
            //Basically a boarding pass is just a binary representation mapping 0 to F and L, and 1 to B and R
            //Likely there are easier ways to parse the number, but this works and is relatively readable.
            int accum = 0;
            foreach (var digit in arg)
            {
                accum <<= 1;
                if (digit == 'B' || digit == 'R')
                    accum++;
            }
            return accum;
        }

        public override void Initialize(string input)
        {
            _boardingPasses = ReadAndSplitInput<string>(input).ToList();
        }
    }
}
