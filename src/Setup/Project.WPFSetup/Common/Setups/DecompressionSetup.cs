using System.IO;
using System.IO.Compression;
using SharpCompress.Archives.SevenZip;

namespace Project.WPFSetup.Common.Setups;

public class DecompressionSetup : ISetup
{
    /// <summary>
    /// 解压资源
    /// </summary>
    public DecompressionSetup() { }

    public string SetupName => "释放资源";

    public int MaxProgress => 100;

    /// <summary>
    /// 释放文件
    /// </summary>
    /// <param name="fileBuffer"></param>
    /// <param name="rootDir"></param>
    /// <param name="process"></param>
    /// <returns></returns>
    async Task<(string, bool)> ExtractFileAsync(
        byte[] fileBuffer,
        string rootDir,
        IProgress<(double, string)> progress
    )
    {
        try
        {
            using (var memoryStream = new MemoryStream(fileBuffer))
            using (var archive = SevenZipArchive.Open(memoryStream))
            {
                long totalBytes = archive.Entries.Sum(x => x.Size);
                long processedBytes = 0;
                byte[] buffer = new byte[81920];

                foreach (var entry in archive.Entries)
                {
                    if (entry.IsDirectory)
                    {
                        string path = Path.Combine(rootDir, entry.Key);
                    }
                    else
                    {
                        string path = Path.Combine(rootDir, entry.Key);
                        if (File.Exists(path))
                            File.Delete(path);
                        string dir = Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        using (var entryStream = entry.OpenEntryStream())
                        using (
                            var fileStream = new FileStream(
                                path,
                                FileMode.Create,
                                FileAccess.Write,
                                FileShare.None,
                                buffer.Length,
                                FileOptions.Asynchronous | FileOptions.SequentialScan
                            )
                        )
                        {
                            while (true)
                            {
                                int bytesRead = await entryStream
                                    .ReadAsync(buffer, 0, buffer.Length)
                                    .ConfigureAwait(false);
                                if (bytesRead == 0)
                                    break;
                                await fileStream
                                    .WriteAsync(buffer, 0, bytesRead)
                                    .ConfigureAwait(false);
                                processedBytes += bytesRead;
                                double percentComplete =
                                    (double)processedBytes / totalBytes * 100.0;
                                progress?.Report((percentComplete, SetupName));
                            }
                        }
                    }
                }
            }
            return ("", true);
        }
        catch (Exception ex)
        {
            return (ex.Message, false);
        }
    }

    public async Task<(string, bool)> ExecuteAsync(
        SetupProperty property,
        IProgress<(double, string)> progress,
        int maxValue
    )
    {
        return await ExtractFileAsync(
                Resources.Resource1.InstallFile,
                property.InstallPath,
                progress
            )
            .ConfigureAwait(false);
    }
}
