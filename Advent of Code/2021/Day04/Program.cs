using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2021, 4);

var numbers = input[0].Split(",").Select(Int32.Parse).ToArray();
var boards = input.Skip(1).Chunk(5).Select(p => BingoBoard.Parse(p.ToArray())).ToList();

BingoBoard? firstWinner = default;
BingoBoard? lastWinner = default;

foreach(var number in numbers) {    
    foreach(var board in boards) board.Mark(number);    

    if(lastWinner != default) {
        Console.WriteLine($"Part 1: {lastWinner.Score() * number}");        
        return;
    }

    if(firstWinner == default && boards.Count(p=>p.Bingo()) == 1) {
        firstWinner = boards.Single(p=>p.Bingo());   
        Console.WriteLine($"Part 2: {firstWinner.Score() * number}");        
    }

    if(boards.Count(p=>!p.Bingo())==1)
        lastWinner = boards.Single(p=>!p.Bingo());
}

class BingoBoard
{
    int[,] Numbers = new int[5, 5];
    bool[,] Marks = new bool[5, 5];
    
    public static BingoBoard Parse(string[] source)
    {
        BingoBoard res = new();
        for (int y = 0; y < source.Length; y++)
        {
            var items = source[y]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(Int32.Parse).ToArray();
            for (int x = 0; x < items.Length; x++)
                res.Numbers[x, y] = items[x];
        }
        return res;
    }
    public void Mark(int entry)
    {
        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
                if (Numbers[x, y] == entry)
                    Marks[x, y] = true;
    }
    public bool Bingo()
        => Enumerable.Range(0, 5).Any(x => Enumerable.Range(0, 5).All(y => Marks[x, y]))
        || Enumerable.Range(0, 5).Any(y => Enumerable.Range(0, 5).All(x => Marks[x, y]));
    public int Score()
        => Numbers.Cast<int>().Zip(Marks.Cast<bool>()).Sum(p => p.Item2 ? 0 : p.Item1);
}
