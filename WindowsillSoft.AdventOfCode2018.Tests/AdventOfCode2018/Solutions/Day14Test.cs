using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day14;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.AdventOfCode2018.Solutions
{
    public class Day14Test
    {
        [Theory]
        [InlineData(Day14Input.Test1Input, Day14Input.Part1Test1Result)]
        [InlineData(Day14Input.Test2Input, Day14Input.Part1Test2Result)]
        [InlineData(Day14Input.Test3Input, Day14Input.Part1Test3Result)]
        [InlineData(Day14Input.Test4Input, Day14Input.Part1Test4Result)]
        public void Part1(string input, string result)
        {
            var test = new Day14Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day14Input.Test1Input, Day14Input.Part2Test1Result)]
        [InlineData(Day14Input.Test2Input, Day14Input.Part2Test2Result)]
        [InlineData(Day14Input.Test3Input, Day14Input.Part2Test3Result)]
        [InlineData(Day14Input.Test4Input, Day14Input.Part2Test4Result)]
        public void Part2(string input, string result)
        {
            var test = new Day14Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day14Solver();
            test.Initialize(Day14Input.FullRunInput);
            Assert.Equal(Day14Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day14Solver();
            test.Initialize(Day14Input.FullRunInput);
            Assert.Equal(Day14Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
