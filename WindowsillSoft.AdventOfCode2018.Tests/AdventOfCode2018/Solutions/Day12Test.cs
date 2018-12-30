using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day12;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.AdventOfCode2018.Solutions
{
public    class Day12Test
    {
        [Theory]
        [InlineData(Day12Input.Test1Input, Day12Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day12Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day12Solver();
            test.Initialize(Day12Input.FullRunInput);
            Assert.Equal(Day12Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day12Solver();
            test.Initialize(Day12Input.FullRunInput);
            Assert.Equal(Day12Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
