using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common
{
    public class IntCodeMachine
    {
        private int[] _program;
        private int _programCounter;
        private int? _lastOutput;

        public int? LastOutput => _lastOutput;
        public bool IsHalted => _program[_programCounter] % 100 == 99;

        public IntCodeMachine(int[] program)
        {
            _program = program;
            _programCounter = 0;
        }

        public StateBase Run()
        {
            if (_programCounter != 0)
                throw new InvalidOperationException("Machine is already running!");

            var res = ContinueExecution();
            Debug.WriteLine(res);

            return res;
        }

        public StateBase AcceptOutputAndContinue()
        {
            if (_program[_programCounter] % 100 != 04)
                throw new InvalidOperationException("Program was halted providing output!");

            _programCounter += 2;

            var res = ContinueExecution();
            Debug.WriteLine(res);

            return res;
        }

        public StateBase ProvideInputAndContinue(int input)
        {
            if (_program[_programCounter] % 100 != 03)
                throw new InvalidOperationException("Program was not expecting input!");

            WriteMemory(_program, _programCounter + 1, input,
                            GetMode(_program[_programCounter], 0));
            _programCounter += 2;

            var res = ContinueExecution();
            Debug.WriteLine(res);

            return res;
        }

        private StateBase ContinueExecution()
        {
            while (true)
            {
                var instruction = _program[_programCounter];
                switch (instruction % 100)
                {
                    case 01: //add
                        WriteMemory(_program, _programCounter + 3,
                            ReadMemory(_program, _programCounter + 2, GetMode(instruction, 1)) + ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)),
                            GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 02: //mult
                        WriteMemory(_program, _programCounter + 3,
                            ReadMemory(_program, _programCounter + 2, GetMode(instruction, 1)) * ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)),
                            GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 03: //read-input
                        return new InputRequestState();
                    case 04: //write-output
                        _lastOutput = (ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)));
                        return new OutputAvailableState { Value = _lastOutput.Value };
                    case 05: //jump-if-true                        
                        if (ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)) != 0)
                            _programCounter = ReadMemory(_program, _programCounter + 2, GetMode(instruction, 1));
                        else _programCounter += 3;
                        break;
                    case 06: //jump-if-true
                        if (ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)) == 0)
                            _programCounter = ReadMemory(_program, _programCounter + 2, GetMode(instruction, 1));
                        else _programCounter += 3;
                        break;
                    case 07: //less-than
                        if (ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)) <
                            ReadMemory(_program, _programCounter + 2, GetMode(instruction, 1)))
                            WriteMemory(_program, _programCounter + 3, 1, GetMode(instruction, 2));
                        else
                            WriteMemory(_program, _programCounter + 3, 0, GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 08: //less-than
                        if (ReadMemory(_program, _programCounter + 1, GetMode(instruction, 0)) ==
                            ReadMemory(_program, _programCounter + 2, GetMode(instruction, 1)))
                            WriteMemory(_program, _programCounter + 3, 1, GetMode(instruction, 2));
                        else
                            WriteMemory(_program, _programCounter + 3, 0, GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 99: //Halt
                        return new MachineHaltedState();
                    default:
                        throw new InvalidOperationException($"Unknown OPCODE {_program[_programCounter]}; halt and catch fire!");
                }
            }
        }

        public StateBase CurrentState =>
            (_program[_programCounter] % 100) switch
            {
                03 => new InputRequestState(),
                04 => new OutputAvailableState { Value = LastOutput!.Value},
                99 => new MachineHaltedState(),
                var p => throw new InvalidOperationException($"Current program state is at a non-halting opcode {p}")
            };

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

        public class StateBase { public override string ToString() => $"{this.GetType().Name}"; }
        public class MachineHaltedState : StateBase { }
        public class InputRequestState : StateBase { }
        public class OutputAvailableState : StateBase { public int Value; public override string ToString() => $"{this.GetType().Name}({Value})"; }
    }
}
