using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
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

    public void InitService()
    {
    }

    [TestMethod]
    public async Task Test1()
    {
        GameWikiClient client = new GameWikiClient();
        var result =  await client.GetEventDataAsync(Waves.Api.Models.Enums.WikiType.Waves);
    }
}
