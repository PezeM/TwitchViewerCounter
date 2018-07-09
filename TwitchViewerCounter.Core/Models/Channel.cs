using RestSharp.Deserializers;
using System;

namespace TwitchViewerCounter.Core.Models
{
    public class Channel
    {
        [DeserializeAs(Name = "mature")]
        public bool Mature { get; set; }

        [DeserializeAs(Name = "partner")]
        public bool Partner { get; set; }

        [DeserializeAs(Name = "status")]
        public string Status { get; set; }

        [DeserializeAs(Name = "broadcaster_language")]
        public string BroadcasterLanguage { get; set; }

        [DeserializeAs(Name = "game")]
        public string Game { get; set; }

        [DeserializeAs(Name = "language")]
        public string Language { get; set; }

        [DeserializeAs(Name = "_id")]
        public long Id { get; set; }

        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        [DeserializeAs(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DeserializeAs(Name = "views")]
        public int Views { get; set; }

        [DeserializeAs(Name = "followers")]
        public int Followers { get; set; }
    }
}
