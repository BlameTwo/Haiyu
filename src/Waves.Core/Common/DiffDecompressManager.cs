using System;
using System.Diagnostics;

namespace Waves.Core.Common;

public class DiffDecompressManager
{
    private string sharedKey;
    private SharedMemory _sharedMemory;
    private Process _process;
    ManualResetEventSlim _processExited = new ManualResetEventSlim(false);
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
        try
        {
            sharedKey = $"launcher_shared_memory_{Process.GetCurrentProcess().Id}_{Guid.NewGuid().ToString("N")}";
            _sharedMemory = new SharedMemory(sharedKey, 4096);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(
                AppDomain.CurrentDomain.BaseDirectory + @"Assets\HpatchzResource\hpatchz.exe",
                new string[6] { OldFolder, DiffFile, NewFolder, "-f", "-d", "-k-" + sharedKey }
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
                    progress.Report((values[4], values[5]));
                }
            }
            await _process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生异常: {ex.Message}");
        }
        finally
        {
            _process.Dispose();
            _processExited.Dispose();
            _sharedMemory.Dispose();
        }

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
        _processExited.Set(); // 触发退出信号
    }

}
