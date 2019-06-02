using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 × 99.
     * Find the largest palindrome made from the product of two 3-digit numbers.
     */

    public class Problem004 : ProjectEuler1_to_100SolverBase
    {

        public Problem004(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            var best = 0;

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var candidate = (999 - i) * (999 - j);
                    if (candidate > best && IsPalindrome(candidate.ToString()))
                    {
                        best = candidate;
                        Console.WriteLine(candidate);
                    }
                }
            }
            return best.ToString();
        }

        public bool IsPalindrome(string str)
            => str.Reverse().SequenceEqual(str);
    }
}
