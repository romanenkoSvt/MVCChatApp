using NLog;
using System;

namespace TelegramBotLibrary.Helpers
{
    public class Log
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Info(string message)
        {
            Logger.Info(message);
        }

        public static void Info(string format, params object[] args)
        {
            Logger.Info(format, args);
        }

        public static void Error(Exception ex)
        {
            Logger.Error(ex.ToString());
        }

        public static void Error(string exception)
        {
            Logger.Error(exception);
        }

        public static void Error(string format, params object[] args)
        {
            Logger.Error(format, args);
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Debug(string format, params object[] args)
        {
            Logger.Debug(format, args);
        }
    }
}
