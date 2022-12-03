using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2021, 14);

var template = input.First();
var rules = input.Skip(1).ToDictionary(p => p.Substring(0, 2), p => p[6]);

//Naïve implementation for part 1 - just scrapes by but real short and neat.
LinkedList<char> polymer = new(template);

void Step()
{
    var iterator = polymer.First.Next;
    while (iterator != null)
    {
        if (rules.TryGetValue($"{iterator.Previous.Value}{iterator.Value}", out var insertion))
            polymer.AddAfter(iterator.Previous, insertion);
        iterator = iterator.Next;
    }
}

for (int step = 0; step < 10; step++) Step();
var elements = polymer.GroupBy(p => p).ToDictionary(p => p.Key, p => p.LongCount());
Console.WriteLine("Part 1: " + (elements.Max(p => p.Value) - elements.Min(p => p.Value)));

//For part 2 we need to be a bit more clever - could most likely be a lot more short and neat still..
var digrams = Enumerable.Range(0, template.Length - 1)
    .Select(p => template.Substring(p, 2))
    .GroupBy(p => p)
    .ToDictionary(p => p.Key, p => p.LongCount());
foreach (var rule in rules)
{
    if (!digrams.ContainsKey(rule.Key))
        digrams[rule.Key] = 0;
}

for (int i = 0; i < 40; i++)
{
    var temp = new Dictionary<string, long>(digrams);
    foreach (var rule in rules)
    {
        if (digrams.ContainsKey(rule.Key))
        {
            temp[rule.Key] -= digrams[rule.Key];
            temp[$"{rule.Key[0]}{rule.Value}"] += digrams[rule.Key];
            temp[$"{rule.Value}{rule.Key[1]}"] += digrams[rule.Key];
        }
    }
    digrams = temp;
}

elements = new Dictionary<char, long>();
foreach (var digram in digrams)
{
    foreach (var element in digram.Key)
    {
        if (!elements.ContainsKey(element)) elements[element] = 0;
        elements[element] += digram.Value;
    }
}

//Every other element is double counted except the first and last since they don't have a left or right neighbour.
elements[template[0]]++;
elements[template.Last()]++;

Console.WriteLine("Part 2: " + (elements.Max(p => p.Value) - elements.Min(p => p.Value)) / 2);
