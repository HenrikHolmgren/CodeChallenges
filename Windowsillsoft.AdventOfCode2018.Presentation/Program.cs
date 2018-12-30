using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode2018.Solutions.Day1;
using WindowsillSoft.AdventOfCode2018.Core;
using WindowsillSoft.AdventOfCode2018.Inputs;
using System.Diagnostics;

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
                menu = menu.Add(solver.Description, () => RunSolver(solver));// { Console.Clear(); solver.SolvePart1(); });
            menu.Add("Exit", () => exit = true);

            while (!exit)
            {
                menu.Display();
                Console.WriteLine();
            }
        }

        private static void RunSolver(IAdventOfCodeSolver solver)
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

        private static List<IAdventOfCodeSolver> GetSolvers()
        {
            return typeof(Day1Solver).Assembly
                .GetTypes()
                .Where(p => typeof(IAdventOfCodeSolver).IsAssignableFrom(p))
                .Where(p => !p.IsAbstract && !p.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IAdventOfCodeSolver>()
                .ToList()
                .OrderBy(p => p.SortOrder)
                .ToList();
        }
    }
}
