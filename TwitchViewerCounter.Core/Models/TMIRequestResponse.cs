using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class TMIRequestResponse
    {
        [DeserializeAs(Name = "chatter_count")]
        public int ChatterCount { get; set; }

        [DeserializeAs(Name = "chatters")]
        public Chatters Chatters { get; set; }
    }
}
