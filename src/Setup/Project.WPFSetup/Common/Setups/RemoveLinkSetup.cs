using System.IO;

namespace Project.WPFSetup.Common.Setups;

public class RemoveLinkSetup : ISetup
{
    public string SetupName => "删除快捷方式";

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
                var startMenuLink = property.GetStartMenuFolder();
                if (Directory.Exists(startMenuLink))
                {
                    Directory.Delete(startMenuLink, true);
                }
                var desktopLin = property.GetDesktopMenuLink();
                if (File.Exists(desktopLin))
                {
                    File.Delete(desktopLin);
                }
            }
            catch { }
            progress.Report((1, ""));
            return ("", true);
        });
    }
}
