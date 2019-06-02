using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day05 : AdventOfCode2017SolverBase
    {
        private int[] _jumpList;

        public Day05(IIOProvider provider) : base(provider) => _jumpList = new int[0];

        public override string Name => "Day 5: A Maze of Twisty Trampolines, All Alike";

        public override void Initialize(string input)
        {
            _jumpList = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => Int32.Parse(p))
                    .ToArray();
        }

        public override string ExecutePart1()
            => GetJumpCount(p => p + 1);

        public override string ExecutePart2()
            => GetJumpCount(p => p >= 3 ? p - 1 : p + 1);

        private string GetJumpCount(Func<int, int> getOffset)
        {
            var offset = 0;
            var jumps = 0;
            var state = (int[])_jumpList.Clone();
            while (offset < state.Length)
            {
                var newOffset = offset + state[offset];
                state[offset] = getOffset(state[offset]);
                offset = newOffset;
                jumps++;
            }
            return jumps.ToString();
        }


    }
}
