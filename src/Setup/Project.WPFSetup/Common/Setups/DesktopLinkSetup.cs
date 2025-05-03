using System.IO;
using IWshRuntimeLibrary;

namespace Project.WPFSetup.Common.Setups;

public class DesktopLinkSetup : ISetup
{
    public string SetupName => "创建桌面快捷方式";

    public int MaxProgress => 1;

    public async Task<(string, bool)> ExecuteAsync(
        SetupProperty property,
        IProgress<(double, string)> progress,
        int maxValue
    )
    {
        return await Task.Run(() =>
        {
            try
            {
                string targetPath = $"{property.InstallPath}\\{property.InstallExeName}";
                string shortcutName = property.InstallExeName;
                var startMenuFolder = property.GetDesktopMenuFolder();
                string startMenuExeLine = property.GetDesktopMenuLink();
                string shortcutDescription = property.InstallName;
                Directory.CreateDirectory(startMenuFolder);
                if (System.IO.File.Exists(startMenuExeLine))
                {
                    System.IO.File.Delete(startMenuExeLine);
                }
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(startMenuExeLine);
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = property.InstallPath;
                shortcut.Description = shortcutDescription;
                shortcut.IconLocation = targetPath;
                shortcut.WindowStyle = 1;
                shortcut.Save();
                progress.Report((1, "成功"));
                return ("", true);
            }
            catch (Exception ex)
            {
                progress.Report((1, $"失败{ex.Message}"));
                return (ex.Message, true);
            }
        });
    }
}
