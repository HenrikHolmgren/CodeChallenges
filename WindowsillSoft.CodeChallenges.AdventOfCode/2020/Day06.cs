using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day06 : AdventOfCode2020SolverBase
    {
        protected List<List<string>> _answers = new();
        public Day06(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 6: Custom Customs";

        public override string ExecutePart1() => _answers.Sum(p => p.SelectMany(q=>q).Distinct().Count()).ToString();
        public override string ExecutePart2() => _answers.Sum(p => p.Aggregate(Enumerable.Range('a', 'z'-'a' + 1).Select(p=>(char)p), (p, q) => p.Intersect(q)).Count()).ToString();
        
        public override void Initialize(string input)
        {
            var groups = input.Split("\r\n\r\n").SelectMany(p => p.Split("\n\n"));
            _answers = groups.Select(p => p.Split('\n').ToList()).ToList();
        }
    }
}
