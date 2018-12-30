using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Solutions.Day15;

namespace WindowsillSoft.AdventOfCode2018.Inputs
{
    public class Day15Input
    {
        public const string Test1Input = @"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";
        public const string Part1Test1Result = "27730";
        public const string Part2Test1Result = "4988";

        public const string Test2Input = @"#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######";
        public const string Part1Test2Result = "36334";

        public const string Test3Input = @"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######";
        public const string Part1Test3Result = "39514";
        public const string Part2Test3Result = "31284";

        public const string Test4Input = @"#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######";
        public const string Part1Test4Result = "27755";
        public const string Part2Test4Result = "3478";

        public const string Test5Input = @"#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######";
        public const string Part1Test5Result = "28944";
        public const string Part2Test5Result = "6474";

        public const string Test6Input = @"#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########";
        public const string Part1Test6Result = "18740";
        public const string Part2Test6Result = "1140";

        [FullRunInput(typeof(Day15Solver))]
        public const string FullRunInput = @"################################
####################.###########
###################..##..#######
###################.###..#######
###################.###.########
##################G.###.########
##..############.#..##..########
#...#####.####.....##...########
#G.....##..###.#.GG#...G.....G##
#...G....G.G##.....#.#.......###
##..............G............###
#......GG.....G............#####
#####.........#####.......E..###
#####.....G..#######.......#####
#####.......#########......###.#
######......#########..........#
#####.....G.#########..........#
##.....#...G#########......#...#
###.#....G..#########G.........#
##..###......#######E..........#
##..###...G...#####E..E.....E..#
###..##...............E.#.E....#
###..##..##E...###......##..####
##..G...###....###......########
##......###E....##....##########
###...#####..E..###...##########
##....####......####.###########
####..#####.....##...###########
############..####....##########
############.#####....##########
##########...#####.#.###########
################################";
        public const string Part1FullRunOutput = "239010";
        public const string Part2FullRunOutput = "62468";
    }
}
