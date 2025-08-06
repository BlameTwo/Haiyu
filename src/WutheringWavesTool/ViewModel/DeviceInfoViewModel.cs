using SqlSugar;
using Waves.Api.Models.QRLogin;

namespace WutheringWavesTool.ViewModel;

public class DeviceInfoDisplayHeader
{
    public string? DisplayName { get; set; }
    public string? Tag { get; set; }

    public DeviceInfoDisplayHeader(string? displayName, string? tag)
    {
        DisplayName = displayName;
        Tag = tag;
    }

}

public partial class DeviceInfoViewModel:ObservableObject
{
    public DeviceInfoViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
    }

    [ObservableProperty]
    public partial ObservableCollection<DeviceInfoDisplayHeader> Displays { get; set; } = new()
    {
        new DeviceInfoDisplayHeader("PC授权","PC"),
        new DeviceInfoDisplayHeader("账号授权", "User")
    };

    [ObservableProperty]
    public partial DeviceInfoDisplayHeader SelectHeader { get; set; }

    [ObservableProperty]
    public partial Visibility DeviceInfosVisibility { get; set; }

    [ObservableProperty]
    public partial Visibility GamerRoleVisibility { get; set; }
    public IWavesClient WavesClient { get; }

    [ObservableProperty]
    public partial ObservableCollection<DeviceDatum> Devices { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        SelectHeader = Displays[0];
        await RefreshAsync();
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        var devices = await WavesClient.GetDeviceInfosAsync();
        if (devices != null)
            this.Devices = devices.Data.Where(x => x != null).ToObservableCollection();
    }

    partial void OnSelectHeaderChanged(DeviceInfoDisplayHeader value)
    {
        if (value == null)
            return;
        if(value.Tag == "PC")
        {
            DeviceInfosVisibility = Visibility.Visible;
            GamerRoleVisibility = Visibility.Collapsed;
        }
        else
        {
            DeviceInfosVisibility = Visibility.Collapsed;
            GamerRoleVisibility = Visibility.Visible;
        }
    }
}
