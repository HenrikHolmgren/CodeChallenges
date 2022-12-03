using var client = new AoC.Api.AoCClient();
var initialFish = await client.GetNumbersAsync(2021, 6);

var school = new Dictionary<int, long> { [0] = 0, [1] = 0, [2] = 0, [3] = 0, [4] = 0, [5] = 0, [6] = 0, [7] = 0, [8] = 0 };
foreach (var fish in initialFish) school[fish]++;

void IncrementGeneration()
{
    var fry = school[0];
    for (int j = 0; j < 8; j++)
        school[j] = school[j + 1];
    school[6] += fry;
    school[8] = fry;
}

for (int i = 0; i < 80; i++)
    IncrementGeneration();
Console.WriteLine(school.Values.Sum());

for (int i = 0; i < 256 - 80; i++)
    IncrementGeneration();
Console.WriteLine(school.Values.Sum());