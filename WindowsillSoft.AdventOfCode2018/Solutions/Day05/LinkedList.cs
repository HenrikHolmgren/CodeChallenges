using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day5
{
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
