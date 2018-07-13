using System;

namespace TwitchViewerCounter.Core.Models
{
    public class StreamStatistics
    {
        public int ChannelId { get; set; }

        public string ChannelName { get; set; }

        public string StreamTitle { get; set; }

        public bool IsFeatured { get; set; }

        public int FeaturedPriority { get; set; }

        public bool IsSponsored { get; set; }

        public DateTime LiveStartedAt { get; set; }

        public int Viewers { get; set; }

        public int Chatters { get; set; }

        public double PercentageOfViewersInChat
        {
            get
            {
                return (double)Chatters / Viewers;
            }
        }
    }
}
