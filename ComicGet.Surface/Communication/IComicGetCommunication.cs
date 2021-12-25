namespace ComicGet.Communication;

public interface IComicGetCommunication
{
    Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default);

    Task<IEnumerable<Issue>> GetWeeklyPackIssuesAsync(WeeklyPack pack, CancellationToken ct = default);
}
