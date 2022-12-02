using var client = new AoC.Api.AoCClient();

var input = await client.GetParsedAsync(2022, 2, line => new Entry(line[0], line[2]));
var part1 = input.Sum(p => p switch
{
    { Thrown: 'A', Response: 'X' } => 1 + 3, // Draw with rock
    { Thrown: 'A', Response: 'Y' } => 2 + 6, // Win with paper
    { Thrown: 'A', Response: 'Z' } => 3 + 0, // Lose with scissors
    { Thrown: 'B', Response: 'X' } => 1 + 0, // Lose with rock
    { Thrown: 'B', Response: 'Y' } => 2 + 3, // Draw with paper
    { Thrown: 'B', Response: 'Z' } => 3 + 6, // Win with scissors
    { Thrown: 'C', Response: 'X' } => 1 + 6, // Win with rock
    { Thrown: 'C', Response: 'Y' } => 2 + 0, // Lose with paper
    { Thrown: 'C', Response: 'Z' } => 3 + 3, // Draw with scissors
    _=>throw new ArgumentOutOfRangeException(p.ToString())
});
System.Console.WriteLine($"Part 1: {part1}");

var part2 = input.Sum(p => p switch
{
    { Thrown: 'A', Response: 'X' } => 3 + 0, // Lose with scissors
    { Thrown: 'A', Response: 'Y' } => 1 + 3, // Draw with rock
    { Thrown: 'A', Response: 'Z' } => 2 + 6, // Win with paper
    { Thrown: 'B', Response: 'X' } => 1 + 0, // Lose with rock
    { Thrown: 'B', Response: 'Y' } => 2 + 3, // Draw with paper
    { Thrown: 'B', Response: 'Z' } => 3 + 6, // Win with scissors
    { Thrown: 'C', Response: 'X' } => 2 + 0, // Lose with paper
    { Thrown: 'C', Response: 'Y' } => 3 + 3, // Draw with scissors
    { Thrown: 'C', Response: 'Z' } => 1 + 6, // Win with rock
    _=>throw new ArgumentOutOfRangeException(p.ToString())
});
System.Console.WriteLine($"Part 2: {part2}");

record Entry(char Thrown, char Response);