using System.Reactive.Subjects;

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

    public async Task<string> GetWeeklyPackDetailPageAsync(WeeklyPack pack, CancellationToken ct = default)
    {
        return await _httpClient.GetStringAsync(pack.url, ct).ConfigureAwait(false);
    }

    public async Task<string> GetIssueDetailPageAsync(Issue issue, CancellationToken ct = default)
    {
        return await _httpClient.GetStringAsync(issue.Url, ct).ConfigureAwait(false);
    }

    public async Task<IIssueDownload> DownloadBookAsync(string bookName, string downloadLink, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(downloadLink, ct).ConfigureAwait(false);
        return new IssueDownload(bookName, response);
        
    }

    private readonly HttpClient _httpClient;
    private static readonly string PacksUri = "https://getcomics.info/";

    private class IssueDownload : IIssueDownload
    {
        public IssueDownload(string bookName, string libraryFolderPath, HttpResponseMessage response)
        {
            this.FileName = response.Content.Headers.ContentDisposition?.Name ?? bookName;
            _httpResponse = response;
            _libraryFolderPath = libraryFolderPath;
        }

        public string FileName { get; }

        public void Dispose()
        {
            _httpResponse?.Dispose();
            _httpResponse = null;
        }

        public Task<Stream> GetStreamAsync()
        {
            return _httpResponse!.Content.ReadAsStreamAsync();
        }

        private async void Start()
        {
            var buffer = new byte[ReadBufferLength];
            var bytesRead = 0L;
            var totalBytesRead = 0L;
            using var downloadStream = await _httpResponse!.Content.ReadAsStreamAsync();
            using var fileStream = File.Create(Path.Combine(_libraryFolderPath, this.FileName));
            do
            {
                bytesRead = await downloadStream.ReadAsync(buffer).ConfigureAwait(false);
                if (bytesRead > 0)
                {
                    await fileStream.WriteAsync(buffer).ConfigureAwait(false);
                    totalBytesRead += bytesRead;
                }
            }
            while (bytesRead > 0);
        }

        private readonly ISubject<float> _progress;
        private HttpResponseMessage? _httpResponse;
        private readonly string _libraryFolderPath;

        private static readonly int ReadBufferLength = 8192;
    }
}
