using Microsoft.AspNetCore.Mvc;

namespace CoreLib.API.LoggerService
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogErrorNew(ControllerContext context, Exception exception);
    }
}
