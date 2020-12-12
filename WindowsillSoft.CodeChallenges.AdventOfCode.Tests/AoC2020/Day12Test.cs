using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2020;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2020
{
    public class Day12Test : TestBase<Day12>
    {
        [Test,
            TestCase("F10\nN3\nF7\nR90\nF11", "25")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("F10\nN3\nF7\nR90\nF11", "286")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }
        [Test]
        public void FullRun()
        {
            var solver = GetSolver("W5\nN3\nW4\nF2\nN3\nR180\nE2\nS4\nE5\nN4\nE2\nL90\nF81\nE5\nR180\nE2\nF88\nL90\nN4\nE1\nF90\nS3\nW3\nR90\nF80\nE4\nF28\nR180\nS1\nF80\nE4\nF18\nS5\nW4\nF13\nR90\nS3\nW5\nN2\nF76\nL90\nN4\nF49\nL180\nE5\nR90\nF51\nE4\nL180\nF86\nS5\nL180\nF3\nE5\nW1\nR90\nF54\nN4\nE5\nR90\nE4\nN3\nW1\nE4\nR180\nW2\nF2\nS5\nW4\nE3\nR90\nF49\nR90\nW5\nN3\nF47\nE1\nL90\nE2\nF86\nE3\nR90\nF100\nF84\nN2\nF12\nL90\nN4\nW5\nF40\nN5\nF68\nW3\nR90\nW4\nN1\nF63\nW5\nS3\nF52\nR180\nW1\nW4\nL90\nN4\nR90\nS4\nL90\nF77\nR90\nE5\nF20\nN3\nW5\nR90\nE5\nR180\nF63\nS1\nW1\nW5\nF12\nE5\nL90\nN4\nF83\nW4\nF92\nW2\nF41\nE3\nS5\nR90\nS5\nW2\nN1\nF4\nN1\nF50\nL180\nF73\nN2\nL90\nE2\nS5\nF19\nL90\nE1\nN4\nL90\nS2\nL90\nF90\nL90\nN3\nE1\nF32\nE1\nF66\nR180\nE1\nS2\nF72\nR90\nS2\nE1\nN3\nF24\nW4\nF32\nW5\nS3\nE2\nF52\nW5\nF54\nE4\nF97\nN1\nR90\nW3\nR180\nE2\nS3\nR90\nN1\nN3\nF76\nR90\nF43\nE3\nN4\nL90\nE4\nF32\nE4\nS3\nF46\nR90\nN4\nE5\nL90\nF33\nW1\nW1\nN1\nE3\nS1\nR90\nW3\nL90\nF59\nW3\nS1\nF7\nW3\nF85\nR90\nF61\nE5\nS5\nF25\nF8\nL90\nN3\nF80\nW4\nF89\nN3\nE5\nN5\nL90\nF50\nF19\nL90\nN2\nR90\nF8\nW5\nS5\nL90\nF63\nS5\nS2\nF44\nN1\nW1\nL90\nR90\nW2\nF24\nW3\nS4\nR90\nF69\nE5\nF77\nW4\nF38\nR180\nW3\nS5\nN2\nF91\nF44\nN2\nW2\nR90\nW5\nF48\nW3\nR90\nF74\nE4\nS3\nR90\nE3\nL90\nF81\nW1\nF69\nW2\nN3\nR90\nE1\nR90\nL90\nE5\nS4\nE2\nS5\nF58\nN3\nF50\nL90\nN1\nL90\nN4\nL180\nW2\nL90\nF61\nL90\nS5\nE1\nS4\nW1\nS3\nE4\nF62\nS2\nL270\nF97\nR90\nS5\nL180\nF66\nE1\nR90\nE3\nL180\nF98\nR90\nF37\nR90\nF18\nN3\nR90\nE2\nS3\nL90\nS4\nF82\nW3\nF72\nN1\nE4\nF67\nL90\nW1\nS2\nF94\nR90\nF62\nN4\nW2\nS5\nR180\nF41\nW5\nF9\nW2\nF34\nL90\nN3\nR90\nF1\nN4\nE4\nR90\nF39\nS5\nW5\nN5\nR180\nF32\nW5\nF97\nR90\nN4\nW3\nR90\nN4\nW2\nN5\nW5\nR90\nS4\nL90\nF99\nN2\nR90\nE4\nN5\nE1\nF67\nR180\nW3\nS2\nE2\nF95\nE1\nS1\nW5\nS3\nE2\nF64\nL90\nF29\nS3\nF33\nF46\nS2\nR90\nN4\nE1\nF11\nF50\nL90\nE2\nF72\nL180\nN2\nE4\nN1\nE3\nS1\nF37\nW1\nR90\nN3\nL90\nW3\nF62\nR90\nF88\nW1\nS4\nE3\nL90\nS4\nR90\nE4\nS2\nF81\nW5\nF82\nL90\nF19\nR90\nR270\nE4\nF27\nR90\nN1\nW3\nR90\nW1\nS4\nL90\nW1\nF24\nL180\nR90\nS1\nE3\nS4\nL90\nE3\nF71\nR180\nS1\nF33\nS1\nF49\nS1\nR180\nE4\nL180\nF44\nR90\nW2\nF26\nR90\nL180\nL180\nF31\nS3\nE4\nR90\nW1\nL90\nW1\nN5\nF25\nN3\nL180\nF4\nN3\nS5\nE4\nR90\nS2\nL90\nF28\nE4\nN3\nL90\nS1\nR90\nN4\nW1\nN2\nR180\nE4\nL90\nS5\nR180\nS5\nF14\nE3\nF38\nS2\nF1\nE1\nF46\nR270\nF69\nL180\nN1\nR90\nW5\nN4\nF22\nR90\nN1\nL180\nF16\nN2\nE1\nN4\nF68\nL90\nE2\nF6\nE2\nF2\nE4\nR90\nW4\nE2\nR90\nS1\nW1\nS5\nF87\nS5\nF9\nW5\nF91\nL90\nS2\nR270\nF73\nL90\nF17\nL90\nE4\nW1\nR90\nF40\nE5\nF7\nR180\nR90\nR180\nE5\nF89\nR180\nW2\nL180\nF31\nE2\nS1\nW2\nF11\nL180\nE1\nF55\nE5\nS4\nL90\nS5\nW4\nR180\nF23\nE3\nR90\nF12\nE3\nF3\nS3\nW3\nL90\nW2\nN5\nE2\nF77\nE4\nS3\nF11\nW4\nF23\nE1\nR90\nF61\nE3\nL90\nS3\nN5\nW2\nR180\nW2\nS2\nN5\nE1\nS2\nL90\nW3\nR90\nF89\nR270\nN3\nL90\nR90\nW4\nS2\nW4\nL90\nS1\nE4\nE4\nS3\nR270\nF47\nL90\nE1\nF10\nL180\nW4\nR90\nN2\nF97\nL180\nF82\nN5\nL90\nS1\nE3\nF14\nR90\nF23\nN2\nF34\nL270\nE2\nF77\nE5\nL90\nS3\nR270\nF12\nN1\nE3\nR90\nE2\nF4\nW3\nE3\nF33\nS4\nR180\nS5\nR90\nE1\nR270\nF53\nN4\nL90\nN1\nW2\nS5\nE2\nR180\nW2\nS3\nL90\nN4\nW3\nN3\nF84\nE5\nN3\nL90\nF48\nW4\nF18\nL90\nW3\nS4\nE2\nS4\nF64\nN1\nF96\nS3\nE5\nN4\nW2\nF22\nW5\nL90\nF23\nE2\nN1\nF92\nF16\nL180\nE4\nN1\nF75\nL90\nW4\nL270\nW3\nS4\nL90\nF29\nW4\nS2\nF47\nR90\nN3\nL90\nS3\nW3\nN1\nF45\nN2\nL90\nE4\nN1\nL90\nE3\nR90\nN3\nF86\nW1\nN5\nW3\nS5\nL90\nS4\nW2\nF44");
            Assert.That(solver.ExecutePart1(), Is.EqualTo("1533")); 
            Assert.That(solver.ExecutePart2(), Is.EqualTo("25235"));
        }
    }
}