using var client = new AoC.Api.AoCClient();
var input = (await client.GetRawInputAsync(2017, 01)).Select(p=>p-'0').ToArray();

System.Console.WriteLine("Part 1: " + GetCaptchaSum(1));
System.Console.WriteLine("Part 2: " + GetCaptchaSum(input.Length/2));

int GetCaptchaSum(int offset)
{
    var sum = 0;

    for (var i = 0; i < input.Length; i++)
    {
        if (input[i] == input[(i + offset) % input.Length])
            sum += input[i];
    }

    return sum;
}