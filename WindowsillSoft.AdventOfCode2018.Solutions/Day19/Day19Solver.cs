using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;
using WindowsillSoft.AdventOfCode2018.Core.Utilities;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day19
{
    public class Day19Solver : IProblemSolver
    {
        public string Description => "Day 19: Go With The Flow";

        public int SortOrder => 19;

        public void Solve()
        {
            var input = File.ReadAllLines("Day19/Day19Input.txt");

            var ipReg = (ElfCodeMachine.Register)int.Parse(input[0].Split()[1]);
            var instructions = input.Skip(1)
                .Select(p => p.Split())
                .Select(p => new ElfCodeMachine.Operation(p[0], (p.Skip(1).Select(q => int.Parse(q)).ToArray())))
                .ToArray();

            var machine = new ElfCodeMachine(instructions)
                .WithProgramCounterRegister(ipReg)
                .WithRegisterState(ElfCodeMachine.Register.R0, 1);

            Console.WriteLine(machine.ListProgram());

            //machine.Execute();
            //Console.WriteLine($"Final state: [{String.Join( " ", machine.GetState())}] in {machine.ExecutedInstructions} steps.");

            for (int i = 0; i < 20; i++)
            {
                var state = machine.Step(true);
            }
            //var state = new RegisterState();

            //long runs = 0;

            //while (true)
            //{
            //    runs++;
            //    var nextInstruction = instructions[state[ipReg]];
            //    var newState = instructionSet[nextInstruction.Instr].DoWork(state, nextInstruction.Para);
            //    if (newState[ipReg] >= instructions.Count()) break;
            //    state = newState;
            //    if (state[ipReg] + 1 >= instructions.Count()) break;

            //    state[ipReg]++;
            //    if (runs % 100_000 == 0) Console.Write(".");
            //}

            //Console.WriteLine();
            //Console.WriteLine($"End state after {runs} instructions: {state}");

            //State with [0] = 1 was run for 10 mins (runs >= 10_000_000_000) without result, so likely program has exponential runtime.
            //Resorted to manual analysis of source:
            /*
                With instruction counter in R3:
                0: ADDI; R3 <- R3 + 16    ; JMP 17
                1: SETI; R5 <- 1          ; R5 = 1
                2: SETI; R2 <- 1          ; R2 = 1
                3: MULR; R1 <- R5 * R2    
                4: EQRR; R1 <- R1 = R4    
                5: ADDR; R3 <- R1 + R3    ; IF (R4 == R2 * R5) 
                6: ADDI; R3 <- R3 + 1     
                7: ADDR; R0 <- R5 + R0    ;   R0 += R5
                8: ADDI; R2 <- R2 + 1     ; R2++
                9: GTRR; R1 <- R2 > R4    
                10: ADDR; R3 <- R3 + R1   ; IF(R2 <= R4)
                11: SETI; R3 <- 2         ;   JMP 3
                12: ADDI; R5 <- R5 + 1    ; R5++
                13: GTRR; R1 <- R5 > R4
                14: ADDR; R3 <- R1 + R3   ; IF(R5 <= R4)
                15: SETI; R3 <- 1         ;   JMP 2
                16: MULR; R3 <- R3 * R3   ; HALT
                17: ADDI; R4 <- R4 + 2
                18: MULR; R4 <- R4 * R4
                19: MULR; R4 <- R3 * R4   
                20: MULI; R4 <- R4 * 11   ; R4 = ((R4 + 2) ^ 2) * 19 * 11 // R4 = (R4 + 2)^2 * 209
                21: ADDI; R1 <- R1 + 6
                22: MULR; R1 <- R1 * R3
                23: ADDI; R1 <- R1 + 21   ; R1 = (R1 + 6) * 22 + 21; // R1 = 22 * R1 + 153
                24: ADDR; R4 <- R4 + R1   ; R4 += R1;
                25: ADDR; R3 <- R3 + R0   ; JMP +R0
                26: SETI; R3 <- 0         ; JMP 1
                27: SETR; R1 <- R3        
                28: MULR; R1 <- R1 * R3
                29: ADDR; R1 <- R3 + R1
                30: MULR; R1 <- R3 * R1
                31: MULI; R1 <- R1 * 14
                32: MULR; R1 <- R1 * R3   ; R1 = 27 * 28 * 2 * 14 // R1 = 21168
                33: ADDR; R4 <- R4 + R1   ; R4 += 21168;
                34: SETI; R0 <- 0         ; R0 = 0
                35: SETI; R3 <- 0         ; JMP 1

            //Pseudo-assembly:
                :init
                R4 = 836;
                R1 = 153;
                R4 += R1; // = 989
                IF(R0 == 1)
                  R1 = (27 * 28 + 29) * 30 * 14 * 32; // = 10_550_400;
                  R4 += R1 // = 10_551_389;  

                R5 = 1
                //[0 153 0 1 989 1] OR [0 10550400 0 1 10551389 1]  
                :loop1
                R2 = 1
                :loop2

                IF (R4 == R2 * R5)
                  R0 += R5
                R2++

                IF(R2 <= R4)
                  JMP loop2
                R5++

                IF(R5 <= R4)
                  JMP loop1
                HALT

            //Pseudo-C#
                
                R4 = 836;
                R1 = 153;
                R4 += R1; // = 989
                
                IF(R0 == 1) {
                  R1 = (27 * 28 + 29) * 30 * 14 * 32; // = 10_550_400;
                  R4 += R1 // = 10_551_389;  
                }

                R5 = 1;
                do {
                    R2 = 1;
                    do {
                        if (R4 == R2 * R5)
                            R0 += R5;
                        R2++;
                    }
                    while (R2 <= R4);
                    R5++;
                }
                while (R5 <= R4);
             */
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
