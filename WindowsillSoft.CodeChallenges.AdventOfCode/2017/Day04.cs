using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day04 : AdventOfCode2017SolverBase
    {
        private string[] _passPhrases;

        public Day04(IIOProvider provider) : base(provider)
            => _passPhrases = new string[0];

        public override string Name => "Day 4: High-Entropy Passphrases";

        public override void Initialize(string input)
            => _passPhrases = input.Split(Environment.NewLine);

        public override string ExecutePart1()
            => _passPhrases.Count(p => IsValidPassphrase(p)).ToString();

        private bool IsValidPassphrase(string candidate)
        {
            return candidate.Split(' ')
                .GroupBy(p => p)
                .All(p => p.Count() == 1);
        }

        public override string ExecutePart2()
            => _passPhrases.Count(p => IsValidAnagramPassphrase(p)).ToString();

        private bool IsValidAnagramPassphrase(string candidate)
        {
            return candidate.Split(' ')
                 .GroupBy(p => new AnagramPhrase(p))
                 .All(p => p.Count() == 1);
        }
    }

    public class AnagramPhrase
    {
        private readonly string _phrase;
        private readonly Dictionary<char, int> members;
        public AnagramPhrase(string phrase)
        {
            _phrase = phrase;
            members = phrase.GroupBy(p => p)
                .ToDictionary(p => p.Key, p => p.Count());
        }

        public override bool Equals(object obj)
        {
            if (obj is AnagramPhrase ap)
                return Equals(ap);
            return base.Equals(obj);
        }

        public bool Equals(AnagramPhrase ap)
        {
            return members.Count == ap.members.Count &&
                members.All(p =>
                    ap.members.ContainsKey(p.Key)
                    && p.Value == ap.members[p.Key]);
        }

        public override int GetHashCode() => members.Sum(p => p.Key.GetHashCode() + p.Value.GetHashCode());
    }

}
