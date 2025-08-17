using System.Text.RegularExpressions;

public static partial class Helpers
{
    [GeneratedRegex("""include "([^"]+)"\;""")]
    public static partial Regex GetIncludeRgx();

    // trDelayedRuleActivation(string name, bool checkForTrigger)
    [GeneratedRegex("""trDelayedRuleActivation\(([^\),;]+),*[^\);]*\)\s*;""")]
    public static partial Regex GetDelayedRuleActivationRgx();

    public static string EscapeText(string text)
    {
        return text.Replace("\"", "\\\"");
    }
}