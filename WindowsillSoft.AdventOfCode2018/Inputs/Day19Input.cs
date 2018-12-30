using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Solutions.Day19;

namespace WindowsillSoft.AdventOfCode2018.Inputs
{
    public class Day19Input
    {
        public const string Test1Input = @"#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5";
        public const string Part1Test1Result = "7";

        [FullRunInput(typeof(Day19Solver))]
        public const string FullRunInput = @"#ip 3
addi 3 16 3
seti 1 2 5
seti 1 3 2
mulr 5 2 1
eqrr 1 4 1
addr 1 3 3
addi 3 1 3
addr 5 0 0
addi 2 1 2
gtrr 2 4 1
addr 3 1 3
seti 2 5 3
addi 5 1 5
gtrr 5 4 1
addr 1 3 3
seti 1 2 3
mulr 3 3 3
addi 4 2 4
mulr 4 4 4
mulr 3 4 4
muli 4 11 4
addi 1 6 1
mulr 1 3 1
addi 1 21 1
addr 4 1 4
addr 3 0 3
seti 0 3 3
setr 3 4 1
mulr 1 3 1
addr 3 1 1
mulr 3 1 1
muli 1 14 1
mulr 1 3 1
addr 4 1 4
seti 0 3 0
seti 0 7 3";
        public const string Part1FullRunOutput = "1056";
        public const string Part2FullRunOutput = "10915260";
    }
}
