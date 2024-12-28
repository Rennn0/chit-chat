using Serilog;
using Serilog.Core;
using TelegramSink;

namespace LLibrary.Logging;

public class TelegramAdapter : AbstractAdapter, ITelegramAdapter
{
    protected override void ConsumeMessage(LogMessage message)
    {
        _telegramLogger.Error(
            $@"
⚠️
    🪪 Message source _ {message.Source}
 
    📬 Message _ {message.Message}

    🌋 Error _ {@message.Exception}

    ⌚ Timestamp _ {DateTime.Now.ToLocalTime()}
⚠️
"
        );
    }

    private readonly Logger _telegramLogger;

    public TelegramAdapter(string apiKey, string chatId)
    {
        _telegramLogger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.TeleSink(telegramApiKey: apiKey, telegramChatId: chatId)
            .CreateLogger();
    }
}
