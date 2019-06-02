using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * The sum of the squares of the first ten natural numbers is,
     * 1^2 + 2^2 + ... + 10^2 = 385
     * The square of the sum of the first ten natural numbers is,
     * (1 + 2 + ... + 10)^2 = 552 = 3025
     * Hence the difference between the sum of the squares of the first ten natural numbers and the square of the sum is 3025 − 385 = 2640.
     * 
     * Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.
     * */
    public class Problem006 : ProjectEuler1_to_100SolverBase
    {
        public Problem006(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            return (Math.Pow(Enumerable.Range(1, 100).Sum(), 2) -
                Enumerable.Range(1, 100).Select(p => p * p).Sum())
                .ToString();
        }
    }
}
