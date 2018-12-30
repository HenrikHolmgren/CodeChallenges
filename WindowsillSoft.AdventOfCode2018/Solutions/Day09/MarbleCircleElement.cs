using System;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day9
{
    public class MarbleCircleElement
    {
        public MarbleCircleElement Left { get; set; }
        public MarbleCircleElement Right { get; set; }
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
