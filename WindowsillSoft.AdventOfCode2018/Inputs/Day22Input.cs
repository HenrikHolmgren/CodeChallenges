using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Solutions.Day22;

namespace WindowsillSoft.AdventOfCode2018.Inputs
{
    public class Day22Input
    {
        public const string Test1Input = @"depth: 510
target: 10,10";
        public const string Part1Test1Result = "114";
        public const string Part2Test1Result = "45";

        [FullRunInput(typeof(Day22Solver))]
        public const string FullRunInput = @"depth: 11991
target: 6,797";
        public const string Part1FullRunOutput = "5622";
        public const string Part2FullRunOutput = "1089";
    }
}
