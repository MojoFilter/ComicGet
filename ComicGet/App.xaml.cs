using ComicGet.Communication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace ComicGet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) => 
                {
                    services.AddComicGetUi()
                            .AddComicGetCommunication()
                            .AddTransient<MainWindow>();
                })
                .UseDefaultServiceProvider(o => o.ValidateOnBuild = true)
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.MainWindow = _host.Services.GetRequiredService<MainWindow>();
            this.MainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync().ConfigureAwait(true);
            }
        }

        private readonly IHost _host;
    }
}
