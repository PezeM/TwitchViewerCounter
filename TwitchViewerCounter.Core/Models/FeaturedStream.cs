using RestSharp.Deserializers;
using System.Collections.Generic;

namespace TwitchViewerCounter.Core.Models
{
    public class FeaturedStream
    {
        [DeserializeAs(Name = "featured")]
        public List<FeaturedStreamInfo> Featured { get; set; }
    }
}
