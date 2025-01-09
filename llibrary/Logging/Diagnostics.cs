using System.Diagnostics;

namespace LLibrary.Logging;

public class Diagnostics
{
    [Flags]
    public enum ThemeOptions
    {
        Error = 0x0000_0001,
        Warning = 0x0000_0010,
        Info = Error | Warning,
    }

    public static void LOG_ERROR(string msg, ThemeOptions theme = ThemeOptions.Error) =>
        Flush(msg, theme);

    public static void LOG_WARNING(string msg, ThemeOptions theme = ThemeOptions.Warning) =>
        Flush(msg, theme);

    public static void LOG_INFO(string msg, ThemeOptions theme = ThemeOptions.Info) =>
        Flush(msg, theme);

    private static void ChangeTheme(ThemeOptions theme)
    {
        Console.ForegroundColor = theme switch
        {
            ThemeOptions.Error => ConsoleColor.Red,
            ThemeOptions.Warning => ConsoleColor.Yellow,
            ThemeOptions.Info => ConsoleColor.Green,
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null),
        };
    }

    private static void Flush(string msg, ThemeOptions theme)
    {
        string text = FormatMsg(msg, theme);
        if (HasConsole())
        {
            Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ");

            ChangeTheme(theme);
            Console.Write(
                $"[{theme switch
                {
                    ThemeOptions.Error => "Error",
                    ThemeOptions.Warning => "Warning",
                    ThemeOptions.Info => "Log",
                    _ => "unknown",
                }}] "
            );
            ResetTheme();

            Console.Write(msg);
            Console.WriteLine();
        }
        else if (Debugger.IsAttached)
        {
            Debug.WriteLine(text);
        }
        else
        {
            FlushToFile(text);
        }
    }

    private static bool HasConsole()
    {
        try
        {
            return Console.WindowHeight > 0;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private static string FormatMsg(string msg, ThemeOptions theme)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logType = theme switch
        {
            ThemeOptions.Error => "err",
            ThemeOptions.Warning => "warning",
            ThemeOptions.Info => "log",
            _ => "unknown",
        };
        return $"[{timestamp}] [{logType}] {msg}";
    }

    private static void FlushToFile(string msg)
    {
        System.IO.File.AppendAllText("diagnostics.txt", msg);
    }

    private static void ResetTheme()
    {
        Console.ResetColor();
    }
}
