using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day22;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day22Test
    {
        [Theory]
        [InlineData(Day22Input.Test1Input, Day22Input.Part1Test1Result)]
        public void Part1(string input, string result)
        {
            var test = new Day22Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day22Input.Test1Input, Day22Input.Part2Test1Result)]
        public void Part2(string input, string result)
        {
            var test = new Day22Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day22Solver();
            test.Initialize(Day22Input.FullRunInput);
            Assert.Equal(Day22Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact(Skip = "Disabled, takes ~3 minutes to complete.")]
        public void FullRunPart2()
        {
            var test = new Day22Solver();
            test.Initialize(Day22Input.FullRunInput);
            Assert.Equal(Day22Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
