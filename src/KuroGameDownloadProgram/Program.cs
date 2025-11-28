using KuroGameDownloadProgram;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.GameContext;
using Waves.Core.GameContext.Contexts;
using Waves.Core.Models.Downloader;

DownloadClient client = new DownloadClient();
//Console.WriteLine("输入鸣潮版本Url资源：");
//var url = Console.ReadLine();
//var resource = await client.GetVersionResource(url);
//Console.WriteLine("输入鸣潮资源下载基地址:");
//var baseUrl = Console.ReadLine();
//Console.WriteLine("输入文件下载地址");
//var folder = Console.ReadLine();
var resourceUrl = "https://pcdownload-aliyun.aki-game.com/launcher/game/G152/10003/2.7.1/VNpaiLhzRkbHBNdinWAGQduNUBYeHaTD/resource.json";
var baseUrl = "https://pcdownload-aliyun.aki-game.com//launcher/game/G152/10003/2.7.1/VNpaiLhzRkbHBNdinWAGQduNUBYeHaTD/zip";
var folder = "E:\\Barkup\\TestDownload";
var resource = await client.GetVersionResource(resourceUrl!);
client.InitDownload(resource, baseUrl, folder);
await client.WaitDownloadAsync();
Console.WriteLine("下载完成！");

