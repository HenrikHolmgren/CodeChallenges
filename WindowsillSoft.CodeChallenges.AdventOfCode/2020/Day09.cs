using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2020
{
    public class Day09 : AdventOfCode2020SolverBase
    {
        private int _preampleLength;
        private long[] _code;

        public Day09(IIOProvider provider) : base(provider) { _code = new long[0]; }

        public override string Name => "Day 9: Encoding Error";

        //Naïve version for now, there is probably good optimization to be had by keeping state.
        public override string ExecutePart1()
        {
            var index = FindFirstBrokenIndex();
            if (index > 0)
                return _code[index].ToString();
            return "No solution found.";
        }

        public override string ExecutePart2()
        {
            var brokenIndex = FindFirstBrokenIndex();
            for (int i = 0; i < brokenIndex; i++)
            {
                for (int j = i; j < brokenIndex; j++)
                {
                    if (_code[i..j].Sum() == _code[brokenIndex])
                        return (_code[i..j].Min() + _code[i..j].Max()).ToString();
                }
            }
            return "No solution found.";
        }

        private int FindFirstBrokenIndex()
        {
            for (int i = _preampleLength; i < _code.Length; i++)
            {
                if (!_code[(i - _preampleLength)..i].Any(p => _code[(i - _preampleLength)..i].Where(q => p != q).Select(q => p + q).Any(p => p == _code[i])))
                    return i;
            }
            return -1;
        }

        public override void Initialize(string input)
        {
            var raw = ReadAndSplitInput<long>(input);
            _preampleLength = (int)raw.First();
            _code = raw.Skip(1).ToArray();
        }
    }
}
