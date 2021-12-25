using System.Text.RegularExpressions;
using System.Web;

namespace ComicGet.Communication;

public class ComicGetCommunication : IComicGetCommunication
{
    public ComicGetCommunication(IComicGetHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default)
    {
        var html = await _httpClient.GetWeeklyPackIndexPageAsync(ct).ConfigureAwait(false);
        var pattern = @"<a href=""(?<url>https://getcomics.info/other-comics/[\d-]+-weekly-pack/)"">(?<name>[\d\.]* Weekly Pack)</a>";
        return from match in Regex.Matches(html, pattern)
               let name = match.Groups["name"].Value
               let url = match.Groups["url"].Value
               select new WeeklyPack(name, url);
    }

    public async Task<IEnumerable<Issue>> GetWeeklyPackIssuesAsync(WeeklyPack pack, CancellationToken ct = default)
    {
        var html = await _httpClient.GetWeeklyPackDetailPageAsync(pack, ct).ConfigureAwait(false);
        var publisherPattern = @"<h3><span style=""color: #ff0000;"">(?<publisher>.*?)</span></h3><ul>(?<books>.*?)</ul>";
        var issuePattern = @"<li><strong>(?<title>.*?) : <span [^>]*?><a .+?href=""(?<url>[^""]+)";

        var publisherMatches = Regex.Matches(html, publisherPattern);
        return from publisherMatch in publisherMatches
               from issueMatch in Regex.Matches(publisherMatch.Value, issuePattern)
               select new Issue(publisherMatch.Groups["publisher"].Value,
                           HttpUtility.HtmlDecode(issueMatch.Groups["title"].Value),
                           issueMatch.Groups["url"].Value);
    }

    private readonly IComicGetHttpClient _httpClient;

}
