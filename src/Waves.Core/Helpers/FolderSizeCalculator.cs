using System;
using System.Collections.Generic;
using System.Text;

namespace Waves.Core.Helpers;

public class FolderSizeCalculator
{
    /// <summary>
    /// 异步计算文件夹总大小（字节）
    /// </summary>
    /// <param name="directoryPath">文件夹路径</param>
    /// <param name="cancellationToken">取消令牌（可选）</param>
    /// <returns>文件夹总大小（字节）</returns>
    public static async Task<long> CalculateFolderSizeAsync(string directoryPath, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException("文件夹不存在");

        var directory = new DirectoryInfo(directoryPath);
        return await CalculateFolderSizeAsync(directory, cancellationToken);
    }

    private static async Task<long> CalculateFolderSizeAsync(DirectoryInfo directory, CancellationToken cancellationToken)
    {
        long totalSize = 0;

        var files = GetAccessibleFiles(directory);
        await Parallel.ForEachAsync(
            files,
            cancellationToken,
            async (file, ct) =>
            {
                try
                {
                    Interlocked.Add(ref totalSize, file.Length);
                }
                catch (FileNotFoundException)
                {
                }
                await Task.CompletedTask;
            }
        );
        var subDirectories = GetAccessibleDirectories(directory);
        await Parallel.ForEachAsync(
            subDirectories,
            cancellationToken,
            async (subDir, ct) =>
            {
                long subDirSize = await CalculateFolderSizeAsync(subDir, ct);
                Interlocked.Add(ref totalSize, subDirSize);
            }
        );

        return totalSize;
    }

    private static FileInfo[] GetAccessibleFiles(DirectoryInfo directory)
    {
        try
        {
            return directory.GetFiles();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<FileInfo>();
        }
    }

    private static DirectoryInfo[] GetAccessibleDirectories(DirectoryInfo directory)
    {
        try
        {
            return directory.GetDirectories();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<DirectoryInfo>();
        }
    }
}
