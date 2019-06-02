using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day23Test : TestBase<Day23>
    {
        [Test,
            TestCase("", ""),
            TestCase("", ""),
            TestCase("", "")]
        public void Part1(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("", ""),
            TestCase("", ""),
            TestCase("", "")]
        public void Part2(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo(""));
            Assert.That(result.Part2Result, Is.EqualTo(""));
        }
    }
}
