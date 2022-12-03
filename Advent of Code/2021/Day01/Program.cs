using var client = new AoC.Api.AoCClient();
var input = await client.GetNumbersAsync(2021, 1);

Console.WriteLine("Part 1: " +
    Enumerable.Range(0, input.Length-1)
    .Where(p=>input[p] < input[p+1])
    .Count());

Console.WriteLine("Part 2: " +
    Enumerable.Range(0, input.Length-3)
    .Where(p=>input[p] < input[p+3]) //Who cares about the middle two numbers? 
    .Count());

