using System.Linq;
using WindowsillSoft.CodeChallenges.AdventOfCode._2019.Common;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day05 : AdventOfCode2019SolverBase
    {
        int[] _program = new int[] { };

        public Day05(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 5: Sunny with a Chance of Asteroids";

        public override string ExecutePart1()
        {
            var machine = new IntCodeMachine()
            {
                RequestInput = () => 1
            };

            var output = machine.Run(_program.ToArray());
            
            return output.Last().ToString();
        }
        public override string ExecutePart2()
        {
            var machine = new IntCodeMachine()
            {
                RequestInput = () => 5
            };

            var output = machine.Run(_program.ToArray());
            
            return output.Single().ToString();
        }

        public override void Initialize(string input) => _program = ReadAndSplitInput<int>(input, ',').ToArray();
    }

}
