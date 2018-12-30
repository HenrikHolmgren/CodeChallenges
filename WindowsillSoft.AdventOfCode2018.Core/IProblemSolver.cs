using System;

namespace WindowsillSoft.AdventOfCode2018.Core
{
    public interface IAdventOfCodeSolver
    {
        //void Solve();
        void Initialize(string input);
        string SolvePart1(bool silent = true);
        string SolvePart2(bool silent = true);

        string Description { get; }
        int SortOrder { get; }
    }
}
