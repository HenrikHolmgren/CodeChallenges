//Skipped parser for this one to start with.
System.Console.WriteLine($"Part 1: {RunTest(true, 20, GetProductionMonkeys())}");
System.Console.WriteLine($"Part 2: {RunTest(false, 10000, GetProductionMonkeys())}");

long RunTest(bool decreaseWorry, int rounds, Monkey[] Monkeys)
{
    //Lucky these are all primes, huh?
    var normalizationFactor = Monkeys.Aggregate(1, (p, q) => p * q.Test); 
    
    for (int round = 0; round < rounds; round++)
    {
        foreach (var monkey in Monkeys)
        {
            foreach (var item in monkey.Items)
            {
                long test = monkey.Operation(item);
                if (decreaseWorry)
                    test /= 3;
                test %= normalizationFactor;
                if (test % monkey.Test == 0)
                    Monkeys[monkey.TargetIfTrue].Items.Add(test);
                else
                    Monkeys[monkey.TargetIfFalse].Items.Add(test);
                monkey.Inspections++;
            }
            monkey.Items.Clear();
        }
    }
    var monkeyBusiness = Monkeys.Select(p => p.Inspections)
        .OrderByDescending(p => p)
        .Take(2)
        .Aggregate(1L, (p, q) => p * q);

    return monkeyBusiness;
}

Monkey[] GetTestMonkeys() => new Monkey[]{
    new Monkey{ Items=new List<long> {79,98},       Operation = p=>p*19, Test = 23, TargetIfTrue = 2, TargetIfFalse = 3 },
    new Monkey{ Items=new List<long> {54,65,75,74}, Operation = p=>p+6,  Test = 19, TargetIfTrue = 2, TargetIfFalse = 0 },
    new Monkey{ Items=new List<long> {79,60,97},    Operation = p=>p*p,  Test = 13, TargetIfTrue = 1, TargetIfFalse = 3 },
    new Monkey{ Items=new List<long> {74},          Operation = p=>p*3,  Test = 17, TargetIfTrue = 0, TargetIfFalse = 1 },
};

Monkey[] GetProductionMonkeys() => new Monkey[]
{
    new Monkey{ Items=new List<long> {73,77},                   Operation = p=>p*5,  Test = 11, TargetIfTrue = 6, TargetIfFalse = 5 },
    new Monkey{ Items=new List<long> {57,88,80},                Operation = p=>p+5,  Test = 19, TargetIfTrue = 6, TargetIfFalse = 0 },
    new Monkey{ Items=new List<long> {61,81,84,69,77,88},       Operation = p=>p*19, Test = 5,  TargetIfTrue = 3, TargetIfFalse = 1 },
    new Monkey{ Items=new List<long> {78,89,71,60,81,84,87,75}, Operation = p=>p+7,  Test = 3,  TargetIfTrue = 1, TargetIfFalse = 0 },
    new Monkey{ Items=new List<long> {60,76,90,63,86,87,89},    Operation = p=>p+2,  Test = 13, TargetIfTrue = 2, TargetIfFalse = 7 },
    new Monkey{ Items=new List<long> {88},                      Operation = p=>p+1,  Test = 17, TargetIfTrue = 4, TargetIfFalse = 7 },
    new Monkey{ Items=new List<long> {84,98,78,85},             Operation = p=>p*p,  Test = 7,  TargetIfTrue = 5, TargetIfFalse = 4 },
    new Monkey{ Items=new List<long> {98,89,78,73,71},          Operation = p=>p+4,  Test = 2,  TargetIfTrue = 3, TargetIfFalse = 2 },
};

class Monkey
{
    public List<long> Items { get; set; }
    public Func<long, long> Operation { get; set; }
    public int Test { get; set; }
    public long TargetIfTrue { get; set; }
    public long TargetIfFalse { get; set; }
    public int Inspections { get; set; }
}