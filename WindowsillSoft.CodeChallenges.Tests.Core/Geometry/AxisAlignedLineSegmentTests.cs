using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WindowsillSoft.CodeChallenges.Core.Geometry;

namespace WindowsillSoft.CodeChallenges.Core.Tests.Geometry;
public class AxisAlignedLineSegmentTests
{
    [Test,
        TestCase([ 1 ], [ 1, 2 ]),
        TestCase([ 1, 3, 2 ], [ 1 ])]
    public void Constructor__Points_must_have_same_dimensionality_throws(int[] p1, int[] p2)
    {
        Assert.That(() => new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2)),
            Throws.InvalidOperationException);
    }

    [Test,
        TestCase([ 1 ], [ 2 ]),
        TestCase([ 1, 3, 2 ], [ 1, 3, 3 ])]
    public void Constructor__Points_must_have_same_dimensionality(int[] p1, int[] p2)
    {
        Assert.That(() => new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2)),
            Throws.Nothing);
    }

    [Test,
        TestCase([ 1, 1, 1 ], [ 1, 2, 2 ]),
        TestCase([ 1, 1, 1 ], [ 2, 1, 2 ]),
        TestCase([ 1, 1, 1 ], [ 2, 2, 1 ]),
        TestCase([ 1, 1, 1 ], [ 2, 2, 2 ])]
    public void Constructor__Points_must_be_axis_aligned_throws(int[] p1, int[] p2)
    {
        Assert.That(() => new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2)),
            Throws.InvalidOperationException);
    }

    [Test,
        TestCase([ 1, 1, 1 ], [ 1, 1, 1 ]),
        TestCase([ 2, 1 ], [ 2, 1 ])]
    public void Constructor__Line_must_have_length(int[] p1, int[] p2)
    {
        Assert.That(() => new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2)),
            Throws.InvalidOperationException);
    }

    [Test,
        TestCase([ 1, 1, 1 ], [ 1, 1, 2 ], [ 1, 1, 1 ], [ 1, 1, 2 ], true),
        TestCase([ 1, 1, 1 ], [ 1, 1, 2 ], [ 1, 1, 1 ], [ 1, 2, 1 ], false),
        TestCase([ 1, 1 ], [ 1, 2 ], [ 3, 4 ], [ 3, 5 ], true),
        TestCase([ 1, 1 ], [ 1, 2 ], [ 3, 4 ], [ 4, 4 ], false),]
    public void ParallelTo(int[] p1, int[] p2, int[] p3, int[] p4, bool expected)
    {
        var segment1 = new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2));
        var segment2 = new AxisAlignedLineSegment(new ManhattanPointNInt(p3), new ManhattanPointNInt(p4));
        Assert.That(segment1.IsParallelTo(segment2), Is.EqualTo(expected));            
    }

    [Test,
        TestCase([ 1 ], [ 2 ], [ 2 ], [ 3 ]),
        TestCase([ 1, 1 ], [ 1, 2 ], [ 1, 2 ], [ 1, 3 ]),
        TestCase([ 1, 1, 1 ], [ 1, 1, 2 ], [ 1, 1, 2 ], [ 1, 1, 3 ])]
    public void GetUniqueIntersectionIfAny__Parallel__Allow_single_point_intersection(int[] p1, int[] p2, int[] p3, int[] p4)
    {
        var segment1 = new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2));
        var segment2 = new AxisAlignedLineSegment(new ManhattanPointNInt(p3), new ManhattanPointNInt(p4));
        Assert.That(()=>segment1.GetUniqueIntersectionIfAny(segment2), Is.Not.Null);
    }

    [Test,
    TestCase([ 1 ], [ 3 ], [ 2 ], [ 4 ]),
    TestCase([ 1, 1 ], [ 1, 3 ], [ 1, 2 ], [ 1, 4 ]),
    TestCase([ 1, 1, 1 ], [ 1, 1, 3 ], [ 1, 1, 2 ], [ 1, 1, 4 ]),
    TestCase([ 3 ], [ 1 ], [ 2 ], [ 4 ]),
    TestCase([ 1, 3 ], [ 1, 1 ], [ 1, 2 ], [ 1, 4 ]),
    TestCase([ 1, 1, 3 ], [ 1, 1, 1 ], [ 1, 1, 2 ], [ 1, 1, 4 ])]
    public void GetUniqueIntersectionIfAny__Parallel__Disallow_multiple_intersections(int[] p1, int[] p2, int[] p3, int[] p4)
    {
        var segment1 = new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2));
        var segment2 = new AxisAlignedLineSegment(new ManhattanPointNInt(p3), new ManhattanPointNInt(p4));
        Assert.That(() => segment1.GetUniqueIntersectionIfAny(segment2), Throws.InvalidOperationException);
    }

    [Test,
        TestCase([ 1 ], [ 2 ], [ 3 ], [ 4 ]),
        TestCase([ 1, 1 ], [ 1, 2 ], [ 1, 3 ], [ 1, 4 ]),
        TestCase([ 1, 1, 1 ], [ 1, 1, 2 ], [ 1, 1, 3 ], [ 1, 1, 4 ]),
        TestCase([ 2 ], [ 1 ], [ 3 ], [ 4 ]),
        TestCase([ 1, 2 ], [ 1, 1 ], [ 1, 3 ], [ 1, 4 ]),
        TestCase([ 1, 1, 2 ], [ 1, 1, 1 ], [ 1, 1, 3 ], [ 1, 1, 4 ])]
    public void GetUniqueIntersectionIfAny__Parallel__Returns_Null_For_No_Intersections(int[] p1, int[] p2, int[] p3, int[] p4)
    {
        var segment1 = new AxisAlignedLineSegment(new ManhattanPointNInt(p1), new ManhattanPointNInt(p2));
        var segment2 = new AxisAlignedLineSegment(new ManhattanPointNInt(p3), new ManhattanPointNInt(p4));
        Assert.That(() => segment1.GetUniqueIntersectionIfAny(segment2), Is.Null);
    }
}