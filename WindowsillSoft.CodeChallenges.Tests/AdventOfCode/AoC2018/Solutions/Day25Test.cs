using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day25;
using WindowsillSoft.CodeChallenges.Inputs;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day25Test
    {
        [Theory]
        [InlineData(Day25Input.Test1Input, Day25Input.Part1Test1Result)]
        [InlineData(Day25Input.Test2Input, Day25Input.Part1Test2Result)]
        [InlineData(Day25Input.Test3Input, Day25Input.Part1Test3Result)]
        [InlineData(Day25Input.Test4Input, Day25Input.Part1Test4Result)]
        public void Part1(string input, string result)
        {
            var test = new Day25Solver();
            test.Initialize(input);
            Assert.Equal(result, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart1()
        {
            var test = new Day25Solver();
            test.Initialize(Day25Input.FullRunInput);
            Assert.Equal(Day25Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day25Solver();
            test.Initialize(Day25Input.FullRunInput);
            Assert.Equal(Day25Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
