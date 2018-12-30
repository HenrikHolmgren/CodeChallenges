using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day8;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day8Test
    {
        [Theory]
        [InlineData(Day8Input.Test1Input, Day8Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day8Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day8Input.Test1Input, Day8Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day8Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day8Solver();
            test.Initialize(Day8Input.FullRunInput);
            Assert.Equal(Day8Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day8Solver();
            test.Initialize(Day8Input.FullRunInput);
            Assert.Equal(Day8Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
