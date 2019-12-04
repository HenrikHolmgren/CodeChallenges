using System;
using System.Collections.Generic;
using System.Linq;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Core.Geometry;

namespace WindowsillSoft.CodeChallenges.AdventOfCode._2019
{
    public class Day03 : AdventOfCode2019SolverBase
    {
        private AxisAlignedWireSegment[][] _wires = new AxisAlignedWireSegment[0][];

        public override string Name => "Day 3: Crossed Wires";

        public Day03(IIOProvider provider) : base(provider) { }

        public override string ExecutePart1()
        {
            var minLength = new ManhattanPointNInt(new[] { 100_000, 100_000 });
            var intersections = new List<ManhattanPointNInt>();
            foreach (var segment0 in _wires[0])
                foreach (var segment1 in _wires[1])
                {
                    var intersection0 = segment0.GetPossibleIntersection(segment1);
                    if (intersection0 != null)
                        intersections.Add(intersection0.Value);

                    var intersection1 = segment1.GetPossibleIntersection(segment0);
                    if (intersection1 != null)
                        intersections.Add(intersection1.Value);
                }
            return intersections.Min(p => p.Length).ToString();
        }

        public override string ExecutePart2()
        {
            int totalLengthIn0 = 0;
            int totalLengthIn1 = 0;
            
            var intersectionsWithDistance = new Dictionary<ManhattanPointNInt, int>();

            foreach (var segment0 in _wires[0])
            {
                totalLengthIn1 = 0;
                foreach (var segment1 in _wires[1])
                {
                    HandleIntersections(totalLengthIn0, totalLengthIn1, intersectionsWithDistance, segment0, segment1);
                    HandleIntersections(totalLengthIn0, totalLengthIn1, intersectionsWithDistance, segment1, segment0);

                    totalLengthIn1 += segment1.Length;
                }
                totalLengthIn0 += segment0.Length;
            }
            //return minLength.Length.ToString();
            return intersectionsWithDistance.Values.Min().ToString();
        }

        private static void HandleIntersections(int totalLengthIn0, int totalLengthIn1, Dictionary<ManhattanPointNInt, int> intersectionsWithDistance, AxisAlignedWireSegment segment0, AxisAlignedWireSegment segment1)
        {
            var intersection0 = segment0.GetPossibleIntersection(segment1);
            if (intersection0 != null)
            {
                var distanceToIntersection = (segment0.Origin - intersection0.Value).Length +
                    (segment1.Origin - intersection0.Value).Length +
                    totalLengthIn0 + totalLengthIn1;

                if (intersectionsWithDistance.ContainsKey(intersection0.Value))
                {
                    intersectionsWithDistance[intersection0.Value] = Math.Min(intersectionsWithDistance[intersection0.Value], distanceToIntersection);
                }
                intersectionsWithDistance[intersection0.Value] = distanceToIntersection;
            }
        }

        public override void Initialize(string input)
        {
            var wires = ReadAndSplitInput<string>(input)
                .Select(p => p.Split(',').Select(q => new WireSegment(q[0], Int32.Parse(q.Substring(1)))).ToArray()).ToArray();
            _wires = wires.Select(BuildAxisAlignedSegments).Select(p => p.ToArray()).ToArray();
        }

        private IEnumerable<AxisAlignedWireSegment> BuildAxisAlignedSegments(WireSegment[] arg1)
        {
            var position = new ManhattanPointNInt(new[] { 0, 0 });
            foreach (var segment in arg1)
            {
                var nextPosition = segment.Direction switch
                {
                    'L' => position + new ManhattanPointNInt(new[] { -segment.Length, 0 }),
                    'R' => position + new ManhattanPointNInt(new[] { segment.Length, 0 }),
                    'U' => position + new ManhattanPointNInt(new[] { 0, segment.Length }),
                    'D' => position + new ManhattanPointNInt(new[] { 0, -segment.Length }),
                    _ => throw new InvalidOperationException($"Unknown direction: {segment.Direction}")
                };
                yield return new AxisAlignedWireSegment(position, nextPosition);
                position = nextPosition;
            }
        }

        private class WireSegment
        {
            public char Direction { get; }
            public int Length { get; }

            public WireSegment(char direction, int length)
            {
                this.Direction = direction;
                this.Length = length;
            }
        }

        private class AxisAlignedWireSegment
        {
            public ManhattanPointNInt Origin { get; }
            public ManhattanPointNInt Target { get; }
            public int Length => (Target - Origin).Length;
            public bool IsVertical => Target[0] == Origin[0];

            public AxisAlignedWireSegment(ManhattanPointNInt origin, ManhattanPointNInt target)
            {
                Origin = origin;
                Target = target;
            }

            public ManhattanPointNInt? GetPossibleIntersection(AxisAlignedWireSegment other)
            {
                if (IsVertical == other.IsVertical)
                {
                    return null;
                }
                var source1 = Origin < Target ? Origin : Target;
                var source2 = other.Origin < other.Target ? other.Origin : other.Target;

                if (source1[0] >= source2[0] && source1[0] <= source2[0] + other.Length &&
                    source2[1] >= source1[1] && source2[1] <= source1[1] + Length &&
                    !(source1[0] == 0 && source1[1] == 0))
                {
                    return new ManhattanPointNInt(new[] { source1[0], source2[1] });
                }
                return null;
            }

            private ManhattanPointNInt? GetPossibleIntersectionInternal(AxisAlignedWireSegment other, int testCoordinate)
            {
                if (Origin[testCoordinate] <= other.Origin[testCoordinate] && Origin[testCoordinate] + Length >= other.Origin[testCoordinate])
                {
                    //Edge case, two similarly aligned 
                    if (Origin[testCoordinate] == other.Origin[testCoordinate] + other.Length
                        || Origin[testCoordinate] + Length == other.Origin[testCoordinate])
                        return Origin;

                    throw new InvalidOperationException("Two segments are overlapping in the same direction. Infinite intersections ensue.");
                }
                return null;
            }
        }
    }
}
