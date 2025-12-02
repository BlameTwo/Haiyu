using System.Diagnostics;
using Waves.Core.Models;

namespace Waves.Core.GameContext;

/*
 DLSS帧生成版本管理
 */

partial class GameContextBase
{

    public async Task<FileVersion> GetLocalFileVersionAsync(string fileName,string displayName)
    {
        var gameFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        var file = Directory
            .GetFiles(gameFolder, fileName, SearchOption.AllDirectories)
            .FirstOrDefault();
        if (file == null)
        {
            return new FileVersion() { DisplayName = displayName, Version = "未找到文件" };
        }
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(file);
        return new FileVersion()
        {
            DisplayName = displayName,
            Subtitle = fileinfo.InternalName,
            FilePath = file,
            Version =
                $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}.{fileinfo.FilePrivatePart}",
        };
    }

    public async Task<FileVersion> GetLocalDLSSAsync()
    {
        return await GetLocalFileVersionAsync("nvngx_dlss.dll", "Xess");
    }
    public async Task<FileVersion> GetLocalDLSSGenerateAsync()
    {
        return await GetLocalFileVersionAsync("nvngx_dlssg.dll", "Dlss 帧生成");
    }

    public async Task<FileVersion> GetLocalXeSSGenerateAsync()
    {
        return await GetLocalFileVersionAsync("libxess.dll", "Xess");
    }
}
