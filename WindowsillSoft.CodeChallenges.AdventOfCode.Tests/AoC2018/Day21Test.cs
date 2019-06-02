using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day21Test : TestBase<Day21>
    {
        [Test]
        public void FullRun()
        {
            var solver = GetSolver("#ip 1\nseti 123 0 5\nbani 5 456 5\neqri 5 72 5\naddr 5 1 1\nseti 0 0 1\nseti 0 7 5\nbori 5 65536 4\nseti 13159625 6 5\nbani 4 255 3\naddr 5 3 5\nbani 5 16777215 5\nmuli 5 65899 5\nbani 5 16777215 5\ngtir 256 4 3\naddr 3 1 1\naddi 1 1 1\nseti 27 9 1\nseti 0 0 3\naddi 3 1 2\nmuli 2 256 2\ngtrr 2 4 2\naddr 2 1 1\naddi 1 1 1\nseti 25 0 1\naddi 3 1 3\nseti 17 4 1\nsetr 3 3 4\nseti 7 5 1\neqrr 5 0 3\naddr 3 1 1\nseti 5 6 1");
            Assert.That("3941014", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("13775890", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

