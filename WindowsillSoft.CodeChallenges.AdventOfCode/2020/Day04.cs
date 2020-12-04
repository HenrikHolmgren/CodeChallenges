using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day04 : AdventOfCode2020SolverBase
    {
        protected List<Dictionary<string, string>> _passports = new();
        protected record ValidationRule(string Key, bool Required, Func<string, bool> Validate);

        List<ValidationRule> _rules = new List<ValidationRule>
        {
            new ValidationRule("byr", true, p => int.TryParse(p, out var res) && res >= 1920 && res <= 2002),
            new ValidationRule("iyr", true, p => int.TryParse(p, out var res) && res >= 2010 && res <= 2020),
            new ValidationRule("eyr", true, p => int.TryParse(p, out var res) && res >= 2020 && res <= 2030),
            new ValidationRule("hgt", true, IsValidHeight),
            new ValidationRule("hcl", true, p=>Regex.IsMatch(p, "^#[0-9a-f]{6}$")),
            new ValidationRule("ecl", true, p=>new[]{"amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(p) ),
            new ValidationRule("pid", true, p=>Regex.IsMatch(p, "^\\d{9}$")),
            new ValidationRule("cid", false, p => true),
        };

        private static bool IsValidHeight(string arg)
        {
            if (arg.EndsWith("cm"))
                return int.TryParse(arg[0..^2], out var res)
                    && res >= 150 && res <= 193;
            else if (arg.EndsWith("in"))
                return int.TryParse(arg[0..^2], out var res)
                    && res >= 59 && res <= 76;
            else
                return false;
        }

        protected record Position(int X, int Y);
        public Day04(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 4: Passport Processing";

        public override string ExecutePart1() => _passports.Count(PassportHasAllRequiredFields).ToString();
        public override string ExecutePart2()// => _passwords.Count(PassportIsValid).ToString();
        {
            var validPassports = _passports.Where(PassportIsValid).ToList();
            return validPassports.Count().ToString();
        }


        private bool PassportHasAllRequiredFields(Dictionary<string, string> passport)
            => _rules.Where(p => p.Required).All(p => passport.Keys.Contains(p.Key));

        private bool PassportIsValid(Dictionary<string, string> passport)
            => _rules.Where(p => p.Required).All(p => passport.Keys.Contains(p.Key)
            && p.Validate(passport[p.Key]));


        public override void Initialize(string input)
        {
            var inputLines = input.Split('\n').Select(p => p.Trim()).ToArray();

            var current = new Dictionary<string, string>();
            foreach (var line in inputLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    _passports.Add(current);
                    current = new();
                    continue;
                }
                foreach (var property in line.Split(' '))
                {
                    var keyValue = property.Split(':');
                    current[keyValue[0]] = keyValue[1];
                }
            }

            if (current.Keys.Any())
                _passports.Add(current);

        }
    }
}
