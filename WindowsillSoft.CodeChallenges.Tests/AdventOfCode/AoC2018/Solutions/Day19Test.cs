using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day19;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day19Test
    {
        [Theory]
        [InlineData(Day19Input.Test1Input, Day19Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day19Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day19Solver();
            test.Initialize(Day19Input.FullRunInput);
            Assert.Equal(Day19Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day19Solver();
            test.Initialize(Day19Input.FullRunInput);
            Assert.Equal(Day19Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
