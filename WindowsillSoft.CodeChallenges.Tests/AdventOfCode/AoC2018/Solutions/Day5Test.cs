using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day5;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day5Test
    {
        [Theory]
        [InlineData(Day5Input.Test1Input, Day5Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day5Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day5Input.Test1Input, Day5Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day5Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day5Solver();
            test.Initialize(Day5Input.FullRunInput);
            Assert.Equal(Day5Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day5Solver();
            test.Initialize(Day5Input.FullRunInput);
            Assert.Equal(Day5Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
