using RestSharp.Deserializers;
using System.Collections.Generic;

namespace TwitchViewerCounter.Core.Models
{
    public class StreamsInformation
    {
        [DeserializeAs(Name = "streams")]
        public List<Stream> StreamsInfo { get; set; }
    }
}
