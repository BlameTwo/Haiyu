using Haiyu.Plugin.Services;
using KuroGameDownloadProgram;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.GameContext;
using Waves.Core.GameContext.Contexts;
using Waves.Core.Models.Downloader;

#region 高速测试下载

//DownloadClient client = new DownloadClient();
//Console.WriteLine("输入鸣潮版本Url资源：");
//var url = Console.ReadLine();
//var resource = await client.GetVersionResource(url);
//Console.WriteLine("输入鸣潮资源下载基地址:");
//var baseUrl = Console.ReadLine();
//Console.WriteLine("输入文件下载地址");
//var folder = Console.ReadLine();
//var resourceUrl = "https://pcdownload-aliyun.aki-game.com/launcher/game/G152/10003/2.7.1/VNpaiLhzRkbHBNdinWAGQduNUBYeHaTD/resource.json";
//var baseUrl = "https://pcdownload-aliyun.aki-game.com//launcher/game/G152/10003/2.7.1/VNpaiLhzRkbHBNdinWAGQduNUBYeHaTD/zip";
//var folder = "E:\\Barkup\\TestDownload";
////2.6.2
//var resource = await client.GetVersionResource(resourceUrl!);
//client.InitDownload(resource, baseUrl, folder);
//await client.WaitDownloadAsync();
//Console.WriteLine("下载完成！");
#endregion

GameContextFactory.GameBassPath =
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Waves";
IServiceProvider serviceProvider = new ServiceCollection().AddGameContext().BuildServiceProvider();
var mainContext = serviceProvider.GetRequiredKeyedService<IGameContext>(
    nameof(WavesMainGameContext)
);
await mainContext.InitAsync();
var biliContext = serviceProvider.GetRequiredKeyedService<IGameContext>(
    nameof(WavesBiliBiliGameContext)
);
await biliContext.InitAsync();
GameServerSwitchTool tool = new GameServerSwitchTool();
var result = await tool.AnalyseAsync(mainContext, biliContext);
Console.WriteLine(
    $"分析结果\r\n新增文件个数:{result.AddFiles.Count}，重写文件个数：{result.RewriterFiles.Count},删除文件个数：{result.DeleteFiles.Count},无变化文件个数:{result.UnchangedFiles.Count}，\r\n转换度:{result.ScoreValue}，评判:{result.IsSwitch}"
);
