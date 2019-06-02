using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day03 : AdventOfCode2017SolverBase
    {
        private int _target;

        public Day03(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 3: Spiral Memory";

        public override void Initialize(string input) => _target = Int32.Parse(input);

        public override string ExecutePart1()
        {
            if (_target == 1)
                return "0";

            var k = Math.Ceiling((Math.Sqrt(_target) - 1) / 2);
            var t = 2 * k + 1;
            var m = t * t;
            t--;

            return (Math.Abs(k - (m - _target) % t) + k).ToString();
        }

        public override string ExecutePart2()
        {
            var values = new Dictionary<(int x, int y), int>() { { (0, 0), 1 }, { (1, 0), 1 } };
            var current = (x: 1, y: 0);
            var currentValue = 1;
            while (currentValue <= _target)
            {
                current = Next(current, values);
                currentValue = NeighbourValues(current, values).Sum();
                values[current] = currentValue;
            }
            return currentValue.ToString();
        }

        private IEnumerable<int> NeighbourValues((int x, int y) current, Dictionary<(int x, int y), int> values)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (values.ContainsKey((current.x + x, current.y + y)))
                        yield return values[(current.x + x, current.y + y)];
                }
            }
        }

        public (int x, int y) Next((int x, int y) current, Dictionary<(int x, int y), int> values)
        {
            //new shell
            if (values.ContainsKey((current.x - 1, current.y))
                && values.ContainsKey((current.x, current.y - 1)))
            {
                return (current.x + 1, current.y);
            }

            if (values.ContainsKey((current.x - 1, current.y))
                || values.ContainsKey((current.x - 1, current.y - 1)))
            {
                return (current.x, current.y - 1);
            }

            if (values.ContainsKey((current.x, current.y + 1))
                || values.ContainsKey((current.x - 1, current.y + 1)))
            {
                return (current.x - 1, current.y);
            }

            if (values.ContainsKey((current.x + 1, current.y))
                || values.ContainsKey((current.x + 1, current.y + 1)))
            {
                return (current.x, current.y + 1);
            }

            if (values.ContainsKey((current.x, current.y - 1))
                || values.ContainsKey((current.x + 1, current.y - 1)))
            {
                return (current.x + 1, current.y);
            }

            throw new InvalidOperationException("Unknown state!");
        }
    }
}
