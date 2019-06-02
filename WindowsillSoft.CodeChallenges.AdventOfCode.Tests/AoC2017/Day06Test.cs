using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day06Test : TestBase<Day06>
    {
        [Test,
            TestCase("0\t2\t7\t0", "5")]
        public void Part1(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("0\t2\t7\t0", "4")]
        public void Part2(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("5\t1\t10\t0\t1\t7\t13\t14\t3\t12\t8\t10\t7\t12\t0\t6");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("5042"));
            Assert.That(result.Part2Result, Is.EqualTo("1086"));
        }
    }
}
