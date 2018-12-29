using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day16
{
    public class Day16Solver : IProblemSolver
    {
        public string Description => "Day 16";

        public int SortOrder => 16;

        public void Solve()
        {

            SanityCheck();

            Part1();

            Part2();
        }

        private void Part1()
        {
            int tripOrMode = 0;

            var input = GetInput("Day16/Day16InputA.txt");
            List<Opcode> InstructionSet = GetInstructionSet();
            foreach (var line in input)
            {
                var opCodeMatches = InstructionSet.Where(p => p.DoWork(line.Start, line.Par).Equals(line.Res));
                if (opCodeMatches.Count() >= 3)
                    tripOrMode++;
            }
            Console.WriteLine($"{tripOrMode} samples behave like 3+ opcodes.");
        }

        private static void Part2()
        {
            var input = GetInput("Day16/Day16InputA.txt");
            var opCodeMap = BuildInstructionMap(input);
            var program = GetProgram("Day16/Day16InputB.txt");
            var state = new RegisterState();
            foreach (var (OpCode, Parameter) in program)
                state = opCodeMap[OpCode].DoWork(state, Parameter);

            Console.WriteLine($"State after program execution: {state}");
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
            while (opCodeMap.Any(p => p.Value.Count > 1))
            {
                foreach (var certainCode in opCodeMap.Where(p => p.Value.Count == 1))
                {
                    foreach (var ambiguousCode in opCodeMap.Where(p => p.Value.Count > 1))
                        ambiguousCode.Value.Remove(certainCode.Value.Single());
                }
            }

            return opCodeMap.ToDictionary(p => p.Key, p => p.Value.Single());
        }

        private static void SanityCheck()
        {
            var registry = new RegisterState(new[] { 3, 2, 1, 1 });
            var paramset = new OpcodeParameterSet(new[] { 2, 1, 2 });

            var result = new RegisterState(new[] { 3, 2, 2, 1 });

            foreach (var opcode in GetInstructionSet())
            {
                if (opcode.DoWork(registry, paramset).Equals(result))
                    Console.WriteLine($"Opcode {opcode.Name} is a candidate.");
            }
        }

        private static List<(RegisterState Start, int Opcode, OpcodeParameterSet Par, RegisterState Res)> GetInput(string file)
        {
            return File.ReadAllLines(file)
                .Select(p => p.Split().Select(q => int.Parse(q)).ToArray())
                .Select(p => (
                    Start: new RegisterState(p.Take(4).ToArray()),
                    Opcode: p[4],
                    Par: new OpcodeParameterSet(p.Skip(5).Take(3).ToArray()),
                    Res: new RegisterState(p.Skip(8).Take(4).ToArray())))
                .ToList();
        }

        private static List<(int OpCode, OpcodeParameterSet Par)> GetProgram(string file)
        {
            return File.ReadAllLines(file)
                .Select(p => p.Split().Select(q => int.Parse(q)).ToArray())
                .Select(p => (
                    Opcode: p[0],
                    Par: new OpcodeParameterSet(p.Skip(1).Take(3).ToArray())))
                .ToList();
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

    public class Opcode
    {
        public string Name { get; set; }
        public Func<RegisterState, OpcodeParameterSet, RegisterState> DoWork { get; set; }
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
                case 0: A = value; break;
                case 1: B = value; break;
                case 2: C = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        private int GetValue(int index)
        {
            switch (index)
            {
                case 0: return A;
                case 1: return B;
                case 2: return C;
                default: throw new IndexOutOfRangeException();
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
                case 0: R0 = value; break;
                case 1: R1 = value; break;
                case 2: R2 = value; break;
                case 3: R3 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        private int GetValue(int index)
        {
            switch (index)
            {
                case 0: return R0;
                case 1: return R1;
                case 2: return R2;
                case 3: return R3;
                default: throw new IndexOutOfRangeException();
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

        public override string ToString()
            => $"(0:{R0} 1:{R1} 2:{R2} 3:{R3})";

    }

}
