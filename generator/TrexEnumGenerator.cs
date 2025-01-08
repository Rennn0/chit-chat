using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace generator;

//[Generator]
public class TrexEnumGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.AdditionalTextsProvider, GenerateEnum);
    }

    private void GenerateEnum(SourceProductionContext context, AdditionalText text)
    {
        if (!text.Path.EndsWith(".trex"))
            return;

        string? fileContent = text.GetText(context.CancellationToken)?.ToString();
        if (string.IsNullOrEmpty(fileContent))
            return;

        List<string> keys = fileContent
            .Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
            .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
            .Select(line => line.Split(['='], 2, StringSplitOptions.RemoveEmptyEntries)[0].Trim())
            .Where(key => !string.IsNullOrWhiteSpace(key))
            .Distinct()
            .ToList();

        string enumSource = GenerateEnumSource(keys);
        string mapperSource = GenerateMapperSource(keys);
        context.AddSource("trex_enum_generated.cs", SourceText.From(enumSource, Encoding.UTF8));
        context.AddSource(
            "trex_enum_mapper_generated.cs",
            SourceText.From(mapperSource, Encoding.UTF8)
        );
    }

    private string GenerateMapperSource(List<string> keys)
    {
        StringBuilder sb = new();

        sb.AppendLine("namespace generator;");
        sb.AppendLine();
        sb.AppendLine("public static class TrexSettingsMapper");
        sb.AppendLine("{");
        sb.AppendLine("     private static readonly Dictionary<TrexSettings,string> _map = new()");
        sb.AppendLine("     {");
        foreach (string key in keys)
        {
            sb.AppendLine($"        {{TrexSettings.{key}, \"{key}\"}},");
        }

        sb.AppendLine("     };");
        sb.AppendLine();
        sb.AppendLine("     public static string ToStringValue(this TrexSettings key)");
        sb.AppendLine("     {");
        sb.AppendLine("         return _map[key];");
        sb.AppendLine("     }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private string GenerateEnumSource(List<string> keys)
    {
        StringBuilder sb = new();

        sb.AppendLine("namespace generator;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Auto-generated enum for Trex Settings keys.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public enum TrexSettings");
        sb.AppendLine("{");

        foreach (string? key in keys)
        {
            sb.AppendLine($"    /// <summary>");
            sb.AppendLine($"    /// Key: {key}");
            sb.AppendLine($"    /// </summary>");
            sb.AppendLine($"    {key},");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}
