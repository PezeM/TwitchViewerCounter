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
                await programStart.Start(programCfg.Config.ClientId);
            }
            catch (ClientIdNotSetException)
            {
                Logger.Log("Set client id inside config.json file.\n" +
                    "Application will now exit.\n" +
                    "Press any key...", LogSeverity.Critical);
                Console.ReadKey();
                Environment.Exit(0);
            }
            catch (InvalidClientIdException)
            {
                Logger.Log("The client id inside config.json is invalid.\n" +
                        "Application will now exit.\n" +
                        "Press any key...", LogSeverity.Critical);
                Console.ReadKey();
                Environment.Exit(0);
            }

            var exitApp = false;
            while (!exitApp)
            {
                Console.Write("Enter Twitch.tv channel name: ");
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
