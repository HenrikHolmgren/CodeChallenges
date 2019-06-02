using Moq;
using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day06Test : TestBase<Day06>
    {
        [Test,
            TestCase("1, 1\n1, 6\n8, 3\n3, 4\n5, 5\n8, 9", "32", "17")]
        public void Part1(string input, string distanceThreshold, string result)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Distance threshold?"))
                .Returns(distanceThreshold);

            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("1, 1\n1, 6\n8, 3\n3, 4\n5, 5\n8, 9","32" , "16")]
        public void Part2(string input, string distanceThreshold, string result)
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Distance threshold?"))
                .Returns(distanceThreshold);

            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var IOProvider = Mock.Get(this._provider)
                .Setup(p => p.RequestInput("Distance threshold?"))
                .Returns("10000");

            var solver = GetSolver("355, 246\n259, 215\n166, 247\n280, 341\n54, 91\n314, 209\n256, 272\n149, 313\n217, 274\n299, 144\n355, 73\n70, 101\n266, 327\n51, 228\n274, 123\n342, 232\n97, 100\n58, 157\n130, 185\n135, 322\n306, 165\n335, 84\n268, 234\n173, 255\n316, 75\n79, 196\n152, 71\n205, 261\n275, 342\n164, 95\n343, 147\n83, 268\n74, 175\n225, 130\n354, 278\n123, 206\n166, 166\n155, 176\n282, 238\n107, 295\n82, 92\n325, 299\n87, 287\n90, 246\n159, 174\n295, 298\n260, 120\n203, 160\n72, 197\n182, 296");
            Assert.That("3238", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("45046", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

