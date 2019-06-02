using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day01 : AdventOfCode2017SolverBase
    {
        private int[] _digits;

        public Day01(IIOProvider provider) : base(provider)
            => _digits = new int[0];

        public override string Name => "Day 1: Inverse Captcha";

        public override void Initialize(string input) => _digits = input.Select(p => p - '0').ToArray();

        public override string ExecutePart1()
            => GetCaptchaSum(1).ToString();

        public override string ExecutePart2()
            => GetCaptchaSum(_digits.Length / 2).ToString();

        private int GetCaptchaSum(int offset)
        {
            var sum = 0;

            for (var i = 0; i < _digits.Length; i++)
            {
                if (_digits[i] == _digits[(i + offset) % _digits.Length])
                    sum += _digits[i];
            }

            return sum;
        }

    }
}
