using RestSharp.Deserializers;
using System.Collections.Generic;

namespace TwitchViewerCounter.Core.Models
{
    public class FeaturedStreams
    {
        [DeserializeAs(Name = "featured")]
        public List<FeaturedStreamInfo> Featured { get; set; }
    }
}
