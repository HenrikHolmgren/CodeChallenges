using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day12
{
    public class Day12Solver : IAdventOfCodeSolver
    {
        private string _initialState;
        private HashSet<string> _livePatterns;

        public string Description => "Day 12: Subterranean Sustainability";

        public int SortOrder => 12;

        public void Initialize(string input)
        {
            var patterns = input.Split(Environment.NewLine);
            _initialState = patterns[0];
            _livePatterns = new HashSet<string>(patterns.Skip(1));
            /*{
                "#####",
                "####.",
                "###..",
                "##...",
                "#..#.",
                "#.#..",
                "##.##",
                ".###.",
                ".##..",
                ".#...",
                "#.#.#",
                ".#.##",
                ".##.#",
                ".#..#",
                ".#.#.",
                "..#..",
                "...##",
            };*/
        }

        public string SolvePart1(bool silent = true)
        {
            var state = _initialState.Select((p, i) => (Index: i, State: p)).ToList();
            int lastSum = 0;
            for (int round = 0; round < 20; round++)
            {
                PadState(state);
                state = Permutate(_livePatterns, state);
                var newSum = state.Where(p => p.State == '#').Sum(p => p.Index);
                lastSum = newSum;
                if (!silent)
                    Console.WriteLine(new string(state.Select(p => p.State).ToArray()));
            }

            return state.Where(p => p.State == '#').Sum(p => p.Index).ToString();
        }

        public string SolvePart2(bool silent = true)
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

            if (!silent)
                Console.WriteLine($"Stable pattern achieved after {index} iterations!");

            var lastPair = values.AsEnumerable().Reverse().Take(2).ToArray();
            var offset = lastPair[0] - lastPair[1];
            if (!silent)
                Console.WriteLine($"Pattern shifts value with {offset} every repeat.");

            var fullOffset = (50_000_000_000 - index) * offset;
            if (!silent)
                Console.WriteLine($"Value in the far future = {fullOffset + lastPair[1]}");

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