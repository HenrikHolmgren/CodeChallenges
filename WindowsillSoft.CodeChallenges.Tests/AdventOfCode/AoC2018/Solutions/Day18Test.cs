using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day18;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day18Test
    {
        [Theory]
        [InlineData(Day18Input.Test1Input, Day18Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day18Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }        

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day18Solver();
            test.Initialize(Day18Input.FullRunInput);
            Assert.Equal(Day18Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day18Solver();
            test.Initialize(Day18Input.FullRunInput);
            Assert.Equal(Day18Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
