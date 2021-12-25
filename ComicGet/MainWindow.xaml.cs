using ComicGet.Ui;
using ReactiveUI;
using System.Windows;

namespace ComicGet;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IViewFor<IMainViewModel>
{
    public MainWindow(IMainViewModel vm)
    {
        InitializeComponent();
        this.ViewModel = vm;
        this.WhenActivated(d => { });
    }

    public IMainViewModel? ViewModel
    {
        get => this.DataContext as IMainViewModel;
        set => this.DataContext = value;
    }

    object? IViewFor.ViewModel
    {
        get => this.ViewModel;
        set => this.ViewModel = value as IMainViewModel;
    }
}
