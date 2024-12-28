using Serilog;
using static LLibrary.Logging.AbstractAdapter;

namespace LLibrary.Logging
{
    public interface ILoggerAdapter
    {
        /// <summary>
        ///     ნებიმისერი ლოგის დროს იძახება ეს ივენთი
        /// </summary>
        event EventHandler<LoggerEventArgs> LogEvent;

        /// <summary>
        ///     ივენთის გამომძახებელი
        /// </summary>
        /// <param name="e"></param>
        public void OnLogEvent(LoggerEventArgs e);
    }

    public interface ITelegramAdapter : ILoggerAdapter { }

    public interface IDebugAdapter : ILoggerAdapter { }
}
