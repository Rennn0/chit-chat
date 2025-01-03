using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace generator;

[Generator]
public class SettingsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<string?> settingsFiles = context
            .AdditionalTextsProvider.Where(f =>
                f.Path.EndsWith(".trex", StringComparison.OrdinalIgnoreCase)
            )
            .Select((at, ct) => at.GetText(ct)?.ToString())
            .Where(content => !string.IsNullOrEmpty(content));

        context.RegisterSourceOutput(settingsFiles, GenerateSettingsClass);
    }

    private void GenerateSettingsClass(SourceProductionContext context, string? fileContent)
    {
        if (string.IsNullOrEmpty(fileContent))
            return;

        Dictionary<string, string> settings = ParseSettingsFile(fileContent);
        string generatedCode = GenerateSettingsCode(settings);
        context.AddSource("settings_generated.cs", SourceText.From(generatedCode, Encoding.UTF8));
    }

    private string GenerateSettingsCode(Dictionary<string, string> settings)
    {
        StringBuilder sb = new();
        sb.Append(
            @$"
namespace GeneratedSettings
{{
    public static class TrexSettigns
    {{
"
        );
        foreach (KeyValuePair<string, string> keyValuePair in settings)
        {
            sb.AppendLine(
                @$"
        public static string {keyValuePair.Key} {{get;}} = ""{keyValuePair.Value}"";
"
            );
        }

        sb.AppendLine(
            @$"
    }}
}}"
        );

        return sb.ToString();
    }

    private Dictionary<string, string> ParseSettingsFile(string? fileContent)
    {
        Dictionary<string, string> settings = new();
        if (string.IsNullOrEmpty(fileContent))
            return settings;

        foreach (
            string line in fileContent.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
        )
        {
            if (line.StartsWith("#") || !line.Contains("="))
                continue;

            string[] parts = line.Split(['='], 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            settings[parts[0]] = parts[1];
        }

        return settings;
    }
}
