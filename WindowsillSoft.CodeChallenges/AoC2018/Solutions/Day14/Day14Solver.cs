using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day14
{
    public class Day14Solver : AoC2018SolverBase
    {
        private string _pattern;
        private int _targetGeneration;
        private const string _initialState = "37";

        public override string Description => "Day 14: Chocolate Charts";

        public override int SortOrder => 14;

        public override void Initialize(string input)
        {
            _targetGeneration = int.Parse(input.Split(',')[0]);
            _pattern = input.Split(',')[1];
        }

        public override string SolvePart1(bool silent = true)
        {
            var latestLocations = new int[] { 0, 1 };

            List<int> state = new List<int>(_initialState.Select(p => p - '0'));

            while (true)
            {
                var newRecipe = state[latestLocations[0]] + state[latestLocations[1]];

                if (newRecipe > 9)
                    state.Add(1);

                state.Add(newRecipe % 10);

                latestLocations[0] += state[latestLocations[0]] + 1;
                latestLocations[0] %= state.Count;

                latestLocations[1] += state[latestLocations[1]] + 1;
                latestLocations[1] %= state.Count;

                if (state.Count >= _targetGeneration + 10)
                    return new string(state.Skip(_targetGeneration).Take(10).Select(p => (char)('0' + p)).ToArray());
            }
        }

        public override string SolvePart2(bool silent = true)
        {
            var latestLocations = new int[] { 0, 1 };

            List<int> state = new List<int>(_initialState.Select(p => p - '0'));
            var searchPattern = _pattern.Select(p => p - '0').ToArray();

            int[] searchSpace = new int[searchPattern.Length + 1];
            int searchSpaceIndex = 0;

            while (true)
            {
                var newRecipe = state[latestLocations[0]] + state[latestLocations[1]];
                bool doubleCheck = false;

                if (newRecipe > 9)
                {
                    state.Add(1);
                    searchSpace[searchSpaceIndex] = 1;
                    searchSpaceIndex = (searchSpaceIndex + 1) % searchSpace.Length;
                    doubleCheck = true;
                }

                state.Add(newRecipe % 10);
                searchSpace[searchSpaceIndex] = newRecipe % 10;
                searchSpaceIndex = (searchSpaceIndex + 1) % searchSpace.Length;

                latestLocations[0] += state[latestLocations[0]] + 1;
                latestLocations[0] %= state.Count;

                latestLocations[1] += state[latestLocations[1]] + 1;
                latestLocations[1] %= state.Count;

                int index = -1;
                if (MatchState(searchPattern, searchSpace, searchSpaceIndex))
                    index = 0;
                else if (doubleCheck && MatchState(searchPattern, searchSpace, searchSpaceIndex - 1))
                    index = 1;

                if (index != -1)
                    return (state.Count - (index + _pattern.Length + 1)).ToString();

            }
        }
        /*
                private void Optimized(string input, int target, string pattern)
                {
                    var latestLocations = new int[] { 0, 1 };

                    bool huntingTarget = true;
                    bool huntingPattern = true;
                    List<int> state = new List<int>(input.Select(p => p - '0'));
                    var searchPattern = pattern.Select(p => p - '0').ToArray();

                    int[] searchSpace = new int[searchPattern.Length + 1];
                    int searchSpaceIndex = 0;

                    while (huntingPattern || huntingTarget)
                    {
                        var newRecipe = state[latestLocations[0]] + state[latestLocations[1]];
                        bool doubleCheck = false;

                        if (newRecipe > 9)
                        {
                            state.Add(1);
                            searchSpace[searchSpaceIndex] = 1;
                            searchSpaceIndex = (searchSpaceIndex + 1) % searchSpace.Length;
                            doubleCheck = true;
                        }

                        state.Add(newRecipe % 10);
                        searchSpace[searchSpaceIndex] = newRecipe % 10;
                        searchSpaceIndex = (searchSpaceIndex + 1) % searchSpace.Length;

                        latestLocations[0] += state[latestLocations[0]] + 1;
                        latestLocations[0] %= state.Count;

                        latestLocations[1] += state[latestLocations[1]] + 1;
                        latestLocations[1] %= state.Count;

                        if (huntingTarget && state.Count >= target + 10)
                        {
                            Console.WriteLine($"Next 10 after {target} is {new string(state.Skip(target).Take(10).Select(p => (char)('0' + p)).ToArray())}");
                            huntingTarget = false;
                        }

                        if (!huntingPattern)
                            continue;

                        int index = -1;
                        if (MatchState(searchPattern, searchSpace, searchSpaceIndex))
                            index = 0;
                        else if (doubleCheck && MatchState(searchPattern, searchSpace, searchSpaceIndex - 1))
                            index = 1;

                        if (index != -1)
                        {
                            Console.WriteLine($"Pattern {pattern} found at index {state.Count - (index + pattern.Length + 1)}");
                            huntingPattern = false;
                        }
                    }
                }
                */

        private bool MatchState(int[] pattern, int[] searchSpace, int circleQueueOffset)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (searchSpace[(circleQueueOffset + i + searchSpace.Length) % searchSpace.Length] != pattern[i])
                    return false;
            }
            return true;
        }

        private void PrintSolution(string input, int[] latestLocations)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (i == latestLocations[0])
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (i == latestLocations[1])
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write(input[i]);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
    }
}
