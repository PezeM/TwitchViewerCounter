using System;
using System.IO;
using TwitchViewerCounter.Core.Constans;

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
                var logMessage = $"[{DateTime.Now,-19}]: {message}";
                Console.ForegroundColor = textColor;
                Console.WriteLine(logMessage);
                Console.ResetColor();

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
                    return ConsoleColor.Red;
                case LogSeverity.Debug:
                    return ConsoleColor.Green;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Info:
                    return ConsoleColor.DarkCyan;
                case LogSeverity.Warning:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
