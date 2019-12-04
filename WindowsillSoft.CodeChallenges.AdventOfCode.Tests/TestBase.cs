using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests
{
    public class TestBase<T> where T : AdventOfCodeSolverBase
    {
        protected IIOProvider _provider { get; private set; }

        [SetUp]
        public void Setup()
        {
            _provider = new Mock<IIOProvider>().Object;
        }

        public T GetSolver(string input)
        {
            Mock.Get(_provider).Setup(p => p.RequestFile(It.IsAny<string>())).Returns(input);
            var solver = Activator.CreateInstance(typeof(T), _provider) as AdventOfCodeSolverBase;
            solver.Initialize();
            return (T)solver;
        }
    }
}
