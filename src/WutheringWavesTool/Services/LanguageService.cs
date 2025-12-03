using Microsoft.Windows.Globalization;

namespace Haiyu.Services;

public sealed class LanguageService : ILanguageService
{
    public IReadOnlyCollection<string> Languages => ["en-us","zh-Hans","zh-Hant"];

    public string GetLanguage()
    {
        return AppSettings.Language;
    }

    public bool SetLanguage(string language)
    {
        AppSettings.Language = language;
        return true;
    }
}
