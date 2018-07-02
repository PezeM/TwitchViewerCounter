using System.IO;
using System.Reflection;

namespace TwitchViewerCounter.Core.Constans
{
    public static class Globals
    {
        public static readonly string ApplicationPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static readonly string DataStoragePath = Path.Combine(ApplicationPath, "data");
    }
}
