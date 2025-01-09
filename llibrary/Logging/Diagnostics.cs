using System.Diagnostics;

namespace LLibrary.Logging;

public class Diagnostics
{
    [Flags]
    public enum THEME_OPTIONS
    {
        Error = 0x0000_0001,
        Warning = 0x0000_0010,
        Info = Error | Warning,
    }

    public static void LOG_ERROR(string msg, THEME_OPTIONS theme = THEME_OPTIONS.Error) =>
        Flush(msg, theme);

    public static void LOG_WARNING(string msg, THEME_OPTIONS theme = THEME_OPTIONS.Warning) =>
        Flush(msg, theme);

    public static void LOG_INFO(string msg, THEME_OPTIONS theme = THEME_OPTIONS.Info) =>
        Flush(msg, theme);

    private static void ChangeTheme(THEME_OPTIONS theme)
    {
        Console.ForegroundColor = theme switch
        {
            THEME_OPTIONS.Error => ConsoleColor.Red,
            THEME_OPTIONS.Warning => ConsoleColor.Yellow,
            THEME_OPTIONS.Info => ConsoleColor.Green,
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null),
        };
    }

    private static void Flush(string msg, THEME_OPTIONS theme)
    {
        string text = FormatMsg(msg, theme);
        if (HasConsole())
        {
            Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ");

            ChangeTheme(theme);
            Console.Write(
                $"[{theme switch
                {
                    THEME_OPTIONS.Error => "Error",
                    THEME_OPTIONS.Warning => "Warning",
                    THEME_OPTIONS.Info => "Log",
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

    private static string FormatMsg(string msg, THEME_OPTIONS theme)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logType = theme switch
        {
            THEME_OPTIONS.Error => "err",
            THEME_OPTIONS.Warning => "warning",
            THEME_OPTIONS.Info => "log",
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
