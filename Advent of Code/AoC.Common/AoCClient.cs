namespace AoC.Api;
public class AoCClient : IDisposable
{
    private string _sessionId;
    private Lazy<HttpClient> _client;
    private string _cacheLocation;
    private bool disposedValue;

    private HttpClient ConfigureClient()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://adventofcode.com");
        client.DefaultRequestHeaders.Add("cookie", $"session={_sessionId}");
        return client;
    }

    public AoCClient() : this(Environment.GetEnvironmentVariable("AoCSessionId") ?? throw new ArgumentNullException("Either use the string overload or configure the AoCSessionId environment variable.")) { }

    public AoCClient(string sessionID)
        => (_sessionId, _client, _cacheLocation) = (sessionID, new Lazy<HttpClient>(ConfigureClient), Path.GetTempPath());

    public AoCClient WithCacheLocation(string cacheLocation)
        => new AoCClient(_sessionId)
        {
            _cacheLocation = cacheLocation
        };

    public Task<string> GetRawInputAsync(int year, int day)
        => EnsureCachedAsync(year, day, "input", async () => await _client.Value.GetStringAsync($"{year}/day/{day}/input"));

    public async Task<string[]> GetLinesAsync(int year, int day, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
    {
        var content = await GetRawInputAsync(year, day);
        return content.Split(new[] { '\r', '\n' }, splitOptions);
    }

    public async Task<int[]> GetNumbersAsync(int year, int day)
    {
        var content = await GetRawInputAsync(year, day);
        return content.Split(new[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(Int32.Parse).ToArray();
    }

    public async Task<T[]> GetParsedAsync<T>(int year, int day, Func<string, T> parse)
    {
        var content = await GetLinesAsync(year, day);
        return content.Select(parse).ToArray();
    }

    public async Task<int[,]> GetIntMatrixAsync(int year, int day)
    {
        var raw = await GetLinesAsync(year, day);
        if (raw.Select(p => p.Length).Distinct().Count() != 1)
            throw new InvalidOperationException("Cannot parse input as int matrix, lines have different width.");
        var res = new int[raw[0].Length, raw.Length];
        for (int j = 0; j < raw.Length; j++)
            for (int i = 0; i < raw[0].Length; i++)
                res[i, j] = raw[i][j] - '0';

        return res;
    }

    private async Task<string> EnsureCachedAsync(int year, int day, string key, Func<Task<string>> resolveFromSource)
    {
        var cacheKey = $"AOC_{year}_{day}_{key}.txt";
        var cacheFile = Path.Combine(_cacheLocation, cacheKey);
        if (File.Exists(cacheFile))
            return await File.ReadAllTextAsync(cacheFile);

        System.Console.WriteLine($"Fetching {year}/{day} data from Advent of Code server.");
        var content = await resolveFromSource();
        await File.WriteAllTextAsync(cacheFile, content);
        return content;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (_client.IsValueCreated)
                    _client.Value.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
