using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day04 : AdventOfCode2019SolverBase
    {
        private int _maxCode;
        private int _minCode;

        public Day04(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 4: Secure Container";

        public override string ExecutePart1()
            => Enumerable.Range(_minCode, _maxCode - _minCode)
            .Count(p => IsValidPassword1(p)).ToString();

        public override string ExecutePart2()
            => Enumerable.Range(_minCode, _maxCode - _minCode)
            .Count(p => IsValidPassword2(p)).ToString();

        public override void Initialize(string input)
        {
            var interval = ReadAndSplitInput<int>(input, '-').ToArray();
            (_minCode, _maxCode) = (interval[0], interval[1]);
        }

        public static bool IsValidPassword1(int candidate)
        {
            var digits = candidate.ToString().Select(p => p - '0').ToArray();
            return digits.Length == 6 &&
                Enumerable.Range(0, 5).Any(p => digits[p] == digits[p + 1]) &&
                !Enumerable.Range(0, 5).Any(p => digits[p] > digits[p + 1]);
        }

        public static bool IsValidPassword2(int candidate)
        {
            var digits = candidate.ToString().Select(p => p - '0').ToArray();
            return digits.Length == 6 &&
                Enumerable.Range(0, 5).Any(p => digits[p] == digits[p + 1] &&
                    (p == 0 || digits[p] != digits[p - 1]) &&
                    (p == 4 || digits[p + 1] != digits[p + 2])) &&
                !Enumerable.Range(0, 5).Any(p => digits[p] > digits[p + 1]);
        }
    }

}
