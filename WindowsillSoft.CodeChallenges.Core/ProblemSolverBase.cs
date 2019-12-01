using System;

namespace WindowsillSoft.CodeChallenges.Core
{
    public abstract class ProblemSolverBase
    {
        public abstract string Name { get; }

        public abstract void Initialize();
        public abstract object ExecuteObject();
    }

    [SolverCategory("Code challenges")]
    public abstract class ProblemSolverBase<TResult> : ProblemSolverBase
    {
        protected IIOProvider IO { get; }

        public abstract TResult Execute();
#pragma warning disable CS8603 // Possible null reference return.
        public override object ExecuteObject() => Execute();
#pragma warning restore CS8603 // Possible null reference return.

        public ProblemSolverBase(IIOProvider provider) => IO = provider;

    }

    public class SolverCategoryAttribute : Attribute
    {
        public string CategoryName { get; }

        public SolverCategoryAttribute(string categoryName) => CategoryName = categoryName;
    }
}
