using ComicGet.Communication;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace ComicGet.Ui;

internal class MainViewModel : ReactiveObject, IMainViewModel, IActivatableViewModel
{
    public MainViewModel(IComicGetCommunication communication)
    {
        _issues = communication.GetWeeklyPacksAsync()
                .ToObservable()
                .Select(packs => packs.First())
                .SelectMany(pack => communication.GetWeeklyPackIssuesAsync(pack))
                .ToProperty(this, nameof(Issues));

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            
        });
    }

    public string Title => "ComicGet";

    public IEnumerable<Issue> Issues => _issues.Value;

    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    private ObservableAsPropertyHelper<IEnumerable<Issue>> _issues;
}
