using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day18 : AdventOfCode2017SolverBase
    {
        private MisunderstoodMachine? _machine1;
        private MulticoreMachine? _machine2;

        public Day18(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 18: Duet";


        public override void Initialize(string input)
        {
            _machine1 = new MisunderstoodMachine(input.Split('\n', StringSplitOptions.RemoveEmptyEntries));
            _machine2 = new MulticoreMachine()
                .WithProgram(input.Split('\n', StringSplitOptions.RemoveEmptyEntries));
        }

        public override string ExecutePart1() => _machine1?.GetFirstRecovedValue().ToString() ?? throw new InvalidOperationException("Machine not initialized!");

        public override string ExecutePart2() => _machine2?.Execute()[1].ToString() ?? throw new InvalidOperationException("Machine not initialized!");

        private class MisunderstoodMachine
        {
            private readonly Dictionary<string, Instruction> _instructionSet = GetInstructionSet()
            .ToDictionary(p => p.OpCode);

            private readonly List<(string opCode, Instruction.Parameters parameters)> _program;

            public MisunderstoodMachine(string[] instructions)
            {
                _program = new List<(string opCode, Instruction.Parameters parameters)>();
                foreach (var line in instructions)
                {
                    var instruction = line.Split(' ');
                    var opCode = instruction[0];
                    var opReg = instruction[1][0];
                    if (instruction.Length > 2)
                    {
                        if (Int32.TryParse(instruction[2], out var value))
                            _program.Add((opCode, new Instruction.Parameters(opReg, value)));
                        else
                            _program.Add((opCode, new Instruction.Parameters(opReg, instruction[2][0])));
                    }
                    else
                    {
                        //TODO: Solution to the single null-result op-code 'snd'.
                        _program.Add((opCode, new Instruction.Parameters(opReg, (char)0)));
                    }
                }
            }

            public long GetFirstRecovedValue()
            {
                var state = new Dictionary<char, long>();
                foreach (var reg in _program.Select(p => p.parameters.Register).Distinct())
                    state[reg] = 0;

                state['#'] = 0; //Instruction pointer
                state['¤'] = 0; //Last sound played
                state['!'] = 0; //Last sound recovered
                state['&'] = 0; //First sound recovered

                while (state['#'] < _program.Count)
                {
                    var next = _program[(int)state['#']];
                    _instructionSet[next.opCode].Apply(next.parameters, state);
                    state['#']++;

                    if (state['&'] != 0)
                        return state['&'];
                }

                return state['&'];
            }

            private string DumpState(Dictionary<char, long> state)
            {
                return "{"
                    + String.Join(" ", state.Keys.OrderBy(p => p)
                    .Select(p => $"{p}:{state[p]}"))
                    + "}";
            }

            private static List<Instruction> GetInstructionSet()
                => new List<Instruction>
                {
                    new Instruction("snd", (p,q)=>q['¤'] = q[p.Register]),
                    new Instruction("set", (p,q)=>q[p.Register] =  p.ValueOrRegister.Match(value => value, register => q[register])),
                    new Instruction("add", (p,q)=>q[p.Register] += p.ValueOrRegister.Match(value => value, register => q[register])),
                    new Instruction("mul", (p,q)=>q[p.Register] *= p.ValueOrRegister.Match(value => value, register => q[register])),
                    new Instruction("mod", (p,q)=>q[p.Register] %= p.ValueOrRegister.Match(value => value, register => q[register])),
                    new Instruction("rcv", (p,q)=> {
                        if (q[p.Register] != 0) {
                            q['!'] = q['¤'];
                            if(q['&'] == 0) q['&'] = q['¤'];
                        }
                    }),
                    new Instruction("jgz", (p,q)=>{
                        if (q[p.Register] > 0)
                            q['#'] += p.ValueOrRegister.Match(value => value, register => q[register]) - 1;
                    })
                };

            private class Instruction
            {
                public string OpCode { get; }
                private readonly Action<Parameters, Dictionary<char, long>> _function;

                public Instruction(string opCode, Action<Parameters, Dictionary<char, long>> function)
                {
                    OpCode = opCode;
                    _function = function;
                }

                public void Apply(Parameters parameters, Dictionary<char, long> state) => _function(parameters, state);

                public class Parameters
                {
                    public char Register { get; }
                    public OneOf<int, char> ValueOrRegister { get; }

                    public Parameters(char register, int value) : this(register) => ValueOrRegister = value;
                    public Parameters(char register, char valueRegister) : this(register) => ValueOrRegister = valueRegister;
                    private Parameters(char register) => Register = register;

                    public override string ToString()
                        => $"{Register} {ValueOrRegister.ToString()}";
                }
            }
        }

        private class MulticoreMachine
        {
            private readonly List<Queue<long>> _buffers;
            private readonly List<Dictionary<char, long>> _states;
            private readonly long[] _programCounters;
            private readonly List<(string opCode, InstructionParameters parameters)> _program;

            public MulticoreMachine()
            {
                _buffers = new List<Queue<long>>();
                _states = new List<Dictionary<char, long>>();
                _programCounters = new long[2];
                _program = new List<(string opCode, InstructionParameters parameters)>();

                for (var i = 0; i < 2; i++)
                {
                    _buffers.Add(new Queue<long>());
                    _states.Add(new Dictionary<char, long>());
                }
            }

            public MulticoreMachine WithProgram(string[] instructions)
            {
                _program.Clear();

                foreach (var line in instructions)
                {
                    var instruction = line.Split(' ');
                    var opCode = instruction[0];
                    var opReg = instruction[1][0];
                    if (instruction.Length > 2)
                    {
                        if (Int32.TryParse(instruction[2], out var value))
                            _program.Add((opCode, new InstructionParameters(opReg, value)));
                        else
                            _program.Add((opCode, new InstructionParameters(opReg, instruction[2][0])));
                    }
                    else
                    {
                        //TODO: Solution to the single null-result op-code 'snd'.
                        _program.Add((opCode, new InstructionParameters(opReg, (char)0)));
                    }
                }

                foreach (var reg in _program.Select(p => p.parameters.Register).Distinct())
                    _states.ForEach(p => p[reg] = Char.IsNumber(reg) ? (reg - '0') : 0);

                _states[0]['p'] = 0;
                _states[1]['p'] = 1;

                return this;
            }

            public int[] Execute()
            {
                var running = true;
                var sends = new int[2];

                while (_programCounters.All(p => p < _program.Count) && running)
                {
                    running = false;

                    for (var i = 0; i < 2; i++)
                    {
                        if (_program[(int)_programCounters[i]].opCode == "snd")
                            sends[i]++;
                        running |= TryExecuteNext(i);
                    }
                }
                if (!running)
                    Console.WriteLine("Deadlock.");
                return sends;
            }

            private bool TryExecuteNext(int machineId)
            {
                var op = _program[(int)_programCounters[machineId]];

                var state = _states[machineId];
                var sendbuffer = _buffers[(machineId + 1) % 2];
                var recvbuffer = _buffers[machineId];

                if (machineId == 1)
                    Console.Write($"{_programCounters[machineId]}: {op} - {DumpState(state)} -> ");

                switch (op.opCode)
                {
                    case "snd":
                        sendbuffer.Enqueue(state[op.parameters.Register]);
                        break;
                    case "set":
                        state[op.parameters.Register] = op.parameters.ResolveValue(state);
                        break;
                    case "add":
                        state[op.parameters.Register] += op.parameters.ResolveValue(state);
                        break;
                    case "mul":
                        state[op.parameters.Register] *= op.parameters.ResolveValue(state);
                        break;
                    case "mod":
                        state[op.parameters.Register] %= op.parameters.ResolveValue(state);
                        break;
                    case "rcv":
                        if (!recvbuffer.Any())
                        {
                            if (machineId == 1)
                                Console.WriteLine($"{DumpState(state)}");
                            return false;
                        }
                        state[op.parameters.Register] = recvbuffer.Dequeue();

                        break;
                    case "jgz":
                        if (state[op.parameters.Register] > 0)
                            _programCounters[machineId] += op.parameters.ResolveValue(state) - 1;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown instruction " + op);
                }
                if (machineId == 1)
                    Console.WriteLine($"{DumpState(state)}");

                _programCounters[machineId]++;

                return true;
            }

            private string DumpState(Dictionary<char, long> state)
            {
                return "{"
                    + String.Join(" ", state.Keys.OrderBy(p => p)
                    .Select(p => $"{p}:{state[p]}"))
                    + "}";
            }

            public class InstructionParameters
            {
                public char Register { get; }
                public OneOf<int, char> ValueOrRegister { get; }

                public InstructionParameters(char register, char valueRegister) : this(register) => ValueOrRegister = valueRegister;

                public InstructionParameters(char register, int value) : this(register) => ValueOrRegister = value;

                private InstructionParameters(char register) => Register = register;

                public override string ToString()
                    => $"{Register} {ValueOrRegister.ToString()}";

                public long ResolveValue(Dictionary<char, long> state)
                    => ValueOrRegister.Match(
                        value => value,
                        register => state[register]);
            }
        }
    }
}
