using System;

namespace TwitchViewerCounter.Database.Entities
{
    public class Streamer : IEntity
    {
        public string Id { get; set; }
        public string ChannelName { get; set; }
        public DateTime Time { get; set; }
        public DateTime LiveStartedAt { get; set; }
        public int Viewers { get; set; }
        public int Chatters { get; set; }
        public double PercentageOfViewersInChat { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsSponsored { get; set; }
        public int FeaturedPriority { get; set; }
    }
}
