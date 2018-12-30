using System;

namespace WindowsillSoft.AdventOfCode.AoC2018.Solutions.Day16
{
    public class Opcode
    {
        public string Name { get; set; }
        public Func<RegisterState, OpcodeParameterSet, RegisterState> DoWork { get; set; }
    }

}
