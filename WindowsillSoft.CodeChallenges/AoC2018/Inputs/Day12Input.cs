using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day12;

namespace WindowsillSoft.CodeChallenges.Inputs
{
    public class Day12Input
    {
        public const string Test1Input = @"#..#.#..##......###...###
...##
..#..
.#...
.#.#.
.#.##
.##..
.####
#.#.#
#.###
##.#.
##.##
###..
###.#
####.";
        public const string Part1Test1Result = "325";

        [FullRunInput(typeof(Day12Solver))]
        public const string FullRunInput = @"#...#...##..####..##.####.#...#...#.#.#.#......##....#....######.####.##..#..#..##.##..##....#######
#####
####.
###..
##...
#..#.
#.#..
##.##
.###.
.##..
.#...
#.#.#
.#.##
.##.#
.#..#
.#.#.
..#..
...##";
        public const string Part1FullRunOutput = "4386";
        public const string Part2FullRunOutput = "5450000001166";
    }
}
