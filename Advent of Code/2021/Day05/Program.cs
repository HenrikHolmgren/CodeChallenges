using AoC.Common;

using var client = new AoC.Api.AoCClient();
var input = await client.GetParsedAsync(2021, 5, p=>{
    var sections = p.Split(" -> ")
    .Select(p=>p.Split(",").Select(Int32.Parse).ToArray())
    .Select(p=>new AoC.Common.Point(p[0], p[1]))
    .OrderBy(p=>p.X).ThenBy(p=>p.Y) // Guarantee natural ordering, so the only thing we need to consider when updating the map is whether a 45 degree segment slopes up or down.
    .ToArray();

    return new LineSegment(sections[0], sections[1]);
});

var alignedMap = buildMap(input.Where(p=>p.IsAxisAligned));

var genericMap = buildMap(input);
Console.WriteLine(alignedMap.Cast<int>().Count(p=>p>1));
Console.WriteLine(genericMap.Cast<int>().Count(p=>p>1));

//Just for fun, lets try rendering the map - Unfortunately, no easter-egg I can spot, just a bunch of lines.
var m = genericMap.Cast<int>().Max();
var b = new System.Drawing.Bitmap(genericMap.GetLength(0), genericMap.GetLength(1));
for(int x = 0; x < genericMap.GetLength(0); x++) 
for(int y = 0; y < genericMap.GetLength(1); y++) {
    var lumen = (byte)(250 * genericMap[x,y] / m); 
    b.SetPixel(x, y,  System.Drawing.Color.FromArgb(lumen, lumen, lumen));
}
b.Save("day5_Map.bmp");

int[,] buildMap(IEnumerable<LineSegment> lines){
    var xMax = lines.Max(p=>p.To.X);
    var yMax = lines.Max(p=>p.To.Y);

    var map = new int[xMax+1,yMax+1];

    foreach(var line in lines){
        int targets = line.From.X == line.To.X 
            ? line.To.Y - line.From.Y
            : line.To.X - line.From.X; //Also covers 45 degree diagonals
        for(int target = 0; target <= targets; target++){
            if(line.IsAxisAligned && line.From.X == line.To.X) map[line.From.X, line.From.Y + target]++;
            else if(line.IsAxisAligned) map[line.From.X + target, line.From.Y]++;
            else if(line.From.Y < line.To.Y) map[line.From.X + target, line.From.Y + target]++;
            else map[line.From.X + target, line.From.Y - target]++;
        }
    }
    return map;
}
