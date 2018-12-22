using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode2018.Solutions.Day1;
using WindowsillSoft.AdventOfCode2018.Core;

namespace Windowsillsoft.AdventOfCode2018.Presentation
{
    public class Program
    {
        static void Main(string[] args)
        {
            var solvers = GetSolvers();

            bool exit = false;
            var menu = new EasyConsoleCore.Menu();
            foreach (var solver in solvers)
                menu = menu.Add(solver.Description, () => { Console.Clear(); solver.Solve(); });
            menu.Add("Exit", () => exit = true);

            while (!exit)
            {
                menu.Display();
                Console.WriteLine();
            }
        }

        private static List<IProblemSolver> GetSolvers()
        {
            return typeof(Day1Solver).Assembly
                .GetTypes()
                .Where(p => typeof(IProblemSolver).IsAssignableFrom(p))
                .Where(p => !p.IsAbstract && !p.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IProblemSolver>()
                .ToList()
                .OrderBy(p => p.SortOrder)
                .ToList();
        }
    }
}
