namespace AoC.Common;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> items, int size)
    {
        Queue<T> q = new(size);
        foreach (var item in items)
        {
            q.Enqueue(item);
            if (q.Count == size)
            {
                yield return [..q];
                q.Dequeue();
            }
        }
    }
}

public static class SAK
{
    public static int[,] LoadMap(string rawInput)
    {
        var source = rawInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var map = new int[source[0].Length, source.Length];

        for (int y = 0; y < source.Length; y++)
            for (int x = 0; x < source[0].Length; x++)
                map[x, y] = source[y][x] - '0';

        return map;
    }

    public static void DrawMap(int[,] map, int align = 1, (Point topLeft, Point bottomRight) window = default)
    {
        if(window == default)
            window = new(new(0,0), new(map.GetLength(0), map.GetLength(1)));

        System.Console.WriteLine("     " + String.Join(" ", Enumerable.Range(window.topLeft.X, window.bottomRight.X - window.topLeft.X).Select(p => p.ToString().PadLeft(align))));
        System.Console.WriteLine("     " + String.Join(" ", Enumerable.Range(window.topLeft.X, window.bottomRight.X - window.topLeft.X).Select(p => new String('-', align))));
        for (int y = window.topLeft.Y; y < window.bottomRight.Y; y++)
        {
            string line = y.ToString().PadLeft(align) + " | ";
            for (int x = window.topLeft.X; x < window.bottomRight.X; x++)
            {
                line += map[x, y].ToString().PadLeft(align) + " ";
            }
            Console.WriteLine(line);
        }
    }

    public static IEnumerable<Point> VonNeumannNeighbourhood(Point location, int maxX, int maxY, bool wrapAround = false)
    {
        // Left
        if (location.X > 0)
            yield return location with { X = location.X - 1 };
        else if (wrapAround)
            yield return location with { X = maxX };

        // Up
        if (location.Y > 0)
            yield return location with { Y = location.Y - 1 };
        else if (wrapAround)
            yield return location with { Y = maxY };

        // Right
        if (location.X < maxX)
            yield return location with { X = location.X + 1 };
        else if (wrapAround)
            yield return location with { X = 0 };

        // Down
        if (location.Y < maxY)
            yield return location with { Y = location.Y + 1 };
        else if (wrapAround)
            yield return location with { Y = 0 };
    }

    public static IEnumerable<Point> MooreNeighbourhood(Point location, int maxX, int maxY)
    {
        for (int x = location.X - 1; x <= location.X + 1; x++)
        {
            for (int y = location.Y - 1; y <= location.Y + 1; y++)
            {
                Point probe = new(x, y);
                if (x >= 0 && y >= 0 && x <= maxX && y <= maxY && probe != location)
                    yield return probe;
            }
        }
    }

    public static IEnumerable<Point> Enumerate(int[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                yield return new(x, y);
    }

    /// <summary>
    /// Get all Von Neumann neighbors (4-directional) for a generic point
    /// </summary>
    public static IEnumerable<Point<T>> VonNeumannNeighbourhood<T>(Point<T> location) where T : INumber<T>
    {
        yield return location + Point<T>.Left;
        yield return location + Point<T>.Up;
        yield return location + Point<T>.Right;
        yield return location + Point<T>.Down;
    }

    /// <summary>
    /// Get all Moore neighbors (8-directional) around a point
    /// </summary>
    public static IEnumerable<Point<T>> MooreNeighbourhood<T>(Point<T> location) where T : INumber<T>
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                T x = location.X + T.CreateChecked(dx);
                T y = location.Y + T.CreateChecked(dy);
                yield return new Point<T>(x, y);
            }
        }
    }
}
