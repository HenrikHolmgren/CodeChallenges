using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day19
{
    public class Day19Solver : IProblemSolver
    {
        public string Description => "Day 19: Go With The Flow";

        public int SortOrder => 19;

        public void Solve()
        {
            var input = File.ReadAllLines("Day19/Day19Input.txt");
            var ipReg = int.Parse(input[0].Split()[1]);
            var instructions = input.Skip(1)
                .Select(p => p.Split())
                .Select(p => 
                    (Instr: p[0], 
                    Para: new OpcodeParameterSet(p.Skip(1).Select(q => int.Parse(q)).ToArray())))
                .ToList();
            
            var state = new RegisterState();

            var instructionSet = GetInstructionSet();
            long runs = 0;

            while(true)
            {
                runs++;
                var nextInstruction = instructions[state[ipReg]];
                var newState = instructionSet[nextInstruction.Instr].DoWork(state, nextInstruction.Para);
                if (newState[ipReg] >= instructions.Count()) break;
                state = newState;
                if (state[ipReg]+1 >= instructions.Count()) break;

                state[ipReg]++;
                //Console.WriteLine(state);
                if (runs % 100_000 == 0) Console.Write(".");
            }

            Console.WriteLine();
            Console.WriteLine($"End state after {runs} instructions: {state}");

            //State with [0] = 1 was run for 10 mins (runs >= 10_000_000_000) without result, so likely program has exponential runtime.
            //Resorted to manual analysis of source.
        }

        private static Dictionary<string, Opcode> GetInstructionSet()
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
            }.ToDictionary(p=>p.Name);
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
        public int R4;
        public int R5;

        public RegisterState(int[] state) =>
            (R0, R1, R2, R3, R4, R5) = (state[0], state[1], state[2], state[3], state[4], state[5]);
        
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
                case 4: R4 = value; break;
                case 5: R5 = value; break;
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
                case 4: return R4;
                case 5: return R5;
                default: throw new IndexOutOfRangeException();
            }
        }

        public RegisterState WithValue(int value, int index)
        {
            var result = new RegisterState();

            (result.R0, result.R1, result.R2, result.R3, result.R4, result.R5) = (R0, R1, R2, R3, R4, R5);
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
                R3 == other.R3 &&
                R4 == other.R4 &&
                R5 == other.R5;
        }

        public override int GetHashCode()
        {
            var res = 0;
            for (int i = 0; i < 6; i++)
                res = res * 13 + this[i].GetHashCode();
            return res;
        }

        public override string ToString()
            => $"(0: {R0} 1: {R1} 2: {R2} 3: {R3} 4: {R4} 5: {R5})";

    }

}
