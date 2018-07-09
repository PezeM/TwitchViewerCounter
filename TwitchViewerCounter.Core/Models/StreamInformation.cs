using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class StreamInformation
    {
        [DeserializeAs(Name = "stream")]
        public Stream StreamInfo { get; set; }
    }
}
