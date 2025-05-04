using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.WPFSetup.Common.Setups
{
    public class UninstallDeleteInstallFolderSetup : ISetup
    {
        public string SetupName => "删除旧文件";

        public int MaxProgress => 1;

        public async Task<(string, bool)> ExecuteAsync(
            SetupProperty property,
            IProgress<(double, string)> progress,
            int maxValue
        )
        {
            var installFolder = property.InstallPath;
            var result = await Remove(property, installFolder);
            progress.Report((1, SetupName));
            return ("", true);
        }

        public async Task<bool> Remove(SetupProperty setupProperty, string directoryPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Console.WriteLine("指定的目录不存在。");
                        return true;
                    }
                    foreach (
                        var file in Directory.GetFiles(
                            directoryPath,
                            "*.*",
                            SearchOption.TopDirectoryOnly
                        )
                    )
                    {
                        if (file.StartsWith(setupProperty.UninstallName))
                        {
                            continue;
                        }
                        try
                        {
                            File.Delete(file);
                            Console.WriteLine($"已删除文件: {file}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"无法删除文件 {file}: {ex.Message}");
                        }
                    }
                    foreach (
                        var dir in Directory.GetDirectories(
                            directoryPath,
                            "*",
                            SearchOption.TopDirectoryOnly
                        )
                    )
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception ex) { }
                return true;
            });
        }
    }
}
