using System;
using System.Collections.Generic;
using TwitchViewerCounter.Core.Models;
using TwitchViewerCounter.Core.Storage;

namespace TwitchViewerCounter.Core.Configuration
{
    public sealed class TwitchViewerCounterConfiguration
    {
        private static readonly TwitchViewerCounterConfiguration instance = new TwitchViewerCounterConfiguration(new DataStorage());
        public static TwitchViewerCounterConfiguration Instance => instance;

        private const string ClientId = "PASTE-CLIENTID-HERE";

        public readonly Config Config;

        public TwitchViewerCounterConfiguration(IDataStorage dataStorage)
        {
            try
            {
                Config = dataStorage.RestoreObject<Config>("config");
            }
            catch (Exception)
            {
                Config = new Config
                {
                    ClientId = ClientId
                };

                dataStorage.StoreObject(Config, "config");
            }
        }

        public bool IsClientIdDefault(string id) => id == ClientId;

        public List<string> GetLiveStreamsList() => Config.LiveCheckList;
    }
}
