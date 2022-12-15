using AoC.Common;
using var client = new AoC.Api.AoCClient();

var input = await client.GetIntMatrixAsync(2022, 12, 'a');
int startingPointValue = 'S' - 'a'; //-14;
int endingPointValue = 'E' - 'a'; //-28;

var startingPoint = SAK.Enumerate(input).First(p=>input[p.X, p.Y] == startingPointValue);
var endPoint =  SAK.Enumerate(input).First(p=>input[p.X, p.Y] == endingPointValue);

input[startingPoint.X, startingPoint.Y] = 'a'-'a';
input[endPoint.X, endPoint.Y] = 'z'-'a';

Queue<(Point, int)> fringe = new();
fringe.Enqueue((endPoint, 2));
var distances = new int[input.GetLength(0), input.GetLength(1)];
distances[endPoint.X, endPoint.Y] = 1;

while(fringe.Any())
{
    var probe = fringe.Dequeue();

    foreach(var neighbour in SAK.VonNeumannNeighbourhood(probe.Item1, input.GetLength(0)-1, input.GetLength(1)-1))
    {
        if(distances[neighbour.X, neighbour.Y] != default) continue;

        var distance = input[neighbour.X, neighbour.Y] - input[probe.Item1.X, probe.Item1.Y];

        if(distance >= -1){
            distances[neighbour.X, neighbour.Y] = probe.Item2;
            fringe.Enqueue((neighbour, probe.Item2+1));
        }
    }
}

System.Console.WriteLine($"Part 1: {distances[startingPoint.X, startingPoint.Y] - 1}");
var bestStartingPoint = SAK.Enumerate(input)
    .Where(p=>input[p.X, p.Y] == 0 && distances[p.X, p.Y] != default)
    .Min(p=>distances[p.X, p.Y]) - 1;
System.Console.WriteLine($"Part 2: {bestStartingPoint}");