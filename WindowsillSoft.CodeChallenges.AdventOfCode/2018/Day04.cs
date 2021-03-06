﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day04 : AdventOfCode2018SolverBase
    {
        private Dictionary<int, List<SleepInterval>> _sleepTimes = new Dictionary<int, List<SleepInterval>>();

        public Day04(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 4: Repose Record";

        public override void Initialize(string input)
        {
            var matcher = new Regex(@"\[(?'year'\d{4})-(?'month'\d{2})-(?'day'\d{2}) (?'hour'\d{2}):(?'minute'\d{2})] (Guard #(?'guard'\d+) )?(?'action'begins shift|wakes up|falls asleep)", RegexOptions.Compiled);
            var logs = ReadAndSplitInput<string>(input)
                .OrderBy(p => p)
                .Select(p => matcher.Match(p))
                .Select(p => new
                {
                    Minute = int.Parse(p.Groups["minute"].Value),
                    Action = p.Groups["action"].Value,
                    Guard = p.Groups["guard"].Success ? int.Parse(p.Groups["guard"].Value) : ((int?)null),
                }).ToArray();

            _sleepTimes = new Dictionary<int, List<SleepInterval>>();
            var state = ParserStates.Start;
            var currentGuard = 0;
            var currentIntervalStart = 0;

            foreach (var line in logs)
            {
                switch (line.Action)
                {
                    case "begins shift":
                        if (state == ParserStates.Asleep)
                            throw new InvalidOperationException("A new guard cannot start shift while the previous is still sleeping.");
                        state = ParserStates.Awake;
                        currentGuard = line.Guard!.Value;
                        break;
                    case "falls asleep":
                        if (state == ParserStates.Asleep)
                            throw new InvalidCastException("Guard cannot fall asleep while current state is " + state);
                        state = ParserStates.Asleep;
                        currentIntervalStart = line.Minute;
                        break;
                    case "wakes up":
                        if (state == ParserStates.Awake || state == ParserStates.Start)
                            throw new InvalidOperationException("Guard cannot wake up when he is just starting his shift or already asleep.");
                        state = ParserStates.Awake;
                        AddSleepInterval(_sleepTimes, currentGuard, new SleepInterval
                        {
                            Start = currentIntervalStart,
                            End = line.Minute
                        });
                        break;
                    default:
                        throw new InvalidOperationException("Unknown state " + state);
                }
            }
        }

        public override string ExecutePart1()
        {
            var bestGuard = _sleepTimes.OrderByDescending(p => p.Value.Sum(q => q.End - q.Start)).First();
            IO.LogIfAttached(() =>
                $"The best guard is guard {bestGuard.Key}, asleep for a total of {bestGuard.Value.Sum(p => p.End - p.Start)} minutes.");

            var mods = bestGuard.Value.Select(p => (Minute: p.Start, Change: 1))
                        .Concat(bestGuard.Value.Select(p => (Minute: p.End, Change: -1)))
                    .OrderBy(p => p.Minute);

            int bestOverlaps = 0;
            int currentOverlaps = 0;
            int bestMinute = 0;

            foreach (var mod in mods)
            {
                currentOverlaps += mod.Change;
                if (currentOverlaps > bestOverlaps)
                {
                    bestOverlaps = currentOverlaps;
                    bestMinute = mod.Minute;
                }
            }

            IO.LogIfAttached(() => $"The guard has the best overlap at {bestMinute} with {bestOverlaps} overlaps.");
            IO.LogIfAttached(() => $"Result code: {bestGuard.Key}x{bestMinute} = {bestGuard.Key * bestMinute}");

            return (bestGuard.Key * bestMinute).ToString();
        }

        public override string ExecutePart2()
        {
            int bestOverlaps = 0;
            int bestMinute = 0;
            int bestGuard = 0;

            foreach (var interval in _sleepTimes)
            {
                int overlaps = 0;
                var mods = interval.Value.Select(p => (Minute: p.Start, Change: 1))
                        .Concat(interval.Value.Select(p => (Minute: p.End, Change: -1)))
                    .OrderBy(p => p.Minute);
                foreach (var mod in mods)
                {
                    overlaps += mod.Change;
                    if (overlaps > bestOverlaps)
                    {
                        bestOverlaps = overlaps;
                        bestMinute = mod.Minute;
                        bestGuard = interval.Key;
                    }
                }
            }

            IO.LogIfAttached(() => $"The best guard for overlaps is guard {bestGuard} with {bestOverlaps} total overlaps at minute {bestMinute}.");
            IO.LogIfAttached(() => $"Result code: {bestGuard}x{bestMinute} = {bestGuard * bestMinute}");

            return (bestGuard * bestMinute).ToString();
        }

        private void AddSleepInterval(Dictionary<int, List<SleepInterval>> sleepTimes, int currentGuard, SleepInterval sleepInterval)
        {
            if (!sleepTimes.ContainsKey(currentGuard))
                sleepTimes[currentGuard] = new List<SleepInterval>();
            sleepTimes[currentGuard].Add(sleepInterval);
        }

        public enum ParserStates
        {
            Start,
            Awake,
            Asleep,
        }

        public struct SleepInterval
        {
            public int Start;
            public int End;
        }
    }
}