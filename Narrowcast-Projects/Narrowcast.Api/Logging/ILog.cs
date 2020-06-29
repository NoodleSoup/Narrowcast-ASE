namespace Narrowcast.Api.Logging
{
    /// <summary>
    /// Interface for NLogger setup containing the different log levels.
    /// </summary>
    public interface ILog
    {
        void Information(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
    }
}
