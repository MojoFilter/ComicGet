namespace ComicGet.Communication;

public interface IComicGetHttpClient
{
    Task<string> GetWeeklyPackIndexPageAsync(CancellationToken ct = default);
    Task<string> GetWeeklyPackDetailPageAsync(WeeklyPack pack, CancellationToken ct = default);
}
