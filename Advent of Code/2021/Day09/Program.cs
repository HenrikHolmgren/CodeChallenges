using AoC.Common;

using var client = new AoC.Api.AoCClient();
var input = await client.GetRawInputAsync(2021,09);
var map = SAK.LoadMap(input);
var points = 
    from x in Enumerable.Range(0, map.GetLength(0))
    from y in Enumerable.Range(0, map.GetLength(1))
        select (new Point(x,y));

var xMax = map.GetLength(0) - 1;
var yMax = map.GetLength(1) - 1;
var lowPoints = points.Where(p=>SAK.VonNeumannNeighbourhood(p, xMax,yMax).All(q=>map[q.X, q.Y] > map[p.X, p.Y]));

Console.WriteLine("Part 1: " + lowPoints.Sum(p=>map[p.X, p.Y]+1));
Console.WriteLine("Part 2: " + lowPoints.Select(Basin).Select(p=>p.Count()).OrderByDescending(p=>p).Take(3).Aggregate(1, (p, q)=>p*q));

IEnumerable<Point> Basin(Point p){
    HashSet<Point> basin = new();
    Stack<Point> fringe = new();
    fringe.Push(p);
    while(fringe.Any()){
        var probe = fringe.Pop();
        if(map[probe.X, probe.Y] == 9) continue;
        if(basin.Contains(probe)) continue;
        basin.Add(probe);
        foreach(var neighbour in SAK.VonNeumannNeighbourhood(probe, xMax, yMax))
            fringe.Push(neighbour);        
    }
    return basin;
}