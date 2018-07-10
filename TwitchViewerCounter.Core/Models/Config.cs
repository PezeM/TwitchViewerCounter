using System.Collections.Generic;

namespace TwitchViewerCounter.Core.Models
{
    public class Config
    {
        /// <summary>
        /// Twitch.tv developer client id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// List of live streams to check if they are live. 
        /// If yes then check their statistics every CheckInterval.
        /// </summary>
        public List<string> LiveStreamsCheckingList { get; set; }

        /// <summary>
        /// Live streams will be checked every CheckInterval(in seconds)
        /// </summary>
        public int CheckInterval { get; set; }
    }
}
