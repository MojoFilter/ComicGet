using ComicGet.Communication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ComicGet.Test
{
    [TestClass]
    public class ComicGetCommunicationTest
    {
        [TestMethod]
        public async Task GetWeeklyPacksGetsWeeklyPacks()
        {
            var provider = new ServiceCollection()
                .AddComicGetCommunication()
                .BuildServiceProvider();

            var comicsClient = provider.GetRequiredService<IComicGetCommunication>();
            var weeklies = await comicsClient.GetWeeklyPacksAsync().ConfigureAwait(false);
        }
    }
}