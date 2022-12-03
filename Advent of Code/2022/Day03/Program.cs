using var client = new AoC.Api.AoCClient();

var input = client.GetLinesAsync(2022, 3).GetAwaiter().GetResult(); //Span<T> can not be used within an async context. Using await results in an implicitly async Main().

int part1 = 0;
int part2 = 0;

for (int i = 0; i < input.Length; i++)
{
    var line = input[i].AsSpan();
    var maskA = AsBitSet(line[(line.Length / 2)..]);
    var maskB = AsBitSet(line[..(line.Length / 2)]);
    var overlap = Overlap(maskA, maskB);
    part1 += ToValue(overlap);
}

foreach(var group in input.Chunk(3)){
    var masks = group.Select(p=>AsBitSet(p.AsSpan())).ToList();
    var overlap = masks.Aggregate(Overlap);
    part2 += ToValue(overlap);
}

System.Console.WriteLine(part1);
System.Console.WriteLine(part2);

static (uint low, uint high) AsBitSet(ReadOnlySpan<char> val)
{
    uint maskLow = 0, maskHigh = 0;
    for (int i = 0; i < val.Length; i++)
    {
        if (val[i] > 'Z')
            maskLow |= (uint)(1 << (val[i] - 'a'));
        else
            maskHigh |= (uint)(1 << (val[i] - 'A'));
    }
    return (maskLow, maskHigh);
}

static (uint low, uint high) Overlap((uint, uint) A, (uint, uint) B)
    => (A.Item1 & B.Item1, A.Item2 & B.Item2);

static int ToValue((uint low, uint high) val)
{
    if (val.low != 0)
        return (int)Math.Log2(val.low) + 1;
    else
        return (int)Math.Log2(val.high) + 27;   
}