using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day20;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.AdventOfCode2018.Solutions
{
public    class Day20Test
    {
        [Theory]
        [InlineData(Day20Input.Test1Input, Day20Input.Part1Test1Result)]
        [InlineData(Day20Input.Test2Input, Day20Input.Part1Test2Result)]
        [InlineData(Day20Input.Test3Input, Day20Input.Part1Test3Result)]
        [InlineData(Day20Input.Test4Input, Day20Input.Part1Test4Result)]
        [InlineData(Day20Input.Test5Input, Day20Input.Part1Test5Result)]
        public void Part1(string input, string result)
        {
            var test = new Day20Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }
                
        [Fact]
        public void FullRunPart1()
        {
            var test = new Day20Solver();
            test.Initialize(Day20Input.FullRunInput);
            Assert.Equal(Day20Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day20Solver();
            test.Initialize(Day20Input.FullRunInput);
            Assert.Equal(Day20Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
