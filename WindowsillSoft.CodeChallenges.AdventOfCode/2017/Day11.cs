using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day11 : AdventOfCode2017SolverBase
    {
        private string[] _steps;

        public Day11(IIOProvider provider) : base(provider)
            => _steps = new string[0];

        public override string Name => "Day 11: Hex Ed";

        private readonly List<Transformation> _transformations = new List<Transformation>
        {
            new Transformation("n", "s", null ),
            new Transformation("ne", "sw", null ),
            new Transformation("nw", "se", null ),

            new Transformation("ne", "s", "se"),
            new Transformation("nw", "s", "sw"),
            new Transformation("se", "n", "ne"),
            new Transformation("sw", "n", "nw"),
            new Transformation("sw", "se", "s"),
            new Transformation("ne", "nw", "n"),
        };

        public override void Initialize(string input) => _steps = input.Split(',');

        public override string ExecutePart1()
        {
            var moves = _steps.GroupBy(p => p)
                            .ToDictionary(p => p.Key, p => p.Count());

            InitializeMoveset(moves);
            ReduceMoveset(moves);

            return moves.Sum(p => p.Value).ToString();
        }


        public override string ExecutePart2()
        {
            var moves = new Dictionary<string, int>();
            InitializeMoveset(moves);

            var maxDist = 0;

            foreach (var move in _steps)
            {
                moves[move]++;
                ReduceMoveset(moves);
                var currentDist = moves.Sum(p => p.Value);
                if (currentDist > maxDist)
                    maxDist = currentDist;
            }
            return maxDist.ToString();
        }

        private void InitializeMoveset(Dictionary<string, int> moves)
        {
            foreach (var rule in _transformations)
            {
                if (!moves.ContainsKey(rule.In1))
                    moves[rule.In1] = 0;
                if (!moves.ContainsKey(rule.In2))
                    moves[rule.In2] = 0;
                if (rule.Result != null && !moves.ContainsKey(rule.Result))
                    moves[rule.Result] = 0;
            }
        }

        private void ReduceMoveset(Dictionary<string, int> moves)
        {
            var nextRule = _transformations.FirstOrDefault(p => moves[p.In1] > 0 && moves[p.In2] > 0);
            while (nextRule != null)
            {
                moves[nextRule.In1]--;
                moves[nextRule.In2]--;
                if (nextRule.Result != null)
                    moves[nextRule.Result]++;

                nextRule = _transformations.FirstOrDefault(p => moves[p.In1] > 0 && moves[p.In2] > 0);
            }
        }

        private class Transformation
        {
            public string In1 { get; }
            public string In2 { get; }
            public string? Result { get; }

            public Transformation(string in1, string in2, string? result)
            {
                In1 = in1;
                In2 = in2;
                Result = result;
            }
        }
    }
}
