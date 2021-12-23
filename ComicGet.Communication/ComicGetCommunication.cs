namespace ComicGet.Communication;

public class ComicGetCommunication : IComicGetCommunication
{
    public ComicGetCommunication(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default)
    {
        var html = await _httpClient.GetStringAsync(PacksUri, ct).ConfigureAwait(false);
        return Enumerable.Empty<WeeklyPack>();
    }

    private readonly HttpClient _httpClient;

    private static readonly string PacksUri = "https://getcomics.info/";
}
