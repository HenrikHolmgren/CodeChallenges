using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
    * If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9. The sum of these multiples is 23.
    * Find the sum of all the multiples of 3 or 5 below 1000.
    */
    public class Problem001 : ProjectEuler1_to_100SolverBase
    {
        public Problem001(IIOProvider provider) : base(provider) { }


        public override string Execute()
        {
            var limit = 1_000;

            var threeFactorCount = Math.Floor(((decimal)limit - 1) / 3);

            var fiveFactorCount = Math.Floor(((decimal)limit - 1) / 5);
            var fifteenFactorCount = Math.Floor(((decimal)limit - 1) / 15);

            var result = threeFactorCount * (threeFactorCount + 1) * 3 / 2
                + fiveFactorCount * (fiveFactorCount + 1) * 5 / 2
                - fifteenFactorCount * (fifteenFactorCount + 1) * 15 / 2;

            return result.ToString();
        }
    }
}
