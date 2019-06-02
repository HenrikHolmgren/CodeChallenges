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
     * By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.
     * What is the 10 001st prime number?
    */
    public class Problem007 : ProjectEuler1_to_100SolverBase
    {
        public Problem007(IIOProvider provider) : base(provider) { }


        public override string Execute()
        {
            return Sequences.Primes().Skip(10_000).First().ToString();
        }
    }
}
