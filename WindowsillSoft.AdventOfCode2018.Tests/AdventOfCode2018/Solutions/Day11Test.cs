using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Inputs;
using WindowsillSoft.AdventOfCode2018.Solutions.Day11;
using Xunit;

namespace WindowsillSoft.AdventOfCode2018.Tests.AdventOfCode2018.Solutions
{
public    class Day11Test
    {
        [Theory]
        [InlineData(Day11Input.Test1Input, Day11Input.Part1Test1Result)]
        [InlineData(Day11Input.Test2Input, Day11Input.Part1Test2Result)]
        public void Part1(string input, string result)
        {
            var test = new Day11Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day11Input.Test1Input, Day11Input.Part2Test1Result)]
        [InlineData(Day11Input.Test2Input, Day11Input.Part2Test2Result)]
        public void Part2(string input, string result)
        {
            var test = new Day11Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day11Solver();
            test.Initialize(Day11Input.FullRunInput);
            Assert.Equal(Day11Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day11Solver();
            test.Initialize(Day11Input.FullRunInput);
            Assert.Equal(Day11Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
