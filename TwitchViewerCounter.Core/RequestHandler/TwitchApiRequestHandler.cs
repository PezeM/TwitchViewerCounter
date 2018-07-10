using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwitchViewerCounter.Core.Constans;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    /// <summary>
    /// Base class for handling request from https://api.twitch.tv/kraken/streams/
    /// </summary>
    public class TwitchApiRequestHandler
    {
        private RestClient Client { get; }
        private string ClientId { get; }

        public TwitchApiRequestHandler(string clientId)
        {
            Client = new RestClient(ReguestConstans.TwitchApiUrl);
            ClientId = clientId;
        }

        /// <summary>
        /// Gets channel informations from https://api.twitch.tv/kraken/streams/{channelName}?client_id={clientId}
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="StreamInformation"/>Channel information response</returns>
        public StreamInformation GetChannelInformation(string channelName)
        {
            var request = new RestRequest("{channelName}?client_id={clientId}", Method.GET);
            request.AddUrlSegment("channelName", channelName.ToLower());
            request.AddUrlSegment("clientId", ClientId);

            var response = Client.Execute<StreamInformation>(request);

            // In case it fails, throw an exception
            if (response.ErrorException != null)
            {
                var message = $"Error retrieving response for {channelName}.";
                Logger.Log(message, LogSeverity.Error);
                throw new ApplicationException(message, response.ErrorException);
            }

            return response.Data;
        }

        /// <summary>
        /// Gets channel informations from https://api.twitch.tv/kraken/streams/{channelName}?client_id={clientId}
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="StreamInformation"/>Channel information response</returns>
        public async Task<StreamInformation> GetChannelInformationAsync(string channelName)
        {
            var request = new RestRequest("{channelName}?client_id={clientId}", Method.GET);
            request.AddUrlSegment("channelName", channelName.ToLower());
            request.AddUrlSegment("clientId", ClientId);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await Client.ExecuteTaskAsync<StreamInformation>(request, cancellationTokenSource.Token);

                // In case it fails, throw an exception
                if (response.ErrorException != null)
                {
                    var message = $"Error retrieving response for {channelName}.";
                    Logger.Log(message, LogSeverity.Error);
                    throw new ApplicationException(message, response.ErrorException);
                }

                return response.Data;
            }
        }

        /// <summary>
        /// Gets list of featured streams(on front page) from https://api.twitch.tv/kraken/streams/featured?geo=PL&lang=pl&limit=100}
        /// </summary>
        /// <returns>Returns a<see cref="FeaturedStream"/>List of featured streamers</returns>
        public async Task<FeaturedStream> GetFeaturedStreamsAsync()
        {
            var request = new RestRequest("featured?geo=PL&lang=pl&limit=100");
            request.AddHeader("Client-ID", ClientId);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await Client.ExecuteTaskAsync<FeaturedStream>(request, cancellationTokenSource.Token);

                // In case it fails, throw an exception
                if (response.ErrorException != null)
                {
                    var message = "Error retrieving response.";
                    Logger.Log(message, LogSeverity.Error);
                    throw new ApplicationException(message, response.ErrorException);
                }

                return response.Data;
            }
        }

        /// <summary>
        /// Gets information for every provided channel
        /// </summary>
        /// <param name="channels">List of channel names</param>
        /// <returns>Returns a <see cref="StreamsInformation"/>List of stream information</returns>
        public async Task<StreamsInformation> GetLiveStreamsInformationAsync(string[] channels)
        {
            var request = new RestRequest("?channel={channels}");
            request.AddUrlSegment("channels", string.Join(",", channels));
            request.AddHeader("Client-ID", ClientId);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await Client.ExecuteTaskAsync<StreamsInformation>(request, cancellationTokenSource.Token);

                // In case it fails, throw an exception
                if (response.ErrorException != null)
                {
                    var message = "Error retrieving response.";
                    Logger.Log(message, LogSeverity.Error);
                    throw new ApplicationException(message, response.ErrorException);
                }

                return response.Data;
            }
        }
    }
}
