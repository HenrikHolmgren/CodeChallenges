using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsillSoft.AdventOfCode2018.Core.Utilities
{
    public class ElfCodeMachine
    {
        private int[] _registers = new int[6];

        private Operation[] _program;
        private Register _programCounterReg;
        private long _instructionCount = 0;

        public ElfCodeMachine(Operation[] program)
        {
            _program = program;
        }

        public string ListProgram()
        {

            return $"With instruction counter in {_programCounterReg}:{Environment.NewLine}{string.Join(Environment.NewLine, _program.Select((p, i) => $"{i}: {Describe(p)}"))}";
        }

        public ElfCodeMachine WithRegisterState(Register register, int state)
        {
            _registers[(int)register] = state;
            return this;
        }

        public ElfCodeMachine WithProgramCounterRegister(Register register)
        {
            _programCounterReg = register;
            return this;
        }

        public int ProgramCounter => _registers[(int)_programCounterReg];

        public int[] GetState()
            => _registers.ToArray();

        public long ExecutedInstructions => _instructionCount;

        public bool IsHalted
            => ProgramCounter >= _program.Length;

        private string Describe(Operation op)
        {
            switch (op.OpCode)
            {
                case Ops.addr: return $"ADDR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} + {(Register)op.Parameters[1]}";
                case Ops.addi: return $"ADDI; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} + {op.Parameters[1]}";
                case Ops.mulr: return $"MULR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} * {(Register)op.Parameters[1]}";
                case Ops.muli: return $"MULI; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} * {op.Parameters[1]}";
                case Ops.banr: return $"BANR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} & {(Register)op.Parameters[1]}";
                case Ops.bani: return $"BANI; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} & {op.Parameters[1]}";
                case Ops.borr: return $"BORR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} | {(Register)op.Parameters[1]}";
                case Ops.bori: return $"BORI; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} | {op.Parameters[1]}";
                case Ops.setr: return $"SETR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]}";
                case Ops.seti: return $"SETI; {(Register)op.Parameters[2]} <- {op.Parameters[0]} ";
                case Ops.gtir: return $"GTIR; {(Register)op.Parameters[2]} <- {op.Parameters[0]} > {(Register)op.Parameters[1]}";
                case Ops.gtri: return $"GTRI; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} > {op.Parameters[1]}";
                case Ops.gtrr: return $"GTRR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} > {(Register)op.Parameters[1]}";
                case Ops.eqir: return $"EQIR; {(Register)op.Parameters[2]} <- {op.Parameters[0]} = {(Register)op.Parameters[1]}";
                case Ops.eqri: return $"EQRI; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} = {op.Parameters[1]}";
                case Ops.eqrr: return $"EQRR; {(Register)op.Parameters[2]} <- {(Register)op.Parameters[0]} = {(Register)op.Parameters[1]}";
            }
            return "UNKNOWN OPCODE " + op.OpCode;
        }

        public int[] Execute()
        {
            while (ProgramCounter < _program.Length)
                Step();
            return _registers;
        }

        public int[] StepVerbose()
        {
            if (ProgramCounter >= _program.Length)
            {
                Console.WriteLine("HALT");
                return _registers;
            }

            _instructionCount++;

            Console.WriteLine(ProgramCounter + ": " + Describe(_program[ProgramCounter]));

            switch (_program[ProgramCounter].OpCode)
            {
                case Ops.addr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] + _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.addi: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] + _program[ProgramCounter].Parameters[1]; break;
                case Ops.mulr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] * _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.muli: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] * _program[ProgramCounter].Parameters[1]; break;
                case Ops.banr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] & _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.bani: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] & _program[ProgramCounter].Parameters[1]; break;
                case Ops.borr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] | _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.bori: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] | _program[ProgramCounter].Parameters[1]; break;
                case Ops.setr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]]; break;
                case Ops.seti: _registers[_program[ProgramCounter].Parameters[2]] = _program[ProgramCounter].Parameters[0]; break;
                case Ops.gtir: _registers[_program[ProgramCounter].Parameters[2]] = _program[ProgramCounter].Parameters[0] > _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
                case Ops.gtri: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] > _program[ProgramCounter].Parameters[1] ? 1 : 0; break;
                case Ops.gtrr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] > _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
                case Ops.eqir: _registers[_program[ProgramCounter].Parameters[2]] = _program[ProgramCounter].Parameters[0] == _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
                case Ops.eqri: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] == _program[ProgramCounter].Parameters[1] ? 1 : 0; break;
                case Ops.eqrr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] == _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
            }
            _registers[(int)_programCounterReg]++;

            Console.WriteLine($"  -> [{String.Join(" ", _registers)}]");
            return _registers;
        }

        public int[] Step()
        {
            _instructionCount++;

            switch (_program[ProgramCounter].OpCode)
            {
                case Ops.addr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] + _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.addi: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] + _program[ProgramCounter].Parameters[1]; break;
                case Ops.mulr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] * _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.muli: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] * _program[ProgramCounter].Parameters[1]; break;
                case Ops.banr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] & _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.bani: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] & _program[ProgramCounter].Parameters[1]; break;
                case Ops.borr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] | _registers[_program[ProgramCounter].Parameters[1]]; break;
                case Ops.bori: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] | _program[ProgramCounter].Parameters[1]; break;
                case Ops.setr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]]; break;
                case Ops.seti: _registers[_program[ProgramCounter].Parameters[2]] = _program[ProgramCounter].Parameters[0]; break;
                case Ops.gtir: _registers[_program[ProgramCounter].Parameters[2]] = _program[ProgramCounter].Parameters[0] > _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
                case Ops.gtri: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] > _program[ProgramCounter].Parameters[1] ? 1 : 0; break;
                case Ops.gtrr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] > _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
                case Ops.eqir: _registers[_program[ProgramCounter].Parameters[2]] = _program[ProgramCounter].Parameters[0] == _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
                case Ops.eqri: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] == _program[ProgramCounter].Parameters[1] ? 1 : 0; break;
                case Ops.eqrr: _registers[_program[ProgramCounter].Parameters[2]] = _registers[_program[ProgramCounter].Parameters[0]] == _registers[_program[ProgramCounter].Parameters[1]] ? 1 : 0; break;
            }

            _registers[(int)_programCounterReg]++;
            return _registers;
        }
        public struct Operation
        {
            public readonly Ops OpCode;
            public readonly int[] Parameters;

            public Operation(string opcode, int[] parameters) : this(Enum.Parse<Ops>(opcode), parameters) { }
            public Operation(Ops opCode, int[] parameters)
            {
                if (parameters.Length != 3)
                    throw new InvalidOperationException("Operation parameters must always have length exactly three.");

                OpCode = opCode;
                Parameters = parameters;
            }
        }

        public enum Ops
        {
            addr = 0,
            addi = 1,
            mulr = 2,
            muli = 3,
            banr = 4,
            bani = 5,
            borr = 6,
            bori = 7,
            setr = 8,
            seti = 9,
            gtir = 10,
            gtri = 11,
            gtrr = 12,
            eqir = 13,
            eqri = 14,
            eqrr = 15,
        }

        public enum Register
        {
            R0 = 0,
            R1 = 1,
            R2 = 2,
            R3 = 3,
            R4 = 4,
            R5 = 5,
        }

        public enum Parameter
        {
            A = 0,
            B = 1,
            C = 2,
            D = 3,
        }
    }

}
