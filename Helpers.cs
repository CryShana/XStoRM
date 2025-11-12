using System.Text.RegularExpressions;

public static partial class Helpers
{
    [GeneratedRegex("""include "([^"]+)"\;""")]
    public static partial Regex GetIncludeRgx();

    // trDelayedRuleActivation(string name, bool checkForTrigger)
    // 1st group -> 1st param (name)
    // 2nd group -> 2nd param (bool) <optional>
    [GeneratedRegex("""trDelayedRuleActivation\(\s*([^\),;]+)\s*,?\s*([^\);]*)\s*\)\s*;""")]
    public static partial Regex GetDelayedRuleActivationRgx();

    public static string EscapeText(string text)
    {
        return text.Replace("\"", "\\\"");
    }
}