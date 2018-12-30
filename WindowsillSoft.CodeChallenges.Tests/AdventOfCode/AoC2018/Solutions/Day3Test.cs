using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day3;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day3Test
    {
        [Theory]
        [InlineData(Day3Input.Test1Input, Day3Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day3Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day3Input.Test1Input, Day3Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day3Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day3Solver();
            test.Initialize(Day3Input.FullRunInput);
            Assert.Equal(Day3Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day3Solver();
            test.Initialize(Day3Input.FullRunInput);
            Assert.Equal(Day3Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
