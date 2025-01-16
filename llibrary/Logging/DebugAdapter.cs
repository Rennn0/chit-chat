using Serilog;
using Serilog.Core;

namespace llibrary.Logging
{
    public class DebugAdapter : AbstractAdapter, IDebugAdapter
    {
        private readonly Logger _debugLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .CreateLogger();

        protected override void ConsumeMessage(LogMessage message)
        {
            _debugLogger.Debug(
                $"""

                 Source = {message.Source}
                 Exception = {message.Exception}

                 """
            );
        }
    }
}