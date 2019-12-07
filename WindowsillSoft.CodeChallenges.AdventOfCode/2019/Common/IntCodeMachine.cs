using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common
{
    public class IntCodeMachine
    {
        private Func<int, int>? _requestInput = null;
        private int[] _inputSequence = new int[0];

        public IntCodeMachine() { }

        public IntCodeMachine WithInput(int[] inputSequence)
        {
            (_inputSequence, _requestInput) = (inputSequence, null);
            return this;
        }

        public IntCodeMachine WithInput(Func<int, int> requestInput)
        {
            (_inputSequence, _requestInput) = (new int[0], requestInput);
            return this;
        }

        public int[] Run(int[] program)
        {
            int _inputIndex = 0;
            var output = new List<int>();
            int pc = 0;
            while (program[pc] != 99)
            {
                var instruction = program[pc];
                switch (instruction % 100)
                {
                    case 01: //add
                        WriteMemory(program, pc + 3,
                            ReadMemory(program, pc + 2, GetMode(instruction, 1)) + ReadMemory(program, pc + 1, GetMode(instruction, 0)),
                            GetMode(instruction, 2));
                        pc += 4;
                        break;
                    case 02: //mult
                        WriteMemory(program, pc + 3,
                            ReadMemory(program, pc + 2, GetMode(instruction, 1)) * ReadMemory(program, pc + 1, GetMode(instruction, 0)),
                            GetMode(instruction, 2));
                        pc += 4;
                        break;
                    case 03: //read-input
                        WriteMemory(program, pc + 1, RequestInput(_inputIndex++),
                            GetMode(instruction, 0));
                        pc += 2;
                        break;
                    case 04: //write-output
                        output.Add(ReadMemory(program, pc + 1, GetMode(instruction, 0)));
                        pc += 2;
                        break;
                    case 05: //jump-if-true                        
                        if (ReadMemory(program, pc + 1, GetMode(instruction, 0)) != 0)
                            pc = ReadMemory(program, pc + 2, GetMode(instruction, 1));
                        else pc += 3;
                        break;
                    case 06: //jump-if-true
                        if (ReadMemory(program, pc + 1, GetMode(instruction, 0)) == 0)
                            pc = ReadMemory(program, pc + 2, GetMode(instruction, 1));
                        else pc += 3;
                        break;
                    case 07: //less-than
                        if (ReadMemory(program, pc + 1, GetMode(instruction, 0)) <
                            ReadMemory(program, pc + 2, GetMode(instruction, 1)))
                            WriteMemory(program, pc + 3, 1, GetMode(instruction, 2));
                        else
                            WriteMemory(program, pc + 3, 0, GetMode(instruction, 2));
                        pc += 4;
                        break;
                    case 08: //less-than
                        if (ReadMemory(program, pc + 1, GetMode(instruction, 0)) ==
                            ReadMemory(program, pc + 2, GetMode(instruction, 1)))
                            WriteMemory(program, pc + 3, 1, GetMode(instruction, 2));
                        else
                            WriteMemory(program, pc + 3, 0, GetMode(instruction, 2));
                        pc += 4;
                        break;
                    case 99: //break
                             //Actually handled by while-loop, but includee here for documentation completeness.
                    default:
                        throw new InvalidOperationException($"Unknown OPCODE {program[pc]}; halt and catch fire!");
                }
            }
            return output.ToArray();
        }

        private int RequestInput(int inputIndex)
        {
            if (_requestInput != null)
                return _requestInput.Invoke(inputIndex);
            if (inputIndex < _inputSequence.Length)
                return _inputSequence[inputIndex];
            throw new InvalidOperationException("Buffer underrun: No more input available!");
        }

        private int GetMode(int instruction, int argumentNumber)
            => (instruction / (int)Math.Pow(10, argumentNumber + 2)) % 10;

        private int ReadMemory(int[] memory, int address, int mode)
        {
            if (mode == 0) return memory[memory[address]];
            else return memory[address];
        }

        private void WriteMemory(int[] memory, int address, int value, int mode)
        {
            if (mode == 0) memory[memory[address]] = value;
            else memory[address] = value;
        }
    }
}
