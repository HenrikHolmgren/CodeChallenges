using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2017
{
    public class Day10 : AdventOfCode2017SolverBase
    {
        private int _queueSize;
        private string _input;

        public Day10(IIOProvider provider) : base(provider)
        {
            _queueSize = 0;
            _input = String.Empty;
        }

        public override string Name => "Day 10: Knot Hash";

        public override void Initialize(string input)
        {
            string? queueSizeString;
            do
            {
                queueSizeString = IO.RequestInput("Queue size?");
            } while (queueSizeString == default || !Int32.TryParse(queueSizeString, out _queueSize));

            _input = input;
        }

        public override string ExecutePart1()
        {
            var input = _input.Split(',')
                .Select(p => Int32.Parse(p))
                .ToArray();
            var queue = new CircleQueue(_queueSize);
            foreach (var swap in input)
            {
                queue.DoPermutation(swap);
                IO.LogLine(queue);
            }
            return queue.Part1Value.ToString();
        }

        public override string ExecutePart2()
        {
            var input = _input.Select(p => (int)p)
                .Concat(new[] { 17, 31, 73, 47, 23 })
                .ToArray();
            
            var queue = new CircleQueue(_queueSize);
            for (var i = 0; i < 64; i++)
            {
                foreach (var swap in input)
                    queue.DoPermutation(swap);
                IO.LogLine(queue);
            }
            return queue.Part2Value;
        }

        public class CircleQueue
        {
            private readonly List<int> _elements;

            private int _currentElement;
            private int _skipSize = 0;

            public int Part1Value => _elements[0] * _elements[1];
            public string Part2Value => BuildDenseHash();

            public CircleQueue(int capacity)
            {
                _elements = Enumerable.Range(0, capacity)
                    .ToList();

                _currentElement = 0;
            }

            public void DoPermutation(int sliceLength)
            {
                var toSwap = ElementsToSwap(sliceLength);
                toSwap.Reverse();
                for (var i = 0; i < toSwap.Count; i++)
                    _elements[(i + _currentElement) % _elements.Count] = toSwap[i];

                _currentElement += sliceLength + _skipSize;
                _currentElement = _currentElement % _elements.Count;
                _skipSize++;
            }

            private List<int> ElementsToSwap(int sliceLength)
            {
                var elements = _elements
                    .Skip(_currentElement).Take(sliceLength)
                    .ToList();
                if (elements.Count < sliceLength)
                    elements.AddRange(_elements.Take(sliceLength - elements.Count));

                return elements;
            }

            private string BuildDenseHash()
            {
                var result = "";
                for (var group = 0; group < 16; group++)
                {
                    var groupSet = _elements.Skip(group * 16).Take(16)
                        .Aggregate(0, (p, q) => p ^ q);
                    result += groupSet.ToString("x2");
                }
                return result;
            }

            public override string ToString()
            {
                var result = "";

                for (var i = 0; i < _elements.Count; i++)
                {
                    if (i == _currentElement)
                        result += $"[{_elements[i]}]";
                    else
                        result += $" {_elements[i]} ";
                }
                return result;
            }
        }
    }
}
