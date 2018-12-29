using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WindowsillSoft.AdventOfCode2018.Core;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day5
{
    public class Day5Solver : IProblemSolver
    {
        public string Description => "Day 5";
        public int SortOrder => 5;

        public void Solve()
        {
            //string input = "dabAcCaCBAcCcaDA";
            string input = File.ReadAllText("Day5/Day5Input.txt");

            LinkedList<char> list = null;

            var uniqueUnits = new HashSet<char>();

            foreach (var unit in input.Reverse())
            {
                list = new LinkedList<char>(unit, list);
                uniqueUnits.Add(char.ToLower(unit));
            }
            list = new LinkedList<char>('¤', list);

            var reducedList = list.Clone().Reduce(IsMatch);

            var bestUnit = '¤';
            var bestLength = int.MaxValue;

            foreach (var unit in uniqueUnits)
            {
                var filteredList = list.Filter(p => p == unit || p == char.ToUpper(unit));

                filteredList = filteredList.Reduce(IsMatch);
                var filteredLength = filteredList.Length();
                if (filteredLength < bestLength)
                {
                    bestLength = filteredLength;
                    bestUnit = unit;
                }
                Console.WriteLine($"After removing {unit} units, it reduces to {filteredList.Length() - 1}.");
            }

            Console.WriteLine($"Initial length : {list.Length() - 1}");
            Console.WriteLine($"Reducing initial list gives: {reducedList.Length() - 1}");
            Console.WriteLine($"Removing all {bestUnit} units allows reduction to: {bestLength - 1}");
        }

        private bool IsMatch(char value1, char value2)
            => value1 != value2 &&
                (char.ToUpper(value1) == value2 || char.ToUpper(value2) == value1);
    }

    public class LinkedList<T>
    {
        public T Value { get; set; }
        public LinkedList<T> Next { get; set; }
        public LinkedList<T> Prev { get; set; }

        public LinkedList(T value) : this(value, null) { }
        public LinkedList(T value, LinkedList<T> next)
        {
            (Value, Next) = (value, next);
            if (Next != null)
                Next.Prev = this;
        }

        public IEnumerable<LinkedList<T>> Enumerate()
        {
            var i = this;
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
            LinkedList<T> generator = null;
            foreach (var node in Enumerate().Reverse())
                generator = new LinkedList<T>(node.Value, generator);
            return generator;
        }

        public LinkedList<T> Reduce(Func<T, T, bool> matcher)
        {
            bool changed;
            do
            {
                //reducedList.Print();

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
