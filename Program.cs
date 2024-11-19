using System.Text;
using System.Text.RegularExpressions;

if (args.Length < 2)
{
    Console.WriteLine("This tool wraps all input XS code with [rmTriggerAddScriptLine] for usage with random maps. All includes are included.");
    Console.WriteLine("Usage: <input XS file> <output XS file> [class_name=ExportedClass]");
    return;
}

var input = args[0];
var output = args[1];

var class_name = args.Length > 2 ? args[2] : "ExportedClass";
if (class_name.Length == 0) class_name = "ExportedClass";

if (!File.Exists(input))
{
    Console.WriteLine("Input file does not exist: " + input);
    return;
}

const string FUNCTION = "_c";

var output_text = ProcessXStoRM(input);

output_text = $$"""
                class {{class_name}} { 
                void {{FUNCTION}}(string l = ""){rmTriggerAddScriptLine(l);}
                void RegisterTriggers()
                {
                {{output_text}}
                }
                };   
                """;

File.WriteAllText(output, output_text);
Console.WriteLine($"Done (Written {output_text.Length} characters)");

string ProcessXStoRM(string file_path, bool wrap = true)
{
    // TODO: maybe consider preprocessor directives?

    var root = Path.GetDirectoryName(file_path);
    var text = File.ReadAllText(file_path);
    var text_with_includes = text;

    // PROCESS INCLUDES
    var include_rgx = Helpers.GetIncludeRgx();
    var to_replace = new List<(string, string)>();
    foreach (Match m in include_rgx.Matches(text))
    {
        var whole_include = m.Groups[0].Value;
        var include = m.Groups[1].Value;
        var include_path = Path.Combine(root ?? ".", include);
        if (!File.Exists(include_path))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Include '" + include + "' not found, will be ignored.");
            Console.ResetColor();
            continue;
        }

        var include_text = ProcessXStoRM(include_path, false);
        text_with_includes = text.Replace(whole_include, include_text);
    }

    if (wrap)
    {
        // DO THE WRAPPING
        StringBuilder builder = new();
        var new_lines = text_with_includes.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in new_lines)
        {
            var corrected_line = line.Replace("\r", "");
            builder.AppendLine($"{FUNCTION}(\"{EscapeText(corrected_line)}\");");
        }
        text_with_includes = builder.ToString();
    }

    return text_with_includes;
}

string EscapeText(string text)
{
    return text.Replace("\"", "\\\"");
}