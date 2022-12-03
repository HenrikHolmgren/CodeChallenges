using AoC.Common;
using System.Linq;

using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2021, 13);

List<Point> dots = new();
List<Fold> folds = new();
foreach (var line in input)
{
    if (line.Contains(","))
    {
        var coord = line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
        dots.Add(new(coord[0], coord[1]));
    }
    else if (line.Contains("fold along"))
    {
        folds.Add(new(line[11], Int32.Parse(line.Substring(13))));
    }
}

Console.WriteLine($"Part 1: {dots.Select(folds[0].Transform).Distinct().Count()}");
Func<Point, Point> fullTransform = folds.Select(p =>  (Func<Point, Point>) p.Transform).Aggregate((a, b) => p => (b(a(p))));
var message = dots.Select(fullTransform).ToHashSet();
Point bounds = new(message.Max(p => p.X), message.Max(p => p.Y));

Console.WriteLine("Part 2: ");
for (var y = 0; y <= bounds.Y; y++)
{
    string line = "";
    for (int x = 0; x <= bounds.X; x++)
    {
        if (message.Contains(new(x, y))) line += '*';
        else line += ' ';
    }
    Console.WriteLine(line);
}

record Fold(char Axis, int Offset)
{
    public Point Transform(Point p) => Axis switch
    {
        'x' => p.X > Offset ? p with { X = Offset - (p.X - Offset) } : p,
        'y' => p.Y > Offset ? p with { Y = Offset - (p.Y - Offset) } : p,
        _ => throw new ArgumentException("Cannot comprehend axis " + Axis)
    };
};