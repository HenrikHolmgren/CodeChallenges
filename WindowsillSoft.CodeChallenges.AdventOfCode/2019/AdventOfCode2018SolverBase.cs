using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    [SolverCategory("2019")]
    public abstract class AdventOfCode2019SolverBase : AdventOfCodeSolverBase
    {
        public AdventOfCode2019SolverBase(IIOProvider provider) : base(provider) { }
    }

    public class Day01 : AdventOfCode2019SolverBase
    {
        private int[] _inputMasses = new int[0];

        public Day01(IIOProvider provider) : base(provider) { }

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
            _inputMasses = input.Split().Select(p => Int32.Parse(p)).ToArray();
        }
    }

}
