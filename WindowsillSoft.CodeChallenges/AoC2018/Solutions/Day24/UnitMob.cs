using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day24
{
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
        }
    }
}
