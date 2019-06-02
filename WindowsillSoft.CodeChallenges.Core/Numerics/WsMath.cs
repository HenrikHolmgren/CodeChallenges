using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsillSoft.CodeChallenges.Core.Numerics
{
    public static class WsMath
    {
        /// <summary>
        /// Performs a modular exponentiation and returns (x^y) % mod
        /// </summary>

        /// <returns></returns>
        public static ulong ModPow(ulong x, ulong y, ulong mod)
        {
            if (mod == 1) return 0;
            if (x == 0 && y == 0) return 1;
            if (x == 0) return 0;

            ulong res = 1L;
            while (y > 0)
            {
                //If remaining exponent is odd, multiply
                if ((y & 1) == 1)
                    res = (res * x) % mod;

                //If y was odd, the above check should have subtracted 1
                //No need for the subtraction if we assume the result would have been even and div 2.
                //If y == 1, the squaring will not affect the result and we will fall out of the while loop immediately.               
                y = y >> 1;
                x = (x * x) % mod;
            }
            return res;
        }

        public static bool IsAmicable(int number)
        {
            var buddy = Sequences.ProperFactors((ulong)number).Sum(q => (int)q);
            return buddy != number
                && Sequences.ProperFactors((ulong)buddy).Sum(q => (int)q) == number;
        }

        public static ulong Choose(ulong n, ulong k)
        {
            if (k > n) throw new ArgumentException("Cannot choose k>n elements from pool of n");
            if (k == 0) return 1;
            if (k > n / 2) return Choose(n, n - k); //symmetry allows us to switch to the smaller 'branch'.
            return n * Choose(n - 1, k - 1) / k;
        }

        public static bool IsPrime(ulong candidate)
            => Sequences.Primes().First(p => p >= candidate) == candidate;

        public static ulong Factorial(ulong n)
        {
            ulong res = 1;
            for (ulong m = 2; m < n; m++)
                res *= m;
            return res;
        }

        public static ulong TriangleNumber(ulong number)
            => number * (number + 1) / 2;
    }
}
