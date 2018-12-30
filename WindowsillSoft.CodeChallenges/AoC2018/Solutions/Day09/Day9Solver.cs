using System.Linq;
using WindowsillSoft.AdventOfCode.AoC2018.Common;
using WindowsillSoft.CodeChallenges.AoC2018.Common;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day9
{
    public class Day9Solver : AoC2018SolverBase
    {
        private int _playerCount;
        private int _lastMarbleValue;

        public override string Description => "Day 9: Marble Mania";

        public override int SortOrder => 9;

        public override void Initialize(string input)
        {
            _playerCount = int.Parse(input.Split(',')[0]);
            _lastMarbleValue = int.Parse(input.Split(',')[1]);
        }

        public override string SolvePart1(bool silent = true)
        {
            return Run(_playerCount, _lastMarbleValue);
        }

        public override string SolvePart2(bool silent = true)
        {
            return Run(_playerCount, _lastMarbleValue * 100);
        }

        private string Run(int playerCount, int lastMarbleValue)
        {
            var Current = new MarbleCircleElement
            {
                Value = 0
            };
            Current.Left = Current.Right = Current;

            long[] scores = new long[playerCount];

            for (int i = 1; i < lastMarbleValue; i++)
            {
                if (i % 23 == 0)
                {
                    scores[i % playerCount] += i;
                    for (int j = 0; j < 7; j++)
                        Current = Current.Left;
                    Current.Left.Right = Current.Right;
                    Current.Right.Left = Current.Left;
                    scores[i % playerCount] += Current.Value;
                    Current = Current.Right;
                }
                else
                {
                    var newElement = new MarbleCircleElement { Value = i };

                    Current.Right.Right.Left = newElement;
                    newElement.Right = Current.Right.Right;

                    Current.Right.Right = newElement;
                    newElement.Left = Current.Right;
                    Current = newElement;
                }

            }
            return scores.Max().ToString();
        }
    }
}
