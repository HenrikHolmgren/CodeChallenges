using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace WindowsillSoft.CodeChallenges.Core.Numerics;

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

    // ========================================
    // Generic Math Functions (C# 11+)
    // ========================================

    /// <summary>
    /// Generic absolute value function
    /// </summary>
    public static T Abs<T>(T value) where T : INumber<T>
        => T.Abs(value);

    /// <summary>
    /// Generic sign function (-1, 0, or 1)
    /// </summary>
    public static int Sign<T>(T value) where T : INumber<T>
        => T.Sign(value);

    /// <summary>
    /// Generic min function
    /// </summary>
    public static T Min<T>(T a, T b) where T : INumber<T>
        => a < b ? a : b;

    /// <summary>
    /// Generic max function
    /// </summary>
    public static T Max<T>(T a, T b) where T : INumber<T>
        => a > b ? a : b;

    /// <summary>
    /// Generic clamp function
    /// </summary>
    public static T Clamp<T>(T value, T min, T max) where T : INumber<T>
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>
    /// Generic triangle number calculation: n * (n + 1) / 2
    /// </summary>
    public static T TriangleNumber<T>(T n) where T : INumber<T>
    {
        T two = T.One + T.One;
        return n * (n + T.One) / two;
    }

    /// <summary>
    /// Generic factorial function
    /// </summary>
    public static T Factorial<T>(T n) where T : INumber<T>
    {
        T result = T.One;
        for (T i = T.One + T.One; i <= n; i += T.One)
            result *= i;
        return result;
    }

    /// <summary>
    /// Generic greatest common divisor (GCD) using Euclid's algorithm
    /// </summary>
    public static T Gcd<T>(T a, T b) where T : INumber<T>
    {
        a = T.Abs(a);
        b = T.Abs(b);

        while (b != T.Zero)
        {
            T temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    /// <summary>
    /// Generic least common multiple (LCM)
    /// </summary>
    public static T Lcm<T>(T a, T b) where T : INumber<T>
        => T.Abs(a * b) / Gcd(a, b);

    /// <summary>
    /// Generic power function
    /// </summary>
    public static T Pow<T>(T baseValue, int exponent) where T : INumber<T>
    {
        if (exponent < 0)
            throw new ArgumentException("Exponent must be non-negative for integer types");

        T result = T.One;
        T currentPower = baseValue;

        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
                result *= currentPower;

            currentPower *= currentPower;
            exponent >>= 1;
        }

        return result;
    }
}
