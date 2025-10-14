using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.GameContext;
using Waves.Core.Services;

namespace Project.Test;

[TestClass()]
public class NewCoreTest
{
    IServiceProvider? Provider { get; set; }
    public static string BassFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";

    public void InitService() { }

    [TestMethod]
    public async Task Test1()
    {
        GameWikiClient client = new GameWikiClient();
        var result = await client.GetEventDataAsync(Waves.Api.Models.Enums.WikiType.Waves);
        List<SideEventDataWrapper> wrappers = new();
        foreach (var item in result)
        {
            wrappers.Add(
                new SideEventDataWrapper()
                {
                    Title = item.Title,
                    ImageUrl = item.ContentUrl,
                    StartTime = item.CountDown.DateRange[0],
                    EndTime = item.CountDown.DateRange[1],
                    TotalSpan = (
                        DateTime.Parse(item.CountDown.DateRange[1]) - DateTime.Now
                    ).ToString(),
                }
            );
        }
    }

    public partial class SideEventDataWrapper
    {
        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string TotalSpan { get; set; }
    }
}
