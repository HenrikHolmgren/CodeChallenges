namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day10
{
    public struct Star
    {
        public int X;
        public int Y;
        public int Vx;
        public int Vy;

        public override bool Equals(object obj)
        {
            return (obj is Star star
                && star.X == X
                && star.Y == Y);
        }

        public override int GetHashCode()
        {
            return X ^ 17 * Y ^ 19;
        }
    }
}
