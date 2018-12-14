using System;

namespace WindowsillSoft.AdventOfCode2018.Core
{
    public interface IProblemSolver
    {
        void Solve();
        string Description { get; }
        int SortOrder { get; }
    }
}
