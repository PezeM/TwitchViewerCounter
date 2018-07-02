using System;
using TwitchViewerCounter.Core.Models;
using TwitchViewerCounter.Core.Storage;

namespace TwitchViewerCounter.Core.Configuration
{
    public class TwitchViewerCounterConfiguration
    {
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

        public static bool IsClientIdDefault(string id) => id == ClientId;
    }
}
