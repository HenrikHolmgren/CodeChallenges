using var client = new AoC.Api.AoCClient();
var input = await client.GetLinesAsync(2021, 3);
var bitmask = (1<<input[0].Length)-1;

var γ = 0;

for(int i = 0; i < input[0].Length; i++) { 
    γ <<= 1;
    if(input.Count(p=>p[i]=='1') > input.Length / 2)
        γ++;
}

var ε = ~γ & bitmask;

System.Console.WriteLine($"Part 1: {γ * ε}");

string Reduce(string[] source, char target)
{
    var limit = source.First().Length;
    for(int i = 0; i < limit; i++){            
        if(source.Count(p => p[i] == target) == source.Count(p => p[i] != target))
            source = source.Where(p=>p[i] == target).ToArray();
        else if(source.Count(p=>p[i]==target) > source.Length/2)
            source = source.Where(p=>p[i] == '1').ToArray();
        else 
            source = source.Where(p=>p[i] == '0').ToArray();

        if(source.Count() == 1)
            return source.Single();
    }

    return source.Single();        
}

var O2 = Convert.ToInt32(Reduce(input, '1'), 2);
var CO2 = Convert.ToInt32(Reduce(input, '0'), 2);

Console.WriteLine($"Part 2: {O2 * CO2}");