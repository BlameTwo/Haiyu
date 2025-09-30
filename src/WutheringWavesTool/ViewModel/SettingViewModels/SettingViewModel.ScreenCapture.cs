using Haiyu.Plugin.Extensions;

namespace Haiyu.ViewModel;

partial class SettingViewModel
{
    [ObservableProperty]
    public partial bool IsOn { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<ModifierKey> CaptureModifierKeys { get; set; } =
        ModifierKey.GetDefault().ToObservableCollection();

    [ObservableProperty]
    public partial ModifierKey CaptureModifierKey { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Keys> CaptureKeys { get; set; } =
        Plugin.Extensions.Keys.GetDefault().ToObservableCollection();


    [ObservableProperty]
    public partial Keys CaptureKey { get; set; }


    partial void OnCaptureModifierKeyChanged(ModifierKey value)
    {
        AppSettings.CaptureModifierKey = value.Name;
        var result =  ScreenCaptureService.Register();
        this.TipShow.ShowMessage(result.Item2, Symbol.Read);
    }

    partial void OnIsOnChanged(bool value)
    {
        AppSettings.IsCapture = value.ToString();

    }

    partial void OnCaptureKeyChanged(Keys value)
    {
        AppSettings.CaptureKey = value.Name;
        var result =  ScreenCaptureService.Register();
        this.TipShow.ShowMessage(result.Item2, Symbol.Read);
    }

    public void InitCapture() 
    {
        this.IsOn = AppSettings.IsCapture == null ? true : Boolean.Parse(AppSettings.IsCapture);
        if (string.IsNullOrWhiteSpace(AppSettings.CaptureModifierKey) || string.IsNullOrWhiteSpace(AppSettings.CaptureKey))
        {
            this.CaptureModifierKey = this.CaptureModifierKeys.Where(x => x.Name == "Win").First();
            this.CaptureKey = this.CaptureKeys.Where(x => x.Name == "F12").First();
        }
        else
        {
            this.CaptureModifierKey = this.CaptureModifierKeys.Where(x => x.Name == AppSettings.CaptureModifierKey).First();
            this.CaptureKey = this.CaptureKeys.Where(x => x.Name == AppSettings.CaptureKey).First();
        }
    }
}
