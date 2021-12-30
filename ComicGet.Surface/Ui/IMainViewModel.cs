namespace ComicGet.Ui;

public interface IMainViewModel
{
    string Title { get; }
    IEnumerable<IOption<Issue>> Issues { get; }
}
