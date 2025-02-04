using static llibrary.Logging.AbstractAdapter;

namespace llibrary.Logging
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

    public interface ICriticalLogger { }

    public interface IWarningLogger { }

    public interface IInformationLogger { }
}
