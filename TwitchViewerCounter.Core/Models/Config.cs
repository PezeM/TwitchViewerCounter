using System.Collections.Generic;

namespace TwitchViewerCounter.Core.Models
{
    public class Config
    {
        public string ClientId { get; set; }
        public List<string> LiveCheckList { get; set; }
    }
}
