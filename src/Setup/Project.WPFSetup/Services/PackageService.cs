using System.IO;
using System.IO.Compression;
using System.Windows.Documents;
using System.Windows.Shapes;
using Project.WPFSetup.Common;

namespace Project.WPFSetup.Services;

public sealed class PackageService
{
    public static async Task<(bool, string)> InstallStartAsync(
        SetupProperty property,
        IProgress<InstallProgressArgs> progress,
        CancellationToken token = default
    )
    {
        for (int i = 0; i < property.Setups.Count(); i++)
        {
            progress.Report(
                new()
                {
                    CurrentSetup = i,
                    MaxSetup = property.Setups.Count(),
                    CurrentSetupProgress = 0,
                    MaxCurrentSetupProgress = property.Setups[i].MaxProgress,
                    SetupName = property.Setups[i].SetupName,
                }
            );
            IProgress<(double, string)> extractFile = new Progress<(double, string)>();
            ((Progress<(double, string)>)extractFile).ProgressChanged += (s, e) =>
            {
                progress.Report(
                    new()
                    {
                        CurrentSetup = i,
                        MaxSetup = property.Setups.Count(),
                        CurrentSetupProgress = e.Item1,
                        MaxCurrentSetupProgress = property.Setups[i].MaxProgress,
                        SetupName = property.Setups[i].SetupName,
                    }
                );
            };
            var result = await property
                .Setups[i]
                .ExecuteAsync(property, extractFile, property.Setups[i].MaxProgress);
            if (result.Item2 == false)
            {
                return (result.Item2, result.Item1);
            }
        }
        progress.Report(
            new()
            {
                CurrentSetup = property.Setups.Count(),
                MaxSetup = property.Setups.Count(),
                CurrentSetupProgress = 0,
                MaxCurrentSetupProgress = 0,
                SetupName = "完成",
            }
        );
        return (true, "");
    }

    /// <summary>
    /// 获得当前安装包版本
    /// </summary>
    /// <returns></returns>
    public (bool, string?) GetInstallVersion(SetupProperty property)
    {
        try
        {
            var productKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{property.ProductId}"
            );
            if (productKey == null)
            {
                return (false, null);
            }
            var version = productKey.GetValue("DisplayVersion");
            if (version == null)
            {
                return (false, null);
            }
            return (true, version.ToString());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
