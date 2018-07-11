using System;
using System.IO;
using System.Reflection;

namespace TwitchViewerCounter.Core.Constans
{
    public static class Globals
    {
        public static readonly string ApplicationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static readonly string DataStoragePath = Path.Combine(ApplicationPath, "data");

        // Logs
        public static readonly string LogsDirectoryPath = Path.Combine(ApplicationPath, "Logs");
        public static readonly string LogFilePath = Path.Combine(LogsDirectoryPath, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.txt");
    }
}
