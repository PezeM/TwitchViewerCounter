using System;
using System.Threading.Tasks;
using TwitchViewerCounter.Core;
using TwitchViewerCounter.Core.Configuration;
using TwitchViewerCounter.Core.Exceptions;
using TwitchViewerCounter.Core.Storage;

namespace TwitchViewerCounter.ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var programStart = new TwitchViewerStats();
            var programCfg = new TwitchViewerCounterConfiguration(new DataStorage());

            try
            {
                programStart.Start(programCfg.Config.ClientId);
            }
            catch (ClientIdNotSetException)
            {
                Logger.Log("Set client id inside config.json file.", LogSeverity.Critical);
                Logger.Log("Application will now exit.", LogSeverity.Critical);
                Logger.Log("Press any key...", LogSeverity.Critical);
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (InvalidClientIdException)
            {
                Logger.Log("The client id inside config.json is invalid.", LogSeverity.Critical);
                Logger.Log("Application will now exit.", LogSeverity.Critical);
                Logger.Log("Press any key...", LogSeverity.Critical);
                Console.ReadKey();
                Environment.Exit(0);
            }

            bool exitApp = false;
            while (!exitApp)
            {
                Console.Write("Type twitch.tv channel name: ");
                var channelName = Console.ReadLine();

                if (channelName == "exit")
                {
                    exitApp = true;
                    continue;
                }

                await programStart.GetViewersInfoAsync(channelName);
            }
            Console.ReadKey();
        }
    }
}
