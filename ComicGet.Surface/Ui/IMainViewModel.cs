namespace ComicGet.Ui;

public interface IMainViewModel
{
    string Title { get; }
    IEnumerable<Issue> Issues { get; }
}
