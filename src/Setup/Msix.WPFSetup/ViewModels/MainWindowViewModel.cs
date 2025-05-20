using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Msix.WPFSetup.Resources;
using Windows.ApplicationModel.Activation;
using Windows.Management.Deployment;
using Windows.System;

namespace Msix.WPFSetup.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    [ObservableProperty]
    public partial object CurrentViewModel { get; set; }

    [ObservableProperty]
    public partial string PackageVersion { get; set; }

    [ObservableProperty]
    public partial string PackageName { get; set; } = Package.PackageName;

    [ObservableProperty]
    public partial double Progress { get; set; }

    [ObservableProperty]
    public partial string TipProgress { get; set; }

    public string CertFile { get; private set; }
    public string MsixFile { get; private set; }

    [RelayCommand]
    async Task Loaded()
    {
        PackageVersion = Package.PackageVersion;
        IProgress<double> progress = new Progress<Double>();
        await Task.Run(async () =>
        {
            var certFile = System.IO.Path.GetTempFileName();
            var msixPack = System.IO.Path.GetTempFileName();
            MemoryStream certFs = new MemoryStream(Resource1.Cert);
            MemoryStream msixFs = new MemoryStream(Resource1.MsixFile);
            using (
                FileStream fs = new FileStream(
                    certFile,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    4096,
                    true
                )
            )
            {
                await certFs.CopyToAsync(fs);
            }
            using (
                FileStream fs = new FileStream(
                    msixPack,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    4096,
                    true
                )
            )
            {
                await msixFs.CopyToAsync(fs);
            }
            this.CertFile = certFile;
            this.MsixFile = msixPack;
        });
    }

    [RelayCommand]
    async Task InstallAsync()
    {
        await Task.Run(async () =>
        {
            try
            {
                #region 安装证书
                X509Certificate2 x509Certificate2 = new X509Certificate2(
                    X509Certificate.CreateFromCertFile(CertFile)
                );
                X509Store store = new(StoreName.TrustedPeople, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection existingCerts = store.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    x509Certificate2.Thumbprint,
                    validOnly: false
                );
                if (existingCerts.Count > 0)
                {
                    Console.WriteLine("证书已存在！");
                }
                else
                {
                    Console.WriteLine("证书不存在，可以添加。");
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(x509Certificate2);
                }

                #endregion

                #region 安装
                PackageManager packageManager = new();
                AddPackageOptions addPackageOptions =
                    new() { ForceAppShutdown = true, RetainFilesOnFailure = true };
                IProgress<DeploymentProgress> progress = new Progress<DeploymentProgress>(p =>
                {
                    TipProgress = $"{p.percentage}";
                    Progress = p.percentage;
                });
                var result = await packageManager
                    .AddPackageByUriAsync(new Uri(MsixFile), addPackageOptions)
                    .AsTask(progress)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                #endregion

            TipProgress = $"100%";
            Progress = 100;
            Environment.Exit(0);
        });
    }
}
