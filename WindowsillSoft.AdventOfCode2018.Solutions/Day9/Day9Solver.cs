using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day9
{
    public class Day9Solver : IProblemSolver
    {
        public string Description => "Day 9";

        public int SortOrder => 9;

        public void Solve()
        {
            var Current = new MarbleCircleElement
            {
                Value = 0
            };
            Current.Left = Current.Right = Current;
            int playerCount = 426;
            int lastMarbleValue = 7205800;

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
            Console.WriteLine(scores.Max());
        }
    }

    public class MarbleCircleElement
    {
        public MarbleCircleElement Left { get; set; }
        public MarbleCircleElement Right { get; set; }
        public int Value { get; set; }

        internal void Print()
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
