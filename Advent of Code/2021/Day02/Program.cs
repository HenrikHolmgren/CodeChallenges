using var client = new AoC.Api.AoCClient();
var input = await client.GetParsedAsync(2021, 2, Command.Parse);

System.Console.WriteLine("Part 1: " + input.Sum(p => p.Vertical) * input.Sum(p => p.Horizontal));

var part2 = input.Aggregate((Aim: 0L, Depth: 0L, Horizontal: 0L), (p, q) =>
{
    var aim = p.Aim + q.Vertical;
    return (aim, p.Depth + q.Horizontal * aim, p.Horizontal + q.Horizontal);
});

System.Console.WriteLine("Part 2: " + part2.Depth * part2.Horizontal);
