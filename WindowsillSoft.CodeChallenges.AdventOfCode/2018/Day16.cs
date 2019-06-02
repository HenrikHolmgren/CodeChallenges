using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day16 : AdventOfCode2018SolverBase
    {
        private static List<(int Opcode, OpcodeParameterSet Par)> _instructions;
        private List<(RegisterState Start, int Opcode, OpcodeParameterSet Par, RegisterState Res)> _states
            = new List<(RegisterState Start, int Opcode, OpcodeParameterSet Par, RegisterState Res)>();

        public Day16(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 16: Chronal Classification";

        public override void Initialize(string input)
        {
            var inputSegments = input.Split($"\n\n\n");

            var statePart = inputSegments[0];
            _states = statePart.Split($"\n\n")
                .Select(p => p.Trim())
                .Select(p => p.Replace("Before:", ""))
                .Select(p => p.Replace("After:", ""))
                .Select(p => p.Replace("[", ""))
                .Select(p => p.Replace("]", ""))
                .Select(p => p.Replace(",", ""))
                .Select(p => p.Replace("\n", " "))
                .Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(q => int.Parse(q)).ToArray())
                .Select(p => (
                    Start: new RegisterState(p.Take(4).ToArray()),
                    Opcode: p[4],
                    Par: new OpcodeParameterSet(p.Skip(5).Take(3).ToArray()),
                    Res: new RegisterState(p.Skip(8).Take(4).ToArray())))
                .ToList();

            var programPart = inputSegments[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
            _instructions = programPart.Select(p => p.Split().Select(q => int.Parse(q)).ToArray())
                .Select(p => (
                    Opcode: p[0],
                    Par: new OpcodeParameterSet(p.Skip(1).Take(3).ToArray())))
                .ToList();
        }

        public override string ExecutePart1()
        {
            int tripOrMore = 0;

            List<Opcode> InstructionSet = GetInstructionSet();
            foreach (var line in _states)
            {
                var opCodeMatches = InstructionSet.Where(p => p.DoWork(line.Start, line.Par).Equals(line.Res));
                if (opCodeMatches.Count() >= 3)
                    tripOrMore++;
            }

            IO.LogIfAttached(() => $"{tripOrMore} samples behave like 3+ opcodes.");
            return tripOrMore.ToString();
        }

        public override string ExecutePart2()
        {
            var opCodeMap = BuildInstructionMap(_states);
            var state = new RegisterState();

            foreach (var (OpCode, Parameter) in _instructions)
                state = opCodeMap[OpCode].DoWork(state, Parameter);

            IO.LogIfAttached(() => $"State after program execution: {state}");

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
                new Opcode("addr", (reg, par) => reg.WithValue(reg[par.A] + reg[par.B], par.C)),
                new Opcode("addi", (reg, par) => reg.WithValue(reg[par.A] + par.B, par.C)),

                new Opcode("mulr", (reg, par) => reg.WithValue(reg[par.A] * reg[par.B], par.C)),
                new Opcode("muli", (reg, par) => reg.WithValue(reg[par.A] * par.B, par.C)),

                new Opcode("banr", (reg, par) => reg.WithValue(reg[par.A] & reg[par.B], par.C)),
                new Opcode("bani", (reg, par) => reg.WithValue(reg[par.A] & par.B, par.C)),

                new Opcode("borr", (reg, par) => reg.WithValue(reg[par.A] | reg[par.B], par.C)),
                new Opcode("bori", (reg, par) => reg.WithValue(reg[par.A] | par.B, par.C)),

                new Opcode("setr", (reg, par) => reg.WithValue(reg[par.A], par.C)),
                new Opcode("seti", (reg, par) => reg.WithValue(par.A, par.C)),

                new Opcode("gtir", (reg, par) => reg.WithValue(par.A > reg[par.B] ? 1 : 0, par.C)),
                new Opcode("gtri", (reg, par) => reg.WithValue(reg[par.A] > par.B ? 1 : 0, par.C)),
                new Opcode("gtrr", (reg, par) => reg.WithValue(reg[par.A] > reg[par.B] ? 1 : 0, par.C)),

                new Opcode("eqir", (reg, par) => reg.WithValue(par.A == reg[par.B] ? 1 : 0, par.C)),
                new Opcode("eqri", (reg, par) => reg.WithValue(reg[par.A] == par.B ? 1 : 0, par.C)),
                new Opcode("eqrr", (reg, par) => reg.WithValue(reg[par.A] == reg[par.B] ? 1 : 0, par.C)),
            };
        }

        public class Opcode
        {
            public Opcode(string name, Func<RegisterState, OpcodeParameterSet, RegisterState> doWork)
            {
                Name = name;
                DoWork = doWork;
            }

            public string Name { get; }
            public Func<RegisterState, OpcodeParameterSet, RegisterState> DoWork { get; }
        }

        public struct OpcodeParameterSet
        {
            public int A;
            public int B;
            public int C;

            public OpcodeParameterSet(int[] state) =>
               (A, B, C) = (state[0], state[1], state[2]);

            public int this[int index]
            {
                get => GetValue(index);
                set => SetValue(index, value);
            }

            private void SetValue(int index, int value)
            {
                switch (index)
                {
                    case 0:
                        A = value;
                        break;
                    case 1:
                        B = value;
                        break;
                    case 2:
                        C = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            private int GetValue(int index)
            {
                switch (index)
                {
                    case 0:
                        return A;
                    case 1:
                        return B;
                    case 2:
                        return C;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            public override string ToString()
                => $"(A:{A} B:{B} C:{C})";
        }
        public struct RegisterState
        {
            public int R0;
            public int R1;
            public int R2;
            public int R3;

            public RegisterState(int[] state) =>
                (R0, R1, R2, R3) = (state[0], state[1], state[2], state[3]);

            public int this[int index]
            {
                get => GetValue(index);
                set => SetValue(index, value);
            }

            private void SetValue(int index, int value)
            {
                switch (index)
                {
                    case 0:
                        R0 = value;
                        break;
                    case 1:
                        R1 = value;
                        break;
                    case 2:
                        R2 = value;
                        break;
                    case 3:
                        R3 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            private int GetValue(int index)
            {
                switch (index)
                {
                    case 0:
                        return R0;
                    case 1:
                        return R1;
                    case 2:
                        return R2;
                    case 3:
                        return R3;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            public RegisterState WithValue(int value, int index)
            {
                var result = new RegisterState();

                (result.R0, result.R1, result.R2, result.R3) = (R0, R1, R2, R3);
                result[index] = value;

                return result;
            }

            public override bool Equals(object obj)
            {
                if (obj is RegisterState reg)
                    return Equals(reg);
                return false;
            }

            public bool Equals(RegisterState other)
            {
                return R0 == other.R0 &&
                    R1 == other.R1 &&
                    R2 == other.R2 &&
                    R3 == other.R3;
            }
            public override int GetHashCode()
                => R0 ^ 17 * R1 ^ 19 * R2 ^ 23 * R3;

            public override string ToString()
                => $"(0:{R0} 1:{R1} 2:{R2} 3:{R3})";

        }
    }
}
