using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TwitchViewerCounter.Core.Configuration;
using TwitchViewerCounter.Core.Exceptions;
using TwitchViewerCounter.Core.Models;
using TwitchViewerCounter.Core.RequestHandler;
using System.Collections.Specialized;
using TwitchViewerCounter.Core.Helpers;
using TwitchViewerCounter.Database.Repositories;
using TwitchViewerCounter.Database;
using TwitchViewerCounter.Database.Entities;

namespace TwitchViewerCounter.Core
{
    public class TwitchViewerStats
    {
        private TwitchApiRequestHandler TwitchApi { get; set; }
        private TMIApiRequestHandler TMIApi { get; set; }
        private ObservableCollection<string> OnlineLiveStreams { get; set; }
        private string ClientId { get; set; }

        public async Task StartAsync(string clientId)
        {
            ClientId = clientId;
            CheckClientId(ClientId);

            TwitchApi = new TwitchApiRequestHandler(ClientId);
            TMIApi = new TMIApiRequestHandler();
            OnlineLiveStreams = new ObservableCollection<string>();
            OnlineLiveStreams.CollectionChanged += OnlineLiveStreams_CollectionChanged;

            SetupTimers();
        }

        private void SetupTimers()
        {
            var liveStreamsCheckList = TwitchViewerCounterConfiguration.Instance.GetLiveStreamsList();
            var liveCheckInterval = TwitchViewerCounterConfiguration.Instance.GetCheckIfLiveInterval();
            var viewersInformationCheckInterval = TwitchViewerCounterConfiguration.Instance.GetCheckViewersInformationInterval();

            // Runs only if the check list is declared
            if (liveStreamsCheckList != null)
            {
                //// Timer that will check list of live streams to see their status
                //var checkLiveStreamsStatusTimer = new Timer(e => CheckLiveStreamsStatusAsync(liveStreamsCheckList),
                //    null, TimeSpan.Zero, TimeSpan.FromSeconds(liveCheckInterval));

                //var getLiveStreamsInformationTimer = new Timer(e => GetLiveStreamsInformation(OnlineLiveStreams), null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

                CheckLiveStreamsStatusAsync(liveStreamsCheckList, liveCheckInterval);
                CheckViewersInformationAsync(OnlineLiveStreams, viewersInformationCheckInterval);
            }
        }

        public async Task GetViewersInfoAsync(string channelName)
        {
            if (channelName.Length == 0)
            {
                Logger.Log("Channel name is empy.");
                return;
            }

            // Change the name to lowercase, because twitch api only works with lowercase names
            channelName = channelName.ToLower();

            Logger.Log($"Getting information for channel: {channelName}...");

            var streamer = new StreamerInformation(channelName, ClientId);
            await streamer.GetChattersInformationAsync();
            await streamer.GetStreamInformationAsync();

            var featuredStreams = await TwitchApi.GetFeaturedStreamsAsync(TwitchViewerCounterConfiguration.Instance.GetFeaturedStreamsLocation(),
                TwitchViewerCounterConfiguration.Instance.GetFeaturedStreamsLanguage());
            var featuredStream = StreamHelpers.CheckIfStreamIsFeatured(streamer.Stream, featuredStreams.Featured);

            await DisplayInformation(streamer, channelName, featuredStream);
        }

        private async Task DisplayInformation(StreamerInformation streamerInformation, string channel, FeaturedStreamInfo featured)
        {
            if (streamerInformation.Chatters == null || streamerInformation.Stream == null)
            {
                Logger.Log($"Can't get information for channel: {channel}.", LogSeverity.Error);
                return;
            }

            var featuredMessage = "";
            if (featured != null)
            {
                featuredMessage = "\nIs stream featured: Yes\n" +
                    $"Priority in front page(from 0 to 10): {featured.Priority}\n" +
                    $"Is stream sponsored: {featured.Sponsored}";
            }

            var message = $"Displaying information for channel: {channel}\n" +
                $"Total viewers: {streamerInformation.Stream.Viewers}\n" +
                $"Viewers in chat: {streamerInformation.Chatters.ChatterCount}\n" +
                $"% of people in chat: {streamerInformation.PercentageOfViewersInChat:0.0%}\n" +
                $"Live started at: {streamerInformation.Stream.LiveStartedAt.ToLocalTime()}" +
                featuredMessage;

            await SaveToDatabase(streamerInformation, featured);
            Logger.Log(message, LogSeverity.Info);
        }

