using ComicGet.Ui;
using Microsoft.Extensions.DependencyInjection;

namespace ComicGet;

public static class Configuration
{
    public static IServiceCollection AddComicGetUi(this IServiceCollection services) =>
        services.AddTransient<IMainViewModel, MainViewModel>()
                .AddTransient<IOptionFactory, OptionFactory>();
}
