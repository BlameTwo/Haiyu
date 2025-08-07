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
        var gameFolder = GameLocalConfig.GetConfig(
            GameLocalSettingName.GameLauncherBassFolder
        );
        var path = Path.Combine(
            gameFolder,
            "Engine\\Plugins\\Runtime\\Nvidia\\DLSS\\",
            "Binaries\\ThirdParty\\Win64",
            "nvngx_dlss.dll"
        );
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(path);
        return new FileVersion()
        {
            DisplayName = "DLSS",
            Subtitle = fileinfo.InternalName,

            FilePath = path,
            Version = $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}",
        };
    }

    public async Task<FileVersion> GetLocalDLSSGenerateAsync()
    {
        var gameFolder = GameLocalConfig.GetConfig(
            GameLocalSettingName.GameLauncherBassFolder
        );
        var path = Path.Combine(
            gameFolder,
            "Engine\\Plugins\\Runtime\\Nvidia\\Streamline\\",
            "Binaries\\ThirdParty\\Win64",
            "nvngx_dlssg.dll"
        );
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(path);
        return new FileVersion()
        {
            DisplayName = "Dlss Generate",
            Subtitle = fileinfo.InternalName,
            FilePath = path,
            Version =
                $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}.{fileinfo.FilePrivatePart}",
        };
    }

    public async Task<FileVersion> GetLocalXeSSGenerateAsync()
    {
        var gameFolder = GameLocalConfig.GetConfig(
            GameLocalSettingName.GameLauncherBassFolder
        );
        var path = Path.Combine(
            gameFolder,
            "Engine\\Plugins\\Runtime\\Intel\\Xess\\",
            "Binaries\\ThirdParty\\Win64",
            "libxess.dll"
        );
        FileVersionInfo fileinfo = FileVersionInfo.GetVersionInfo(path);
        return new FileVersion()
        {
            DisplayName = "XeSS",
            Subtitle = fileinfo.InternalName,
            FilePath = path,
            Version =
                $"{fileinfo.FileMajorPart}.{fileinfo.FileMinorPart}.{fileinfo.FileBuildPart}.{fileinfo.FilePrivatePart}",
        };
    }
}
