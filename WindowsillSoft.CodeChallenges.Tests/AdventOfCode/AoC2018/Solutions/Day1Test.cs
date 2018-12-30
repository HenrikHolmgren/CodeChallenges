using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day1;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day1Test
    {
        [Theory]
        [InlineData(Day1Input.Part1Test1Input, Day1Input.Part1Test1Result)]
        [InlineData(Day1Input.Part1Test2Input, Day1Input.Part1Test2Result)]
        [InlineData(Day1Input.Part1Test3Input, Day1Input.Part1Test3Result)]
        public void Part1(string input, string result)
        {
            var test = new Day1Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Theory]
        [InlineData(Day1Input.Part2Test1Input, Day1Input.Part2Test1Result)]
        [InlineData(Day1Input.Part2Test2Input, Day1Input.Part2Test2Result)]
        [InlineData(Day1Input.Part2Test3Input, Day1Input.Part2Test3Result)]
        public void Part2(string input, string result)
        {
            var test = new Day1Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart2());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day1Solver();
            test.Initialize(Day1Input.FullRunInput);
            Assert.Equal(Day1Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day1Solver();
            test.Initialize(Day1Input.FullRunInput);
            Assert.Equal(Day1Input.Part2FullRunOutput, test.SolvePart2());
        }
    }

}
