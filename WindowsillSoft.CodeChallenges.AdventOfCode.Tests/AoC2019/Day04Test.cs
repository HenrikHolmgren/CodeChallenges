using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2019
{
    public class Day04Test : TestBase<Day04>
    {    
        [Test, TestCase(111111, true), TestCase(223450, false), TestCase(123789, false)]
        public void IsValidPassword1(int candidate, bool expected)
        {
            Assert.That(Day04.IsValidPassword1(candidate), Is.EqualTo(expected));
        }

        [Test, TestCase(111111, false), TestCase(223450, false), TestCase(123789, false), 
            TestCase(123444, false), TestCase(111122, true)]
        public void IsValidPassword2(int candidate, bool expected)
        {
            Assert.That(Day04.IsValidPassword2(candidate), Is.EqualTo(expected));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("240298-784956");
            Assert.That(solver.ExecutePart1(), Is.EqualTo("1150"));
            Assert.That(solver.ExecutePart2(), Is.EqualTo("748"));            
        }
    }
}

