using System;
using System.IO;
using TwitchViewerCounter.Core.Constans;

namespace TwitchViewerCounter.Core
{
    public static class Logger
    {
        private static readonly object lockObj = new object();

        /// <summary>
        /// Log message to file and write it to console
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="logSeverity">Severity of the message.</param>
        public static void Log(string message, LogSeverity logSeverity = LogSeverity.Default)
        {
            lock (lockObj)
            {
                var textColor = SeverityToConsoleColor(logSeverity);
                var logMessage = $"[{DateTime.Now,-19}]: {message}";

                Console.ForegroundColor = textColor;
                Console.WriteLine(logMessage);
                Console.ResetColor();

                if (!Directory.Exists(Globals.LogsDirectoryPath))
                    Directory.CreateDirectory(Globals.LogsDirectoryPath);

                File.AppendAllText(Globals.LogFilePath, $"{logMessage}\n");
            }
        }

        /// <summary>
        /// Change console text color depending of severity
        /// </summary>
        /// <param name="severity">Severity of the log</param>
        /// <returns>Console text color</returns>
        private static ConsoleColor SeverityToConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.DarkRed;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                case LogSeverity.Debug:
                    return ConsoleColor.Green;
                case LogSeverity.Info:
                    return ConsoleColor.DarkCyan;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
