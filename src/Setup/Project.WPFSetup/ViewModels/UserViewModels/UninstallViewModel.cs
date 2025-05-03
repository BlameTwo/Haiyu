using CommunityToolkit.Mvvm.ComponentModel;

namespace Project.WPFSetup.ViewModels.UserViewModels;

public sealed partial class UninstallViewModel : ObservableRecipient
{
    [ObservableProperty]
    public partial string Version { get; set; }
}
