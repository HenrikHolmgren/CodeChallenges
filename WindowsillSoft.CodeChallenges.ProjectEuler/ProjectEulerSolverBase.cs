﻿using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    [SolverCategory("Project Euler")]
    public abstract class ProjectEulerSolverBase : ProblemSolverBase<string>
    {
        public ProjectEulerSolverBase(IIOProvider provider) : base(provider)
        {
        }
    }
}
