using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day16
{
    public class Day16Solver : AoC2018SolverBase
    {
        private static List<(int Opcode, OpcodeParameterSet Par)> _instructions;
        private List<(RegisterState Start, int Opcode, OpcodeParameterSet Par, RegisterState Res)> _states;

        public override string Description => "Day 16: Chronal Classification";

        public override int SortOrder => 16;

        public override void Initialize(string input)
        {
            var inputSegments = input.Split($"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}");

            var statePart = inputSegments[0];
            _states = statePart.Split($"{Environment.NewLine}{Environment.NewLine}")
                .Select(p => p.Trim())
                .Select(p => p.Replace("Before:", ""))
                .Select(p => p.Replace("After:", ""))
                .Select(p => p.Replace("[", ""))
                .Select(p => p.Replace("]", ""))
                .Select(p => p.Replace(",", ""))
                .Select(p => p.Replace(Environment.NewLine, " "))
                .Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(q => int.Parse(q)).ToArray())
                .Select(p => (
                    Start: new RegisterState(p.Take(4).ToArray()),
                    Opcode: p[4],
                    Par: new OpcodeParameterSet(p.Skip(5).Take(3).ToArray()),
                    Res: new RegisterState(p.Skip(8).Take(4).ToArray())))
                .ToList();

            var programPart = inputSegments[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            _instructions = programPart.Select(p => p.Split().Select(q => int.Parse(q)).ToArray())
                .Select(p => (
                    Opcode: p[0],
                    Par: new OpcodeParameterSet(p.Skip(1).Take(3).ToArray())))
                .ToList();
        }

        public override string SolvePart1(bool silent = true)
        {
            int tripOrMore = 0;

            List<Opcode> InstructionSet = GetInstructionSet();
            foreach (var line in _states)
            {
                var opCodeMatches = InstructionSet.Where(p => p.DoWork(line.Start, line.Par).Equals(line.Res));
                if (opCodeMatches.Count() >= 3)
                    tripOrMore++;
            }

            if (!silent)
                Console.WriteLine($"{tripOrMore} samples behave like 3+ opcodes.");
            return tripOrMore.ToString();
        }

        public override string SolvePart2(bool silent = true)
        {
            var opCodeMap = BuildInstructionMap(_states);
            var state = new RegisterState();

            foreach (var (OpCode, Parameter) in _instructions)
                state = opCodeMap[OpCode].DoWork(state, Parameter);

            if (!silent)
                Console.WriteLine($"State after program execution: {state}");

            return state.R0.ToString();
        }

        private static Dictionary<int, Opcode> BuildInstructionMap(List<(RegisterState Start, int Opcode, OpcodeParameterSet Par, RegisterState Res)> input)
        {
            var instructionSet = GetInstructionSet();
            var possibleOpcodes = input.Select(p => p.Opcode).Distinct();

            var opCodeMap = possibleOpcodes.ToDictionary(p => p, p => instructionSet.ToList());
            foreach (var candidate in input)
            {
                var opcode = opCodeMap[candidate.Opcode];
                foreach (var candidateCode in opcode.ToList())
                {
                    try
                    {
                        var res = candidateCode.DoWork(candidate.Start, candidate.Par);
                        if (!res.Equals(candidate.Res))
                            opCodeMap[candidate.Opcode].Remove(candidateCode);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        opCodeMap[candidate.Opcode].Remove(candidateCode);
                    }
                }
            }

            Console.WriteLine("Potential Codes Mapped, reducing set...");

            var resolvedOpcodes = new Dictionary<int, Opcode>();

            while (opCodeMap.Any(p => p.Value.Count > 1))
            {
                var matchedCodes = new List<(int Instruction, Opcode OpCode)>();
                foreach (var certainCode in opCodeMap.Where(p => p.Value.Count == 1))
                {
                    foreach (var ambiguousCode in opCodeMap.Where(p => p.Value.Count > 1))
                        ambiguousCode.Value.Remove(certainCode.Value.Single());

                    matchedCodes.Add((certainCode.Key, certainCode.Value.Single()));
                }

                foreach (var matchedCode in matchedCodes)
                {
                    resolvedOpcodes[matchedCode.Instruction] = matchedCode.OpCode;
                    opCodeMap.Remove(matchedCode.Instruction);
                }
            }

            return resolvedOpcodes;
        }

        private static List<Opcode> GetInstructionSet()
        {
            return new List<Opcode>
            {
                new Opcode{Name = "addr", DoWork = (reg, par) => reg.WithValue(reg[par.A] + reg[par.B], par.C)},
                new Opcode{Name = "addi", DoWork = (reg, par) => reg.WithValue(reg[par.A] + par.B, par.C)},

                new Opcode{Name = "mulr", DoWork = (reg, par) => reg.WithValue(reg[par.A] * reg[par.B], par.C)},
                new Opcode{Name = "muli", DoWork = (reg, par) => reg.WithValue(reg[par.A] * par.B, par.C)},

                new Opcode{Name = "banr", DoWork = (reg, par) => reg.WithValue(reg[par.A] & reg[par.B], par.C)},
                new Opcode{Name = "bani", DoWork = (reg, par) => reg.WithValue(reg[par.A] & par.B, par.C)},

                new Opcode{Name = "borr", DoWork = (reg, par) => reg.WithValue(reg[par.A] | reg[par.B], par.C)},
                new Opcode{Name = "bori", DoWork = (reg, par) => reg.WithValue(reg[par.A] | par.B, par.C)},

                new Opcode{Name = "setr", DoWork = (reg, par) => reg.WithValue(reg[par.A], par.C)},
                new Opcode{Name = "seti", DoWork = (reg, par) => reg.WithValue(par.A, par.C)},

                new Opcode{Name = "gtir", DoWork = (reg, par) => reg.WithValue(par.A > reg[par.B] ? 1 : 0, par.C)},
                new Opcode{Name = "gtri", DoWork = (reg, par) => reg.WithValue(reg[par.A] > par.B ? 1 : 0, par.C)},
                new Opcode{Name = "gtrr", DoWork = (reg, par) => reg.WithValue(reg[par.A] > reg[par.B] ? 1 : 0, par.C)},

                new Opcode{Name = "eqir", DoWork = (reg, par) => reg.WithValue(par.A == reg[par.B] ? 1 : 0, par.C)},
                new Opcode{Name = "eqri", DoWork = (reg, par) => reg.WithValue(reg[par.A] == par.B ? 1 : 0, par.C)},
                new Opcode{Name = "eqrr", DoWork = (reg, par) => reg.WithValue(reg[par.A] == reg[par.B] ? 1 : 0, par.C)},
            };
        }

    }

}
