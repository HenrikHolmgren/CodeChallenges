using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * n! means n × (n − 1) × ... × 3 × 2 × 1
     * 
     * For example, 10! = 10 × 9 × ... × 3 × 2 × 1 = 3628800,
     * and the sum of the digits in the number 10! is 3 + 6 + 2 + 8 + 8 + 0 + 0 = 27.
     * 
     * Find the sum of the digits in the number 100!
     */

    public class Problem020 : ProjectEuler1_to_100SolverBase
    {
        public Problem020(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            var b = BigInteger.One;
            for (int i = 1; i <= 100; i++)
                b *= i;

            return b.ToString().Sum(p => p - '0').ToString();
        }
    }
}
