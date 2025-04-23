using llibrary.Guards;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace llibrary.Logging
{
    public static class Extensions
    {
        private const string c_defaultDebugOutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration Debug(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel logEventLevel = LevelAlias.Minimum,
            IFormatProvider? formatProvider = null,
            string template = c_defaultDebugOutputTemplate
        )
        {
            Guard.AgainstNull(sinkConfiguration, nameof(sinkConfiguration));
            Guard.AgainstNull(template, nameof(template));

            MessageTemplateTextFormatter formatter = new MessageTemplateTextFormatter(
                template,
                formatProvider
            );

            return sinkConfiguration.Sink(
                logEventSink: new DebugSink(formatter),
                restrictedToMinimumLevel: logEventLevel
            );
        }

        private sealed class DebugSink : ILogEventSink
        {
            private readonly ITextFormatter _formatter;

            public DebugSink(ITextFormatter formatter)
            {
                _formatter = formatter;
            }

            public void Emit(LogEvent logEvent)
            {
                Console.WriteLine(logEvent.MessageTemplate);
                using StringWriter sw = new StringWriter();
                _formatter.Format(logEvent, sw);
                System.Diagnostics.Debug.WriteLine(sw.ToString());
            }
        }
    }
}
