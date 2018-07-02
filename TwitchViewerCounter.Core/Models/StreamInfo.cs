using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class StreamInfo
    {
        [DeserializeAs(Name = "_id")]
        public long Id { get; set; }
        [DeserializeAs(Name = "game")]
        public string Game { get; set; }

        [DeserializeAs(Name = "viewers")]
        public int Viewers { get; set; }

        [DeserializeAs(Name = "stream_type")]
        public string StreamType { get; set; }
    }
}
