using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day11;

namespace WindowsillSoft.CodeChallenges.Inputs
{
    public class Day11Input
    {
        public const string Test1Input = "300,18";
        public const string Part1Test1Result = "33,45";
        public const string Part2Test1Result = "90,269,16";

        public const string Test2Input = "300,42";
        public const string Part1Test2Result = "21,61";
        public const string Part2Test2Result = "232,251,12";

        [FullRunInput(typeof(Day11Solver))]
        public const string FullRunInput = "300,7347";
        public const string Part1FullRunOutput = "243,17";
        public const string Part2FullRunOutput = "233,228,12";
    }
}
