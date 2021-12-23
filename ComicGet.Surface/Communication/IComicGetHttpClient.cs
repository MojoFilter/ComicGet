namespace ComicGet.Communication;

public interface IComicGetHttpClient
{
    Task<string> GetWeeklyPackIndexPageAsync(CancellationToken ct = default);
}
