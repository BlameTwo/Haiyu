using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using WutheringWavesTool.Helpers;

namespace WutheringWavesTool.Services;

public delegate void WallpaperPletteChangedDelegate(object sender, PletteArgs color);

public class WallpaperService : IWallpaperService
{
    private WallpaperPletteChangedDelegate? wallpaperPletteChangedDelegate;

    public WallpaperService(ITipShow tipShow)
    {
        TipShow = tipShow;
        this.ColorPlette = new OctreeColorExtractor();
    }

    public event WallpaperPletteChangedDelegate WallpaperPletteChanged
    {
        add => wallpaperPletteChangedDelegate += value;
        remove => wallpaperPletteChangedDelegate -= value;
    }

    /// <summary>
    /// 切换壁纸自动取色开关
    /// </summary>
    public bool PletteEnable { get; set; } = true;

    public string BaseFolder { get; private set; }
    public Controls.ImageEx ImageHost { get; private set; }
    public ITipShow TipShow { get; }
    public string NowHexValue { get; private set; }

    public void RegisterHostPath(string folder)
    {
        this.BaseFolder = folder;
    }

    public async Task RegisterImageHostAsync(Controls.ImageEx image)
    {
        this.ImageHost = image;
        if (!string.IsNullOrWhiteSpace(AppSettings.WallpaperPath))
        {
            await this.SetWallpaperAsync(AppSettings.WallpaperPath);
        }
        else
        {
            //await this.SetWallpaperAsync(
            //    AppDomain.CurrentDomain.BaseDirectory + "Assets\\Images\\changli.png"
            //);
            TipShow.ShowMessage("当前为默认壁纸，请前往设置更换", Symbol.Pictures);
        }
    }

    public async Task<bool> SetWallpaperAsync(string path)
    {
        try
        {
            var result = await ImageIOHelper.HexImageAsync(this.BaseFolder, path);
            if (this.PletteEnable)
            {
                var color = await this.ColorPlette.GetThemeColorAsync(
                    await result.Item2.GetImageDataAsync()
                );

                this.wallpaperPletteChangedDelegate?.Invoke(
                    this,
                    new PletteArgs(color.Item1.Value, color.Item2[0], color.Item2[1])
                );
            }
            if (result.Item1 != null)
            {
                this.ImageHost.Source = result.Item1;
                this.NowHexValue = result.Item3!;
                AppSettings.WallpaperPath = result.Item2;
                return true;
            }
            else
            {
                TipShow.ShowMessage(result.Item2, Microsoft.UI.Xaml.Controls.Symbol.Pictures);
                return false;
            }
        }
        catch (Exception ex)
        {
            TipShow.ShowMessage($"图片路径或格式不合法,{ex.Message}", Symbol.Pictures);
            return false;
        }
    }

    public OctreeColorExtractor ColorPlette { get; private set; }

    //public ImageColorPaletteHelper ColorPlette { get; private set; }



    public async IAsyncEnumerable<WallpaperModel> GetFilesAsync(
        [EnumeratorCancellation] CancellationToken token = default
    )
    {
        List<WallpaperModel> models = new();
        var folder = new DirectoryInfo(this.BaseFolder);
        using (MD5 md5 = MD5.Create())
        {
            var files = Directory
                .GetFiles(this.BaseFolder, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s =>
                    s.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    || s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                );
            foreach (var item in files)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                using (
                    var stream = new FileStream(
                        item,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read,
                        bufferSize: 4096,
                        useAsync: true
                    )
                )
                {
                    byte[] hashBytes = await md5.ComputeHashAsync(stream);
                    var md5Value = BitConverter
                        .ToString(hashBytes)
                        .Replace("-", "")
                        .ToLowerInvariant();
                    var softImage = await ImageIOHelper.ConvertBitmapImageAsync(
                        await stream.ConvertStreamToRandomAccessStream(),
                        400
                    );
                    if (softImage == null)
                        continue;
                    yield return new()
                    {
                        FilePath = item,
                        Image = softImage,
                        Md5String = md5Value,
                    };
                }
            }
        }
    }
}
