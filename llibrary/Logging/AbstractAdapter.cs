using System.Collections.Concurrent;

namespace llibrary.Logging
{
    public abstract class AbstractAdapter : ILoggerAdapter, IDisposable
    {
        /// <summary>
        ///     არგუმენტების ლოგის შესაქმნელად
        /// </summary>
        /// <param name="source">წარმოშობის წყარო</param>
        /// <param name="message">მესიჯი</param>
        /// <param name="exception">გამონაკლისი ობიექტი სერიალიზდება JSONად, გაჩუმების პრინციპით null </param>
        public class LoggerEventArgs(string source, string message, object? exception = null)
            : EventArgs
        {
            public string Source { get; } = source;
            public string Message { get; } = message;
            public object? Exception { get; } = exception;
        }

        public event EventHandler<LoggerEventArgs> LogEvent;

        public void OnLogEvent(LoggerEventArgs e) => LogEvent.Invoke(this, e);

        protected AbstractAdapter()
        {
            _logQueue = new BlockingCollection<LogMessage>();

            _cancellationTokenSource = new CancellationTokenSource();

            LogEvent += SerilogAdapter_LogEvent;

            new Thread(ConsumeQueue) { IsBackground = true }.Start();
        }

        protected virtual void SerilogAdapter_LogEvent(object? sender, LoggerEventArgs e) =>
            EnqueueLog(new LogMessage(e.Source, e.Message, e.Exception));

        protected abstract void ConsumeMessage(LogMessage message);

        private void EnqueueLog(LogMessage message) => _logQueue.Add(message);

        private void ConsumeQueue()
        {
            try
            {
                foreach (
                    LogMessage log in _logQueue.GetConsumingEnumerable(
                        _cancellationTokenSource.Token
                    )
                )
                {
                    ConsumeMessage(message: log);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("log_dump", $"Error consuming messages: {@ex}");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _logQueue.CompleteAdding();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        protected record LogMessage(string Source, string Message, object? Exception = null);

        private readonly BlockingCollection<LogMessage> _logQueue;

        private readonly CancellationTokenSource _cancellationTokenSource;
    }
}