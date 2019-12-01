using System;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day01 : AdventOfCode2019SolverBase
    {
        private int[] _inputMasses = new int[0];

        public Day01(IIOProvider provider) : base(provider) { }

        public override string Name => "Day 1: The Tyranny of the Rocket Equation";

        public override string ExecutePart1() => _inputMasses.Sum(GetFuelRequirement).ToString(); 

        public override string ExecutePart2() => _inputMasses.Sum(GetCompoundFuelRequirements).ToString();

        protected int GetCompoundFuelRequirements(int moduleMass)
        {
            var totalFuel = GetFuelRequirement(moduleMass);
            var fuelForFuel = totalFuel;
            while ((fuelForFuel = GetFuelRequirement(fuelForFuel)) > 0)
                totalFuel += fuelForFuel;
            return totalFuel;
        }

        protected int GetFuelRequirement(int mass)
            => (int)Math.Floor(mass / 3M) - 2;

        public override void Initialize(string input)
        {
            _inputMasses = ReadAndSplitInput<int>(input).ToArray();// input.Split().Select(p => Int32.Parse(p)).ToArray();
        }
    }
}
