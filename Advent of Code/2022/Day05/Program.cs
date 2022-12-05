using System.Text.RegularExpressions;

using var client = new AoC.Api.AoCClient();

var input = await client.GetRawInputAsync(2022, 05);

List<Stack<char>> State = new();
var initialStateInput = input.Split("\n\n")[0]
    .Split('\n', StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
int columnCount = initialStateInput[0].Length / 4 + 1;
for (int i = 0; i < columnCount ; i++)
{
    State.Add(new());
    State.Add(new());
}

foreach (var line in initialStateInput.Skip(1))
{
    foreach (var entry in line.Chunk(4).Select((p, i) => (Label: p.Skip(1).First(), Column: i)))
    {
        if (entry.Label != ' ')
        {
            State[entry.Column].Push(entry.Label);
            State[entry.Column + columnCount].Push(entry.Label);
        }
    }
}

var commands = input.Split("\n\n")[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(MoveCommand.Parse).ToArray();

Stack<char> scratch = new();
foreach(var command in commands)
{
    for (int i = 0; i < command.Count; i++)
    {
        State[command.To].Push(State[command.From].Pop());
        scratch.Push(State[command.From + columnCount].Pop());
    }

    for (int i = 0; i < command.Count; i++)
        State[command.To+columnCount].Push(scratch.Pop());
}

Console.WriteLine("Part 1: " + string.Join("", State.Take(columnCount).Select(p=>p.Peek())));
Console.WriteLine("Part 2: " + string.Join("", State.Skip(columnCount).Select(p=>p.Peek())));

record MoveCommand(int Count, int From, int To)
{
    private static Regex _parser = new Regex(@"move (\d+) from (\d+) to (\d+)");
    public static MoveCommand Parse(string commandText)
    {
        var match = _parser.Match(commandText);
        if (!match.Success) throw new FormatException(commandText + " does not match expected pattern.");
        return new(
            Int32.Parse(match.Groups[1].Value),
            Int32.Parse(match.Groups[2].Value) - 1,
            Int32.Parse(match.Groups[3].Value) - 1);
    }
}