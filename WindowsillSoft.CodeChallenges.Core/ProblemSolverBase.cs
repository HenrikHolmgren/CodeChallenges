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
        public override object ExecuteObject() => Execute();

        public ProblemSolverBase(IIOProvider provider) => IO = provider;

    }

    public class SolverCategoryAttribute : Attribute
    {
        public string CategoryName { get; }

        public SolverCategoryAttribute(string categoryName) => CategoryName = categoryName;
    }
}
