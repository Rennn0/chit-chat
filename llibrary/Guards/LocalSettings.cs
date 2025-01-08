namespace LLibrary.Guards;

// TODO binarebshi sheinaxe
public class LocalSettings
{
    public static Local Default = new Local();

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

    public static int Init(string file = ".trex", char spliter = '=')
    {
        if (!File.Exists(file))
            throw new FileNotFoundException(file);

        // TODO requesti apidan wamoigos key

        string read = Encryption.ReadFromDisk("maisi");

        _file = file;
        _spliter = spliter;
        _settings = [];

        string[] context = File.ReadAllLines(_file);

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

    private static bool InvalidLine(string s) =>
        !s.Contains(_spliter ?? '=') || string.IsNullOrWhiteSpace(s);

    private static System.Tuple<string, string> ParseLine(string s)
    {
        string[] parts = s.Split(separator: _spliter ?? '=', count: 2);
        return Tuple.Create(parts[0].Trim(), parts[1].Trim());
    }

    private static string? _file;
    private static char? _spliter;
    private static Dictionary<string, string>? _settings;
}
