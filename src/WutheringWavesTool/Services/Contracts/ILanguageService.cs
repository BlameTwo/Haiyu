namespace Haiyu.Services.Contracts;

public interface ILanguageService
{
    public IReadOnlyCollection<string> Languages { get; }

    public bool SetLanguage(string language);

    public string GetLanguage();
}
