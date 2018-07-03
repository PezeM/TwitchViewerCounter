using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class ChannelInformation
    {
        [DeserializeAs(Name = "stream")]
        public StreamInfo StreamInfo { get; set; }
    }
}
