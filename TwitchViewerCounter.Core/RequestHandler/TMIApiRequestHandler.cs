using RestSharp;
using System;
using TwitchViewerCounter.Core.Constans;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Core.RequestHandler
{
    public class TMIApiRequestHandler : IRequestHandler
    {
        private RestClient Client { get; }

        public TMIApiRequestHandler()
        {
            Client = new RestClient(ReguestConstans.Url);
        }

        /// <summary>
        /// Gets response from http://tmi.twitch.tv/group/user/{channelName}/chatters with current chatters
        /// and theirs nickname and roles on channel.
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="TMIRequestResponse"/> response</returns>
        public TMIRequestResponse GetChatterResponse(string channelName)
        {
            var request = new RestRequest("{name}/chatters", Method.GET);
            request.AddParameter("name", channelName.ToLower(), ParameterType.UrlSegment);

            var response = Client.Execute<TMIRequestResponse>(request);

            // In case it fails, throw an exception
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }

            return response.Data;
        }
    }
}
