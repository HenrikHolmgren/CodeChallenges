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
     * 2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.
     * 
     * What is the sum of the digits of the number 2^1000
     */

    public class Problem016 : ProjectEuler1_to_100SolverBase
    {
        public Problem016(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            BigInteger b = BigInteger.One;
            b <<= 1000;

            return b.ToString().Select(p => p - '0').Sum().ToString();
        }
    }
}
