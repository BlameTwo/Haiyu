using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.GameContext;
using Waves.Core.GameContext.Contexts;
IServiceProvider service = new ServiceCollection().AddGameContext().BuildServiceProvider();
var main = service.GetKeyedService<IGameContext>(nameof(MainGameContext))!;

await main.InitAsync();
var versions = await main.GetGameLauncherSourceAsync();
Console.WriteLine("");