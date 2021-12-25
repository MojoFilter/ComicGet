namespace ComicGet.Communication;

internal class ComicGetHttpClient : IComicGetHttpClient
{
    public ComicGetHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetWeeklyPackIndexPageAsync(CancellationToken ct = default)
    {
        return await _httpClient.GetStringAsync(PacksUri, ct).ConfigureAwait(false);
    }

    public Task<string> GetWeeklyPackDetailPageAsync(WeeklyPack pack, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    private readonly HttpClient _httpClient;
    private static readonly string PacksUri = "https://getcomics.info/";


}
