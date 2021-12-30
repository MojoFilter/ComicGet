using ComicGet.Communication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComicGet.Test;

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

        var comicsClient = this.GetCommunication();
        var weeklies = await comicsClient.GetWeeklyPacksAsync().ConfigureAwait(false);
        Assert.IsTrue(expectedWeeklies.SequenceEqual(weeklies));
    }


    [TestMethod]
    public async Task GetIssuesFromWeeklyParses()
    {
        var client = this.GetCommunication();
        var allIssues = await client.GetWeeklyPackIssuesAsync(default).ConfigureAwait(false);
        var issues = allIssues.DistinctBy(x => x.Publisher);

        Assert.IsTrue(ExpectedIssues.SequenceEqual(issues));
    }


    [TestMethod]
    public async Task IssueCanDownload()
    {
        var issue = ExpectedIssues.First();
        var client = this.GetCommunication(false);
        using var download = await client.GetIssueDownloadAsync(issue).ConfigureAwait(false);
        using var data = new MemoryStream();
        using (var downloadStream = await download.GetStreamAsync().ConfigureAwait(false))
        {
            await downloadStream.CopyToAsync(data).ConfigureAwait(false);
        }

        data.Position = 0;
        using var reader = new StreamReader(data);
        var content = await reader.ReadToEndAsync().ConfigureAwait(false);
        Assert.AreEqual(ExpectedIssueContent, content);
    }

    private IComicGetCommunication GetCommunication(bool mockHttpClient = true)
    {
        Task<string> html(string filename) => Task.Run(() =>
        {
            using var stream = this.GetType().Assembly.GetManifestResourceStream($"ComicGet.Test.{filename}.html");
            using var reader = new StreamReader(stream!);
            return reader.ReadToEnd();
        });

        var services = new ServiceCollection().AddComicGetCommunication();
        if (mockHttpClient)
        {
            var httpMock = new Mock<IComicGetHttpClient>();
            httpMock.Setup(x => x.GetWeeklyPackIndexPageAsync(It.IsAny<CancellationToken>()))
                    .Returns(() => html("WeeklyPacks"));

            httpMock.Setup(x => x.GetWeeklyPackDetailPageAsync(It.IsAny<WeeklyPack>(), It.IsAny<CancellationToken>()))
                    .Returns(() => html("WeekPublishers"));

            var downloadMock = new Mock<IIssueDownload>().SetupProperty(x => x.FileName, ExpectedIssueName);
            downloadMock.Setup(x => x.GetStreamAsync()).ReturnsAsync(() => new MemoryStream(ExpectedIssueData));
            httpMock.Setup(x => x.DownloadBookAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(downloadMock.Object));

            services.AddSingleton(httpMock.Object);
        }
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IComicGetCommunication>();
    }

    private static readonly Issue[] ExpectedIssues = new Issue[]
    {
        new("DC COMICS", "Batman – Catwoman #9", "https://getcomics.info/dc/batman-catwoman-9-2021/"),
        new("MARVEL COMICS", "Amazing Spider-Man #82", "https://getcomics.info/marvel/amazing-spider-man-82-2021/"),
        new("IMAGE COMICS", "A Righteous Thirst for Vengeance #3", "https://getcomics.info/other-comics/a-righteous-thirst-for-vengeance-3-2021/")
    };

    private static readonly string ExpectedIssueName = "Your Mom - The Revenge.cbr";
    private static readonly string ExpectedIssueContent = "Your mom";
    private static readonly byte[] ExpectedIssueData = Encoding.UTF8.GetBytes(ExpectedIssueContent);
}
