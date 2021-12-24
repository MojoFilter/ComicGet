namespace ComicGet.Communication;

public interface IComicGetCommunication
{
    Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default);

    Task<IEnumerable<Issue>> GetWeeklyPackIssuesAsync(CancellationToken ct = default);
}
