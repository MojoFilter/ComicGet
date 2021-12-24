using System.Text.RegularExpressions;

namespace ComicGet.Communication;

public class ComicGetCommunication : IComicGetCommunication
{
    public ComicGetCommunication(IComicGetHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default)
    {
        var html = await _httpClient.GetWeeklyPackIndexPageAsync(ct);
        var pattern = @"<a href=""(?<url>https://getcomics.info/other-comics/[\d-]+-weekly-pack/)"">(?<name>[\d\.]* Weekly Pack)</a>";
        return from match in Regex.Matches(html, pattern)
               let name = match.Groups["name"].Value
               let url = match.Groups["url"].Value
               select new WeeklyPack(name, url);
    }

    public async Task<IEnumerable<Issue>> GetWeeklyPackIssuesAsync(CancellationToken ct = default)
    {
        var publisherPattern = @"<h3><span style=""color: #ff0000;"">(?<publisher>.*?)</span></h3><ul>(?<books>.*?)</ul>";
        var issuePattern = @"<li><strong>(?<title>.*?)<span [^>]*?><a .+?href=""(?<url>[^""]+?)";
        return Enumerable.Empty<Issue>();
    }

    private readonly IComicGetHttpClient _httpClient;

}