        private async Task SaveToDatabase(StreamerInformation streamer, FeaturedStreamInfo featuredStream)
        {
            var streamerEntity = new Streamer
            {
                ChannelName = streamer.ChannelName,
                Chatters = streamer.Chatters.ChatterCount,
                Viewers = streamer.Stream.Viewers,
                PercentageOfViewersInChat = streamer.PercentageOfViewersInChat,
                LiveStartedAt = streamer.Stream.LiveStartedAt.UtcDateTime,
                Time = DateTime.Now,
                IsFeatured = featuredStream != null,
                FeaturedPriority = featuredStream == null ? -1 : featuredStream.Priority,
                IsSponsored = featuredStream == null ? false : featuredStream.Sponsored
            };

            try
            {
                var context = new MongoDataContext();
                var streamerRepository = new StreamerRepository(context);

                await streamerRepository.SaveAsync(streamerEntity);
                Logger.Log($"Saved {streamerEntity.ChannelName} to database.", LogSeverity.Debug);
            }
            catch (Exception ex)
            {
                Logger.Log($"Couldn't save {streamerEntity.ChannelName} to database.\n {ex}", LogSeverity.Critical);
                throw;
            }
        }


        private static void CheckClientId(string clientId)
        {
            if (clientId?.Length == 0)
            {
                const string message = "Client ID inside config file is empty.";
                Logger.Log(message, LogSeverity.Critical);
                throw new InvalidClientIdException(message);
            }

            if (TwitchViewerCounterConfiguration.Instance.IsClientIdDefault(clientId))
            {
                const string message = "Client ID inside config file is default, change it!";
                Logger.Log(message, LogSeverity.Critical);
                throw new ClientIdNotSetException(message);
            }
        }

        private async void OnlineLiveStreams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var stream in e.NewItems)
                {
                    Logger.Log($"User went live: {stream}", LogSeverity.Warning);
                    await GetViewersInfoAsync(stream.ToString());
                }
            }
            if (e.OldItems != null)
            {
                foreach (var stream in e.OldItems)
                {
                    Logger.Log($"User went offline: {stream}", LogSeverity.Warning);
                }
            }
        }

        private async Task CheckLiveStreamsStatusAsync(List<string> liveStreamsCheckList, int liveCheckInterval)
        {
            while (true)
            {
                var liveStreams = await TwitchApi.GetLiveStreamsInformationAsync(liveStreamsCheckList.ToArray());
                Logger.Log("Running check live streams status...");
                foreach (var live in liveStreams.StreamsInfo)
                {
                    // Add stream to online streams list if it turns online
                    if (StreamHelpers.IsLiveOnline(live) && !OnlineLiveStreams.Contains(live.Channel.Name))
                        OnlineLiveStreams.Add(live.Channel.Name);

                    // Remove stream from online streams list if it turns offline
                    if (OnlineLiveStreams.Contains(live.Channel.Name) && !StreamHelpers.IsLiveOnline(live))
                        OnlineLiveStreams.Remove(live.Channel.Name);
                }

                await Task.Delay(TimeSpan.FromSeconds(liveCheckInterval));
            }
        }

        private async Task CheckViewersInformationAsync(ObservableCollection<string> onlineLiveStreams, int liveCheckInterval)
        {
            while (true)
            {
                Logger.Log("Running get live streams information...");
                if (onlineLiveStreams.Count != 0)
                {
                    foreach (var live in onlineLiveStreams)
                    {
                        await GetViewersInfoAsync(live);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(liveCheckInterval));
            }
        }

        #region Other version of timers

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
                if (StreamHelpers.IsLiveOnline(live) && !OnlineLiveStreams.Contains(live.Channel.Name))
                    OnlineLiveStreams.Add(live.Channel.Name);
            }
        }

        #endregion
    }
}
