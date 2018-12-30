using System;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day23
{
    public class Drone
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int R { get; }

        public Drone(int x, int y, int z, int r)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
        }

        public int Distance(Drone other)
            => Distance((other.X, other.Y, other.Z));

        public override string ToString()
            => $"<{X},{Y},{Z}>,R{R}";

        public int Distance((int x, int y, int z) other)
            => Math.Abs(other.x - X)
                + Math.Abs(other.y - Y)
                + Math.Abs(other.z - Z);

        internal bool OverlapsCube(int x, int y, int z, int size)
        {
            var d = 0;
            if (X > x + size) d += X - (x + size);
            if (X < x) d += x - X;
            if (Y > y + size) d += Y - (y + size);
            if (Y < y) d += y - Y;
            if (Z > z + size) d += Z - (z + size);
            if (Z < z) d += z - Z;
            return d <= R;
        }
    }
}
