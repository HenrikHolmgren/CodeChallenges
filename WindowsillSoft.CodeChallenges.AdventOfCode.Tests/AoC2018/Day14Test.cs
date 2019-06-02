using Moq;
using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day14Test : TestBase<Day14>
    {

        [Test,
            TestCase("51589", "9", "5158916779"),
            TestCase("01245", "5", "0124515891"),
            TestCase("92510", "18", "9251071085"),
            TestCase("59414", "2018", "5941429882")]
        public void Part1(string input, string targetGeneration, string result)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Target generation?"))
                .Returns(targetGeneration);

            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("51589", "9"),
            TestCase("01245", "5"),
            TestCase("92510", "18"),
            TestCase("59414", "2018")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var IOProvider = Mock.Get(this._provider)
               .Setup(p => p.RequestInput("Target generation?"))
               .Returns("110201");

            var solver = GetSolver("110201");
            Assert.That("6107101544", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("20291131", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

