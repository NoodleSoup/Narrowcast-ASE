﻿using NLog;

namespace Narrowcast.Api.Logging
{
    /// <summary>
    /// Custom logger implementation to write exceptions etc. to files.
    /// </summary>
    public class NLogger : ILog
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}
