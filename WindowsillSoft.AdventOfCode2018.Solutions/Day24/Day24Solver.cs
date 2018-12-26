using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day24
{
    public class Day24Solver : IProblemSolver
    {
        public string Description => "Day 24: Immune System Simulator 20XX";

        public int SortOrder => 24;

        public void Solve()
        {
            var originalArmies = GetInput("Day24/Day24Input.txt")
                .ToList();

            Part1(originalArmies);
            Part2(originalArmies);
        }

        private void Part1(List<UnitMob> originalArmies)
        {
            var mobs = GetWinners(originalArmies, 0);
            Console.WriteLine($"Winning army has: {mobs.Where(p => !p.IsDestroyed).Sum(p => p.UnitCount)}");
        }


        private void Part2(List<UnitMob> originalArmies)
        {
            var interval = (from: 0, to: 1);

            while (GetWinners(originalArmies, interval.to).FirstOrDefault()?.Faction == "Infection")
            {
                interval = (interval.from, interval.to * 2);
                Console.WriteLine(interval);
            }

            while (interval.to - interval.from > 1)
            {
                var probe = ((interval.to - interval.from) / 2) + interval.from;
                if (GetWinners(originalArmies, probe).FirstOrDefault()?.Faction == "Infection")
                    interval = (probe, interval.to);
                else
                    interval = (interval.from, probe);
                Console.WriteLine(interval);
            }

            var winners1 = GetWinners(originalArmies, interval.from);
            var winners2 = GetWinners(originalArmies, interval.to);
            if (winners1.FirstOrDefault()?.Faction == "Infection")
                Console.WriteLine($"Optimal boost is {interval.to}, leading to a victory by {winners2.First().Faction} with {winners2.Sum(p => p.UnitCount)} units remaining.");

            else
                Console.WriteLine($"Optimal boost is {interval.from}, leading to a victory by {winners1.First().Faction} with {winners1.Sum(p => p.UnitCount)} units remaining.");

        }

        private List<UnitMob> GetWinners(List<UnitMob> originalArmies, int to)
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
                    Console.WriteLine("Stalemate! Interpreting as a win for infection...");
                    return mobs.Where(p => p.Faction == "Infection").ToList();
                }
            }
            return mobs;
        }

        private IEnumerable<UnitMob> GetInput(string path)
        {
            var parser = new Regex(@"(?'count'\d+) units each with (?'hitpoints'\d+) hit points\s*(\((?'enhancements'([^\)]*;?))\))? with an attack that does (?'damCount'\d+) (?'damType'\w+) damage at initiative (?'initiative'\d+)", RegexOptions.Compiled);

            string currentFaction = "Unknown";
            foreach (var line in File.ReadAllLines(path))
            {
                if (line.EndsWith(":"))
                    currentFaction = line.TrimEnd(':');
                else if (!string.IsNullOrEmpty(line))
                {
                    var match = parser.Match(line);
                    yield return new UnitMob(currentFaction, int.Parse(match.Groups["count"].Value),
                        int.Parse(match.Groups["hitpoints"].Value),
                        int.Parse(match.Groups["damCount"].Value),
                        match.Groups["damType"].Value,
                        int.Parse(match.Groups["initiative"].Value))
                        .WithEnhancements(match.Groups["enhancements"]?.Value);
                }
            }
        }
    }
    public class UnitMob
    {
        private int _HitPoints;
        private int _Damage;
        private string _DamageType;
        List<string> _Weaknesses;
        List<string> _Immunities;

        public string Faction { get; }
        public int Initiative { get; }
        public int UnitCount { get; private set; }
        public bool IsDestroyed => UnitCount <= 0;
        public int EffectivePower => UnitCount * _Damage;

        public UnitMob(string faction, int count, int hitPoints, int damage, string damageType, int initiative)
        {
            this.Faction = faction;
            this.UnitCount = count;
            this._HitPoints = hitPoints;
            this._Damage = damage;
            this._DamageType = damageType;
            this.Initiative = initiative;
            _Weaknesses = new List<string>();
            _Immunities = new List<string>();
        }

        public UnitMob Clone()
        {
            return new UnitMob(Faction, UnitCount, _HitPoints, _Damage, _DamageType, Initiative)
            {
                _Weaknesses = _Weaknesses,
                _Immunities = _Immunities
            };
        }

        public UnitMob WithBoost(int boostAmount)
        {
            _Damage += boostAmount;
            return this;
        }

        public UnitMob WithEnhancements(string value)
        {
            if (string.IsNullOrEmpty(value))
                return this;
            var enhancementParser = new Regex(@"(?'type'(weak|immune)) to (?'damtype'[\w, ]+)*");
            foreach (var enhancement in value.Split(';'))
            {
                var match = enhancementParser.Match(enhancement);
                List<string> target;
                switch (match.Groups["type"].Value)
                {
                    case "weak": target = _Weaknesses; break;
                    case "immune": target = _Immunities; break;
                    default: throw new InvalidOperationException("Unknown damage enhancement type " + match.Groups["type"].Value);
                }
                foreach (var damType in match.Groups["damtype"].Value.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries))
                    target.Add(damType.Trim());
            }

            return this;
        }

        public int GetDamage(UnitMob attacker)
        {
            if (_Immunities.Contains(attacker._DamageType))
                return 0;
            var attackPower = attacker._Damage * attacker.UnitCount;
            if (_Weaknesses.Contains(attacker._DamageType))
                attackPower *= 2;
            return attackPower;
        }

        public override string ToString()
        {
            var description = $"{UnitCount} units each with {_HitPoints} hit points";
            if (_Weaknesses.Any() || _Immunities.Any())
            {
                description += "(";
                if (_Weaknesses.Any())
                {
                    description += "weak to " + string.Join(", ", _Weaknesses);
                    if (_Immunities.Any())
                        description += "; ";
                }
                if (_Immunities.Any())
                    description += "immune to " + string.Join(", ", _Immunities);
                description += ")";
            }
            return description + $" with an attack that does {_Damage} {_DamageType} damage at initiative {Initiative}";
        }

        internal int Attack(UnitMob defender)
        {
            double damage = defender.GetDamage(this);
            var unitsDestroyed = Math.Min(defender.UnitCount, (int)Math.Floor(damage / defender._HitPoints));
            defender.UnitCount -= unitsDestroyed;
            return unitsDestroyed;
            //Console.WriteLine($"{Faction} attacked {defender.Faction} for {damage} killing {unitsDestroyed} units ({defender.UnitCount} remaining).");
        }
    }
}
