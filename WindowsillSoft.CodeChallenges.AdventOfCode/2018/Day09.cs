using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day09 : AdventOfCode2018SolverBase
    {
        private int _playerCount;
        private int _lastMarbleValue;

        public Day09(IIOProvider provider) : base(provider)
        {
        }

        public override string Name => "Day 9: Marble Mania";

        public override void Initialize(string input)
        {
            _playerCount = int.Parse(input.Split(',')[0]);
            _lastMarbleValue = int.Parse(input.Split(',')[1]);
        }

        public override string ExecutePart1()
        {
            return Run(_playerCount, _lastMarbleValue);
        }

        public override string ExecutePart2()
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

        public class MarbleCircleElement
        {
            //TODO: Handle non-nullable fields properly.
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
            public MarbleCircleElement Left { get; set; }
            public MarbleCircleElement Right { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
            public int Value { get; set; }

            public void Print()
            {
                var iterator = this;
                Console.Write($"({iterator.Value}) ");
                iterator = iterator.Left;
                while (iterator != this)
                {
                    Console.Write($"{iterator.Value} ");
                    iterator = iterator.Left;
                }

                Console.WriteLine();
            }
        }
    }
}
