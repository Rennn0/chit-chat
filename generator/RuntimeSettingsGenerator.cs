using Microsoft.CodeAnalysis;

namespace generator;

//[Generator]
public class RuntimeSettingsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c =>
        {
            string dynamicLoadrer = GenerateDynamicSettingsLoader();
            c.AddSource("runtime_settings_generated.cs", dynamicLoadrer);
        });
    }

    private string GenerateDynamicSettingsLoader()
    {
        return @"
using System;
using System.IO;
using System.Collections.Generic;

namespace generator
{
    /*
        Trex is auto generated settings manager, has 2 side : 
        first is static, compile time settings
        second is runtime and reflects on changes in file
        (place .trex in app's root dir)
    */
    public static class RuntimeTrexSettings
    {
        private static readonly string _settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "".trex"");
        private static Dictionary<string, string> _settingsCache;
        private static readonly object _lock = new();

        static RuntimeTrexSettings()
        {
            LoadSettings();

            var watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(_settingsFile) ?? AppDomain.CurrentDomain.BaseDirectory,
                Filter = Path.GetFileName(_settingsFile),
                NotifyFilter = NotifyFilters.LastWrite
            };

            watcher.Changed += (sender, args) => LoadSettings();
            watcher.EnableRaisingEvents = true;
        }
        
        public static void UpdateSetting(TrexSettings key,string value)
        {
            UpdateSetting(key.ToStringValue(),value);
        }

        public static void UpdateSetting(string key, string value)
        {
            if( _settingsCache is null ) LoadSettings();

            lock(_lock)
            {
                if (!File.Exists(_settingsFile)) return;

                var lines = File.ReadAllLines(_settingsFile).ToList();

                bool keyFound = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith(key + ""=""))
                    {
                        lines[i] = $""{key}={value}"";
                        keyFound = true;
                        break;
                    }
                }

                if (!keyFound)
                {
                    lines.Add($""{key}={value}"");
                }

                _settingsCache[key]=value;

                File.WriteAllLines(_settingsFile, lines);
                
            }
        }

        public static void LoadSettings()
        {
            lock(_lock)
            {
                var newSettings = new Dictionary<string, string>();

                if (!File.Exists(_settingsFile)) 
                {
                    _settingsCache = newSettings;
                }

                System.Diagnostics.Debug.WriteLine($""[Loading Settings From {_settingsFile}]"");

                foreach (var line in File.ReadAllLines(_settingsFile))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith(""#"")) continue;

                    var parts = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        newSettings[parts[0].Trim()] =  parts[1].Trim();
                    }
                }

                LogChanges(newSettings);
                _settingsCache  = newSettings;
            }
        }

        private static void LogChanges(Dictionary<string, string> newSettings)
        {
            if (_settingsCache == null)
            {
                foreach (var kvp in newSettings)
                {
                    System.Diagnostics.Debug.WriteLine($""[New Setting] Key: {kvp.Key}, Value: {kvp.Value}"");
                }
                return;
            }

            foreach (var kvp in newSettings)
            {
                if (_settingsCache.TryGetValue(kvp.Key, out var oldValue))
                {
                    if (oldValue != kvp.Value)
                    {
                        System.Diagnostics.Debug.WriteLine($""[Updated Setting] Key: {kvp.Key}, Old Value: {oldValue}, New Value: {kvp.Value}"");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($""[New Setting] Key: {kvp.Key}, Value: {kvp.Value}"");
                }
            }

            foreach (var kvp in _settingsCache)
            {
                if (!newSettings.ContainsKey(kvp.Key))
                {
                    System.Diagnostics.Debug.WriteLine($""[Removed Setting] Key: {kvp.Key}, Old Value: {kvp.Value}"");
                }
            }
        }

        public static string Get(string key)
        {
            if( _settingsCache is null ) LoadSettings();

            if (_settingsCache.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new KeyNotFoundException($""Key '{key}' not found in settings."");
        }

        public static string Get(TrexSettings key)
        {
            return Get(key.ToStringValue());
        }
    }
}
";
    }
}
