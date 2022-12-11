using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2022, 10);

int[] checkpoints = new[] { 20, 60, 100, 140, 180, 220 };
int currentCycle = 0;
int x = 1;
int totalStrengh = 0;
string output = "";

void appendOutput(int cycle, int x)
{
    var col = cycle % 40;
    if (Math.Abs(x - col) <= 1) output += '#';
    else output += '.';
}

foreach (var cmd in input)
{

    switch (cmd)
    {
        case string noop when noop.Equals("noop"):
            appendOutput(currentCycle, x);
            currentCycle += 1;
            if (checkpoints.Contains(currentCycle))
                totalStrengh += currentCycle * x;
            break;
        case string addX when addX.StartsWith("addx"):
            if (checkpoints.Contains(currentCycle + 1))
                totalStrengh += (currentCycle + 1) * x;
            if (checkpoints.Contains(currentCycle + 2))
                totalStrengh += (currentCycle + 2) * x;

            appendOutput(currentCycle, x);
            appendOutput(currentCycle + 1, x);

            currentCycle += 2;            
            x += Int32.Parse(addX[5..]);
            break;
    }
}

System.Console.WriteLine($"Part 1: {totalStrengh}");
System.Console.WriteLine("Part 2:");
System.Console.WriteLine(String.Join(Environment.NewLine, output.Chunk(40).Select(p => new String(p))));