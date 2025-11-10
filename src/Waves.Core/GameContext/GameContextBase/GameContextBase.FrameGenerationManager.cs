using System.Diagnostics;
using Waves.Core.Models;

namespace Waves.Core.GameContext;

/*
 DLSS帧生成版本管理
 */

partial class GameContextBase
{
    public async Task<FileVersion> GetLocalDLSSAsync()
    {
        var gameFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        var file = Directory
            .GetFiles(gameFolder, "nvngx_dlss.dll", SearchOption.AllDirectories)
            .FirstOrDefault();
        if (file == null)
        {
            return null;
        }
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(file);
        return new FileVersion()
        {
            DisplayName = "Dlss",
            Subtitle = fileinfo.InternalName,
            FilePath = file,
            Version =
                $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}.{fileinfo.FilePrivatePart}",
        };
    }

    public async Task<FileVersion> GetLocalDLSSGenerateAsync()
    {
        var gameFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        var file = Directory
            .GetFiles(gameFolder, "nvngx_dlssg.dll", SearchOption.AllDirectories)
            .FirstOrDefault();
        if (file == null)
        {
            return null;
        }
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(file);
        return new FileVersion()
        {
            DisplayName = "Dlss 帧生成",
            Subtitle = fileinfo.InternalName,
            FilePath = file,
            Version =
                $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}.{fileinfo.FilePrivatePart}",
        };
    }

    public async Task<FileVersion> GetLocalXeSSGenerateAsync()
    {
        var gameFolder = GameLocalConfig.GetConfig(GameLocalSettingName.GameLauncherBassFolder);
        var file = Directory
            .GetFiles(gameFolder, "libxess.dll", SearchOption.AllDirectories)
            .FirstOrDefault();
        if (file == null)
        {
            return null;
        }
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(file);
        return new FileVersion()
        {
            DisplayName = "XeSS",
            Subtitle = fileinfo.InternalName,
            FilePath = file,
            Version =
                $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}.{fileinfo.FilePrivatePart}",
        };
    }
}
