using Haiyu.Helpers;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.Globalization;
using Windows.ApplicationModel.Core;

namespace Haiyu.ViewModel.OOBEViewModels;

public sealed partial class LanguageSelectViewModel : ViewModelBase
{
    public LanguageSelectViewModel()
    {
        this.Languages = LanguageService.Languages.ToList();
        for (int i = 0; i < Languages.Count; i++)
        {
            if (Languages[i] == LanguageService.GetLanguage())
            {
                this.SelectLanguage = Languages[i];
            }
        }
    }



    [ObservableProperty]
    public partial List<string> Languages { get; set; }

    [ObservableProperty]
    public partial string SelectLanguage { get; set; }

    partial void OnSelectLanguageChanged(string value)
    {
        if (LanguageService.GetLanguage() != value)
        {
            LanguageService.SetLanguage(value);
            ApplicationLanguages.PrimaryLanguageOverride = value;
            AppRestartFailureReason restartError = AppInstance.Restart(null);
        }
    }


    [RelayCommand]
    void Loaded()
    {
        WeakReferenceMessenger.Default.Send<OOBEArgsMessager>(
            new OOBEArgsMessager() { IsNext = true }
        );
    }
}
