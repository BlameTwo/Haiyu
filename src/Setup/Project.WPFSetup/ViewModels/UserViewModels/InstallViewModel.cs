using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Project.WPFSetup.Common;
using Project.WPFSetup.Common.Setups;
using Project.WPFSetup.Resources;
using Project.WPFSetup.Services;

namespace Project.WPFSetup.ViewModels.UserViewModels;

public sealed partial class InstallViewModel : ObservableRecipient
{
    public InstallViewModel(PackageService packageService)
    {
        PackageService = packageService;
        this.SetupProperty = SetupPropertyFactory.CreateInstall();
        this.SelectInstallVisibility = Visibility.Visible;
        InstallingVisibility = Visibility.Collapsed;
    }

    [ObservableProperty]
    public partial string InstallFolder { get; set; }

    [ObservableProperty]
    public partial Visibility SelectInstallVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility InstallingVisibility { get; set; } = Visibility.Collapsed;

    #region InstallProperty
    [ObservableProperty]
    public partial InstallProgressArgs InstallProgressArgs { get; set; }

    [ObservableProperty]
    public partial string SetupString { get; set; }

    #endregion

    [ObservableProperty]
    public partial bool CreateStartMenuCheck { get; set; }

    [ObservableProperty]
    public partial bool CreateDesktopCheck { get; set; }

    public PackageService PackageService { get; }
    public SetupProperty SetupProperty { get; }

    [RelayCommand]
    void OpenFolder()
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            if (dialog.FileName.StartsWith("C"))
            {
                MessageBox.Show("本程序不允许安装在C盘");
                return;
            }
            InstallFolder = dialog.FileName;
            this.SetupProperty.InstallPath = InstallFolder;
        }
    }

    [RelayCommand]
    async Task InstallAsync()
    {
        this.SelectInstallVisibility = Visibility.Collapsed;
        InstallingVisibility = Visibility.Visible;
        if (CreateStartMenuCheck)
        {
            SetupProperty.Setups.Add(new StartMenLinkSetup());
            SetupProperty.Setups.Add(new CreateUninstallSetup());
        }
        if (CreateDesktopCheck)
        {
            SetupProperty.Setups.Add(new DesktopLinkSetup());
        }
        SetupProperty.Setups.Add(new RisgrayKeyWriterSetup());
        IProgress<InstallProgressArgs> installProgress = new Progress<InstallProgressArgs>();
        (installProgress as Progress<InstallProgressArgs>)!.ProgressChanged += (s, e) =>
        {
            this.InstallProgressArgs = e;
            this.SetupString = e.GetCurrentSetupString();
        };
        var result = await PackageService.InstallStartAsync(this.SetupProperty, installProgress);
        if (result.Item1)
        {
            MessageBox.Show("安装成功！");
        }
        else
        {
            MessageBox.Show($"安装失败！{result.Item2}，请退出安装程序进行反馈");
        }
    }
}
