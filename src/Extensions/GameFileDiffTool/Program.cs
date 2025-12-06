using GameFileDiffTool;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.GameContext;
using Waves.Core.GameContext.Contexts;
using Waves.Core.Models.Downloader;

IServiceProvider provider = new ServiceCollection().AddGameContext().BuildServiceProvider();
var main = provider.GetKeyedService<IGameContext>(nameof(WavesMainGameContext))!;
var bilibili = provider.GetKeyedService<IGameContext>(nameof(WavesBiliBiliGameContext))!;
await main.InitAsync();
await bilibili.InitAsync();
var mainLauncher = await main.GetGameLauncherSourceAsync()!;
var bilibiliLauncher = await bilibili.GetGameLauncherSourceAsync()!;
var mainResource = await main.GetGameResourceAsync(mainLauncher.ResourceDefault);
var bilibiliResource = await bilibili.GetGameResourceAsync(bilibiliLauncher.ResourceDefault);


Console.ReadKey();