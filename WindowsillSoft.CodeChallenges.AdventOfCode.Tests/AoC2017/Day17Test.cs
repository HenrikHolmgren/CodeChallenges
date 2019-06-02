using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day17Test : TestBase<Day17>
    {
        [Test,
            TestCase("3", "638")]
        public void Part1(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("3", "1222153")]
        public void Part2(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("312");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("772"));
            Assert.That(result.Part2Result, Is.EqualTo("42729050"));
        }
    }
}
