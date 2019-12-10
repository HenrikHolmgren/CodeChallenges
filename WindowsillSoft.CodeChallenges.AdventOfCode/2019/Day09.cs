using System.Linq;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day09 : AdventOfCode2019SolverBase
    {
        private long[] _input = new long[0];

        public override string Name => "Day 9: Sensor Boost";
        public Day09(IIOProvider provider) : base(provider) { }

        public override string ExecutePart1()
        {
            var program = _input.ToArray();
            
            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(1);
            return machine.LastOutput?.ToString()??"";
        }

        public override string ExecutePart2()
        {
            var program = _input.ToArray();

            var machine = new IntCodeMachine(program);
            machine.Run();
            machine.ProvideInputAndContinue(2);
            return machine.LastOutput?.ToString() ?? "";
        }

        public override void Initialize(string input) => _input = ReadAndSplitInput<long>(input, ',').ToArray();
    }
}
