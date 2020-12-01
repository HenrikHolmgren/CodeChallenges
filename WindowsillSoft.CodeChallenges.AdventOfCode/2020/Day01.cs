using System;
using System.Diagnostics;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day01 : AdventOfCode2020SolverBase
    {
        private int[] _expenses = new int[0];

        public Day01(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 1: Report Repair";

        public override string ExecutePart1() => LocateSum(_expenses, 2020).ToString();
        public override string ExecutePart2() => LocateSumThree(_expenses, 2020).ToString();

        //O(n) excluding the time spent ordering the collection on load in Initialize
        private int LocateSum(int[] expenses, int target, int taboo = -1)
        {
            int low = 0, high = expenses.Length - 1;
            while (low < expenses.Length - 1)
            {
                while (expenses[low] + expenses[high] > target)
                    high--;
                if (expenses[low] + expenses[high] == target
                    && low != taboo && high != taboo)
                    return expenses[low] * expenses[high];

                low++;
                if (high <= low)
                    return 0;
            }
            return 0;
        }

        //O(n^2) since LocateSum is O(n) 
        private int LocateSumThree(int[] expenses, int target)
        {
            for (int i = 1; i < expenses.Length - 1; i++)
            {
                var value = LocateSum(expenses, target - expenses[i], i);
                if (value != 0)
                    return value * expenses[i];
            }

            return 0;
        }

        public override void Initialize(string input)
        {
            _expenses = ReadAndSplitInput<int>(input).OrderBy(p => p).ToArray();
        }
    }
}
