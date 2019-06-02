using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WindowsillSoft.CodeChallenges.ProjectEuler._1_100;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Numerics;

namespace WindowsillSoft.CodeChallenges.ProjectEuler
{
    /*
     * 2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.
     * What is the smallest positive number that is evenly divisible by all of the numbers from 1 to 20?
    */
    public class Problem005 : ProjectEuler1_to_100SolverBase
    {
        public Problem005(IIOProvider provider) : base(provider) { }

        public override string Execute()
        {
            Dictionary<int, int> factorCount = new Dictionary<int, int>();
            for (int i = 2; i < 20; i++)
            {
                foreach (var group in Sequences.PrimeFactors((ulong)i).Select(p => (int)p).GroupBy(p => p))
                {
                    if (!factorCount.ContainsKey(group.Key))
                        factorCount[group.Key] = 0;
                    if (group.Count() > factorCount[group.Key])
                        factorCount[group.Key] = group.Count();
                }
            }
            return factorCount.Select(p => Math.Pow(p.Key , p.Value)).Aggregate(1d, (p, q) => p * q).ToString();
        }
    }
}
