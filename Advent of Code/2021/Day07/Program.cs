using var client = new AoC.Api.AoCClient();
var input = await client.GetNumbersAsync(2021, 7);

int FuelUse(int position) => input.Sum(p=>Math.Abs(p-position));
int FuelUseCompound(int position) => input.Sum(p=>(Math.Abs(p-position)+1)  * Math.Abs(p-position) / 2);

var median = input.OrderBy(p=>p).Skip(input.Length/2).First();
Console.WriteLine($"Part 1: {FuelUse(median)}");

var guess = median;
var compoundUse = FuelUseCompound(guess);
var seekDirection = compoundUse > FuelUseCompound(guess-1) ? -1 : 1;

while(compoundUse > FuelUseCompound(guess + seekDirection)){
    guess += seekDirection;
    compoundUse = FuelUseCompound(guess);
}

Console.WriteLine($"Part 2: {compoundUse}");