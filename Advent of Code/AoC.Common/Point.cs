using System.Numerics;

namespace AoC.Common;

/// <summary>
/// Generic 2D point with arithmetic operations using generic math.
/// Works with any numeric type (int, long, double, etc.)
/// </summary>
public record Point<T>(T X, T Y) where T : INumber<T>
{
    /// <summary>
    /// Zero point (origin)
    /// </summary>
    public static Point<T> Zero => new(T.Zero, T.Zero);

    /// <summary>
    /// Unit point (1, 1)
    /// </summary>
    public static Point<T> One => new(T.One, T.One);

    /// <summary>
    /// Cardinal directions
    /// </summary>
    public static Point<T> Up => new(T.Zero, -T.One);
    public static Point<T> Down => new(T.Zero, T.One);
    public static Point<T> Left => new(-T.One, T.Zero);
    public static Point<T> Right => new(T.One, T.Zero);

    /// <summary>
    /// Add two points
    /// </summary>
    public static Point<T> operator +(Point<T> a, Point<T> b)
        => new(a.X + b.X, a.Y + b.Y);

    /// <summary>
    /// Subtract two points
    /// </summary>
    public static Point<T> operator -(Point<T> a, Point<T> b)
        => new(a.X - b.X, a.Y - b.Y);

    /// <summary>
    /// Negate a point
    /// </summary>
    public static Point<T> operator -(Point<T> p)
        => new(-p.X, -p.Y);

    /// <summary>
    /// Multiply point by scalar
    /// </summary>
    public static Point<T> operator *(Point<T> p, T scalar)
        => new(p.X * scalar, p.Y * scalar);

    /// <summary>
    /// Multiply scalar by point
    /// </summary>
    public static Point<T> operator *(T scalar, Point<T> p)
        => new(p.X * scalar, p.Y * scalar);

    /// <summary>
    /// Divide point by scalar
    /// </summary>
    public static Point<T> operator /(Point<T> p, T scalar)
        => new(p.X / scalar, p.Y / scalar);

    /// <summary>
    /// Manhattan distance to another point
    /// </summary>
    public T ManhattanDistance(Point<T> other)
        => T.Abs(X - other.X) + T.Abs(Y - other.Y);

    /// <summary>
    /// Manhattan distance from origin
    /// </summary>
    public T ManhattanLength => T.Abs(X) + T.Abs(Y);

    /// <summary>
    /// Chebyshev distance (max of absolute differences)
    /// </summary>
    public T ChebyshevDistance(Point<T> other)
        => T.Max(T.Abs(X - other.X), T.Abs(Y - other.Y));

    public override string ToString() => $"({X},{Y})";
}

/// <summary>
/// Type alias for the most common case: integer points
/// </summary>
public record Point(int X, int Y) : Point<int>(X, Y);