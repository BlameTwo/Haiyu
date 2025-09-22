using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.GameContext;
using Waves.Core.GameContext.Contexts.Waves;

namespace Project.Test;

[TestClass()]
public class NewCoreTest
{
    IServiceProvider? Provider { get; set; }
    public static string BassFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";

    public void InitService()
    {
        GameContextFactory.GameBassPath = BassFolder;
        Provider = new ServiceCollection().AddGameContext().BuildServiceProvider();
    }

    [TestMethod]
    public async Task Test1()
    {
        InitService();
        var mainGame = Provider!.GetRequiredKeyedService<IGameContext>(nameof(MainGameContext));
        await mainGame.InitAsync();
        var result = await mainGame.GetGameLauncherSourceAsync();
    }
}
