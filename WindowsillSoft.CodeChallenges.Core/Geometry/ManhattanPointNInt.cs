using System;
using System.Linq;

namespace WindowsillSoft.CodeChallenges.Core.Geometry
{
    public struct ManhattanPointNInt
    {
        private readonly int[] _coordinates;

        public int Length
            => _coordinates.Sum(p => Math.Abs(p));

        public ManhattanPointNInt(int[] coordinates)
            => _coordinates = coordinates;

        public int Distance(ManhattanPointNInt other)
            => _coordinates.Zip(other._coordinates, (p, q) => Math.Abs(p - q)).Sum();

        public int DistanceToCube(ManhattanPointNInt p1, ManhattanPointNInt p2)
        {
            var d = 0;
            var maxMin = p1._coordinates
                .Zip(p2._coordinates, (p, q) => new
                {
                    Max = Math.Max(p, q),
                    Min = Math.Min(p, q)
                }).ToArray();

            for (var i = 0; i < _coordinates.Length; i++)
            {
                if (_coordinates[i] > maxMin[i].Max)
                    d += _coordinates[i] - maxMin[i].Max;
                if (_coordinates[i] < maxMin[i].Min)
                    d += _coordinates[i] - maxMin[i].Min;
            }
            return d;
        }

        public ManhattanPointNInt Zip(ManhattanPointNInt point, Func<int, int, int> selector)
            => new ManhattanPointNInt(_coordinates.Zip(point._coordinates, selector).ToArray());

        public override bool Equals(object obj)
        {
            if (obj is ManhattanPointNInt manhattanPoint)
                return Equals(manhattanPoint);
            return false;
        }

        public bool Equals(ManhattanPointNInt other)
            => this == other;

        public override int GetHashCode()
        {
            var prime = 17;
            return _coordinates.Aggregate(prime, (p, q) => p * prime + q);
        }

        public int this[int i]
            => _coordinates[i];

        public static ManhattanPointNInt operator +(ManhattanPointNInt a, ManhattanPointNInt b)
            => new ManhattanPointNInt(a._coordinates.Zip(b._coordinates, (p, q) => p + q).ToArray());

        public static ManhattanPointNInt operator -(ManhattanPointNInt a, ManhattanPointNInt b)
            => new ManhattanPointNInt(a._coordinates.Zip(b._coordinates, (p, q) => p - q).ToArray());

        public static bool operator ==(ManhattanPointNInt a, ManhattanPointNInt b)
        {
            for (var i = 0; i < a._coordinates.Length; i++)
            {
                if (a._coordinates[i] != b._coordinates[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(ManhattanPointNInt a, ManhattanPointNInt b)
            => !(a == b);

        public static bool operator <(ManhattanPointNInt a, ManhattanPointNInt b)
        {
            for (var i = 0; i < a._coordinates.Length; i++)
            {
                if (a._coordinates[i] < b._coordinates[i])
                    return true;
                if (a._coordinates[i] > b._coordinates[i])
                    return false;
            }
            return false;
        }

        public static bool operator >(ManhattanPointNInt a, ManhattanPointNInt b)
            => b < a;
        public static bool operator <=(ManhattanPointNInt a, ManhattanPointNInt b)
            => a == b || a < b;
        public static bool operator >=(ManhattanPointNInt a, ManhattanPointNInt b)
            => b <= a;

        public override string ToString()
            => $"<{String.Join(", ", _coordinates)}>";

    }
}
