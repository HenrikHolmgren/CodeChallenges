using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.AdventOfCode._2018.Common;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day21 : AdventOfCode2018SolverBase
    {
        private ElfCodeMachine.Register _instructionPointer;
        private ElfCodeMachine.Operation[] _instructions = new ElfCodeMachine.Operation[0];

        public override string Name => "Day 21: Chronal Conversion";

        //NOTE: This magic number is obtained by analysing the input code. 
        //It will likely be different for your input set; look for an instruction like
        // eqrr 5 0 3 
        //which in my case would jump out of the program on equality.
        private int _targetInstructionIndex = 28;

        public Day21(IIOProvider provider) : base(provider) { }

        public override void Initialize(string input)
        {
            var inputLines = ReadAndSplitInput<string>(input).ToArray();
            _instructionPointer = (ElfCodeMachine.Register)int.Parse(inputLines[0].Split()[1]);
            _instructions = inputLines.Skip(1)
                .Select(p => p.Split())
                .Select(p => new ElfCodeMachine.Operation(p[0], (p.Skip(1).Select(q => int.Parse(q)).ToArray())))
                .ToArray();
        }

        public override string ExecutePart1()
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

        public override string ExecutePart2()
        {
            var machine = new ElfCodeMachine(_instructions)
                .WithProgramCounterRegister(_instructionPointer);
            Dictionary<int, long> O0CompValues = new Dictionary<int, long>();

            while (true)
            {
                if (machine.ProgramCounter == _targetInstructionIndex)
                {
                    var R5 = machine.GetState()[5];

                    IO.LogIfAttached(() => $"R0 compared with R5 = {R5}");

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
                new Opcode("addr", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} + R{par.B}"); */return reg.WithValue(reg[par.A] + reg[par.B], par.C); } ) ,
                new Opcode("addi", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} + {par.B}");*/ return reg.WithValue(reg[par.A] + par.B, par.C); } ),

                new Opcode("mulr", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} * R{par.B}"); */return reg.WithValue(reg[par.A] * reg[par.B], par.C);} ),
                new Opcode("muli", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} * {par.B}");*/ return reg.WithValue(reg[par.A] * par.B, par.C);} ),

                new Opcode("banr", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} & R{par.B}"); */return reg.WithValue(reg[par.A] & reg[par.B], par.C); } ),
                new Opcode("bani", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} & {par.B}");*/ return reg.WithValue(reg[par.A] & par.B, par.C); } ),

                new Opcode("borr", (reg, par) => {/* Console.WriteLine($"R{par.C} <- R{par.A} | R{par.B}");*/ return reg.WithValue(reg[par.A] | reg[par.B], par.C); } ),
                new Opcode("bori", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} | {par.B}");*/ return reg.WithValue(reg[par.A] | par.B, par.C); } ),

                new Opcode("setr", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A} ");*/ return reg.WithValue(reg[par.A], par.C); } ),
                new Opcode("seti", (reg, par) => { /*Console.WriteLine($"R{par.C} <- {par.A}");*/ return reg.WithValue(par.A, par.C); } ),

                new Opcode("gtir", (reg, par) => { /*Console.WriteLine($"R{par.C} <- {par.A}>R{par.B}");*/ return reg.WithValue(par.A > reg[par.B] ? 1 : 0, par.C); } ),
                new Opcode("gtri", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}>{par.B}");*/ return reg.WithValue(reg[par.A] > par.B ? 1 : 0, par.C); } ),
                new Opcode("gtrr", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}>R{par.B}"); */return reg.WithValue(reg[par.A] > reg[par.B] ? 1 : 0, par.C); } ),

                new Opcode("eqir", (reg, par) => { /*Console.WriteLine($"R{par.C} <- {par.A}=R{par.B}"); */return reg.WithValue(par.A == reg[par.B] ? 1 : 0, par.C); } ),
                new Opcode("eqri", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}={par.B}"); */return reg.WithValue(reg[par.A] == par.B ? 1 : 0, par.C); }),
                new Opcode("eqrr", (reg, par) => { /*Console.WriteLine($"R{par.C} <- R{par.A}=R{par.B}"); */return reg.WithValue(reg[par.A] == reg[par.B] ? 1 : 0, par.C); }),
            }.ToDictionary(p => p.Name);
        }

        public void Initialize(object fullRunInput)
        {
            throw new NotImplementedException();
        }
    }
}

