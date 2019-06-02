using Moq;
using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2018
{
    public class Day07Test : TestBase<Day07>
    {
        [Test,
            TestCase("Step C must be finished before step A can begin.\nStep C must be finished before step F can begin.\nStep A must be finished before step B can begin.\nStep A must be finished before step D can begin.\nStep B must be finished before step E can begin.\nStep D must be finished before step E can begin.\nStep F must be finished before step E can begin.", "2", "0", "CABDFE")]
        public void Part1(string input, string workerCount, string additionalTime, string result)
        {
            var IOProvider = Mock.Get(this._provider);
            IOProvider.Setup(p => p.RequestInput("Worker count?"))
                .Returns(workerCount);

            IOProvider.Setup(p => p.RequestInput("Additional time pr. task?"))
                .Returns(additionalTime);

            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("Step C must be finished before step A can begin.\nStep C must be finished before step F can begin.\nStep A must be finished before step B can begin.\nStep A must be finished before step D can begin.\nStep B must be finished before step E can begin.\nStep D must be finished before step E can begin.\nStep F must be finished before step E can begin.", "2", "0", "15")]
        public void Part2(string input, string workerCount, string additionalTime, string result)
        {
            var IOProvider = Mock.Get(this._provider);
            IOProvider.Setup(p => p.RequestInput("Worker count?"))
                .Returns(workerCount);

            IOProvider.Setup(p => p.RequestInput("Additional time pr. task?"))
                .Returns(additionalTime);

            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }

        [Test]
        public void FullRun()
        {
            var IOProvider = Mock.Get(this._provider);
            IOProvider.Setup(p => p.RequestInput("Worker count?"))
                .Returns("5");

            IOProvider.Setup(p => p.RequestInput("Additional time pr. task?"))
                .Returns("60");

            var solver = GetSolver("Step B must be finished before step X can begin.\nStep V must be finished before step F can begin.\nStep K must be finished before step C can begin.\nStep S must be finished before step D can begin.\nStep C must be finished before step A can begin.\nStep H must be finished before step X can begin.\nStep Q must be finished before step W can begin.\nStep X must be finished before step F can begin.\nStep J must be finished before step R can begin.\nStep D must be finished before step O can begin.\nStep F must be finished before step P can begin.\nStep M must be finished before step Z can begin.\nStep R must be finished before step I can begin.\nStep Y must be finished before step O can begin.\nStep G must be finished before step Z can begin.\nStep Z must be finished before step P can begin.\nStep O must be finished before step L can begin.\nStep A must be finished before step P can begin.\nStep U must be finished before step L can begin.\nStep L must be finished before step W can begin.\nStep P must be finished before step W can begin.\nStep I must be finished before step W can begin.\nStep E must be finished before step N can begin.\nStep W must be finished before step N can begin.\nStep T must be finished before step N can begin.\nStep G must be finished before step E can begin.\nStep K must be finished before step T can begin.\nStep I must be finished before step T can begin.\nStep V must be finished before step H can begin.\nStep W must be finished before step T can begin.\nStep M must be finished before step A can begin.\nStep C must be finished before step W can begin.\nStep B must be finished before step Y can begin.\nStep Y must be finished before step N can begin.\nStep L must be finished before step N can begin.\nStep M must be finished before step R can begin.\nStep L must be finished before step I can begin.\nStep J must be finished before step N can begin.\nStep K must be finished before step M can begin.\nStep O must be finished before step U can begin.\nStep P must be finished before step N can begin.\nStep Y must be finished before step I can begin.\nStep V must be finished before step Q can begin.\nStep H must be finished before step R can begin.\nStep M must be finished before step P can begin.\nStep K must be finished before step L can begin.\nStep J must be finished before step A can begin.\nStep D must be finished before step F can begin.\nStep Q must be finished before step P can begin.\nStep C must be finished before step H can begin.\nStep U must be finished before step I can begin.\nStep A must be finished before step T can begin.\nStep C must be finished before step P can begin.\nStep U must be finished before step T can begin.\nStep O must be finished before step T can begin.\nStep O must be finished before step I can begin.\nStep S must be finished before step I can begin.\nStep Z must be finished before step E can begin.\nStep Y must be finished before step T can begin.\nStep K must be finished before step O can begin.\nStep O must be finished before step A can begin.\nStep Z must be finished before step T can begin.\nStep Z must be finished before step U can begin.\nStep U must be finished before step P can begin.\nStep P must be finished before step I can begin.\nStep S must be finished before step W can begin.\nStep S must be finished before step P can begin.\nStep S must be finished before step Q can begin.\nStep C must be finished before step E can begin.\nStep G must be finished before step U can begin.\nStep D must be finished before step L can begin.\nStep K must be finished before step S can begin.\nStep R must be finished before step O can begin.\nStep C must be finished before step G can begin.\nStep V must be finished before step G can begin.\nStep A must be finished before step W can begin.\nStep Z must be finished before step O can begin.\nStep J must be finished before step O can begin.\nStep F must be finished before step E can begin.\nStep U must be finished before step E can begin.\nStep E must be finished before step W can begin.\nStep M must be finished before step O can begin.\nStep C must be finished before step U can begin.\nStep G must be finished before step P can begin.\nStep C must be finished before step I can begin.\nStep Z must be finished before step A can begin.\nStep C must be finished before step J can begin.\nStep Q must be finished before step R can begin.\nStep E must be finished before step T can begin.\nStep F must be finished before step Y can begin.\nStep Z must be finished before step N can begin.\nStep I must be finished before step N can begin.\nStep X must be finished before step E can begin.\nStep I must be finished before step E can begin.\nStep Q must be finished before step O can begin.\nStep R must be finished before step L can begin.\nStep K must be finished before step W can begin.\nStep Y must be finished before step L can begin.\nStep M must be finished before step I can begin.\nStep F must be finished before step O can begin.\nStep A must be finished before step E can begin.");
            Assert.That("BKCJMSDVGHQRXFYZOAULPIEWTN", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("1040", Is.EqualTo(solver.ExecutePart2()));
        }
    }
}

