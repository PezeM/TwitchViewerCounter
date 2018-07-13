using System;

namespace TwitchViewerCounter.Database.Entities
{
    public class Streamer : IEntity
    {
        public string Id { get; set; }
        public string ChannelName { get; set; }
        public DateTime Time { get; set; }
        public int Viewers { get; set; }
        public int Chatters { get; set; }
    }
}
