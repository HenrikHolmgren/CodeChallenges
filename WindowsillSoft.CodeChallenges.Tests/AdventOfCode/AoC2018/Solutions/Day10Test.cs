using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day10;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public  class Day10Test
    {
        [Theory]
        [InlineData(Day10Input.Test1Input, Day10Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day10Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day10Input.Test1Input, Day10Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day10Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day10Solver();
            test.Initialize(Day10Input.FullRunInput);
            Assert.Equal(Day10Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day10Solver();
            test.Initialize(Day10Input.FullRunInput);
            Assert.Equal(Day10Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
