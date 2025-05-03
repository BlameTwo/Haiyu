using CommunityToolkit.Mvvm.ComponentModel;

namespace Project.WPFSetup;

public partial class InstallProgressArgs : ObservableObject
{
    [ObservableProperty]
    public partial string SetupName { get; set; }

    [ObservableProperty]
    public partial double MaxSetup { get; set; }

    [ObservableProperty]
    public partial double CurrentSetup { get; set; }

    [ObservableProperty]
    public partial double MaxCurrentSetupProgress { get; set; }

    [ObservableProperty]
    public partial double CurrentSetupProgress { get; set; }

    public string GetCurrentSetupString() => $"{SetupName}：[{CurrentSetup}/{MaxSetup}]";
}
