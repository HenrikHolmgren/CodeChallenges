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
     * The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
     * Find the sum of all the primes below two million.
     */
    public class Problem010 : ProjectEuler1_to_100SolverBase
    {
        public Problem010(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            return Sequences.Primes().TakeWhile(p => p < 2_000_000)
                .Sum(p => (long)p) //No Linq overload for Sum of ulongs?!
                .ToString();
        }
    }
}
