using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day7;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.Solutions
{
    public class Day7Test
    {
        [Theory]
        [InlineData(Day7Input.Test1Input, Day7Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day7Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day7Input.Test1Input, Day7Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day7Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day7Solver();
            test.Initialize(Day7Input.FullRunInput);
            Assert.Equal(Day7Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day7Solver();
            test.Initialize(Day7Input.FullRunInput);
            Assert.Equal(Day7Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
