using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using FluentWPF.Models;
using Microsoft.Extensions.DependencyInjection;
using Project.WPFSetup.Common;
using Project.WPFSetup.Services;
using Project.WPFSetup.ViewModels;
using Project.WPFSetup.ViewModels.UserViewModels;

namespace Project.WPFSetup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider Service { get; private set; }

        public App()
        {
            InitializeComponent();
            InitService();
            FluentWPF.Instance.InitTheme(this);
            FluentWPF.Instance.InstanceLogOutputEvent += Instance_InstanceLogOutputEvent;
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var win = App.GetService<MainWindow>();
            if (e.Args.Count() != 0 && e.Args[0] == "uninstall")
            {
                if (e.Args.Length == 2 && e.Args[1] == "run")
                {
                    (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                        App.GetService<UninstallViewModel>();
                    win.Show();
                }
                else
                {
                    var property = SetupPropertyFactory.CreateInstall();
                    string sourcePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var uninstallTemp = Path.GetTempFileName();
                    File.Copy(sourcePath, uninstallTemp + ".exe", true);
                    var param = $"uninstall run";
                    Process.Start(
                        new ProcessStartInfo()
                        {
                            Arguments = param,
                            Verb = "runas",
                            FileName = uninstallTemp + ".exe",
                        }
                    );
                    Environment.Exit(0);
                }
            }
            else
            {
                var property = SetupPropertyFactory.CreateInstall();
                var version = GetService<PackageService>();
                var current = version.GetInstallVersion(property);
                if (current.Item1 && current.Item2 == property.Version)
                {
                    (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                        App.GetService<RepairViewModel>();
                }
                else if (current.Item1 && current.Item2 != property.Version)
                {
                    (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                        App.GetService<UpdateViewModel>();
                }
                else
                {
                    (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                        App.GetService<InstallViewModel>();
                }
            }
            win.Show();
        }

        private void Instance_InstanceLogOutputEvent(object sender, UILogModel message) { }

        private void InitService()
        {
            Service = new ServiceCollection()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<PackageService>()
                .AddSingleton<InstallViewModel>()
                .AddSingleton<UninstallViewModel>()
                .AddSingleton<UpdateViewModel>()
                .AddSingleton<RepairViewModel>()
                .BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            var service = Service.GetRequiredService<T>();
            if (service == null)
            {
                throw new InvalidOperationException($"服务{typeof(T).Name}未注册");
            }
            return service;
        }
    }
}
