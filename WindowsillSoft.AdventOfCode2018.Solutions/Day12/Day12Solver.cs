using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day12
{
    public class Day12Solver : IProblemSolver
    {
        public string Description => "Day 12";

        public int SortOrder => 12;

        public void Solve()
        {
            var initialState = "#...#...##..####..##.####.#...#...#.#.#.#......##....#....######.####.##..#..#..##.##..##....#######";

            var livePatterns = new HashSet<string> {
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
            };
            var state = initialState.Select((p, i) => (Index: i, State: p)).ToList();
            int lastSum = 0;
            for (int round = 0; round < 20; round++)
            {
                PadState(state);
                state = Permutate(livePatterns, state);
                var newSum = state.Where(p => p.State == '#').Sum(p => p.Index);
                lastSum = newSum;

                Console.WriteLine(new string(state.Select(p => p.State).ToArray()));
            }

            Console.WriteLine(state.Where(p => p.State == '#').Sum(p => p.Index));

            int index = 0;
            string lastPattern = string.Empty;
            state = initialState.Select((p, i) => (Index: i, State: p)).ToList();
            var values = new List<int>();

            while (true)
            {
                PadState(state);
                state = Permutate(livePatterns, state);
                var entry = new string(state.Select(p => p.State).ToArray());
                values.Add(state.Where(p => p.State == '#').Sum(p => p.Index));
                if (lastPattern == entry)
                    break;
                lastPattern = entry;
                index++;
            }

            Console.WriteLine($"Stable pattern achieved after {index} iterations!");

            var lastPair = values.AsEnumerable().Reverse().Take(2).ToArray();
            var offset = lastPair[0] - lastPair[1];
            Console.WriteLine($"Pattern shifts value with {offset} every repeat.");

            var fullOffset = (50_000_000_000 - index) * offset;
            Console.WriteLine($"Value in the far future = {fullOffset + lastPair[1]}");
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