using AoC.Common;

using var client = new AoC.Api.AoCClient();
var input = await client.GetRawInputAsync(2021, 11);
var map = SAK.LoadMap(input);

int flashCount = 0;
for (int i = 0; i < 100; i++)
    flashCount += StepAndCountFlashes(map);
Console.WriteLine("Part 1: " + flashCount);

int steps = 101;
while (StepAndCountFlashes(map) != map.Length) steps++;
Console.WriteLine("Part 2: " + steps);


int StepAndCountFlashes(int[,] map)
{
    int flashCount = 0;
    for (int x = 0; x < map.GetLength(0); x++)
        for (int y = 0; y < map.GetLength(1); y++)
        {
            map[x, y]++;
        }
    bool iterate = false;
    do
    {
        iterate = false;
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] > 9)
                {
                    foreach (var neighbour in SAK.MooreNeighbourhood(new Point(x, y), map.GetLength(0) - 1, map.GetLength(1) - 1))
                    {
                        if (map[neighbour.X, neighbour.Y] != 0)
                        { //Don't increment 0s, they have already flashed.
                            map[neighbour.X, neighbour.Y]++;
                            iterate = true;
                        }
                    }
                    map[x, y] = 0;
                    flashCount++;
                }
            }
    } while (iterate);
    return flashCount;
}