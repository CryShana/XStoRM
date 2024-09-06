using System.Text.RegularExpressions;

public static partial class Helpers
{
    [GeneratedRegex("""include "([^"]+)"\;""")]
    public static partial Regex GetIncludeRgx();
}