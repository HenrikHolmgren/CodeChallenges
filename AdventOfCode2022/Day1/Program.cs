using var client = new AoC.Api.AoCClient();

// Simple version
/*
var inputRaw = await client.GetRawInputAsync(2022, 1);

var input = inputRaw.Split('\n');
List<List<int>> caloryGroups = new();
List<int> accumulator = new();
foreach (var line in input) {
    if(String.IsNullOrEmpty(line)) {
        caloryGroups.Add(accumulator);
        accumulator = new();
    }
    else {
        accumulator.Add(Int32.Parse(line));
    }
}
if(accumulator.Any())
    caloryGroups.Add(accumulator);
}
System.Console.WriteLine($"Part 1: {caloryGroups.Max(Enumerable.Sum)}");
System.Console.WriteLine($"Part 2: {caloryGroups.Select(Enumerable.Sum).OrderByDescending(p=>p).Take(3).Sum()}");
*/

// Speed-optimized version
// Note, to reliably measure the acutal perf difference, I had to run the workload 10.000 times for meaningful measurements.
// At that point, the speed-optimized version won out at 11.47µs vs. the simple version at 74.54µs pr. run.
// We need a bigger input file ;)
var input = await client.GetRawInputAsync(2022, 1);

var totalLength = input.Length;
var bestPacks = new int[3] { 0, 0, 0 };

int parsed = 0;
int currentPack = 0;
for (int i = 0; i < totalLength; i++) {
    var val = input[i];
    if (val == '\n') {
        if (parsed == 0) { //pack boundary
            if (currentPack > bestPacks[0]) {
                bestPacks[2] = bestPacks[1];
                bestPacks[1] = bestPacks[0];
                bestPacks[0] = currentPack;
            }
            else if (currentPack > bestPacks[1]) {
                bestPacks[2] = bestPacks[1];
                bestPacks[1] = currentPack;
            }
            else if (currentPack > bestPacks[2]) {
                bestPacks[2] = currentPack;
            }
            currentPack = 0;
        }
        else {
            currentPack += parsed;
            parsed = 0;
        }
    }
    else {
        parsed = parsed * 10 + (val - '0');
    }
}
System.Console.WriteLine($"Part 1: {bestPacks[0]}");
System.Console.WriteLine($"Part 2: {bestPacks.Sum()}");