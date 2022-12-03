using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2021, 10);


var parserResults = input.Select(Parse).ToArray();
Console.WriteLine("Part 1: " + parserResults.Where(p => p.SyntaxError).Sum(p => Cost(p.FirstError.Value)));
var scores = parserResults.Where(p => !p.SyntaxError).Select(p => p.Missing.Aggregate(0L, (p, q) => p * 5 + Cost(q))).OrderBy(p => p).ToArray();
Console.WriteLine("Part 2: " + scores[scores.Length / 2]);

//We can leverage that the state in the Parse method actually contains the opening brackets, not the missing closing.
//If we don't spend time reversing the brackets for part 2, everything fits in one switch.
int Cost(char c) => c switch { ')' => 3, ']' => 57, '}' => 1197, '>' => 25137, '(' => 1, '[' => 2, '{' => 3, '<' => 4, _ => throw new InvalidOperationException("What even is that?") };

ParserState Parse(string input)
{
    Stack<char> state = new();
    foreach (var next in input)
    {
        if (new[] { '{', '[', '<', '(' }.Contains(next))
            state.Push(next);
        else
        {
            if (Math.Abs(state.Peek() - next) < 3) //A bit of a cheat - opening and closing brackets of all four kinds are very close in the ascii table.
                state.Pop();
            else
                return new ParserState(true, next, Array.Empty<char>());
        }
    }
    return new ParserState(false, null, state.ToArray());
}

record ParserState(bool SyntaxError, char? FirstError, char[] Missing);