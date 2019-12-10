using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common
{
    public class IntCodeMachine
    {
        private Dictionary<long, long> _memory;
        private long _programCounter;
        private long _relativeBase;
        private long? _lastOutput;

        public long? LastOutput => _lastOutput;
        public bool IsHalted => _memory[_programCounter] % 100 == 99;
        public long Peek(long address) => _memory.ContainsKey(address) ? _memory[address] : 0;

        public IntCodeMachine(long[] program)
        {
            _memory = Enumerable.Range(0, program.Length)
                .ToDictionary(p => (long)p, p => program[p]);
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
            if (_memory[_programCounter] % 100 != 04)
                throw new InvalidOperationException("Program was halted providing output!");

            _programCounter += 2;

            var res = ContinueExecution();
            Debug.WriteLine(res);

            return res;
        }

        public StateBase ProvideInputAndContinue(long input)
        {
            if (SafeRead(_memory, _programCounter) % 100 != 03)
                throw new InvalidOperationException("Program was not expecting input!");

            WriteMemory(_memory, _programCounter + 1, input,
                GetMode(SafeRead(_memory, _programCounter), 0));
            _programCounter += 2;

            var res = ContinueExecution();
            Debug.WriteLine(res);

            return res;
        }

        private StateBase ContinueExecution()
        {
            while (true)
            {
                var instruction = SafeRead(_memory, _programCounter);
                switch (instruction % 100)
                {
                    case 01: //add
                        WriteMemory(_memory, _programCounter + 3,
                            ReadMemory(_memory, _programCounter + 2, GetMode(instruction, 1)) + ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)),
                            GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 02: //mult
                        WriteMemory(_memory, _programCounter + 3,
                            ReadMemory(_memory, _programCounter + 2, GetMode(instruction, 1)) * ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)),
                            GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 03: //read-input
                        return new InputRequestState();
                    case 04: //write-output
                        _lastOutput = (ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)));
                        return new OutputAvailableState { Value = _lastOutput.Value };
                    case 05: //jump-if-true                        
                        if (ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)) != 0)
                            _programCounter = ReadMemory(_memory, _programCounter + 2, GetMode(instruction, 1));
                        else _programCounter += 3;
                        break;
                    case 06: //jump-if-true
                        if (ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)) == 0)
                            _programCounter = ReadMemory(_memory, _programCounter + 2, GetMode(instruction, 1));
                        else _programCounter += 3;
                        break;
                    case 07: //less-than
                        if (ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)) <
                            ReadMemory(_memory, _programCounter + 2, GetMode(instruction, 1)))
                            WriteMemory(_memory, _programCounter + 3, 1, GetMode(instruction, 2));
                        else
                            WriteMemory(_memory, _programCounter + 3, 0, GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 08: //less-than
                        if (ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0)) ==
                            ReadMemory(_memory, _programCounter + 2, GetMode(instruction, 1)))
                            WriteMemory(_memory, _programCounter + 3, 1, GetMode(instruction, 2));
                        else
                            WriteMemory(_memory, _programCounter + 3, 0, GetMode(instruction, 2));
                        _programCounter += 4;
                        break;
                    case 09: //update relativebase
                        _relativeBase += ReadMemory(_memory, _programCounter + 1, GetMode(instruction, 0));
                        _programCounter += 2;
                        break;
                    case 99: //Halt
                        return new MachineHaltedState();
                    default:
                        throw new InvalidOperationException($"Unknown OPCODE {SafeRead(_memory, _programCounter)}; halt and catch fire!");
                }
            }
        }

        public StateBase CurrentState =>
            (SafeRead(_memory, _programCounter) % 100) switch
            {
                03 => new InputRequestState(),
                04 => new OutputAvailableState { Value = LastOutput!.Value },
                99 => new MachineHaltedState(),
                var p => throw new InvalidOperationException($"Current program state is at a non-halting opcode {p}")
            };

        private int GetMode(long instruction, int argumentNumber)
            => (int)(instruction / (int)Math.Pow(10, argumentNumber + 2)) % 10;

        private long ReadMemory(Dictionary<long, long> memory, long address, int mode)
            => mode switch
            {
                0 => SafeRead(memory, SafeRead(memory, address)),
                1 => SafeRead(memory, address),
                2 => SafeRead(memory, _relativeBase + SafeRead(memory, address)),
                _ => throw new InvalidOperationException($"Unknown parameter mode {mode}.")
            };

        private long WriteMemory(Dictionary<long, long> memory, long address, long value, int mode)
            => mode switch
            {
                0 => memory[SafeRead(memory, address)] = value,
                1 => memory[address] = value,
                2 => memory[_relativeBase + SafeRead(memory, address)] = value,
                _ => throw new InvalidOperationException($"Unknown parameter mode {mode}.")
            };

        private long SafeRead(Dictionary<long, long> memory, long address) =>
            memory.ContainsKey(address) ? memory[address] : 0;

        public class StateBase { public override string ToString() => $"{this.GetType().Name}"; }
        public class MachineHaltedState : StateBase { }
        public class InputRequestState : StateBase { }
        public class OutputAvailableState : StateBase { public long Value; public override string ToString() => $"{this.GetType().Name}({Value})"; }
    }
}
