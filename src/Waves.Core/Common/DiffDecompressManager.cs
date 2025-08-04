using System.Diagnostics;

namespace Waves.Core.Common;

public class DiffDecompressManager
{
    private string sharedKey;
    private SharedMemory _sharedMemory;
    private Process _process;

    public DiffDecompressManager(string oldFolder,string newFolder,string diffFile)
    {
        OldFolder = oldFolder;
        NewFolder = newFolder;
        DiffFile = diffFile;
    }

    public string OldFolder { get; }
    public string NewFolder { get; }
    public string DiffFile { get; }

    public async Task StartAsync(IProgress<(double, double)> progress)
    {
        sharedKey = $"launcher_shared_memory_{Process.GetCurrentProcess().Id}_{Guid.NewGuid().ToString("N")}";
        _sharedMemory = new SharedMemory(sharedKey, 4096);
        var oldPath = "E:\\Barkup\\2.4.1";
        var newPath = "E:\\Barkup\\NewVersion";
        var diffFile = @"E:\Barkup\NewVersion\2.4.1_2.5.0_1752551264536.krdiff";
        ;
        var sharedMemoryValue = new SharedMemory(sharedKey, 4096);
        ProcessStartInfo processStartInfo = new ProcessStartInfo(
            @"""D:\WorkSpace\WutheringWavesTool\src\WutheringWavesTool\Assets\HpatchzResource\hpatchz.exe""",
            new string[6] { oldPath, diffFile, newPath, "-f", "-d", "-k-" + sharedKey }
        //[$"-f {oldPath}", $"-d {diffFile}", $"-k- {sharedMemoryKey}"]
        )
        {
            RedirectStandardError = false,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = false,
        };
        Process _process = new Process();
        _process = new Process { StartInfo = processStartInfo, EnableRaisingEvents = true };
        _process.Exited += PatchProgressExitedEventHandler;
        if (_process.Start())
        {
            while (!_process.HasExited)
            {
                await Task.Delay(500);
                ulong[] values = GetProgress(TimeSpan.FromSeconds(1));
                Debug.WriteLine(
                    $"六个进度:当前文件{values[0]},总文件{values[1]},{values[2]},{values[3]},压缩字节{values[4]},总字节{values[5]},"
                );
            }
        }
        await _process.WaitForExitAsync();
    }

    ulong[]? GetProgress(TimeSpan? timeout = null)
    {
        if (_sharedMemory == null)
        {
            return null;
        }
        int count = 6;
        var result =  _sharedMemory.ReadUlong(0,count,out var data,timeout);
        if (result)
        {
            return data;
        }
        return null;
    }

    private void PatchProgressExitedEventHandler(object? sender, EventArgs e)
    {
    }

}
