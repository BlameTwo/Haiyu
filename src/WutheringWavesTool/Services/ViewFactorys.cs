using Haiyu.Common.Contracts;
using Haiyu.Pages.Communitys;
using Haiyu.Pages.Toolkits;
using Waves.Api.Models.CloudGame;
using Waves.Core.Models.CloudGame;

namespace Haiyu.Services;

public class ViewFactorys : IViewFactorys
{
    private static readonly WindowsOption SignWindowOption = new()
    {
        Width = 400,
        Height = 400,
        MaxWidth = 400,
        MaxHeight = 400,
        IsResizable = false,
        IsMaximizable = false,
        CenterOnScreen = true,
        IsExtendWindowTitle = true,
    };

    private static readonly WindowsOption GeetWindowOption = new()
    {
        Width = 700,
        Height = 510,
        MaxWidth = 700,
        MaxHeight = 510,
        IsResizable = false,
        IsMaximizable = false,
        CenterOnScreen = true,
        IsExtendWindowTitle = true,
    };

    private static readonly WindowsOption DeviceInfoWindowOption = new()
    {
        Width = 750,
        Height = 530,
        MaxWidth = 750,
        MaxHeight = 530,
        IsResizable = false,
        IsMaximizable = false,
        CenterOnScreen = true,
        IsExtendWindowTitle = true,
    };

    private static readonly WindowsOption WindowsOptionAnalysisRecord = new ()
    {
        Width = 1200,
        Height = 750,
        MaxWidth = 1200,
        MaxHeight = 750,
        IsResizable = false,
        IsMaximizable = false,
        CenterOnScreen = true,
        IsExtendWindowTitle = true,
    };

    private static readonly WindowsOption WindowsOptionAutoToken = new()
    {
        Width = 900,
        Height = 650,
        MaxWidth = 900,
        MaxHeight = 650,
        IsResizable = false,
        IsMaximizable = false,
        CenterOnScreen = true,
        IsExtendWindowTitle = true,
    };

    public ViewFactorys(IAppContext<App> appContext)
    {
        AppContext = appContext;
    }

    public IAppContext<App> AppContext { get; }

    public GetGeetWindow CreateGeetWindow(nint value, GeetType type)
    {
        return new GetGeetWindow(value, type, GeetWindowOption);
    }

    public void ShowSignWindow(GameRoilDataItem role)
    {
        this.AppContext.WindowManager.CreateWindow<GamerSignPage>(
            new Models.Options.WindowManagerOption()
            {
                WindowConfig = SignWindowOption,
                Key = role.GetSignId,
                Paramter = role,
            }
        );
    }

    public void ShowAdminDevice()
    {
        this.AppContext.WindowManager.CreateWindowBase<DeviceInfoPage>(
            new Models.Options.WindowManagerOption()
            {
                WindowConfig = DeviceInfoWindowOption,
                Key = "KuroDevice",
                Paramter = null,
            },
            WindowNative.GetWindowHandle(AppContext.WindowManager.Shell.GetWindow())
        );
    }

    public void ShowAnalysisRecordV2(CloudGameLoginSession selectLogin)
    {
        this.AppContext.WindowManager.CreateWindowBase<WavesAnalysisRecordPage>(
            new Models.Options.WindowManagerOption()
            {
                WindowConfig = WindowsOptionAnalysisRecord,
                Key = $"{selectLogin.GetId()}:CloudWaves",
                Paramter = selectLogin,
            },
            WindowNative.GetWindowHandle(AppContext.WindowManager.Shell.GetWindow())
        );
    }

    public void ShowAutoKruoTokenWindow()
    {
        this.AppContext.WindowManager.CreateWindow<AutoKuroTokenPage>(
            new Models.Options.WindowManagerOption()
            {
                WindowConfig = WindowsOptionAutoToken,
                Key = $"AutoKuroToken",
                Paramter = null,
            }
        );
    }

    public void ShowMonitorToolWindow()
    {
        //var win = new TransparentWindowBase();
        //var page = Instance.Host.Services!.GetRequiredService<MonitorToolPage>();
        //page.SetWindow(win);
        //win.Content = page;
        //return win;
    }
}
