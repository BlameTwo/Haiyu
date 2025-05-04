using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.WPFSetup.Common;
using Project.WPFSetup.Common.Setups;
using Project.WPFSetup.Services;

namespace Project.WPFSetup.ViewModels.UserViewModels;

public sealed partial class RepairViewModel : ObservableRecipient
{
    public RepairViewModel(PackageService packageService)
    {
        PackageService = packageService;
    }

    public PackageService PackageService { get; }

    [ObservableProperty]
    public partial string CurrentVersion { get; set; }

    [ObservableProperty]
    public partial Visibility RepairVisibility { get; set; }

    [ObservableProperty]
    public partial Visibility RepairingVisibility { get; set; }

    [ObservableProperty]
    public partial Visibility InstalledVisibility { get; set; }

    #region InstallProperty
    [ObservableProperty]
    public partial InstallProgressArgs InstallProgressArgs { get; set; }

    [ObservableProperty]
    public partial string SetupString { get; set; }
    public SetupProperty SetupProperty { get; private set; }

    #endregion

    [RelayCommand]
    void Loaded()
    {
        this.SetupProperty = SetupPropertyFactory.CreateInstall();
        var result = PackageService.GetInstallVersion(SetupProperty);
        if (result.Item1)
        {
            this.CurrentVersion = result.Item2;
        }
        else
        {
            this.CurrentVersion = "NULL";
        }
        this.RepairingVisibility = Visibility.Collapsed;
        this.InstalledVisibility = Visibility.Collapsed;
        RepairVisibility = Visibility.Visible;
    }

    [RelayCommand]
    async Task Repair()
    {
        this.RepairingVisibility = Visibility.Visible;
        RepairVisibility = Visibility.Collapsed;
        this.InstalledVisibility = Visibility.Collapsed;
        var installLocal = PackageService.GetInstallLocation(this.SetupProperty);
        if (installLocal.Item1)
        {
            SetupProperty.Setups.Add(new RisgrayKeyWriterSetup());
            IProgress<InstallProgressArgs> installProgress = new Progress<InstallProgressArgs>();
            (installProgress as Progress<InstallProgressArgs>)!.ProgressChanged += (s, e) =>
            {
                this.InstallProgressArgs = e;
                this.SetupString = e.GetCurrentSetupString();
            };
            this.SetupProperty.InstallPath = installLocal.Item2!;
            var result = await this.PackageService.InvokeSetup(this.SetupProperty, installProgress);
            if (result.Item1 == true)
            {
                this.RepairingVisibility = Visibility.Collapsed;
                RepairVisibility = Visibility.Collapsed;
                this.InstalledVisibility = Visibility.Visible;
                return;
            }
        }
        else
        {
            MessageBox.Show(
                $"修复失败！请打开注册表将此注册信息删除：计算机\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{SetupProperty.ProductId}\r\n，删除后，重新安装"
            );
            Environment.Exit(0);
        }
    }

    [RelayCommand]
    void Close()
    {
        Environment.Exit(0);
    }
}
