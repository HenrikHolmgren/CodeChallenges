using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day10 : AdventOfCode2020SolverBase
    {
        int[] _adapterJotages = new int[0];

        public Day10(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 10: Adapter Array";
        public override string ExecutePart1()
        {
            int[] voltageDiffs = new int[4]; //Wasting a byte here, but it means we don't have to subtract for every diff registration.

            for (int i = 1; i < _adapterJotages.Length; i++)
            {
                var diff = _adapterJotages[i] - _adapterJotages[i - 1];
                if (diff > 3)
                    break; //A junction in the adapter train would result in an illegal diff, break out of the count.
                voltageDiffs[diff]++;
            }
            return (voltageDiffs[1] * voltageDiffs[3]).ToString();
        }

        public override string ExecutePart2()
        {
            //By reviewing data, it seems there are exclusively 3- and 1-jolt gaps. 
            //This neatly segregates the permutation possibilities into islands of 1-jolt gaps separated by 3-jolt gap 'walls'.
            //Additionally, there are only 0, 1, 2, 3 or 4 1-jolt gaps in a row.
            //This means we can map as follows:
            //33 (two 3-jolt gaps in a row) -> only 1 possible permutation, any change would break sequence.
            //313 (a single 1-jolt run) -> Still only 1n possible permutation, removing the single joltage island would break sequence.
            //3113 (a double 1-jolt run) -> The first value is removable turning into a 323 run, so 2 possible permutations.
            //31113 (a triple 1-jolt run) -> Allows for mutation into 31113, 3213, 3123, 333, giving 4 possible permutations.
            //311113 (a quadruple 1-jolt run) -> Allows for mutation into 311113, 32113, 31213, 31123, 3223, 3313, 3131 -> 7 permutations.
            var runCollection = BuildRunCollection();
            return (1 // 33
                * 1 //313 
                * Math.Pow(2, runCollection[2]) //3113
                * Math.Pow(4, runCollection[3]) //31113
                * Math.Pow(7, runCollection[4]) //311113
                ).ToString();
        }

        private int[] BuildRunCollection()
        {
            int[] runs = new int[5]; //Max run size of 4 allowed.
            var run = 0;
            for (int i = 0; i < _adapterJotages.Length - 1; i++)
            {
                var diff = _adapterJotages[i + 1] - _adapterJotages[i];
                if (diff != 1 && diff != 3)
                    throw new InvalidOperationException("Solution model does not match data, a non-1 and non-3 gap was detected.");
                if (diff == 1)
                {
                    run++;
                    if (run > 4) throw new InvalidOperationException("Solution model does not match data, a run of more than 3 single-jolt gaps was detected.");
                }
                else
                {
                    runs[run]++;
                    run = 0;
                }
            }
            return runs;
        }

        //Note, this is a prime candidate for radix sort down the line, as the input is clearly bounded and easily mapped to integer keys.
        //Left as simple n log n sort for now to avoid unnecessary rocket surgery.
        public override void Initialize(string input)
        {
            var rawInput = ReadAndSplitInput<int>(input).OrderBy(p => p).ToList();
            rawInput.Insert(0, 0); //Wall outlet
            rawInput.Add(rawInput.Last() + 3); //Device adapter
            _adapterJotages = rawInput.ToArray();
        }
    }
}
