namespace ComicGet.Communication;

public interface IComicGetHttpClient
{
    Task<string> GetWeeklyPackIndexPageAsync(CancellationToken ct = default);
    Task<string> GetWeeklyPackDetailPageAsync(WeeklyPack pack, CancellationToken ct = default);
    Task<string> GetIssueDetailPageAsync(Issue issue, CancellationToken ct = default);
    Task<IIssueDownload> DownloadBookAsync(string bookName, string downloadLink, CancellationToken ct);
}
