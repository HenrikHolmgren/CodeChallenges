using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2022, 07);

Directory root = new("/");
var current = root;

foreach (var line in input)
{
    if (line.StartsWith("$ cd"))
    {
        if (line[5] == '/')
            current = root;
        else if (line[5] == '.')
            current = current.Parent;
        else
        {
            Directory child = new(line[5..]);
            current.AddChild(child);
            current = child;
        }
    }
    else if (!line.StartsWith("$ ls") && !line.StartsWith("dir"))
    {
        var parts = line.Split(' ');
        current.AddFile(new(parts[1], Int32.Parse(parts[0])));
    }
}

var free = 70_000_000 - root.Size;
var missing = 30_000_000 - free;

var allDirectories = root.GetAllDirectorySizes().ToArray()
    .OrderBy(p => p.Size).ToArray();

System.Console.WriteLine($"Part 1: {allDirectories.TakeWhile(p => p.Size < 100000).Sum(p => p.Size)}");
System.Console.WriteLine($"Part 2: {allDirectories.SkipWhile(p => p.Size < missing).First().Size}");

class Directory
{
    public Directory Parent { get; private set; }
    public string Name { get; }

    public int Size => _size.Value;
    public IEnumerable<Directory> Children => _children;
    public IEnumerable<ItemWithSize> Files => _files;

    private List<Directory> _children = new();
    private List<ItemWithSize> _files = new();
    private Lazy<int> _size;

    public Directory(string name)
        => (Name, _size)
         = (name, new Lazy<int>(() => BuildSize(this)));

    public void AddChild(Directory child)
    {
        ThrowIfAlreadyLocked();
        _children.Add(child);
        child.Parent = this;
    }

    public void AddFile(ItemWithSize file)
    {
        ThrowIfAlreadyLocked();
        _files.Add(file);
    }

    public IEnumerable<ItemWithSize> GetAllDirectorySizes()
    {
        yield return new(Name, Size);
        foreach (var child in Children.SelectMany(p => p.GetAllDirectorySizes()))
            yield return new(child.Name, child.Size);
    }

    private static int BuildSize(Directory dir)
     => dir.Children.Sum(p => p.Size) + dir.Files.Sum(p => p.Size);

    private void ThrowIfAlreadyLocked()
    {
        if (_size.IsValueCreated) throw new InvalidOperationException("Tree size has been accessed. Tree structure is locked.");
    }

}

record ItemWithSize(string Name, int Size);