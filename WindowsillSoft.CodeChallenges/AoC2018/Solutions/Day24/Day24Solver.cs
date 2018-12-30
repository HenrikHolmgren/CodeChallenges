using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day24
{
    public class Day24Solver : AoC2018SolverBase
    {
        private List<UnitMob> _allUnits;

        public override string Description => "Day 24: Immune System Simulator 20XX";

        public override int SortOrder => 24;

        public override void Initialize(string input)
        {
            var parser = new Regex(@"(?'count'\d+) units each with (?'hitpoints'\d+) hit points\s*(\((?'enhancements'([^\)]*;?))\))? with an attack that does (?'damCount'\d+) (?'damType'\w+) damage at initiative (?'initiative'\d+)", RegexOptions.Compiled);

            _allUnits = new List<UnitMob>();

            string currentFaction = "Unknown";
            foreach (var line in input.Split(Environment.NewLine))
            {
                if (line.EndsWith(":"))
                    currentFaction = line.TrimEnd(':');
                else if (!string.IsNullOrEmpty(line))
                {
                    var match = parser.Match(line);
                    _allUnits.Add(
                        new UnitMob(currentFaction, int.Parse(match.Groups["count"].Value),
                            int.Parse(match.Groups["hitpoints"].Value),
                            int.Parse(match.Groups["damCount"].Value),
                            match.Groups["damType"].Value,
                            int.Parse(match.Groups["initiative"].Value))
                        .WithEnhancements(match.Groups["enhancements"]?.Value));
                }
            }
        }

        public override string SolvePart1(bool silent = true)
        {
            var mobs = GetWinners(_allUnits, 0, silent);
            return mobs.Where(p => !p.IsDestroyed).Sum(p => p.UnitCount).ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            var interval = (from: 0, to: 1);

            while (GetWinners(_allUnits, interval.to, silent).FirstOrDefault()?.Faction == "Infection")
            {
                interval = (interval.from, interval.to * 2);
                Console.WriteLine(interval);
            }

            while (interval.to - interval.from > 1)
            {
                var probe = ((interval.to - interval.from) / 2) + interval.from;
                if (GetWinners(_allUnits, probe, silent).FirstOrDefault()?.Faction == "Infection")
                    interval = (probe, interval.to);
                else
                    interval = (interval.from, probe);
                Console.WriteLine(interval);
            }

            var winners1 = GetWinners(_allUnits, interval.from, silent);
            var winners2 = GetWinners(_allUnits, interval.to, silent);

            if (winners1.FirstOrDefault()?.Faction == "Infection")
            {
                if (!silent)
                    Console.WriteLine($"Optimal boost is {interval.to}, leading to a victory by {winners2.First().Faction} with {winners2.Sum(p => p.UnitCount)} units remaining.");
                return winners2.Sum(p => p.UnitCount).ToString();
            }
            else
            {
                if (!silent)
                    Console.WriteLine($"Optimal boost is {interval.from}, leading to a victory by {winners1.First().Faction} with {winners1.Sum(p => p.UnitCount)} units remaining.");
                return winners1.Sum(p => p.UnitCount).ToString();
            }
        }

        private List<UnitMob> GetWinners(List<UnitMob> originalArmies, int to, bool silent)
        {
            var mobs = originalArmies.Select(p => p.Clone()).ToList();

            mobs.Where(p => p.Faction == "Immune System").ToList().ForEach(p => p.WithBoost(to));

            while (mobs.GroupBy(p => p.Faction).Count() > 1)
            {
                var attackOrder = mobs.OrderByDescending(p => p.EffectivePower)
                    .ThenByDescending(p => p.Initiative);
                var availableTargets = mobs.ToList();

                var attackPairs = new List<(UnitMob, UnitMob)>();

                foreach (var mob in attackOrder)
                {
                    var target = availableTargets
                        .Where(p => p.Faction != mob.Faction)
                        .OrderByDescending(p => p.GetDamage(mob))
                        .ThenByDescending(p => p.EffectivePower)
                        .ThenByDescending(p => p.Initiative)
                        .FirstOrDefault();
                    if (target != null && target.GetDamage(mob) != 0)
                    {
                        availableTargets.Remove(target);
                        attackPairs.Add((mob, target));
                    }
                }

                var totalDestroyed = 0;

                foreach (var pair in attackPairs.OrderByDescending(p => p.Item1.Initiative))
                    if (!pair.Item1.IsDestroyed)
                        totalDestroyed += pair.Item1.Attack(pair.Item2);

                mobs = mobs.Where(p => !p.IsDestroyed)
                    .ToList();

                if (totalDestroyed == 0)
                {
                    if (!silent)
                        Console.WriteLine("Stalemate! Interpreting as a win for infection...");
                    return mobs.Where(p => p.Faction == "Infection").ToList();
                }
            }
            return mobs;
        }
    }
}
