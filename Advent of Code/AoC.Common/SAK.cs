namespace AoC.Common;

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

    public static void DrawMap(int[,] map, int align = 1)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            string line = "";
            for (int x = 0; x < map.GetLength(0); x++)
            {
                line += map[x, y].ToString().PadLeft(align) + " ";
            }
            Console.WriteLine(line);
        }
    }

    public static IEnumerable<Point> VonNeumannNeighbourhood(Point location, int maxX, int maxY, bool wrapAround = false)
    {
        if (location.X > 0) yield return location with { X = location.X - 1 };
        else if (wrapAround) yield return location with { X = maxX };
        if (location.Y > 0) yield return location with { Y = location.Y - 1 };
        else if (wrapAround) yield return location with { Y = maxY };
        if (location.X < maxX) yield return location with { X = location.X + 1 };
        else if (wrapAround) yield return location with { X = 0 };
        if (location.Y < maxY) yield return location with { Y = location.Y + 1 };
        else if (wrapAround) yield return location with { Y = 0 };
    }

    public static IEnumerable<Point> MooreNeighbourhood(Point location, int maxX, int maxY)
    {
        for (int x = location.X - 1; x <= location.X + 1; x++)
            for (int y = location.Y - 1; y <= location.Y + 1; y++)
            {
                var probe = new Point(x, y);
                if (x >= 0 && y >= 0 &&
                x <= maxX && y <= maxY &&
                probe != location)
                    yield return probe;
            }
    }
}
