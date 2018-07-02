using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class TwitchApiRequestResponse
    {
        [DeserializeAs(Name = "stream")]
        public StreamInfo StreamInfo { get; set; }
    }
}
