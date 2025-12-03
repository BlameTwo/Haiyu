using Microsoft.UI.Xaml.Markup;
using Microsoft.Windows.Globalization;

namespace Haiyu.Helpers;

public static class LanguageHelper
{
    public static IReadOnlyCollection<string> Languages => [];

    public static void SetLanguage(string language)
    {
        ApplicationLanguages.PrimaryLanguageOverride = language ;
    }

    public static string GetLanguage()
    {
        return ApplicationLanguages.PrimaryLanguageOverride;
    }

    public static string? ReadResource(string key,string value)
    {
        try
        {
            var view = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
            var subtree = view.GetSubtree(key);
            return subtree.GetValue(value).ValueAsString;
        }
        catch (Exception)
        {
            return null;
        }

    }

    public static string? GetString(string key)
    {
        return ReadResource("GamePage", key); 
    }

}
