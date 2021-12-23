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

    private readonly IComicGetHttpClient _httpClient;

}
