using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day23;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day23Test
    {
        [Theory]
        [InlineData(Day23Input.Test1Input, Day23Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day23Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day23Input.Test2Input, Day23Input.Part2Test2Result)]
        public void Part2(string input, string result)
        {
            var test = new Day23Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day23Solver();
            test.Initialize(Day23Input.FullRunInput);
            Assert.Equal(Day23Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day23Solver();
            test.Initialize(Day23Input.FullRunInput);
            Assert.Equal(Day23Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
