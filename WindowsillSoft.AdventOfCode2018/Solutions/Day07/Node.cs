using System.Collections.Generic;

namespace WindowsillSoft.AdventOfCode2018.Solutions.Day7
{
    public class Node
    {
        public List<Node> Parents { get; } = new List<Node>();
        public List<Node> Children { get; } = new List<Node>();
        public char Value { get; set; }
    }
}
