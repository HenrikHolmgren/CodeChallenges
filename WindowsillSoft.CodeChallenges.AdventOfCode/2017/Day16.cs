using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day16 : AdventOfCode2017SolverBase
    {
        private int _programCount;
        private DanceMove[] _danceCard;

        public Day16(IIOProvider provider) : base(provider) => _danceCard = new DanceMove[0];

        public override string Name => "Day 16: Permutation Promenade";

        public override void Initialize(string input)
        {
            string? programCount;
            do
            {
                programCount = IO.RequestInput("Program count?");
            } while (programCount == default || !Int32.TryParse(programCount, out _programCount));

            _danceCard = input.Split(',')
                .Select(p => new DanceMove(p))
                .ToArray();
        }

        public override string ExecutePart1()
            => Stringify(
                DanceDanceWhereverYouMayBe(
                    Enumerable.Range(0, _programCount)
                    .ToList()));


        public override string ExecutePart2()
        {
            var programs = Enumerable.Range(0, _programCount).ToList();
            var history = new Dictionary<string, int>();

            Console.WriteLine("-1: " + Stringify(programs));
            //history[Stringify(programs)] = 0;
            var runs = 0;
            var repeatStart = 0;
            while (true)
            {
                programs = DanceDanceWhereverYouMayBe(programs);
                var entry = Stringify(programs);

                Console.WriteLine($"{runs}: {entry}");

                if (history.ContainsKey(entry))
                {
                    repeatStart = history[entry];
                    break;
                }
                else
                {
                    history[entry] = runs++;
                }
            }

            var repeatEnd = history.Max(p => p.Value);

            IO.LogLine($"Pattern found - history repeats from {repeatStart} to {repeatEnd}");

            var target = 1_000_000_000 % (repeatEnd - repeatStart + 1) - 1;

            while (target < repeatStart)
                target += (repeatEnd - repeatStart);

            IO.LogLine($"This means a repeat offset of {target}, for a final value of {history.Single(p => p.Value == target).Key}");

            return history.Single(p => p.Value == target).Key;
        }

        private string Stringify(List<int> programs) => new string(programs.Select(p => (char)(p + 'a')).ToArray());

        private List<int> DanceDanceWhereverYouMayBe(List<int> programs)
        {
            foreach (var move in _danceCard)
            {
                switch (move.Step)
                {
                    case DanceMove.StepType.Spin:
                        var temp1 = programs.Skip(_programCount - move.P1).Concat(programs.Take(_programCount - move.P1)).ToList();
                        programs = temp1;
                        break;
                    case DanceMove.StepType.ExchangeIndex:
                        var temp2 = programs[move.P1];
                        programs[move.P1] = programs[move.P2];
                        programs[move.P2] = temp2;
                        break;
                    case DanceMove.StepType.ExchangePrograms:
                        var p1Index = programs.IndexOf(move.P1);
                        var p2Index = programs.IndexOf(move.P2);
                        var temp3 = programs[p1Index];
                        programs[p1Index] = programs[p2Index];
                        programs[p2Index] = temp3;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown dance move " + move.Step);
                }
            }

            return programs;
        }

        private struct DanceMove
        {
            public int P1, P2;
            public StepType Step;

            public DanceMove(string description)
            {
                P1 = P2 = 0;

                switch (description[0])
                {
                    case 's':
                        P1 = Int32.Parse(description.Substring(1));
                        Step = StepType.Spin;
                        break;
                    case 'x':
                        var indexParameters = description.Substring(1)
                            .Split('/').Select(p => Int32.Parse(p))
                            .ToArray();

                        P1 = indexParameters[0];
                        P2 = indexParameters[1];
                        Step = StepType.ExchangeIndex;
                        break;
                    case 'p':
                        var programParameters = description.Substring(1)
                            .Split('/').Select(p => p[0] - 'a')
                            .ToArray();
                        P1 = programParameters[0];
                        P2 = programParameters[1];
                        Step = StepType.ExchangePrograms;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown step-type " + description[0]);
                }
            }

            public enum StepType
            {
                Spin = 0,
                ExchangeIndex = 1,
                ExchangePrograms = 2,
            }
        }
    }
}
