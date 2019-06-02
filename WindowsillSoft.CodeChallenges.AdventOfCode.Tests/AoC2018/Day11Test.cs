using Moq;
using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day11Test : TestBase<Day11>
    {
        [Test,
            TestCase("18", "300", "33,45"),
            TestCase("42", "300", "21,61")]
        public void Part1(string input, string gridSize, string result)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Grid size?"))
                .Returns(gridSize);

            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("18", "300", "90,269,16"),
            TestCase("42", "300", "232,251,12")]
        public void Part2(string input, string gridSize, string result)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Grid size?"))
                .Returns(gridSize);
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Grid size?"))
                .Returns("300");

            var solver = GetSolver("7347");
            Assert.That("243,17", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("233,228,12", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

