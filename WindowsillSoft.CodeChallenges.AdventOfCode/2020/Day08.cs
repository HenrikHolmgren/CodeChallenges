using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day08 : AdventOfCode2020SolverBase
    {
        protected record Statement(string Instruction, int Parameter);
        protected record State(int Accumulator, int ProgramCounter);

        List<Statement> _program = new();
        protected Dictionary<string, Func<State, int, State>> _instructions = new Dictionary<string, Func<State, int, State>>
        {
            ["nop"] = (state, param) => state with { ProgramCounter = state.ProgramCounter + 1 },
            ["acc"] = (state, param) => state with { Accumulator = state.Accumulator + param, ProgramCounter = state.ProgramCounter + 1 },
            ["jmp"] = (state, param) => state with { ProgramCounter = state.ProgramCounter + param },
        };

        public Day08(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 8: Handheld Halting";
        public override string ExecutePart1() => RunProgram(_program).Accumulator.ToString();
        public override string ExecutePart2()
        {
            var copy = _program.ToList();

            for (int i = 0; i < copy.Count; i++)
            {
                if (copy[i].Instruction == "acc") continue;
                var tmp = copy[i];
                if (copy[i].Instruction == "nop")
                    copy[i] = copy[i] with { Instruction = "jmp" };
                else
                    copy[i] = copy[i] with { Instruction = "nop" };

                var finalState = RunProgram(copy);
                if (finalState.ProgramCounter == copy.Count)
                    return finalState.Accumulator.ToString();

                copy[i] = tmp;
            }
            return "No final state reached";
        }

        private State RunProgram(List<Statement> program)
        {
            HashSet<int> visitedInstructions = new();
            var state = new State(0, 0);
            while (true)
            {
                visitedInstructions.Add(state.ProgramCounter);

                var instruction = program[state.ProgramCounter];
                var newState = _instructions[instruction.Instruction](state, instruction.Parameter);
                if (visitedInstructions.Contains(newState.ProgramCounter) || newState.ProgramCounter >= program.Count)
                    return newState;

                if (visitedInstructions.Contains(newState.ProgramCounter))
                    return state;
                if (newState.ProgramCounter >= program.Count)
                    return state;
                state = newState;
            }
        }

        public override void Initialize(string input)
        {
            _program = ReadAndSplitInput<Statement>(input,
                p => new Statement(p[..3], int.Parse(p[4..])), '\r', '\n')
                .ToList();
        }
    }
}
