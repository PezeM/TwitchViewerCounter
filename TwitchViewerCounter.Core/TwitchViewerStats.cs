using System.Threading.Tasks;
using TwitchViewerCounter.Core.Configuration;
using TwitchViewerCounter.Core.Exceptions;
using TwitchViewerCounter.Core.Models;
using TwitchViewerCounter.Core.RequestHandler;

namespace TwitchViewerCounter.Core
{
    public class TwitchViewerStats
    {
        private TwitchApiRequestHandler TwitchApi { get; set; }
        private TMIApiRequestHandler TMIApi { get; set; }

        public void Start(string clientId)
        {
            CheckClientId(clientId);

            TwitchApi = new TwitchApiRequestHandler(clientId);
            TMIApi = new TMIApiRequestHandler();
        }

        public async Task GetViewersInfoAsync(string channelName)
        {
            if (channelName.Length == 0)
            {
                Logger.Log("Channel name is empy.");
                return;
            }

            Logger.Log($"Getting information for channel: {channelName}...");
            var tmiResponse = await TMIApi.GetChatterResponseAsync(channelName);
            var twitchResponse = await TwitchApi.GetResponseAsync(channelName);
            DisplayInformation(tmiResponse, twitchResponse.StreamInfo, channelName);
        }

        private void DisplayInformation(TMIRequestResponse tmiResponse, StreamInfo streamInfo, string channel)
        {
            if (tmiResponse == null || streamInfo == null)
            {
                Logger.Log($"Can't get information for channel: {channel}.", LogSeverity.Error);
                return;
            }
            var percentageOfViewersInChat = (double)tmiResponse.ChatterCount / streamInfo.Viewers;
            Logger.Log($"Total viewers: {streamInfo.Viewers}, viewers in chat: {tmiResponse.ChatterCount}, % of people in chat: {percentageOfViewersInChat:0.0%}");
        }

        private static void CheckClientId(string clientId)
        {
            if (clientId?.Length == 0)
            {
                const string message = "Client ID inside config file is empty.";
                Logger.Log(message);
                throw new InvalidClientIdException(message);
            }

            if (TwitchViewerCounterConfiguration.IsClientIdDefault(clientId))
            {
                const string message = "Client ID inside config file is default, change it!";
                Logger.Log(message);
                throw new ClientIdNotSetException(message);
            }
        }
    }
}
