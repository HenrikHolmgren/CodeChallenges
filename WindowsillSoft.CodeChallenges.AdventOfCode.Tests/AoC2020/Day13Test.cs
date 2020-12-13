using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2020;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2020
{
    public class Day13Test : TestBase<Day13>
    {
        [Test,
            TestCase("939\n7,13,x,x,59,x,31,19", "295")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("939\n7,13,x,x,59,x,31,19", "1068781"),
            TestCase("1\n17,-1,13,19", "3417"),
            TestCase("1\n67,7,59,61", "754018"),
            TestCase("1\n67,-1,7,59,61", "779210"),
            TestCase("1\n67,7,-1,59,61", "1261476"),
            TestCase("1\n1789,37,47,1889", "1202161486")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("1000067\n17,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,439,x,29,x,x,x,x,x,x,x,x,x,x,13,x,x,x,x,x,x,x,x,x,23,x,x,x,x,x,x,x,787,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,19");
            Assert.That(solver.ExecutePart1(), Is.EqualTo("205"));
            Assert.That(solver.ExecutePart2(), Is.EqualTo("803025030761664"));
        }
    }
}