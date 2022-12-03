Graph g = new();
using var client = new AoC.Api.AoCClient();
var input = await client.GetParsedAsync(2021, 12, P => P.Split('-'));
foreach (var line in input)
    g.Add(line[0], line[1]);

Console.WriteLine("Part 1: " + g.GetPaths("start", "end", false).Count());
Console.WriteLine("Part 2: " + g.GetPaths("start", "end", true).Count());

class Graph
{
    public record Node(string Name, List<Node> Neighbours);
    public Dictionary<string, Node> Nodes = new();

    public void Add(string from, string to)
    {
        if (!Nodes.ContainsKey(from)) Nodes[from] = new(from, new());
        if (!Nodes.ContainsKey(to)) Nodes[to] = new(to, new());
        Nodes[from].Neighbours.Add(Nodes[to]);
        Nodes[to].Neighbours.Add(Nodes[from]);
    }

    public IEnumerable<IEnumerable<string>> GetPaths(string from, string to, bool allowDuplicateVisit)
    {
        var visits = Nodes.Keys.ToDictionary(p => p, p => 0);
        if (allowDuplicateVisit) visits[from]++;

        return GetPathsInternal(from, to, allowDuplicateVisit, visits);
    }

    protected IEnumerable<IEnumerable<string>> GetPathsInternal(string from, string to, bool allowDuplicateVisit, Dictionary<string, int> visits)
    {
        Dictionary<string, int> newVisits = new(visits);

        if (from == to)
        {
            yield return new[] { to };
            yield break;
        }

        var node = Nodes[from];
        if (!node.Name.All(Char.IsUpper))
            newVisits[from]++;

        if (newVisits.Count(p => p.Value > 1) > (allowDuplicateVisit ? 2 : 0))
            yield break;

        foreach (var neighbour in node.Neighbours.Select(p => p.Name))
        {
            if (newVisits[neighbour] == 0 || allowDuplicateVisit && newVisits[neighbour] == 1)
            {
                foreach (var subPath in GetPathsInternal(neighbour, to, allowDuplicateVisit, newVisits))
                    yield return new[] { from }.Concat(subPath);
            }
        }
    }
}