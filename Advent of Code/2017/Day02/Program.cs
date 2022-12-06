using var client = new AoC.Api.AoCClient();
var input = await client.GetRawInputAsync(2017, 02);

int[][] sheet = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(p => p.Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(q => Int32.Parse(q)).ToArray())
    .ToArray();

System.Console.WriteLine("Part 1: "+ sheet.Select(p => p.Max() - p.Min()).Sum());
System.Console.WriteLine("Part 1: "+ sheet.Select(GetPart2Division).Sum());

int GetPart2Division(int[] line)
{
    for (var i = 0; i < line.Length; i++)
    {
        for (var j = i + 1; j < line.Length; j++)
        {
            if (line[i] % line[j] == 0)
                return line[i] / line[j];
            else if (line[j] % line[i] == 0)
                return line[j] / line[i];
        }
    }

    throw new InvalidOperationException("No evenly divisible candidates found!");
}