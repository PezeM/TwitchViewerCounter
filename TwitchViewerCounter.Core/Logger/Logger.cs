using System;
using System.IO;

namespace TwitchViewerCounter.Core
{
    public static class Logger
    {
        private static readonly object lockObj = new object();

        public static void Log(string message, LogSeverity logSeverity = LogSeverity.Info)
        {
            lock (lockObj)
            {
                var textColor = SeverityToConsoleColor(logSeverity);
                var logMessage = $"[{DateTime.Now.ToLongTimeString()}]: {message}";
                Console.ForegroundColor = textColor;
                Console.WriteLine(logMessage);
                Console.ResetColor();

                File.AppendAllText("log.txt", $"{logMessage}\n");
            }
        }

        private static ConsoleColor SeverityToConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Debug:
                    return ConsoleColor.Green;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Info:
                    return ConsoleColor.Cyan;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
