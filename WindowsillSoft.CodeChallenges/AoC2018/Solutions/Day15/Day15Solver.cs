using System;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day15
{
    public class Day15Solver : AoC2018SolverBase
    {
        private string[] _mapLayout;

        public override string Description => "Day 15: Beverage Bandits";

        public override int SortOrder => 15;

        public override void Initialize(string input)
        {
            _mapLayout = input.Split(Environment.NewLine);
        }

        public override string SolvePart1(bool silent = true)
        {
            var map = Map.Parse(_mapLayout);
            if (!silent)
                Console.WriteLine($"Part 1 - simulating battle...");

            while (map.Step(false)) ;
            if (!silent)
                map.PrintStats();

            return map.GetScore(verbose: false);
        }

        public override string SolvePart2(bool silent = true)
        {
            int boost = 1;
            if (!silent)
                Console.WriteLine("Part 2 - seeking min boost...");

            //Do NOT try to use binary search here - cost me several hours only to find that at boost 13, elves all survive, but at 14, the goblins take one of them out again >_<
            while (true)
            {
                if (AnyElvesLost(boost, silent))
                    boost++;
                else
                    break;
            }

            if (!silent)
                Console.WriteLine($"Optimal boost: {boost}");

            var map = Map.Parse(_mapLayout);
            map.ApplyElfBonus(boost);
            while (map.Step(false)) ;

            return map.GetScore(verbose: false);
        }

        private bool AnyElvesLost(int boost, bool silent)
        {
            var map = Map.Parse(_mapLayout);
            map.ApplyElfBonus(boost);
            while (map.Step(false)) ;

            if (!map.Actors.Any(p => p.Kind == 'E' && p.Health < 0))
            {
                if (!silent)
                    Console.WriteLine($"With a boost of {boost}, every elf survives the battle. Board score: {map.GetScore(verbose: true)}.");
                return false;
            }
            else
            {
                if (!silent)
                    Console.WriteLine($"With a boost of {boost}, {map.Actors.Where(p => p.Kind == 'E').Where(p => p.Health < 0).Count()} elves died :(");
                return true;
            }
        }
    }
}
