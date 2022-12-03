namespace AoC.Common;

public record LineSegment(Point From, Point To)
{
    public bool IsAxisAligned => From.X == To.X || From.Y == To.Y;
};
