using RestSharp.Deserializers;

namespace TwitchViewerCounter.Core.Models
{
    public class Channel
    {
        [DeserializeAs(Name = "_id")]
        public long Id { get; set; }

        [DeserializeAs(Name = "name")]
        public string Name { get; set; }
    }
}
