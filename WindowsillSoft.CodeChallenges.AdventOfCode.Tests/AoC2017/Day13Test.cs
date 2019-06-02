using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2017;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2017
{
    public class Day13Test : TestBase<Day13>
    {
        [Test,
            TestCase("0: 3\n1: 2\n4: 4\n6: 4", "24")]
        public void Part1(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart1(), Is.EqualTo(output));
        }

        [Test,
            TestCase("0: 3\n1: 2\n4: 4\n6: 4", "10")]
        public void Part2(string input, string output)
        {
            var solver = GetSolver(input);
            Assert.That(solver.ExecutePart2(), Is.EqualTo(output));
        }

        [Test]
        public void FullRun()
        {
            var solver = GetSolver("0: 3\n1: 2\n2: 4\n4: 6\n6: 5\n8: 8\n10: 6\n12: 4\n14: 8\n16: 6\n18: 8\n20: 8\n22: 6\n24: 8\n26: 9\n28: 12\n30: 8\n32: 14\n34: 10\n36: 12\n38: 12\n40: 10\n42: 12\n44: 12\n46: 12\n48: 12\n50: 14\n52: 12\n54: 14\n56: 12\n60: 14\n62: 12\n64: 14\n66: 14\n68: 14\n70: 14\n72: 14\n74: 14\n78: 26\n80: 18\n82: 17\n86: 18\n88: 14\n96: 18");
            var result = solver.Execute();
            Assert.That(result.Part1Result, Is.EqualTo("748"));
            Assert.That(result.Part2Result, Is.EqualTo("3873662"));
        }
    }
}
