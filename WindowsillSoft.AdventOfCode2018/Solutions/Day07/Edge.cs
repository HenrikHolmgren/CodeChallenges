namespace WindowsillSoft.AdventOfCode2018.Solutions.Day7
{
    public struct Edge
    {
        public char From { get; set; }
        public char To { get; set; }
        public Edge(char from, char to)
            => (From, To) = (from, to);
    }
}
