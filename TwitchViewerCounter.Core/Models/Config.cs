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
        /// Location for checking if stream is at front page for that country
        /// </summary>
        public string LocationForFeaturedStreams { get; set; }

        /// <summary>
        /// Checking if stream with this language is at front page
        /// </summary>
        public string LanguageForFeaturedStreams { get; set; }

        /// <summary>
        /// List of live streams to check if they are live. 
        /// If yes then check their statistics every CheckInterval.
        /// </summary>
        public List<string> LiveStreamsCheckingList { get; set; }

        /// <summary>
        /// Live streams will be checked every CheckIfLiveInterval(in seconds)
        /// </summary>
        public int CheckIfLiveInterval { get; set; }

        /// <summary>
        /// Information about viewers will be checked every CheckViewersInformationInterval(in seconds)
        /// </summary>
        public int CheckViewersInformationInterval { get; set; }
    }
}
