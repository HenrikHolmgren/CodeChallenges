using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day1;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Inputs;
using System.Diagnostics;
using WindowsillSoft.CodeChallenges.Common;

namespace WindowsillSoft.CodeChallenges.Presentation
{
    public class Program
    {
        static void Main(string[] args)
        {
            var solvers = GetSolvers();

            bool exit = false;
            var categoryMenu = new EasyConsoleCore.Menu();
            foreach (var category in solvers.Select(p => p.Category).Distinct().OrderBy(p => p))
                categoryMenu.Add(category, () => ShowCategoryMenu(category, solvers));
            categoryMenu.Add("Exit", () => exit = true);

            while (!exit)
            {
                Console.WriteLine("Select category:");
                categoryMenu.Display();
                Console.WriteLine();
            }
        }

        private static void ShowCategoryMenu(string category, List<CodeChallengeSolverBase> solvers)
        {
            Console.WriteLine("Select solver:");
            var menu = new EasyConsoleCore.Menu();
            foreach (var solver in solvers.Where(p => p.Category == category))
                menu = menu.Add(solver.Description, () => RunSolver(solver));

            menu.Display();
        }

        private static void RunSolver(CodeChallengeSolverBase solver)
        {
            switch (solver)
            {
                case AdventOfCodeSolverBase aocSolver:
                    RunAoCSolver(aocSolver);
                    break;
            }
        }

        private static void RunAoCSolver(AdventOfCodeSolverBase solver)
        {
            var input = typeof(Day1Input).Assembly
                .GetTypes()
                .SelectMany(p => p.GetFields()
                    .Where(q => q.GetCustomAttributes(false)
                    .OfType<FullRunInputAttribute>().Any()))
                .Where(p => p.GetCustomAttributes(false)
                    .OfType<FullRunInputAttribute>()
                    .Single().SolverTarget == solver.GetType())
                .Single().GetValue(null);

            Console.Clear();

            var sw = Stopwatch.StartNew();
            solver.Initialize((string)input);
            var initializeTime = sw.ElapsedMilliseconds;
            sw.Restart();

            var part1Value = solver.SolvePart1(silent: false);
            var part1Time = sw.ElapsedMilliseconds;
            sw.Restart();

            var part2Value = solver.SolvePart2(silent: false);
            var part2Time = sw.ElapsedMilliseconds;

            Console.WriteLine("Part1:");
            Console.WriteLine(part1Value);
            Console.WriteLine();
            Console.WriteLine("Part2:");
            Console.WriteLine(part2Value);
            Console.WriteLine();
            Console.WriteLine($"Time taken: Initialization: {initializeTime}ms, part 1 in {part1Time}ms, part 2 in {part2Time}ms.");

        }

        private static List<CodeChallengeSolverBase> GetSolvers()
        {
            return typeof(Day1Solver).Assembly
                .GetTypes()
                .Where(p => typeof(CodeChallengeSolverBase).IsAssignableFrom(p))
                .Where(p => !p.IsAbstract && !p.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<CodeChallengeSolverBase>()
                .ToList()
                .OrderBy(p => p.SortOrder)
                .ToList();
        }
    }
}
