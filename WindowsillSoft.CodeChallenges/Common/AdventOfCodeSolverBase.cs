using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.Common
{
    public abstract class AdventOfCodeSolverBase : CodeChallengeSolverBase
    {
        public abstract int Year { get; }
        public override string Category => "Advent of Code " + Year;
        public abstract string SolvePart1(bool silent = true);
        public abstract string SolvePart2(bool silent = true);
    }
}
