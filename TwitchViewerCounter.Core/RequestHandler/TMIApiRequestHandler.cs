using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwitchViewerCounter.Core.Constans;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    /// <summary>
    /// Base class for handling any request from http://tmi.twitch.tv/
    /// </summary>
    public class TMIApiRequestHandler : IRequestHandler
    {
        private RestClient Client { get; }

        public TMIApiRequestHandler()
        {
            Client = new RestClient(ReguestConstans.TMIApiUrl);
        }

        /// <summary>
        /// Gets response from http://tmi.twitch.tv/group/user/{channelName}/chatters with current chatters
        /// and theirs nickname and roles on channel.
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="TMIRequestResponse"/> response</returns>
        public TMIRequestResponse GetResponse(string channelName)
        {
            var request = new RestRequest("{name}/chatters", Method.GET);
            request.AddParameter("name", channelName.ToLower(), ParameterType.UrlSegment);

            var response = Client.Execute<TMIRequestResponse>(request);

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
        /// Gets response from http://tmi.twitch.tv/group/user/{channelName}/chatters with current chatters
        /// and theirs nickname and roles on channel.
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="TMIRequestResponse"/> response</returns>
        public async Task<TMIRequestResponse> GetChatterResponseAsync(string channelName)
        {
            var request = new RestRequest("{name}/chatters", Method.GET);
            request.AddParameter("name", channelName.ToLower(), ParameterType.UrlSegment);
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await Client.ExecuteTaskAsync<TMIRequestResponse>(request, cancellationTokenSource.Token);
                //Client.DownloadData(request).SaveAs($"{channelName}_{DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm")}.txt");

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
    }
}
