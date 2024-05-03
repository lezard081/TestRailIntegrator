namespace CID.Logger
{
    public interface ILoggerWrapper
    {
        void LogTrace(string message, params object[] values);
        void LogDebug(string message, params object[] values);
        void LogInformation(string message, params object[] values);
        void LogWarning(string message, params object[] values);
        void LogError(string message, params object[] values);
        void LogCritical(string message, params object[] values);
    }
}
