using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day14;

namespace WindowsillSoft.CodeChallenges.Inputs
{
    public class Day14Input
    {
        public const string Test1Input = "9,51589";
        public const string Part1Test1Result = "5158916779";
        public const string Part2Test1Result = "9";

        public const string Test2Input = "5,01245";
        public const string Part1Test2Result = "0124515891";
        public const string Part2Test2Result = "5";

        public const string Test3Input = "18,92510";
        public const string Part1Test3Result = "9251071085";
        public const string Part2Test3Result = "18";

        public const string Test4Input = "2018,59414";
        public const string Part1Test4Result = "5941429882";
        public const string Part2Test4Result = "2018";

        [FullRunInput(typeof(Day14Solver))]
        public const string FullRunInput = "110201,110201";
        public const string Part1FullRunOutput = "6107101544";
        public const string Part2FullRunOutput = "20291131";
    }
}
