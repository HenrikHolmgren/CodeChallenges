using WindowsillSoft.CodeChallenges.Inputs;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day16;
using Xunit;

namespace WindowsillSoft.CodeChallenges.Tests.AdventOfCode.AoC2018.Solutions
{
    public class Day16Test
    {
        [Fact]
        public void FullRunPart1()
        {
            var test = new Day16Solver();
            test.Initialize(Day16Input.FullRunInput);
            Assert.Equal(Day16Input.Part1FullRunOutput, test.SolvePart1());
        }

        [Fact]
        public void FullRunPart2()
        {
            var test = new Day16Solver();
            test.Initialize(Day16Input.FullRunInput);
            Assert.Equal(Day16Input.Part2FullRunOutput, test.SolvePart2());
        }
    }
}
