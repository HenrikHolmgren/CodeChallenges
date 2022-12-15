using AoC.Common;
using var client = new AoC.Api.AoCClient();
var packets = await client.GetParsedAsync(2022, 13, IPacket.Parse);
var comparer = new PacketComparer();

var part1 = packets.Chunk(2)
    .Select((p,i) => (p, i))
    .Where(p=>comparer.Compare(p.p[0], p.p[1]) != 1)
    //Who the heck uses 1-indexed packets?!
    .Sum(p=>p.i+1);
Console.WriteLine($"Part 1: {part1}");

var dividerA = IPacket.Parse("[[2]]");
var dividerB = IPacket.Parse("[[6]]");

var ordered = packets.Union(new[] { dividerA, dividerB })
    .OrderBy(p => p, new PacketComparer())
    .ToArray();

//Seriously, what's with damn 1-indexed packets?!
System.Console.WriteLine($"Part 2: {(Array.BinarySearch(ordered, dividerA, comparer) + 1) * (Array.BinarySearch(ordered, dividerB, comparer) + 1)}"); 

class PacketComparer : IComparer<IPacket>
{
    public int Compare(IPacket? x, IPacket? y) {
        if(x == null) return -1;
        if(y == null) return 0;
        if (x is ItemPacket leftItem && y is ItemPacket rightItem) return leftItem.Content.CompareTo(rightItem.Content);
        else if (x is ListPacket leftList && y is ListPacket rightList) {
            for(int i = 0; i < leftList.Content.Count; i++){
                if(rightList.Content.Count <= i) return 1;
                var cmp = Compare(leftList.Content[i], rightList.Content[i]);
                if(cmp != 0) return cmp;
            }        
            if(rightList.Content.Count != leftList.Content.Count) return -1;
            return 0;
        }
        else if (y is ItemPacket)
            return Compare(x, new ListPacket(new() { y }));
        else
            return Compare(new ListPacket(new() { x }), y);
    }
}

interface IPacket{

    static IPacket Parse(string value)
    {
        Stack<ListPacket> inFlight = new();
        int ix = 0;
        while (ix < value.Length)
        {
            if (value[ix] == '[') inFlight.Push(new ListPacket(new()));
            else if (value[ix] == ']')
            {
                var packet = inFlight.Pop();
                if (inFlight.Count == 0) return packet;
                else inFlight.Peek().Content.Add(packet);
            }
            else if (value[ix] != ',')
            {
                var itemval = 0;
                while (value[ix] >= '0' && value[ix] <= '9')
                    itemval = itemval * 10 + value[ix++] - '0';
                ix--;
                inFlight.Peek().Content.Add(new ItemPacket(itemval));
            }
            ix++;
        }
        throw new InvalidOperationException("Packet never ended!");
    }
}

record ItemPacket(int Content) : IPacket { public override string ToString() => Content.ToString(); };
record ListPacket(List<IPacket> Content) : IPacket { public override string ToString() => $"[{String.Join(", ", Content.Select(p => p.ToString()))}]"; };