using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2018
{
    public class Day05 : AdventOfCode2018SolverBase
    {
        private HashSet<char> _uniqueUnits = new HashSet<char>();
        private LinkedList<char> _list = new LinkedList<char>('¤');

        public Day05(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 5: Alchemical Reduction";

        public override void Initialize(string input)
        {
            LinkedList<char>? list = null;

            _uniqueUnits = new HashSet<char>();

            foreach (var unit in input.Reverse())
            {
                if (list == null)
                    list = new LinkedList<char>(unit);
                else
                    list = new LinkedList<char>(unit, list);
                _uniqueUnits.Add(char.ToLower(unit));
            }
            if (list != null)
                _list = new LinkedList<char>('¤', list);
        }

        public override string ExecutePart1()
        {
            var reducedList = _list.Clone().Reduce(IsMatch);
            return (reducedList.Length() - 1).ToString();
        }

        public override string ExecutePart2()
        {
            var bestUnit = '¤';
            var bestLength = int.MaxValue;

            foreach (var unit in _uniqueUnits)
            {
                var filteredList = _list.Filter(p => p == unit || p == char.ToUpper(unit));

                filteredList = filteredList.Reduce(IsMatch);
                var filteredLength = filteredList.Length();
                if (filteredLength < bestLength)
                {
                    bestLength = filteredLength;
                    bestUnit = unit;
                }
                IO.LogIfAttached(() => $"After removing {unit} units, it reduces to {filteredList.Length() - 1}.");
            }
            return (bestLength - 1).ToString();
        }

        private bool IsMatch(char value1, char value2)
            => value1 != value2 &&
                (char.ToUpper(value1) == value2 || char.ToUpper(value2) == value1);

        public class LinkedList<T>
        {
            public T Value { get; set; }
            public LinkedList<T>? Next { get; set; }
            public LinkedList<T>? Prev { get; set; }

            public LinkedList(T value) : this(value, null) { }
            public LinkedList(T value, LinkedList<T>? next)
            {
                (Value, Next) = (value, next);
                if (Next != null)
                    Next.Prev = this;
            }

            public IEnumerable<LinkedList<T>> Enumerate()
            {
                LinkedList<T>? i = this;
                while (i != null)
                {
                    yield return i;
                    i = i.Next;
                }
            }

            internal void Print()
            {
                foreach (var node in Enumerate())
                    Console.Write(node.Value);
                Console.WriteLine();
            }

            public int Length()
                => Enumerate().Count();


            public LinkedList<T> Clone()
            {
                LinkedList<T>? generator = null;
                foreach (var node in Enumerate().Reverse())
                    generator = new LinkedList<T>(node.Value, generator);
                return generator!;
            }

            public LinkedList<T> Reduce(Func<T, T, bool> matcher)
            {
                bool changed;
                do
                {
                    changed = false;
                    var iterator = this;

                    while (iterator.Next?.Next != null)
                    {
                        if (matcher(iterator.Next.Value, iterator.Next.Next.Value))
                        {
                            iterator.Next = iterator.Next.Next.Next;
                            changed = true;
                            if (iterator.Prev != null)
                                iterator = iterator.Prev;
                        }
                        else
                            iterator = iterator.Next;
                    }
                }
                while (changed);
                return this;
            }

            public LinkedList<T> Filter(Func<T, bool> match)
            {
                var result = Clone();
                var i = result;
                while (i.Next != null)
                {
                    if (match(i.Next.Value))
                        i.Next = i.Next.Next;
                    else
                        i = i.Next;
                }
                return result;
            }
        }
    }
}