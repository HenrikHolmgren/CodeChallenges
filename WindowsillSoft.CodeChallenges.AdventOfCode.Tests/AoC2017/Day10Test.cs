using Moq;
using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day10Test : TestBase<Day10>
    {
        [Test,
            TestCase("3, 4, 1, 5", "5", "12")]
        public void Part1(string input, string queueLength, string output)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Queue size?"))
                .Returns(queueLength);

            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("", "256", "a2582a3a0e66e6e86e3812dcb672a272"),
            TestCase("AoC 2017", "256", "33efeb34ea91902bb2f59c9920caa6cd"),
            TestCase("1,2,3", "256", "3efbe78a8d82f29979031a4aa0b16a9d"),
            TestCase("1,2,4", "256", "63960835bcdc130f0b66d7ff4f6a5a8e")]
        public void Part2(string input, string queueLength, string output)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Queue size?"))
                .Returns(queueLength);

            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Queue size?"))
                .Returns("256");

            var solver = GetSolver("199,0,255,136,174,254,227,16,51,85,1,2,22,17,7,192");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("3770"));
            Assert.That(result.Part2Result, Is.EqualTo("a9d0e68649d0174c8756a59ba21d4dc6"));
        }
    }
}
