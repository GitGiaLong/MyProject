using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CoreLib.API.LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message) => logger.Debug(message);

        public void LogError(string message) => logger.Error(message);
        public void LogErrorNew(ControllerContext context, Exception exception)
        {
            logger.Error($"'{context.ActionDescriptor.ControllerName}'-'{context.ActionDescriptor.ActionName}()' " +
                $"-> [Exception: {exception.Message}]" +
                $"-> [Content:{exception.StackTrace}]");
        }

        public void LogInfo(string message) => logger.Info(message);

        public void LogWarn(string message) => logger.Warn(message);
    }
}
