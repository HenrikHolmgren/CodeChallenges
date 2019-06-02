using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day08 : AdventOfCode2017SolverBase
    {
        private List<Instruction> _Instructions;

        public Day08(IIOProvider provider) : base(provider) => _Instructions = new List<Instruction>();

        public override string Name => "Day 8: I Heard You Like Registers";

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"(?'reg'[a-zA-Z]+) (?'dir'(inc|dec)) (?'value'-?\d+) if (?'cmpa'[a-zA-Z]+) (?'operator'==|!=|>=|<=|>|<) (?'cmpb'-?\d+)", RegexOptions.Compiled);
            _Instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => matcher.Match(p))
                .Select(p => new Instruction(
                    p.Groups["reg"].Value,
                    Int32.Parse(p.Groups["value"].Value)
                        * (p.Groups["dir"].Value == "inc" ? 1 : -1),
                    p.Groups["cmpa"].Value,
                    p.Groups["operator"].Value,
                    Int32.Parse(p.Groups["cmpb"].Value)))
                .ToList();
        }

        public override string ExecutePart1()
        {
            var registerValues = new Dictionary<string, int>();
            foreach (var instruction in _Instructions)
            {
                instruction.Apply(registerValues);
            }
            return registerValues.Max(p => p.Value).ToString();
        }


        public override string ExecutePart2()
        {
            var maxValue = Int32.MinValue;

            var registerValues = new Dictionary<string, int>() { { "¤", Int32.MinValue } };
            foreach (var instruction in _Instructions)
            {
                instruction.Apply(registerValues);

                var currentMax = registerValues.Max(p => p.Value);
                if (currentMax > maxValue)
                    maxValue = currentMax;
            }
            return maxValue.ToString();
        }

        public class Instruction
        {
            public string Register { get; }
            public int ValueChange { get; }
            public string CmpA { get; }
            public string Operator { get; }
            public int CmpB { get; }

            public Instruction(string register, int valueChange, string cmpA, string op, int cmpB)
            {
                Register = register;
                ValueChange = valueChange;
                CmpA = cmpA;
                Operator = op;
                CmpB = cmpB;
            }

            internal void Apply(Dictionary<string, int> registerValues)
            {
                if (ComparisonMatch(registerValues))
                {
                    if (!registerValues.ContainsKey(Register))
                        registerValues[Register] = 0;
                    registerValues[Register] += ValueChange;
                }
            }

            private bool ComparisonMatch(Dictionary<string, int> registerValues)
            {
                var registerValue = 0;
                if (registerValues.ContainsKey(CmpA))
                    registerValue = registerValues[CmpA];
                switch (Operator)
                {
                    case "==":
                        return registerValue == CmpB;
                    case "!=":
                        return registerValue != CmpB;
                    case "<":
                        return registerValue < CmpB;
                    case ">":
                        return registerValue > CmpB;
                    case "<=":
                        return registerValue <= CmpB;
                    case ">=":
                        return registerValue >= CmpB;
                    default:
                        throw new InvalidOperationException("Unknown operator " + Operator);
                }
            }
        }
    }
}
