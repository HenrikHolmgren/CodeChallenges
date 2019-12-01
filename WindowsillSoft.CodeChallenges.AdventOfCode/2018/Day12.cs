using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day12 : AdventOfCode2018SolverBase
    {
        private string _initialState = string.Empty;
        private HashSet<string> _livePatterns = new HashSet<string>();

        public Day12(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 12: Subterranean Sustainability";

        public override void Initialize(string input)
        {
            var patterns = ReadAndSplitInput<string>(input);
            _initialState = patterns.First();
            _livePatterns = new HashSet<string>(patterns.Skip(1));
        }

        public override string ExecutePart1()
        {
            var state = _initialState.Select((p, i) => (Index: i, State: p)).ToList();
            int lastSum = 0;
            for (int round = 0; round < 20; round++)
            {
                PadState(state);
                state = Permutate(_livePatterns, state);
                var newSum = state.Where(p => p.State == '#').Sum(p => p.Index);
                lastSum = newSum;
                IO.LogIfAttached(() => new string(state.Select(p => p.State).ToArray()));
            }

            return state.Where(p => p.State == '#').Sum(p => p.Index).ToString();
        }

        public override string ExecutePart2()
        {
            int index = 0;
            string lastPattern = string.Empty;
            var state = _initialState.Select((p, i) => (Index: i, State: p)).ToList();
            var values = new List<int>();

            while (true)
            {
                PadState(state);
                state = Permutate(_livePatterns, state);
                var entry = new string(state.Select(p => p.State).ToArray());
                values.Add(state.Where(p => p.State == '#').Sum(p => p.Index));
                if (lastPattern == entry)
                    break;
                lastPattern = entry;
                index++;
            }

            IO.LogIfAttached(() => $"Stable pattern achieved after {index} iterations!");

            var lastPair = values.AsEnumerable().Reverse().Take(2).ToArray();
            var offset = lastPair[0] - lastPair[1];
            IO.LogIfAttached(() => $"Pattern shifts value with {offset} every repeat.");

            var fullOffset = (50_000_000_000 - index) * offset;
            IO.LogIfAttached(() => $"Value in the far future = {fullOffset + lastPair[1]}");

            return (fullOffset + lastPair[1]).ToString();
        }

        private static List<(int Index, char State)> Permutate(HashSet<string> livePatterns, List<(int Index, char State)> state)
        {
            var newState = new List<(int Index, char State)>();
            for (int i = 0; i < state.Count - 5; i++)
            {
                var pattern = new String(state.Skip(i).Take(5).Select(p => p.State).ToArray());
                var match = livePatterns.Contains(pattern) ? '#' : '.';
                newState.Add((state[i].Index + 2, match));
            }
            return newState;
        }

        private void PadState(List<(int Index, char State)> state)
        {
            while (new string(state.Take(4).Select(p => p.State).ToArray()) != "....")
                state.Insert(0, (state[0].Index - 1, '.'));

            while (new string(state.AsEnumerable().Reverse().Take(4).Select(p => p.State).ToArray()) != "....")
                state.Add((state.Last().Index + 1, '.'));

            while (state[4].State == '.')
                state.RemoveAt(0);

            while (state[state.Count - 5].State == '.')
                state.RemoveAt(state.Count - 1);
        }
    }

}
