using LanguageEditer.Model;
using Microsoft.Windows.Globalization;

namespace Haiyu.Services;

public sealed class LanguageService : ILanguageService
{
    public IReadOnlyCollection<string> Languages => ["en-us","zh-Hans","zh-Hant","ja-jp"];

    public List<LanguageItem>? Zh_Hans { get; set; }
    public List<LanguageItem>? Zh_Hant { get; set; }

    public List<LanguageItem>? En_Us { get; set; }

    public List<LanguageItem>? Ja_Jp { get; set; }

    public string GetLanguage()
    {
        return AppSettings.Language;
    }

    public async Task InitAsync()
    {
        try
        {
            this.Zh_Hans = JsonSerializer.Deserialize<List<LanguageItem>>(await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory+"\\Assets\\Languages\\zh-Hans.json"),ProjectLanguageModelContext.Default.ListLanguageItem);
            this.Zh_Hant = JsonSerializer.Deserialize<List<LanguageItem>>(await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory+ "\\Assets\\Languages\\zh-Hant.json"), ProjectLanguageModelContext.Default.ListLanguageItem);
            this.En_Us = JsonSerializer.Deserialize<List<LanguageItem>>(await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory+ "\\Assets\\Languages\\en-US.json"), ProjectLanguageModelContext.Default.ListLanguageItem);
            this.Ja_Jp = JsonSerializer.Deserialize<List<LanguageItem>>(await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory+ "\\Assets\\Languages\\ja-JP.json"), ProjectLanguageModelContext.Default.ListLanguageItem);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool SetLanguage(string language)
    {
        AppSettings.Language = language;
        return true;
    }
}
