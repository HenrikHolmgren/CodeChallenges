using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day06 : AdventOfCode2017SolverBase
    {
        private int[] _banks;

        public Day06(IIOProvider provider) : base(provider) => _banks = new int[0];

        public override string Name => "Day 6: Memory Reallocation";

        public override void Initialize(string input) => _banks = input.Trim().Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(p => Int32.Parse(p)).ToArray();

        public override string ExecutePart1()
        {
            var banks = (int[])_banks.Clone();

            var i = GetMaxIndex(_banks);
            var bankString = Stringify(_banks);
            var seenBanks = new HashSet<string>();
            while (!seenBanks.Contains(bankString))
            {
                seenBanks.Add(bankString);
                var index = GetMaxIndex(banks);
                var count = banks[index];
                banks[index] = 0;
                while (count > 0)
                {
                    index = (index + 1) % banks.Length;
                    banks[index]++;
                    count--;
                }
                bankString = Stringify(banks);
            }
            return seenBanks.Count.ToString();
        }

        public override string ExecutePart2()
        {
            var banks = (int[])_banks.Clone();
            var currentLoop = 0;
            var i = GetMaxIndex(_banks);
            var bankString = Stringify(_banks);
            var seenBanks = new Dictionary<string, int>();
            while (!seenBanks.ContainsKey(bankString))
            {
                seenBanks.Add(bankString, currentLoop++);
                var index = GetMaxIndex(banks);
                var count = banks[index];
                banks[index] = 0;
                while (count > 0)
                {
                    index = (index + 1) % banks.Length;
                    banks[index]++;
                    count--;
                }
                bankString = Stringify(banks);
            }
            return (seenBanks.Count - seenBanks[bankString]).ToString();
        }

        private string Stringify(int[] banks)
            => String.Join(" ", banks);

        private int GetMaxIndex(int[] banks)
        {
            var max = Int32.MinValue;
            var maxIndex = 0;
            for (var i = 0; i < banks.Length; i++)
            {
                if (banks[i] > max)
                {
                    max = banks[i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

    }
}
