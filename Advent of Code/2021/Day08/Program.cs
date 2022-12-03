using var client = new AoC.Api.AoCClient();
var input = await client.GetParsedAsync<(string[] Signals, string[] Output)>(2021, 8, p =>
{
    var signals = p.Split('|')[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
    var outputs = p.Split('|')[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
    return (signals, outputs);
});

int ResolveDisplayValue((string[] Signals, string[] Output) state)
{
    var map = new HashSet<char>[10];
    map[1] = state.Signals.Single(p => p.Length == 2).ToHashSet();
    map[7] = state.Signals.Single(p => p.Length == 3).ToHashSet();
    map[4] = state.Signals.Single(p => p.Length == 4).ToHashSet();
    map[8] = state.Signals.Single(p => p.Length == 7).ToHashSet();
    map[9] = state.Signals.Single(p => p.Length == 6 && p.Intersect(map[1]).Count() == 2 && p.Intersect(map[4]).Count() == 4).ToHashSet();
    map[0] = state.Signals.Single(p => p.Length == 6 && p.Intersect(map[1]).Count() == 2 && p.Intersect(map[9]).Count() != 6).ToHashSet();
    map[6] = state.Signals.Single(p => p.Length == 6 && p.Intersect(map[0]).Count() != 6 && p.Intersect(map[9]).Count() != 6).ToHashSet();
    map[2] = state.Signals.Single(p => p.Length == 5 && p.Intersect(map[4]).Count() == 2).ToHashSet();
    map[3] = state.Signals.Single(p => p.Length == 5 && p.Intersect(map[1]).Count() == 2 && p.Intersect(map[2]).Count() != 5).ToHashSet();
    map[5] = state.Signals.Single(p => p.Length == 5 && p.Intersect(map[2]).Count() != 5 && p.Intersect(map[3]).Count() != 5).ToHashSet();

    int sum = 0;
    foreach (var digit in state.Output)
    {
        sum *= 10;
        for (int i = 0; i < 10; i++)
            if (map[i].SetEquals(digit)) { sum += i; break; }
    }
    return sum;
}

Console.WriteLine("Part 1: " + input.SelectMany(p => p.Output).Count(p => new[] { 2, 3, 4, 7 }.Contains(p.Length)));
Console.WriteLine("Part 2: " + input.Sum(ResolveDisplayValue));
