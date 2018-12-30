using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day9;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.Solutions
{
    public class Day9Test
    {
        [Theory]
        [InlineData(Day9Input.Test1Input, Day9Input.Test1Result)]
        [InlineData(Day9Input.Test2Input, Day9Input.Test2Result)]
        [InlineData(Day9Input.Test3Input, Day9Input.Test3Result)]
        [InlineData(Day9Input.Test4Input, Day9Input.Test4Result)]
        [InlineData(Day9Input.Test5Input, Day9Input.Test5Result)]
        public void Part1(string input, string result)
        {
            var test = new Day9Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day9Solver();
            test.Initialize(Day9Input.FullRunInput);
            Assert.Equal(Day9Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day9Solver();
            test.Initialize(Day9Input.FullRunInput);
            Assert.Equal(Day9Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
