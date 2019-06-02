using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day15Test : TestBase<Day15>
    {
        [Test,
            TestCase("65,8921", "588")]
        public void Part1(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("65,8921", "309")]
        public void Part2(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("873,583");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("631"));
            Assert.That(result.Part2Result, Is.EqualTo("279"));
        }
    }
}
