using RestSharp;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwitchViewerCounter.Core.Constans;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    public class TwitchApiRequestHandler
    {
        private RestClient Client { get; }
        private string ClientId { get; }

        public TwitchApiRequestHandler(string clientId)
        {
            Client = new RestClient(ReguestConstans.TwitchApiUrl);
            ClientId = clientId;
        }

        public ChannelInformation GetChannelInformation(string channelName)
        {
            var request = new RestRequest("{channelName}?client_id={clientId}", Method.GET);
            request.AddUrlSegment("channelName", channelName.ToLower());
            request.AddUrlSegment("clientId", ClientId);

            var response = Client.Execute<ChannelInformation>(request);

            // In case it fails, throw an exception
            if (response.ErrorException != null)
            {
                var message = $"Error retrieving response for {channelName}.";
                Logger.Log(message, LogSeverity.Error);
                throw new ApplicationException(message, response.ErrorException);
            }

            return response.Data;
        }

        public async Task<ChannelInformation> GetChannelInformationAsync(string channelName)
        {
            var request = new RestRequest("{channelName}?client_id={clientId}", Method.GET);
            request.AddUrlSegment("channelName", channelName.ToLower());
            request.AddUrlSegment("clientId", ClientId);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await Client.ExecuteTaskAsync<ChannelInformation>(request, cancellationTokenSource.Token);

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

        public async Task<FeaturedStream> GetFeaturedStreamsAsync()
        {
            var request = new RestRequest("featured?geo=PL&lang=pl&limit=100");
            request.AddHeader("Client-ID", "og5i89qkvg35o2ji7pxtjjqa3x0npj");

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await Client.ExecuteTaskAsync<FeaturedStream>(request, cancellationTokenSource.Token);

                // In case it fails, throw an exception
                if (response.ErrorException != null)
                {
                    var message = $"Error retrieving response.";
                    Logger.Log(message, LogSeverity.Error);
                    throw new ApplicationException(message, response.ErrorException);
                }

                return response.Data;
            }
        }
    }
}
