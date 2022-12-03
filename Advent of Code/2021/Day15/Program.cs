using AoC.Common;

using var client = new AoC.Api.AoCClient();
var raw = await client.GetRawInputAsync(2021, 15);
var input = SAK.LoadMap(raw);

int ExitDistance(int[,] map)
{
    Point origin = new(0, 0);
    Point target = new(map.GetLength(0) - 1, map.GetLength(1) - 1);
    var distance = new int[map.GetLength(0), map.GetLength(1)];
    var visited = new bool[map.GetLength(0), map.GetLength(1)];
    distance[0, 0] = map[0, 0];
    HashSet<Point> fringe = new() { origin }; //Yeah, this really should be a PriQ, but the full map executed in 5s, so no need to waste time building one until the optimization/cleanup pass.
    while (!visited[target.X, target.Y])
    {
        var candidate = fringe.OrderBy(p => distance[p.X, p.Y]).First();
        fringe.Remove(candidate);
        foreach (var neighbour in SAK.VonNeumannNeighbourhood(candidate, target.X, target.Y))
        {
            if (!visited[neighbour.X, neighbour.Y] && (distance[neighbour.X, neighbour.Y] == 0 || distance[neighbour.X, neighbour.Y] > distance[candidate.X, candidate.Y] + map[neighbour.X, neighbour.Y]))
                distance[neighbour.X, neighbour.Y] = distance[candidate.X, candidate.Y] + map[neighbour.X, neighbour.Y];
            if (!visited[neighbour.X, neighbour.Y])
                fringe.Add(neighbour);
        }
        visited[candidate.X, candidate.Y] = true;
    }
    return distance[target.X, target.Y] - distance[origin.X, origin.Y];
}

int[,] ExtendMap(int[,] template, int count)
{
    var result = new int[template.GetLength(0) * count, template.GetLength(1) * count];
    foreach (var extension in (from x in Enumerable.Range(0, count) from y in Enumerable.Range(0, count) select (x, y)))
    {
        for (int x = 0; x < template.GetLength(0); x++)
            for (int y = 0; y < template.GetLength(1); y++)
            {
                var value = template[x, y] + extension.x + extension.y;
                while (value > 9) value -= 9;
                result[x + extension.x * template.GetLength(0), y + extension.y * template.GetLength(0)] = value;
            }
    }
    return result;
}

Console.WriteLine("Part 1: " + ExitDistance(input));
Console.WriteLine("Part 2: " + ExitDistance(ExtendMap(input, 5)));