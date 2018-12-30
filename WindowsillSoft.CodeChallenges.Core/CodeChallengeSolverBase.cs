using System;

namespace WindowsillSoft.CodeChallenges.Core
{
    public abstract class CodeChallengeSolverBase
    {
        public abstract void Initialize(string input);
        public abstract string Description { get; }
        public abstract int SortOrder { get; }
        public abstract string Category { get; }
    }
}
