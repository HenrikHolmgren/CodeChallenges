using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day9;

namespace WindowsillSoft.CodeChallenges.Inputs
{
    public class Day9Input
    {
        public const string Test1Input = "10,1618";
        public const string Test1Result = "8317";
        public const string Test2Input = "13,7999";
        public const string Test2Result = "146373";
        public const string Test3Input = "17,1104";
        public const string Test3Result = "2764";
        public const string Test4Input = "21,6111";
        public const string Test4Result = "54718";
        public const string Test5Input = "30,5807";
        public const string Test5Result = "37305";

        [FullRunInput(typeof(Day9Solver))]
        public const string FullRunInput = "426,72058";
        public const string Part1FullRunOutput = "424112";
        public const string Part2FullRunOutput = "3487352628";
    }
}
