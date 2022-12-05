using var client = new AoC.Api.AoCClient();

var input = await client.GetParsedAsync(2022, 4, line =>
{
    var sections = line.Split(',');
    return new Assignment(Section.Parse(sections[0]), Section.Parse(sections[1]));
});

Console.WriteLine("Part 1: " + input.Count(p => p.HasFullOverlap));
Console.WriteLine("Part 2: " + input.Count(p => p.WastesWork));

record Assignment(Section A, Section B)
{
    public bool HasFullOverlap => A.Contains(B) || B.Contains(A);
    public bool WastesWork => HasFullOverlap || A.OverlapsRight(B) || B.OverlapsRight(A);
}

record Section(int From, int To)
{
    public static Section Parse(string val)
    {
        var boundaries = val.Split("-").Select(Int32.Parse).ToArray();
        return new(boundaries[0], boundaries[1]);
    }

    public bool Contains(Section other) => From <= other.From && To >= other.To;
    public bool OverlapsRight(Section other) => From <= other.From && other.From <= To;
}