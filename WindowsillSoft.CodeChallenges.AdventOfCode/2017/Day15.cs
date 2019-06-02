using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day15 : AdventOfCode2017SolverBase
    {
        private long[] _initialValues;

        public Day15(IIOProvider provider) : base(provider) => _initialValues = new long[0];

        public override string Name => "Day 15: Dueling Generators";

        public override void Initialize(string input)
        {
            _initialValues = input.Split(",")
                .Select(p => Int64.Parse(p))
                .ToArray();
        }

        public override string ExecutePart1()
        {
            var hits = 0;

            var mask = (1 << 16) - 1;

            var GenA = _initialValues[0];
            var GenB = _initialValues[1];
            for (var i = 0; i < 40_000_000; i++)
            {
                GenA *= 16807;
                GenA %= Int32.MaxValue;

                GenB *= 48271;
                GenB %= Int32.MaxValue;

                if ((GenA & mask) == (GenB & mask))
                    hits++;
            }

            return hits.ToString();
        }

        public override string ExecutePart2()
        {
            var hits = 0;

            var mask = (1 << 16) - 1;

            var GenA = _initialValues[0];
            var GenB = _initialValues[1];
            for (var i = 0; i < 5_000_000; i++)
            {
                do
                {
                    GenA *= 16807;
                    GenA %= Int32.MaxValue;
                } while ((GenA & 3) != 0);

                do
                {
                    GenB *= 48271;
                    GenB %= Int32.MaxValue;
                } while ((GenB & 7) != 0);

                if ((GenA & mask) == (GenB & mask))
                    hits++;
            }

            return hits.ToString();
        }
    }
}
