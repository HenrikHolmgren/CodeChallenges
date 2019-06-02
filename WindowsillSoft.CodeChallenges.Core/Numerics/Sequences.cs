using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsillSoft.CodeChallenges.Core.Numerics
{
    public static class Sequences
    {
        public static IEnumerable<long> Fibonacci()
        {
            long a = 1;
            long b = 2;
            while (true)
            {
                yield return a;
                a += b;
                yield return b;
                b += a;
            }
        }

        public static IEnumerable<ulong> PrimeFactors(ulong composite)
        {
            var candiates = Primes();
            while (true)
            {
                var test = candiates.First();

                if (test * test > composite)
                {
                    yield return composite;
                    yield break;
                }

                while (composite % test == 0)
                {
                    composite /= test;
                    yield return test;
                }

                candiates = candiates.Skip(1);
            }
        }
        public static ulong[] ProperFactors(ulong composite)
        {
            var primeFactors = PrimeFactors(composite).ToArray();
            return SubSets(primeFactors).Select(p => p.Aggregate((ulong)1, (q, r) => q * r))
                .Distinct()
                .Where(p => p != composite)
                .ToArray();
        }

        public static IEnumerable<IEnumerable<T>> SubSets<T>(IEnumerable<T> sequence)
        {
            if (!sequence.Any()) yield return Enumerable.Empty<T>();
            else
            {
                var head = sequence.First();
                foreach (var sub in SubSets(sequence.Skip(1)))
                {
                    yield return new[] { head }.Concat(sub);
                    yield return sub;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> sequence)
        {
            if (!sequence.Any())
                return Enumerable.Empty<IEnumerable<T>>();
            else if (!sequence.Skip(1).Any())
                return new[] { sequence };
            else
                return sequence.SelectMany(
                    (v, i) => Permutations(sequence.Take(i).Concat(sequence.Skip(i + 1))),
                    (v, p) => p.Prepend(v));
        }

        public static IEnumerable<ulong> Primes()
        {
            List<ulong> VisitedPrimes = new List<ulong>() { 2, 3, 5 };
            yield return 2;
            yield return 3;
            yield return 5;

            ulong candidate = 7;
            while (true)
            {
                if (!VisitedPrimes
                    .TakeWhile(p => p * p <= candidate)
                    .Any(p => candidate % p == 0))
                {
                    VisitedPrimes.Add(candidate);
                    yield return candidate;
                }
                candidate += 2;
            }
        }

        public static IEnumerable<ulong> Collatz(ulong n)
        {
            while (n != 1)
            {
                yield return n;
                if ((n & 1) == 0) n = n >> 1;
                else n = 3 * n + 1;
            }
            yield return 1;
        }
    }
}
