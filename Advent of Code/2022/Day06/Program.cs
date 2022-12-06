using var client = new AoC.Api.AoCClient();
var input = await client.GetRawInputAsync(2022, 06);

Console.WriteLine($"Part 1: {Buffer.FindFirstDuplicate(input, 4) + 1}");
Console.WriteLine($"Part 2: {Buffer.FindFirstDuplicate(input, 14) + 1}");

class Buffer
{
    private char[] _queue;
    private int _index;

    public Buffer(int capacity) => (_index, _queue) = (0, new char[capacity]);

    public void Add(char item)
    {
        _queue[_index++] = item;
        _index %= _queue.Length;
    }

    public bool HasDuplicates() => _queue.Count() != _queue.Distinct().Count();

    public static int FindFirstDuplicate(string packet, int queueSize)
    {
        var q = new Buffer(queueSize);

        for (int i = 0; i < packet.Length; i++)
        {
            q.Add(packet[i]);
            if (i >= queueSize && !q.HasDuplicates())
                return i;            
        }
        return -1;
    }
}