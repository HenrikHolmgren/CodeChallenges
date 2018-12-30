using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day13;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day13Test
    {
        [Theory]
        [InlineData(Day13Input.Part1Test1Input, Day13Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day13Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day13Input.Part2Test1Input, Day13Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day13Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day13Solver();
            test.Initialize(Day13Input.FullRunInput);
            Assert.Equal(Day13Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day13Solver();
            test.Initialize(Day13Input.FullRunInput);
            Assert.Equal(Day13Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
