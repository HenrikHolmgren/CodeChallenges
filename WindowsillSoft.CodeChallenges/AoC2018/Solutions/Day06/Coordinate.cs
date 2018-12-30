using System;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day6
{
    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Distance(Coordinate c2)
            => Math.Abs(X - c2.X) + Math.Abs(Y - c2.Y);

        public Coordinate(int x, int y)
            => (X, Y) = (x, y);
    }
}


