using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day03Test : TestBase<Day03>
    {
        [Test, 
            TestCase("1", "0"),
            TestCase("12", "3"),
            TestCase("23", "2"),
            TestCase("1024", "31")]
        public void Part1(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("1", "2"),
            TestCase("12", "23"),
            TestCase("59", "122"),
            TestCase("747", "806")]
        public void Part2(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        public void FullRun()
        {
            var solver = GetSolver("325489");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("552"));
            Assert.That(result.Part2Result, Is.EqualTo("330785"));
        }

    }
}
