namespace ComicGet.Communication;

public interface IIssueDownload : IDisposable
{
    string FileName { get; }
    Task<Stream> GetStreamAsync();
}
