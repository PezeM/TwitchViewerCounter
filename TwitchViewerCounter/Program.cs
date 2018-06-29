using System;
using TwitchViewerCounter.Core.RequestHandler;

namespace TwitchViewerCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exitApp = false;
            var client = new TMIApiRequestHandler();
            while (!exitApp)
            {
                Console.Write("Type twitch.tv channel name: ");
                var channelName = Console.ReadLine();
                if (channelName == "exit")
                {
                    exitApp = true;
                    continue;
                }
                var response = client.GetChatterResponse(channelName);
                Console.WriteLine($"Current chatters: {response.ChatterCount}");
            }
            Console.ReadKey();
        }
    }
}
