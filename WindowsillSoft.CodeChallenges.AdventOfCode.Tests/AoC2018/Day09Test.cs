using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day09Test : TestBase<Day09>
    {
        [Test,
            TestCase("10,1618", "8317"),
            TestCase("13,7999", "146373"),
            TestCase("17,1104", "2764"),
            TestCase("21,6111", "54718"),
            TestCase("30,5807", "37305")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test]
        public void Part2(string input, string result)
        {
            Assert.Pass("No tests available for part 2");
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("426,72058");
            Assert.That("424112", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("3487352628", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

