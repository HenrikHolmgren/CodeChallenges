using var client = new AoC.Api.AoCClient();
var input = await client.GetRawInputAsync(2021, 16);
input = input.Trim();

//I know .. this is dreadful - but for now it seems a _lot_ easier to deal with a stream of bit chars than dealing with byte boundaries...
var binary = String.Join("", input.Select(p => Convert.ToString(Convert.ToByte(p.ToString(), 16), 2).PadLeft(4, '0')));

var p = new Packet(binary);

Console.WriteLine(p.VersionSum);
Console.WriteLine(p.Evaluate());

class Packet
{
    public int Version;
    public int Type;
    public long LiteralValue;
    public List<Packet> SubPackets = new();
    public int Length;

    public Packet(string packet, int offset = 0)
    {
        Version = GetValue(packet, offset, 3);
        Type = GetValue(packet, offset + 3, 3);
        if (Type == 4)
        {
            Length = ReadLiteral(packet, offset + 6) - offset;
        }
        else
        {
            var LengthType = packet[offset + 6];
            var subPackagesOffset = 0;
            switch (LengthType)
            {
                case '0':
                    var totalLength = GetValue(packet, offset + 7, 15);
                    offset += 22;
                    do
                    {
                        var subPacket = new Packet(packet, subPackagesOffset + offset);
                        subPackagesOffset += subPacket.Length;
                        SubPackets.Add(subPacket);
                    } while (subPackagesOffset < totalLength);
                    Length = subPackagesOffset + 22;
                    break;
                case '1':
                    var subPackets = GetValue(packet, offset + 7, 11);
                    offset += 18;
                    for (int i = 0; i < subPackets; i++)
                    {
                        var subPacket = new Packet(packet, subPackagesOffset + offset);
                        subPackagesOffset += subPacket.Length;
                        SubPackets.Add(subPacket);
                    }
                    Length = subPackagesOffset + 18;
                    break;
            }
        }
    }

    int ReadLiteral(string packet, int offset)
    {
        byte lastRead;
        do
        {
            LiteralValue <<= 4;
            lastRead = (byte)GetValue(packet, offset, 5);
            LiteralValue |= (byte)(lastRead & 0xF);
            offset += 5;
        } while (lastRead >= 16);
        return offset;
    }

    int GetValue(string packet, int offset, int count)
        => Convert.ToInt32(packet.Substring(offset, count), 2);

    public override string ToString() => $"<{Version}/{Type} {LiteralValue}{String.Join(",", SubPackets)}/{Length}>";

    public int VersionSum => Version + SubPackets.Sum(p => p.VersionSum);
    public long Evaluate() => Type switch
    {
        0 => SubPackets.Sum(p => p.Evaluate()),
        1 => SubPackets.Aggregate(1l, (p, q) => p * q.Evaluate()),
        2 => SubPackets.Min(p => p.Evaluate()),
        3 => SubPackets.Max(p => p.Evaluate()),
        4 => LiteralValue,
        5 => SubPackets[0].Evaluate() > SubPackets[1].Evaluate() ? 1 : 0,
        6 => SubPackets[0].Evaluate() < SubPackets[1].Evaluate() ? 1 : 0,
        7 => SubPackets[0].Evaluate() == SubPackets[1].Evaluate() ? 1 : 0
    };
}
