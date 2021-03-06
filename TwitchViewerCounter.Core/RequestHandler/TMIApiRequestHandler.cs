﻿using RestSharp;
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
        /// <summary>
        /// Gets response from http://tmi.twitch.tv/group/user/{channelName}/chatters with current chatters
        /// and theirs nickname and roles on channel.
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="TMIRequestResponse"/> response</returns>
        public ChattersInfo GetResponse(string channelName)
        {
            var client = new RestClient(ReguestConstans.TMIApiUrl);
            var request = new RestRequest("{name}/chatters", Method.GET);
            request.AddParameter("name", ParameterType.UrlSegment);

            var response = client.Execute<ChattersInfo>(request);

            // In case it fails to get response
            if (response.ErrorException != null)
            {
                var message = $"Error retrieving response for {channelName} from tmi.twitch.tv." +
                    $"\n{response.ErrorException}";
                Logger.Log(message, LogSeverity.Error);
                return null;
            }

            return response.Data;
        }

        /// <summary>
        /// Gets response from http://tmi.twitch.tv/group/user/{channelName}/chatters with current chatters
        /// and theirs nickname and roles on channel.
        /// </summary>
        /// <param name="channelName">Twitch.tv channel name</param>
        /// <returns>Returns a<see cref="TMIRequestResponse"/> response</returns>
        public async Task<ChattersInfo> GetChatterResponseAsync(string channelName)
        {
            var client = new RestClient(ReguestConstans.TMIApiUrl);
            var request = new RestRequest("{name}/chatters", Method.GET);
            request.AddParameter("name", channelName, ParameterType.UrlSegment);

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var response = await client.ExecuteTaskAsync<ChattersInfo>(request, cancellationTokenSource.Token);
                //Client.DownloadData(request).SaveAs($"{channelName}_{DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm")}.txt");

                // In case it fails to get response
                if (response.ErrorException != null)
                {
                    var message = $"Error retrieving response for {channelName} from tmi.twitch.tv." +
                        $"\n{response.ErrorException}";
                    Logger.Log(message, LogSeverity.Error);
                    return null;
                }

                return response.Data;
            }
        }
    }
}
