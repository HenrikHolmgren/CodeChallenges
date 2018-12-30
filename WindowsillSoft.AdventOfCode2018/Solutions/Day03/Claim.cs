namespace WindowsillSoft.AdventOfCode2018.Solutions.Day3
{
    public struct Claim
    {
        public int Id;
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public int X2 => X + Width;
        public int Y2 => Y + Height;

        public bool Contains(int x, int y)
            => x >= X && x < X + Width
            && y >= Y && y < Y + Height;

        public bool Overlaps(Claim c2) =>
            Contains(c2.X, c2.Y) ||
            Contains(c2.X2, c2.Y) ||
            Contains(c2.X2, c2.Y2) ||
            Contains(c2.X, c2.Y2);
    }
}
