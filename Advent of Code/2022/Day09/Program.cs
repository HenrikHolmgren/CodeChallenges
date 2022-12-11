using AoC.Common;

using var client = new AoC.Api.AoCClient();
var input = await client.GetParsedAsync(2022, 9, p => new Command(p[0], Int32.Parse(p[2..])));

System.Console.WriteLine($"Part 1: {new Rope(2).RunSim(input)}");
System.Console.WriteLine($"Part 2: {new Rope(10).RunSim(input)}");

record Command(char Direction, int Amount);

class Rope
{
    private List<Point> _knots;

    public Rope(int length) => _knots = Enumerable.Repeat(new Point(0, 0), length).ToList();

    public int RunSim(IEnumerable<Command> commands)
    {
        bool IsTouching(Point a, Point b)
            => Math.Abs(a.X - b.X) <= 1 &&
            Math.Abs(a.Y - b.Y) <= 1;

        HashSet<Point> visited = new();
        foreach (var cmd in commands)
        {
            for (int i = 0; i < cmd.Amount; i++)
            {
                switch (cmd.Direction)
                {
                    case 'R': _knots[0] = _knots[0] with { X = _knots[0].X + 1 }; break;
                    case 'L': _knots[0] = _knots[0] with { X = _knots[0].X - 1 }; break;
                    case 'U': _knots[0] = _knots[0] with { Y = _knots[0].Y + 1 }; break;
                    case 'D': _knots[0] = _knots[0] with { Y = _knots[0].Y - 1 }; break;
                }

                for (int knot = 1; knot < _knots.Count; knot++)
                {
                    if (IsTouching(_knots[knot], _knots[knot - 1]))
                        continue;

                    var leader = _knots[knot - 1];
                    var follower = _knots[knot];
                    if (follower.X == leader.X && follower.Y < leader.Y)
                        follower = follower with { Y = follower.Y + 1 };
                    else if (follower.X == leader.X)
                        follower = follower with { Y = follower.Y - 1 };
                    else if (follower.Y == leader.Y && follower.X < leader.X)
                        follower = follower with { X = follower.X + 1 };
                    else if (follower.Y == leader.Y)
                        follower = follower with { X = follower.X - 1 };
                    else if (follower.X < leader.X && follower.Y < leader.Y)
                        follower = follower with { X = follower.X + 1, Y = follower.Y + 1 };
                    else if (follower.X < leader.X)
                        follower = follower with { X = follower.X + 1, Y = follower.Y - 1 };
                    else if (follower.Y < leader.Y)
                        follower = follower with { X = follower.X - 1, Y = follower.Y + 1 };
                    else
                        follower = follower with { X = follower.X - 1, Y = follower.Y - 1 };

                    _knots[knot] = follower;
                }

                visited.Add(_knots.Last());
            }
        }
        return visited.Count();
    }
}