using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day02 : AdventOfCode2020SolverBase
    {
        private static Regex parser = new Regex(@"(?'from'\d+)-(?'to'\d+)\s+(?'pattern'\w):\s+(?'subject'\w+)", RegexOptions.Compiled);

        protected record PasswordWithRule(
            int From,
            int To,
            char Pattern,
            string Subject);

        private PasswordWithRule[] Passwords = new PasswordWithRule[0];

        public Day02(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 2: Password Philosophy";

        public override string ExecutePart1()
        {
            int matches = 0;
            foreach (var entry in Passwords)
            {
                var histogram = entry.Subject
                    .ToLookup(p => p);

                var occurencesOfPattern = histogram[entry.Pattern].Count();
                if (entry.From <= occurencesOfPattern && occurencesOfPattern <= entry.To)
                    matches++;
            }
            return matches.ToString();
        }

        public override string ExecutePart2()
        {
            var matches = 0;
            foreach (var entry in Passwords)
            {
                if (entry.Subject[entry.From - 1] == entry.Pattern ^
                    entry.Subject[entry.To - 1] == entry.Pattern)
                    matches++;                
            }
            return matches.ToString();
        }

        public override void Initialize(string input)
        {
            Passwords = ReadAndSplitInput(input, MapEntry, '\n').ToArray();
        }

        private PasswordWithRule MapEntry(string arg)
        {
            var match = parser.Match(arg);
            if (!match.Success)
                throw new ArgumentException($"Invalid input {arg} doesn't parse as a password entry.");
            return new PasswordWithRule(
                int.Parse(match.Groups["from"].Value),
                int.Parse(match.Groups["to"].Value),
                match.Groups["pattern"].Value.Single(),
                match.Groups["subject"].Value);
        }
    }
}
