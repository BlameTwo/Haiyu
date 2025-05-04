using System.Configuration;
using System.Data;
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
                (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                    App.GetService<UninstallViewModel>();
            }
            else
            {
                var property = SetupPropertyFactory.CreateInstall();
                var version = GetService<PackageService>();
                var current = version.GetInstallVersion(property);
                if (current.Item1 && current.Item2 == property.Version)
                {
                    (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                        App.GetService<UpdateViewModel>();
                }
                else if (current.Item1)
                {
                    (win.DataContext as MainWindowViewModel)!.CurrentViewModel =
                        App.GetService<RepairViewModel>();
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
                .AddSingleton<InstallViewModel>()
                .AddSingleton<PackageService>()
                .AddSingleton<UninstallViewModel>()
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
