using System.Windows.Input;

namespace ComicGet.Ui;

internal class MainViewModel : ReactiveObject, IMainViewModel, IActivatableViewModel
{
    public MainViewModel(IComicGetCommunication communication,
        IOptionFactory optionFactory)
    {
        _issues = communication.GetWeeklyPacksAsync()
                .ToObservable()
                .Select(packs => packs.First())
                .SelectMany(pack => communication.GetWeeklyPackIssuesAsync(pack))
                .Select(pack => optionFactory.Create(pack, i => i.Name))
                .ToProperty(this, nameof(Issues));

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            
        });
    }

    public string Title => "ComicGet";

    public IEnumerable<IOption<Issue>> Issues => _issues.Value;

    public IIssueDownload[] Downloads => _downloads.Value;

    public ICommand DownloadIssuesCommand { get; }

    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    private Task<IIssueDownload[]> DownloadSelectedIssuesAsync()
    {
        var selectedBooks = this.Issues
                                .Where(o => o.IsSelected)
                                .Select(o => o.Value)
                                .ToList();
        var tasks = selectedBooks.Select(issue => _communication.GetIssueDownloadAsync(issue)).ToArray();
        return Task.WhenAll(tasks);
    }

    private readonly ObservableAsPropertyHelper<IEnumerable<IOption<Issue>>> _issues;
    private readonly ObservableAsPropertyHelper<IIssueDownload[]> _downloads;
    private readonly IComicGetCommunication _communication;
}
