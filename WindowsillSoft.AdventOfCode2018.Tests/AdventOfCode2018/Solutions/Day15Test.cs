using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day15;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.AdventOfCode2018.Solutions
{
    public class Day15Test
    {
        [Theory]
        [InlineData(Day15Input.Test1Input, Day15Input.Part1Test1Result)]
        [InlineData(Day15Input.Test2Input, Day15Input.Part1Test2Result)]
        [InlineData(Day15Input.Test3Input, Day15Input.Part1Test3Result)]
        [InlineData(Day15Input.Test4Input, Day15Input.Part1Test4Result)]
        [InlineData(Day15Input.Test5Input, Day15Input.Part1Test5Result)]
        [InlineData(Day15Input.Test6Input, Day15Input.Part1Test6Result)]
        public void Part1(string input, string result)
        {
            var test = new Day15Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day15Input.Test1Input, Day15Input.Part2Test1Result)]        
        [InlineData(Day15Input.Test3Input, Day15Input.Part2Test3Result)]
        [InlineData(Day15Input.Test4Input, Day15Input.Part2Test4Result)]
        [InlineData(Day15Input.Test5Input, Day15Input.Part2Test5Result)]
        [InlineData(Day15Input.Test6Input, Day15Input.Part2Test6Result)]
        public void Part2(string input, string result)
        {
            var test = new Day15Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day15Solver();
            test.Initialize(Day15Input.FullRunInput);
            Assert.Equal(Day15Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day15Solver();
            test.Initialize(Day15Input.FullRunInput);
            Assert.Equal(Day15Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
