using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day05 : AdventOfCode2019SolverBase
    {
        int[] _program = new int[] { };

        public Day05(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 5: Sunny with a Chance of Asteroids";

        public override string ExecutePart1()
        {
            var machine = new IntCodeMachine(_program.ToArray());
            return RunAndGetLastOutput(machine, 1).ToString();
        }

        public override string ExecutePart2()
        {
            var machine = new IntCodeMachine(_program.ToArray());
            return RunAndGetLastOutput(machine, 5).ToString();
        }

        private int RunAndGetLastOutput(IntCodeMachine machine, int inputValue)
        {
            if (!(machine.Run() is IntCodeMachine.InputRequestState))
                throw new InvalidOperationException("Expected an input request");

            int lastOutput = 0;

            var res = machine.ProvideInputAndContinue(inputValue);
            while(res is IntCodeMachine.OutputAvailableState output)
            {
                lastOutput = output.Value;
                res = machine.AcceptOutputAndContinue();
            }            
            return lastOutput;
        }

        public override void Initialize(string input) => _program = ReadAndSplitInput<int>(input, ',').ToArray();
    }
}
