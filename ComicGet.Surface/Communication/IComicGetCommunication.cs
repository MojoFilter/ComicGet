namespace ComicGet.Communication;

public interface IComicGetCommunication
{
    Task<IEnumerable<WeeklyPack>> GetWeeklyPacksAsync(CancellationToken ct = default);
}
