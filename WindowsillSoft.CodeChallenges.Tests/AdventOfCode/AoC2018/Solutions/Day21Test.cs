using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day21;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day21Test
    {
        [Fact]
        public void FullRunPart1()
        {
            var test = new Day21Solver();
            test.Initialize(Day21Input.FullRunInput);
            Assert.Equal(Day21Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact(Skip = "Disabled, takes ~3 minutes to complete.")]
        public void FullRunPart2()
        {
            var test = new Day21Solver();
            test.Initialize(Day21Input.FullRunInput);
            Assert.Equal(Day21Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
