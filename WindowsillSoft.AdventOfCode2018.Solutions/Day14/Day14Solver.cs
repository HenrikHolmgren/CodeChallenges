using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day14
{
    public class Day14Solver : IProblemSolver
    {
        public string Description => "Day 14";

        public int SortOrder => 14;

        public void Solve()
        {
            var input = "37";
            int target = 110201;
            string pattern = "110201";

            Optimized(input, target, pattern);
        }

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
                    Console.WriteLine($"Pattern {pattern} found at index {state.Count - (index + pattern.Length+ 1)}");
                    huntingPattern = false;
                }
            }
        }

        private bool MatchState(int[] pattern, int[] searchSpace, int circleQueueOffset)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (searchSpace[(circleQueueOffset + i + searchSpace.Length) % searchSpace.Length] != pattern[i])
                    return false;
            }
            return true;
        }

        private void Naive(string state, int target, string pattern)
        {
            var latestLocations = new int[] { 0, 1 };

            bool huntingTarget = true;
            bool huntingPattern = true;
            while (huntingPattern || huntingTarget)
            {
                //Output test solution for verification
                if (state.Length < 20)
                    PrintSolution(state, latestLocations);

                var newRecipe = state[latestLocations[0]] + state[latestLocations[1]] - (2 * '0');
                state += newRecipe;

                latestLocations[0] += state[latestLocations[0]] - '0' + 1;
                latestLocations[0] %= state.Length;

                latestLocations[1] += state[latestLocations[1]] - '0' + 1;
                latestLocations[1] %= state.Length;

                if (huntingTarget && state.Length >= target + 10)
                {
                    Console.WriteLine($"Next 10 after {target} is {new string(state.Skip(target).Take(10).ToArray())}");
                    huntingTarget = false;
                }

                if (!huntingPattern)
                    continue;

                var index = state.IndexOf(pattern);
                if (index != -1)
                {
                    Console.WriteLine($"Pattern {pattern} found at index {index}");
                    huntingPattern = false;
                }
            }
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
