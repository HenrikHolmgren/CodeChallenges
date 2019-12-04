using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Extensions;

namespace WindowsillSoft.CodeChallenges.AdventOfCode
{
    [SolverCategory("Advent of Code")]
    public abstract class AdventOfCodeSolverBase : ProblemSolverBase<AdventOfCodeResult>
    {
        public AdventOfCodeSolverBase(IIOProvider provider) : base(provider)
        { }

        public override string Name
            => GetType().Name.ToProperName();

        public override void Initialize()
        {
            var input = IO.RequestFile("Solver input?");
            if (input != null)
                Initialize(input);
        }

        public override AdventOfCodeResult Execute()
        {
            var sw = Stopwatch.StartNew();

            var part1Result = ExecutePart1();
            var part1Timing = sw.Elapsed;
            sw.Restart();
            var part2Result = ExecutePart2();
            var part2Timing = sw.Elapsed;

            return new AdventOfCodeResult(
                part1Result, part1Timing,
                part2Result, part2Timing);
        }

        public abstract void Initialize(string input);
        public abstract string ExecutePart1();
        public abstract string ExecutePart2();

        protected IEnumerable<T> ReadAndSplitInput<T>(string input, params char[] separator)
            => input.Split(separator)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => Convert.ChangeType(p, typeof(T)))
            .Cast<T>();
    }

    public class AdventOfCodeResult
    {
        public string Part1Result { get; }
        public string Part2Result { get; }
        public TimeSpan Part1Time { get; }
        public TimeSpan Part2Time { get; }

        public AdventOfCodeResult(string part1Result, TimeSpan part1Time, string part2Result, TimeSpan part2Time)
        {
            Part1Result = part1Result;
            Part2Result = part2Result;
            Part1Time = part1Time;
            Part2Time = part2Time;
        }

        public override string? ToString() => $"AoC result set:" + Environment.NewLine +
            $"Part 1 ({Part1Time.TotalSeconds}s): {Part1Result}" + Environment.NewLine +
            $"Part 2 ({Part2Time.TotalSeconds}s): {Part2Result}";
    }
}
