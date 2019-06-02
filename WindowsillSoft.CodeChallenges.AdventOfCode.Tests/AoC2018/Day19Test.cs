using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day19Test : TestBase<Day19>
    {
        [Test,
            TestCase("#ip 0\nseti 5 0 1\nseti 6 0 2\naddi 0 1 0\naddr 1 2 3\nsetr 1 0 0\nseti 8 0 4\nseti 9 0 5", "7")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("#ip 3\naddi 3 16 3\nseti 1 2 5\nseti 1 3 2\nmulr 5 2 1\neqrr 1 4 1\naddr 1 3 3\naddi 3 1 3\naddr 5 0 0\naddi 2 1 2\ngtrr 2 4 1\naddr 3 1 3\nseti 2 5 3\naddi 5 1 5\ngtrr 5 4 1\naddr 1 3 3\nseti 1 2 3\nmulr 3 3 3\naddi 4 2 4\nmulr 4 4 4\nmulr 3 4 4\nmuli 4 11 4\naddi 1 6 1\nmulr 1 3 1\naddi 1 21 1\naddr 4 1 4\naddr 3 0 3\nseti 0 3 3\nsetr 3 4 1\nmulr 1 3 1\naddr 3 1 1\nmulr 3 1 1\nmuli 1 14 1\nmulr 1 3 1\naddr 4 1 4\nseti 0 3 0\nseti 0 7 3");
            Assert.That("1056", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("10915260", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

