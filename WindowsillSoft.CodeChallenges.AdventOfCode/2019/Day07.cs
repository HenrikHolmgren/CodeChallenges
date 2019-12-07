using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Numerics;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day07 : AdventOfCode2019SolverBase
    {
        private int[] _program;

        public Day07(IIOProvider provider) : base(provider) { _program = new int[] { }; }

        public override string Name => "";

        public override string ExecutePart1()
            => SimulateStageConfiguration(_program,
                GetBestStageConfiguration(_program, 5))
                .ToString();

        public override string ExecutePart2()
        {
            return "";
        }

        public int[] GetBestStageConfiguration(int[] program, int stageCount)
        {
            var configurationOutputs = Sequences.Permutations(Enumerable.Range(0, stageCount))
                .Select(p => (Sequence: p.ToArray(), LastStageOutput: SimulateStageConfiguration(program, p.ToArray())));
            return configurationOutputs.OrderByDescending(p => p.LastStageOutput)
                .First().Sequence;
        }

        public int SimulateStageConfiguration(int[] program, int[] phaseSequence)
        {
            int lastStateOutput = 0;

            var machineRunner = new IntCodeMachine();
            foreach (var phase in phaseSequence)
            {
                var inputSequence = new[] { phase, lastStateOutput };
                var output = machineRunner.WithInput(inputSequence).Run((int[])program.Clone());
                lastStateOutput = output.Single();
            }
            return lastStateOutput;
        }

        public override void Initialize(string input) => _program = ReadAndSplitInput<int>(input, ',').ToArray();
    }
}
