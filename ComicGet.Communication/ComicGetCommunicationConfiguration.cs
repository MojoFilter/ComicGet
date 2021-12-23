using Microsoft.Extensions.DependencyInjection;

namespace ComicGet.Communication;

public static class ComicGetCommunicationConfiguration
{
    public static IServiceCollection AddComicGetCommunication(this IServiceCollection services) =>
        services.AddTransient<IComicGetCommunication, ComicGetCommunication>()
                .AddTransient<HttpClient>();
}
