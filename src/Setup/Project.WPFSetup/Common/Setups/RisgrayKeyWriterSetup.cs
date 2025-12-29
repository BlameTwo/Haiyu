using Project.WPFSetup.Resources;

namespace Project.WPFSetup.Common.Setups;

public class RisgrayKeyWriterSetup : ISetup
{
    public string SetupName => "写入注册信息";

    public int MaxProgress => 16;

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
                var productKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(
                    $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{property.ProductId}"
                );
                int count = 0;
                foreach (
                    var item in RegisterModel
                        .CreateRegister(property, Resource1.InstallFile.Length / 1024)
                        .Keys
                )
                {
                    productKey.SetValue(
                            item.Key,
                            item.Value.Item2,
                            item.Value.Item1
                        );
                    count++;
                    progress.Report((count, "成功"));
                }
                return ("", true);
            }
            catch (Exception ex)
            {
                progress.Report((0, "失败"));
                return (ex.Message, false);
            }
        });
    }
}
