using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class FeaturedStreamInfo
    {
        [DeserializeAs(Name = "priority")]
        public long Priority { get; set; }

        [DeserializeAs(Name = "sponsored")]
        public bool Sponsored { get; set; }

        [DeserializeAs(Name = "stream")]
        public StreamInfo Stream { get; set; }
    }
}
