using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day17;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.AdventOfCode2018.Solutions
{
    public class Day17Test
    {
        [Theory]
        [InlineData(Day17Input.Test1Input, Day17Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day17Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day17Input.Test1Input, Day17Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day17Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day17Solver();
            test.Initialize(Day17Input.FullRunInput);
            Assert.Equal(Day17Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day17Solver();
            test.Initialize(Day17Input.FullRunInput);
            Assert.Equal(Day17Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
