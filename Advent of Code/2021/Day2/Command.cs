public record Command(string Direction, int Length)
{
    public static Command Parse(string str)
    {
        var items = str.Split(' ');
        return new Command(items[0], Int32.Parse(items[1]));
    }

    public int Vertical => Direction switch { "down" => Length, "up" => -Length, _ => 0 };
    public int Horizontal => Direction switch { "forward" => Length, _ => 0 };
}