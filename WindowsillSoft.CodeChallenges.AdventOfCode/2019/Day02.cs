using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day02 : AdventOfCode2019SolverBase
    {
        private int[] _input = new int[0];

        public override string Name => "Day 2: 1202 Program Alarm";
        public Day02(IIOProvider provider) : base(provider) { }

        public override string ExecutePart1()
        {
            var program = _input.ToArray();

            program[1] = 12;
            program[2] = 2;

            var machine = new IntCodeMachine(program);
            machine.Run();

            return program[0].ToString();
        }

        public override string ExecutePart2()
        {

            //There very likely exists a better analytical solution than this brute-force approach, but for now it works.
            for (int noun = 0; noun <= 99; noun++)
                for (int verb = 0; verb <= 99; verb++)
                {
                    var program = _input.ToArray();
                    
                    program[1] = noun;
                    program[2] = verb;

                    var machine = new IntCodeMachine(program);
                    machine.Run();

                    if (program[0] == 19690720)
                        return $"{noun:00}{verb:00}";
                }
            throw new InvalidOperationException("Unable to find solution!");
        }

        public override void Initialize(string input) => _input = ReadAndSplitInput<int>(input, ',').ToArray();
    }
}
