using Microsoft.Extensions.Logging;

namespace CID.Logger
{
    public class LoggerWrapper : ILoggerWrapper
    {
        private static ILogger _logger;

        public LoggerWrapper(ILogger<LoggerWrapper> logger)
        {
            _logger = logger;
        }

        public void LogCritical(string message, params object[] values)
        {
            _logger.LogCritical(message, values);
        }

        public void LogDebug(string message, params object[] values)
        {
            _logger.LogDebug(message, values);
        }

        public void LogError(string message, params object[] values)
        {
            _logger.LogError(message, values);
        }

        public void LogInformation(string message, params object[] values)
        {
            _logger.LogInformation(message, values);
        }

        public void LogTrace(string message, params object[] values)
        {
            _logger.LogTrace(message, values);
        }

        public void LogWarning(string message, params object[] values)
        {
            _logger.LogTrace(message, values);
        }
    }
}
