using System;
using NLog;

namespace OnlineExamAPI.common
{
    public class LoggerService : ILog
    {
        public LoggerService()
        {

        }
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Information(object message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}