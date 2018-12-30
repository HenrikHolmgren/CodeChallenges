using System;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day20
{
    [Flags]
    public enum Direction : byte
    {
        None = 0b0000,
        North = 0b0001,
        South = 0b0010,
        East = 0b0100,
        West = 0b1000,
    }
}
