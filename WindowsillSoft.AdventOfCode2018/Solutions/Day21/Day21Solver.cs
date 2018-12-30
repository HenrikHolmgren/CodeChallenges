using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;
using WindowsillSoft.AdventOfCode2018.Core.Utilities;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day21
{
    public class Day21Solver : IAdventOfCodeSolver
    {
        private ElfCodeMachine.Register _instructionPointer;
        private ElfCodeMachine.Operation[] _instructions;

        public string Description => "Day 21: Chronal Conversion";
        public int SortOrder => 21;

        //NOTE: This magic number is obtained by analysing the input code. 
        //It will likely be different for your input set; look for an instruction like
        // eqrr 5 0 3 
        //which in my case would jump out of the program on equality.
        private int _targetInstructionIndex = 28;

        public void Initialize(string input)
        {
            var inputLines = input.Split(Environment.NewLine);
            _instructionPointer = (ElfCodeMachine.Register)int.Parse(inputLines[0].Split()[1]);
            _instructions = inputLines.Skip(1)
                .Select(p => p.Split())
                .Select(p => new ElfCodeMachine.Operation(p[0], (p.Skip(1).Select(q => int.Parse(q)).ToArray())))
                .ToArray();
        }

        public string SolvePart1(bool silent = true)
        {
            var machine = new ElfCodeMachine(_instructions)
                .WithProgramCounterRegister(_instructionPointer);
            Dictionary<int, long> O0CompValues = new Dictionary<int, long>();

            while (true)
            {
                if (machine.ProgramCounter == _targetInstructionIndex)
                {
                    var R5 = machine.GetState()[5];
                    return R5.ToString();
                }
                machine.Step();
                if (machine.IsHalted)
                    break;
            }
            return "ERROR";
        }

        public string SolvePart2(bool silent = true)
        {
            var machine = new ElfCodeMachine(_instructions)
                .WithProgramCounterRegister(_instructionPointer);
            Dictionary<int, long> O0CompValues = new Dictionary<int, long>();

            while (true)
            {
                if (machine.ProgramCounter == _targetInstructionIndex)
                {
                    var R5 = machine.GetState()[5];

                    if (!silent)
                        Console.WriteLine($"R0 compared with R5 = {R5}");

                    if (O0CompValues.ContainsKey(R5))
                        break;
                    else
                        O0CompValues[R5] = machine.ExecutedInstructions;
                }
                machine.Step();
                if (machine.IsHalted)
                    break;
            }

            return O0CompValues.OrderBy(p => p.Value).Last().Key.ToString();
        }

        public void Solve()
        {
            var machine = new ElfCodeMachine(_instructions);
            Dictionary<int, long> O0CompValues = new Dictionary<int, long>();

            while (true)
            {
                if (machine.ProgramCounter == 28)
                {
                    var R5 = machine.GetState()[5];
                    Console.WriteLine($"R0 compared with R5 = {R5}");
                    if (O0CompValues.ContainsKey(R5))
                        break;
                    else
                        O0CompValues[R5] = machine.ExecutedInstructions;

                }
                machine.Step();
                if (machine.IsHalted)
                    break;
            }

            Console.WriteLine();
            Console.WriteLine($"Value of R0 for lowest runs: {O0CompValues.OrderBy(p => p.Value).First().Key}");
            Console.WriteLine($"Value of R0 for highest runs: {O0CompValues.OrderBy(p => p.Value).Last().Key}");
            /* Value of R0 for lowest runs: 3941014
             * Value of R0 for highest runs: 13775890 */
        }

        private static Dictionary<string, Opcode> GetInstructionSet()
        {
            return new List<Opcode>
            {
                new Opcode{Name = "addr", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} + R{par.B}"); */return reg.WithValue(reg[par.A] + reg[par.B], par.C); } } ,
                new Opcode{Name = "addi", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} + {par.B}");*/ return reg.WithValue(reg[par.A] + par.B, par.C); } },

                new Opcode{Name = "mulr", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} * R{par.B}"); */return reg.WithValue(reg[par.A] * reg[par.B], par.C);} },
                new Opcode{Name = "muli", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} * {par.B}");*/ return reg.WithValue(reg[par.A] * par.B, par.C);} },

                new Opcode{Name = "banr", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} & R{par.B}"); */return reg.WithValue(reg[par.A] & reg[par.B], par.C); } },
                new Opcode{Name = "bani", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} & {par.B}");*/ return reg.WithValue(reg[par.A] & par.B, par.C); } },

                new Opcode{Name = "borr", DoWork = (reg, par) => {/* Console.WriteLine($"R{par.C} <- R{par.A} | R{par.B}");*/ return reg.WithValue(reg[par.A] | reg[par.B], par.C); } },
                new Opcode{Name = "bori", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} | {par.B}");*/ return reg.WithValue(reg[par.A] | par.B, par.C); } },

                new Opcode{Name = "setr", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} ");*/ return reg.WithValue(reg[par.A], par.C); } },
                new Opcode{Name = "seti", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- {par.A}");*/ return reg.WithValue(par.A, par.C); } },

                new Opcode{Name = "gtir", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- {par.A}>R{par.B}");*/ return reg.WithValue(par.A > reg[par.B] ? 1 : 0, par.C); } },
                new Opcode{Name = "gtri", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}>{par.B}");*/ return reg.WithValue(reg[par.A] > par.B ? 1 : 0, par.C); } },
                new Opcode{Name = "gtrr", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}>R{par.B}"); */return reg.WithValue(reg[par.A] > reg[par.B] ? 1 : 0, par.C); } },

                new Opcode{Name = "eqir", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- {par.A}=R{par.B}"); */return reg.WithValue(par.A == reg[par.B] ? 1 : 0, par.C); } },
                new Opcode{Name = "eqri", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}={par.B}"); */return reg.WithValue(reg[par.A] == par.B ? 1 : 0, par.C); } },
                new Opcode{Name = "eqrr", DoWork = (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}=R{par.B}"); */return reg.WithValue(reg[par.A] == reg[par.B] ? 1 : 0, par.C); } },
            }.ToDictionary(p => p.Name);
        }

        public void Initialize(object fullRunInput)
        {
            throw new NotImplementedException();
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
            => $"(R0:{R0} R1:{R1} R2:{R2} R3:{R3} R4:{R4} R5:{R5})";

    }

}
