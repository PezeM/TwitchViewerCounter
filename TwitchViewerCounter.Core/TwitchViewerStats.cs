﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using TwitchViewerCounter.Core.Configuration;
using TwitchViewerCounter.Core.Exceptions;
using TwitchViewerCounter.Core.Models;
using TwitchViewerCounter.Core.RequestHandler;
using System.Collections.Specialized;

namespace TwitchViewerCounter.Core
{
    public class TwitchViewerStats
    {
        private TwitchApiRequestHandler TwitchApi { get; set; }
        private TMIApiRequestHandler TMIApi { get; set; }
        private ObservableCollection<string> OnlineLiveStreams { get; set; }

        public async Task Start(string clientId)
        {
            CheckClientId(clientId);

            TwitchApi = new TwitchApiRequestHandler(clientId);
            TMIApi = new TMIApiRequestHandler();
            OnlineLiveStreams = new ObservableCollection<string>();
            OnlineLiveStreams.CollectionChanged += OnlineLiveStreams_CollectionChanged;

            SetupTimers();
        }

        private void SetupTimers()
        {
            var liveStreamsCheckList = TwitchViewerCounterConfiguration.Instance.GetLiveStreamsList();
            var liveCheckInterval = TwitchViewerCounterConfiguration.Instance.GetCheckInterval();

            // Runs only if the check list is declared
            if (liveStreamsCheckList != null)
            {
                // Timer that will check list of live streams to see their status
                var checkLiveStreamsStatusTimer = new Timer(e => CheckLiveStreamsStatusAsync(liveStreamsCheckList),
                    null, TimeSpan.Zero, TimeSpan.FromSeconds(liveCheckInterval));

                var getLiveStreamsInformationTimer = new Timer(e => GetLiveStreamsInformation(OnlineLiveStreams), null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            }
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

        private void DisplayInformation(TMIRequestResponse tmiResponse, Stream streamInfo, string channel, FeaturedStreamInfo featured)
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

        private FeaturedStreamInfo CheckIfStreamIsFeatured(Stream streamInfo, List<FeaturedStreamInfo> featured)
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

            if (TwitchViewerCounterConfiguration.Instance.IsClientIdDefault(clientId))
            {
                const string message = "Client ID inside config file is default, change it!";
                Logger.Log(message);
                throw new ClientIdNotSetException(message);
            }
        }

        private void OnlineLiveStreams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Logger.Log("Running on live streams collection changed...");
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    Logger.Log($"User went live: {item}");
                }
            }
        }

        private async Task GetLiveStreamsInformation(ObservableCollection<string> onlineLiveStreams)
        {
            Logger.Log("Running get live streams information...");
            if (onlineLiveStreams.Count != 0)
            {
                foreach (var live in onlineLiveStreams)
                {
                    await GetViewersInfoAsync(live);
                }
            }
        }

        private async Task CheckLiveStreamsStatusAsync(List<string> liveStreamsCheckList)
        {
            var liveStreams = await TwitchApi.GetLiveStreamsInformationAsync(liveStreamsCheckList.ToArray());
            Logger.Log("Running check live streams status...");
            foreach (var live in liveStreams.StreamsInfo)
            {
                if (IsLiveOnline(live) && !OnlineLiveStreams.Contains(live.Channel.Name))
                    OnlineLiveStreams.Add(live.Channel.Name);
            }
        }

        private bool IsLiveOnline(Stream live) => live.StreamType == "live";
    }
}
