using System.Collections.Generic;
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
            var twitchResponse = await TwitchApi.GetChannelInformationAsync(channelName);
            var featuredStreams = await TwitchApi.GetFeaturedStreamsAsync();
            var featuredStream = CheckIfStreamIsFeatured(twitchResponse.StreamInfo, featuredStreams.Featured);
            DisplayInformation(tmiResponse, twitchResponse.StreamInfo, channelName, featuredStream);
        }

        private void DisplayInformation(TMIRequestResponse tmiResponse, StreamInfo streamInfo, string channel, FeaturedStreamInfo featured)
        {
            if (tmiResponse == null || streamInfo == null)
            {
                Logger.Log($"Can't get information for channel: {channel}.", LogSeverity.Error);
                return;
            }

            var percentageOfViewersInChat = (double)tmiResponse.ChatterCount / streamInfo.Viewers;

            var featuredMessage = "";
            if (featured != null)
            {
                featuredMessage = $"Is stream featured: Yes\n" +
                    $"Priority in front page(from 0 to 10): {featured.Priority}\n" +
                    $"Is stream sponsored: {featured.Sponsored}";
            }

            var message = $"Displaying information for channel: {channel}\n" +
                $"Total viewers: {streamInfo.Viewers}\n" +
                $"Viewers in chat: {tmiResponse.ChatterCount}\n" +
                $"% of people in chat: {percentageOfViewersInChat:0.0%}\n" +
                $"Live started at: {streamInfo.LiveStartedAt.ToLocalTime()}\n" +
                featuredMessage;

            Logger.Log(message);
        }

        private FeaturedStreamInfo CheckIfStreamIsFeatured(StreamInfo streamInfo, List<FeaturedStreamInfo> featured)
        {
            if (streamInfo == null || featured == null)
                return null;

            foreach (var featuredInfo in featured)
            {
                if (featuredInfo.Stream.Channel.Id == streamInfo.Channel.Id)
                    return featuredInfo;
            }

            return null;
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
