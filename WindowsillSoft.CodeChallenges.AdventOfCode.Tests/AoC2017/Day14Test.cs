using Moq;
using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day14Test : TestBase<Day14>
    {
        [Test,
            TestCase("flqrgnkx", "8108")]
        public void Part1(string input, string output)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Queue size?"))
                .Returns("256");

            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("flqrgnkx", "1242")]
        public void Part2(string input, string output)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Queue size?"))
                .Returns("256");

            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Queue size?"))
                .Returns("256");

            var solver = GetSolver("nbysizxe");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("8216"));
            Assert.That(result.Part2Result, Is.EqualTo("1139"));
        }
    }
}
