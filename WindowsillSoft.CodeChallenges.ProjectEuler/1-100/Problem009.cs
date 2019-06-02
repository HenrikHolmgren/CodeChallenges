using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * A Pythagorean triplet is a set of three natural numbers, a < b < c, for which,
     * a^2 + b^2 = c^2
     * For example, 3^2 + 4^2 = 9 + 16 = 25 = 52.
     * 
     * There exists exactly one Pythagorean triplet for which a + b + c = 1000.
     * Find the product abc.
     */

    public class Problem009 : ProjectEuler1_to_100SolverBase
    {
        public Problem009(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            for (int a = 1; a < 998; a++)
                for (int b = 1; b < 1000 - a; b++)
                {
                    var c = 1000 - (a + b);
                    if (a * a + b * b == c * c)
                        return (a * b * c).ToString();

                }
            return "Wtf?!";
        }
    }
}
