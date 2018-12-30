using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsillSoft.CodeChallenges.Inputs
{
    public class FullRunInputAttribute : Attribute
    {
        public Type SolverTarget { get; }

        public FullRunInputAttribute(Type target)
            => SolverTarget = target;
    }
}
