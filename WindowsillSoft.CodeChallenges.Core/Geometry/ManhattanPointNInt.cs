using System;
using System.Linq;

namespace WindowsillSoft.CodeChallenges.Core.Geometry
{
    public struct ManhattanPointNInt
    {
        private readonly int[] _coordinates;

        public int Length
            => _coordinates.Sum(p => Math.Abs(p));
        public int Dimensions
            => _coordinates.Length;

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

        public override bool Equals(object? obj)
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

    public class AxisAlignedLineSegment
    {
        public ManhattanPointNInt P1 { get; }
        public ManhattanPointNInt P2 { get; }

        protected int AlignedAxis;

        public AxisAlignedLineSegment(ManhattanPointNInt p1, ManhattanPointNInt p2)
        {
            if (p1.Dimensions != p2.Dimensions)
                throw new InvalidOperationException("Cannot draw lines between points of different dimensionality.");

            if (p1 == p2)
                throw new InvalidOperationException("A segment cannot have line 0 and still have an alignment.");
            if (Enumerable.Range(0, p1.Dimensions)
                .Select(p => p1[p] != p2[p])
                .Count(p => p == true) != 1)
                throw new InvalidOperationException("The points are not axis-aligned. Please use a more generic line segment.");

            P1 = p1;
            P2 = p2;
            AlignedAxis = Enumerable.Range(0, p1.Dimensions).Single(p => p1[p] != p2[p]);
        }

        public bool IsParallelTo(AxisAlignedLineSegment other)
            => AlignedAxis == other.AlignedAxis;
        public int Length
            => (P2 - P1).Length;

        public ManhattanPointNInt? GetUniqueIntersectionIfAny(AxisAlignedLineSegment other)
        {
            if (Enumerable.Range(0, P1.Dimensions).Count(p => P1[p] != other.P1[p]) > 2) return null;
            if (IsParallelTo(other))
            {
                //allowed degenerated case
                if (P1 == other.P1 && P2 != other.P2) return P1;
                if (P1 == other.P2 && P2 != other.P1) return P1;
                if (P2 == other.P2 && P1 != other.P1) return P2;
                if (P2 == other.P1 && P1 != other.P2) return P2;

                var intervalA = P1 < P2 ? new[] { P1[AlignedAxis], P2[AlignedAxis] } : new[] { P2[AlignedAxis], P1[AlignedAxis] };
                var intervalB = other.P1 < other.P2 ? new[] { other.P1[AlignedAxis], other.P2[AlignedAxis] } : new[] { other.P2[AlignedAxis], other.P1[AlignedAxis] };

                if((intervalA[0] < intervalB[0] && intervalB[0] < intervalA[1])
                    || (intervalA[0] > intervalB[0] && intervalA[0] < intervalB[1]))
                    throw new InvalidOperationException("Parallel line segments have multiple intersections");
               
                return null;
            }

            throw new NotImplementedException("Unfinished");
            //return null;
        }
    }
}
