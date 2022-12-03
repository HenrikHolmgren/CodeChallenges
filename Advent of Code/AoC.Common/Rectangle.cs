namespace AoC.Common;

public record Rectangle(int MinX, int MaxX, int MinY, int MaxY)
{
    public Rectangle(Point P1, Point P2) : this(Math.Min(P1.X, P2.X), Math.Max(P1.X, P2.X), Math.Min(P1.Y, P2.Y), Math.Max(P1.Y, P2.Y)) { }
    public bool Contains(Point p) => MinX <= p.X && p.X <= MaxX && MinY <= p.Y && p.Y <= MaxY;
}