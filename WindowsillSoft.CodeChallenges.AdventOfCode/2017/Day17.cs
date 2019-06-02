using System;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day17 : AdventOfCode2017SolverBase
    {
        private int _stepCount;

        public Day17(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 17: Spinlock";

        public override void Initialize(string input) => _stepCount = Int32.Parse(input);

        public override string ExecutePart1()
        {
            var queue = new CircleQueue<int>(0);
            for (var i = 1; i <= 2017; i++)
            {
                for (var j = 0; j < _stepCount; j++)
                    queue = queue.Next;
                queue = queue.AddNext(i);
            }
            return queue.Next.Value.ToString();
        }

        public override string ExecutePart2()
        {
            Console.Write("Building queue ");
            var queue = new CircleQueue<int>(0);
            for (var i = 1; i <= 50_000_000; i++)
            {
                for (var j = 0; j < _stepCount; j++)
                    queue = queue.Next;
                queue = queue.AddNext(i);
                if (i % 500_000 == 0)
                    Console.Write('.');
            }
            Console.WriteLine();
            while (queue.Value != 0)
                queue = queue.Next;
            return queue.Next.Value.ToString();
        }

        private class CircleQueue<T>
        {
            public CircleQueue<T> Next { get; set; }
            public T Value { get; }

            public CircleQueue(T value)
            {
                Value = value;
                Next = this;
            }

            public CircleQueue<T> AddNext(T nextValue)
            {
                var nextElement = new CircleQueue<T>(nextValue)
                {
                    Next = Next
                };
                Next = nextElement;
                return nextElement;
            }
        }
    }
}
