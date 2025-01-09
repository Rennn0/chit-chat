using System.Text;

namespace LLibrary.Guards;

public class LocalSettings
{
    public static Local Default { get; } = new Local();

    public class Local
    {
        public string this[string key]
        {
            get
            {
                _settings ??= [];

                return _settings.TryGetValue(key, out string? value) ? value : string.Empty;
            }
        }
    }

    public static int Init(string secret, string file = ".trex", char spliter = '=')
    {
        if (!File.Exists(file))
            throw new FileNotFoundException(file);

        _secret = secret;
        _file = file;
        _spliter = spliter;
        _settings = [];

        string[] context = Encryption
            .ReadFromDisk(encryptionKey: secret, file: file)
            .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (context.Length == 0)
            return 0;

        for (int i = 0; i < context.Length; i++)
        {
            if (InvalidLine(context[i]))
                continue;
            (string key, string value) = ParseLine(context[i]);
            _settings.TryAdd(key, value);
        }

        return _settings.Count;
    }

    public static bool UpdateOrCreate(string key, string value)
    {
        Guard.AgainstNull(_settings);
        Guard.AgainstNull(_secret);
        Guard.AgainstNull(_file);

        _settings[key] = value;

        StringBuilder newSettings = new StringBuilder();
        foreach ((string s, string v) in _settings)
        {
            newSettings.AppendLine($"{s}={v}");
        }

        return !string.IsNullOrEmpty(
            Encryption.FlushOnDisk(
                rawText: newSettings.ToString(),
                encryptionKey: _secret,
                file: _file
            )
        );
    }

    private static bool InvalidLine(string s) =>
        !s.Contains(_spliter ?? '=') || string.IsNullOrWhiteSpace(s);

    private static System.Tuple<string, string> ParseLine(string s)
    {
        string[] parts = s.Split(separator: _spliter ?? '=', count: 2);
        return Tuple.Create(parts[0].Trim(), parts[1].Trim());
    }

    private static string? _secret;
    private static string? _file;
    private static char? _spliter;
    private static Dictionary<string, string>? _settings;
}
