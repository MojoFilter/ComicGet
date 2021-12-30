namespace ComicGet.Communication;

public interface IComicGetCommunication
{
    Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default);

    Task<IEnumerable<Issue>> GetWeeklyPackIssuesAsync(WeeklyPack pack, CancellationToken ct = default);

    Task<IIssueDownload> GetIssueDownloadAsync(Issue issue, CancellationToken ct = default);
}
