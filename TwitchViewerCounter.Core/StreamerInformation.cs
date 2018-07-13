using System.Threading.Tasks;
using TwitchViewerCounter.Core.Models;
using TwitchViewerCounter.Core.RequestHandler;

namespace TwitchViewerCounter.Core
{
    internal class StreamerInformation
    {
        private string ApiClientIp { get; }
        private TMIApiRequestHandler TMIApi { get; }
        private TwitchApiRequestHandler TwitchApi { get; }

        public string ChannelName { get; set; }
        public ChattersInfo Chatters { get; set; }
        public Stream Stream { get; set; }

        public double PercentageOfViewersInChat
        {
            get
            {
                return (double)Chatters.ChatterCount / Stream.Viewers;
            }
        }

        public StreamerInformation(string channelName, string apiClientIp)
        {
            ChannelName = channelName;
            ApiClientIp = apiClientIp;
            TMIApi = new TMIApiRequestHandler();
            TwitchApi = new TwitchApiRequestHandler(ApiClientIp);
        }

        public async Task GetChattersInformationAsync()
        {
            Chatters = await TMIApi.GetChatterResponseAsync(ChannelName);
        }

        public async Task GetStreamInformationAsync()
        {
            var streamInfo = await TwitchApi.GetChannelInformationAsync(ChannelName);
            Stream = streamInfo.StreamInfo;
        }
    }
}