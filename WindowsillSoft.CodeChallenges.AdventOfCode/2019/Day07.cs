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
                GetBestStageConfiguration(_program, 5, 0))
                .ToString();

        public override string ExecutePart2()
            => SimulateStageConfiguration(_program,
                GetBestStageConfiguration(_program, 5, 5))
                .ToString();

        public int[] GetBestStageConfiguration(int[] program, int stageCount, int phaseOffset)
        {
            var configurationOutputs = Sequences.Permutations(Enumerable.Range(phaseOffset, stageCount))
                .Select(p => (Sequence: p.ToArray(), LastStageOutput: SimulateStageConfiguration(program, p.ToArray())));
            return configurationOutputs.OrderByDescending(p => p.LastStageOutput)
                .First().Sequence;
        }

        public int SimulateStageConfiguration(int[] program, int[] phaseSequence)
        {
            var machines = phaseSequence.Select(p => new IntCodeMachine(program.ToArray())).ToArray();

            //prime with phase
            for (int i = 0; i < phaseSequence.Length; i++)
            {
                var machine = machines[i];

                if (!(machine.Run() is IntCodeMachine.InputRequestState))
                    throw new InvalidOperationException("Expected machine to request phase state input");
                machine.ProvideInputAndContinue(phaseSequence[i]);
            }

            while (!machines.All(p => p.IsHalted))
            {
                for (int i = 0; i < phaseSequence.Length; i++)
                {
                    var machine = machines[i];

                    if (machine.CurrentState is IntCodeMachine.InputRequestState)
                        machine.ProvideInputAndContinue(machines[(i + machines.Length - 1) % machines.Length].LastOutput ?? 0);

                    if (machine.CurrentState is IntCodeMachine.OutputAvailableState)
                        machine.AcceptOutputAndContinue();
                }
            }
            return machines.Last().LastOutput ?? 0;
        }

        public override void Initialize(string input) => _program = ReadAndSplitInput<int>(input, ',').ToArray();
    }
}
