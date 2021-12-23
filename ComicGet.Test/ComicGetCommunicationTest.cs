using ComicGet.Communication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComicGet.Test
{
    [TestClass]
    public class ComicGetCommunicationTest
    {
        [TestMethod]
        public async Task GetWeeklyPacksGetsWeeklyPacks()
        {
            var expectedWeeklies = new WeeklyPack[]
            {
                new("2021.12.22 Weekly Pack", "https://getcomics.info/other-comics/2021-12-22-weekly-pack/"),
                new("2021.12.15 Weekly Pack", "https://getcomics.info/other-comics/2021-12-15-weekly-pack/"),
                new("2021.12.08 Weekly Pack", "https://getcomics.info/other-comics/2021-12-08-weekly-pack/"),
                new("2021.12.22 Weekly Pack","https://getcomics.info/other-comics/2021-12-22-weekly-pack/")
            };

            var httpMock = new Mock<IComicGetHttpClient>();
            httpMock.Setup(x => x.GetWeeklyPackIndexPageAsync(It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(() =>
               {
                   var files = this.GetType().Assembly.GetManifestResourceNames();
                   using var stream = this.GetType().Assembly.GetManifestResourceStream("ComicGet.Test.WeeklyPacks.html");
                   using var reader = new StreamReader(stream!);
                   return reader.ReadToEnd();
               }));

            var provider = new ServiceCollection()
                .AddComicGetCommunication()
                .AddSingleton(httpMock.Object)
                .BuildServiceProvider();

            var comicsClient = provider.GetRequiredService<IComicGetCommunication>();
            var weeklies = await comicsClient.GetWeeklyPacksAsync().ConfigureAwait(false);
            Assert.IsTrue(expectedWeeklies.SequenceEqual(weeklies));
        }
    }
}