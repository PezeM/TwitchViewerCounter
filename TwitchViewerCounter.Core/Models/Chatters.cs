using RestSharp.Deserializers;
using System.Collections.Generic;

namespace TwitchViewerCounter.Core.Models
{
    public class Chatters
    {
        [DeserializeAs(Name = "moderators")]
        public List<string> Moderators { get; set; }

        [DeserializeAs(Name = "staff")]
        public List<string> Staff { get; set; }

        [DeserializeAs(Name = "admins")]
        public List<string> Admins { get; set; }

        [DeserializeAs(Name = "global_mods")]
        public List<string> GlobalMods { get; set; }

        [DeserializeAs(Name = "viewers")]
        public List<string> Viewers { get; set; }
    }
}
