using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Numerics;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * The prime factors of 13195 are 5, 7, 13 and 29.
     * What is the largest prime factor of the number 600851475143 ?
    */
    class Problem003 : ProjectEuler1_to_100SolverBase
    {

        public Problem003(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            //var factors = Sequences.PrimeFactors(2).ToList();

            //foreach (var factor in factors)
            //    Console.WriteLine(factor);

            return Sequences.PrimeFactors(600_851_475_143).Last().ToString();
        }
    }
}
