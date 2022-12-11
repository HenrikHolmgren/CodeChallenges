using AoC.Common;

using var client = new AoC.Api.AoCClient();
var input = await client.GetIntMatrixAsync(2022, 08);

int visibleCount = 0;
for (int i = 0; i < input.GetLength(0); i++)
    for (int j = 0; j < input.GetLength(1); j++)
        if (!Enumerable.Range(0, i).Any(p => input[p, j] >= input[i, j]) ||
            !Enumerable.Range(i + 1, input.GetLength(0) - i - 1).Any(p => input[p, j] >= input[i, j]) ||
            !Enumerable.Range(0, j).Any(p => input[i, p] >= input[i, j]) ||
            !Enumerable.Range(j + 1, input.GetLength(1) - j - 1).Any(p => input[i, p] >= input[i, j]))
        {
            visibleCount++;
        }

System.Console.WriteLine($"Part 1: {visibleCount}");

//Very very brute-forcy - there are bound to be much better solutions out there for later.
var bestScenicScore = 0;
for (int i = 0; i < input.GetLength(0); i++)
{
    for (int j = 0; j < input.GetLength(1); j++)
    {
        if (i == 0 || j == 0 || i == input.GetLength(0) - 1 || j == input.GetLength(1) - 1)
            continue;

        var up = Enumerable.Range(1, i).Cast<int?>().FirstOrDefault(p => input[i - p.Value, j] >= input[i, j]) ?? i;
        var left = Enumerable.Range(1, j).Cast<int?>().FirstOrDefault(p => input[i, j - p.Value] >= input[i, j]) ?? j;
        var down = Enumerable.Range(1, input.GetLength(0) - i - 1).Cast<int?>().FirstOrDefault(p => input[i + p.Value, j] >= input[i, j]) ?? input.GetLength(0) - i - 1;
        var right = Enumerable.Range(1, input.GetLength(1) - j - 1).Cast<int?>().FirstOrDefault(p => input[i, j + p.Value] >= input[i, j]) ?? input.GetLength(1) - j - 1;

        var score = up * down * left * right;

        if (score > bestScenicScore)
            bestScenicScore = score;
    }
}
System.Console.WriteLine($"Part 2: {bestScenicScore}");
