using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day22Test : TestBase<Day22>
    {
        [Test,
            TestCase("depth: 510\ntarget: 10,10", "114")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("depth: 510\ntarget: 10,10", "45")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("depth: 11991\ntarget: 6,797");
            Assert.That("5622", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("1089", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

