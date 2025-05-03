using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace Project.WPFSetup.Common.Setups
{
    /// <summary>
    /// 开始菜单步骤
    /// </summary>
    internal class StartMenLinkSetup : ISetup
    {
        public string SetupName => "创建开始菜单";

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
                    var startMenuFolder = property.GetStartMenuFolder();
                    string startMenuExeLine = property.GetStartMenuLink();
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
                    // 保存快捷方式
                    shortcut.Save();
                    progress.Report((1, "成功"));
                    return ("", true);
                }
                catch (Exception ex)
                {
                    progress.Report((1, $"失败{ex.Message}"));
                    return (ex.Message, false);
                }
            });
        }
    }
}
