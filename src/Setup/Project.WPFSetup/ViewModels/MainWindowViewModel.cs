using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.WPFSetup;
using Project.WPFSetup.ViewModels.UserViewModels;

namespace Project.WPFSetup.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient
    {
        public MainWindowViewModel() { }

        [ObservableProperty]
        public partial object CurrentViewModel { get; set; }

        [RelayCommand]
        void Loaded() { }
    }
}
